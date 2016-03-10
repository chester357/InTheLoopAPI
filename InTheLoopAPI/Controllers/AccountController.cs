using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using InTheLoopAPI.Models;
using InTheLoopAPI.Providers;
using InTheLoopAPI.Results;
using InTheLoopAPI.Models.Request;
using System.Text;
using InTheLoopAPI.Helpers;
using System.IO;
using InTheLoopAPI.Service;
using System.Web.Helpers;
using InTheLoopAPI.App_Start;
using InTheLoopAPI.Models.Database;
using System.Web.Http.Cors;

namespace InTheLoopAPI.Controllers
{
    // Allow CORS for all origins. (Caution!)
    //[EnableCors(origins: "*", headers: "*", methods: "*")]

    // [RequireHttps]
    [Authorize, RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [HttpPost, Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
                Quote = model.Quote,
                ImageURL = model.ImageURL
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost, Route("TakenEmail")]
        public IHttpActionResult TakenEmail(EmailModel model)
        {
            try
            {
                var isTaken = new DatabaseContext().Users.Any(x => x.Email == model.Email);

                var jsonResult = new Dictionary<String, Boolean>() {{"IsTaken", isTaken}};

                return Ok(jsonResult);
            }
            catch {
                return InternalServerError();
            }
        }

        // POST api/Account/Update
        [Route("UpdateProfile")]
        public IHttpActionResult UpdateProfile(UpdateBindingModel model)
        {
            try
            {
                DatabaseContext context = new DatabaseContext();

                var userId = User.Identity.GetUserId();

                var user = context.Users.SingleOrDefault(x => x.Id == userId);

                if(user.Email != model.Email)
                {
                    var taken = context.Users.Any(x => x.Email == model.Email);

                    if(taken) { return BadRequest("This email address is already taken"); }

                    user.Email = model.Email;
                }

                if (user.UserName != model.UserName)
                {
                    var taken = context.Users.Any(x => x.UserName == model.UserName);

                    if (taken) { return BadRequest("This username is already taken"); }

                    user.UserName = model.UserName;
                }

                if (user.ImageURL != model.ImageURL)
                {
                    user.ImageURL = model.ImageURL;
                }

                context.SaveChanges();

                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
            
        }

        // POST api/Account/ProfileImage
       // [Route("ProfileImage")]
       // public IHttpActionResult UploadImage()
       // {
       //     try
       //     {
       //         DatabaseContext context = new DatabaseContext();

       //         string userId = User.Identity.GetUserId();

       //         if (userId == null) return BadRequest();

       //         var user = context.Users.SingleOrDefault(x => x.Id == userId);

       //         if (user == null) return BadRequest();

       //         var image = HttpContext.Current.Request.Files[0];

       //         if (image == null) return BadRequest("No content.");

       //         if (image.ContentType.Substring(0, 5) != "image") return BadRequest("Invalid content type.");

       //         BinaryReader reader = new BinaryReader(image.InputStream);

       //         user.Image = reader.ReadBytes(image.ContentLength);

       //         context.SaveChanges();

       //         return Ok();
       //     }
       //     catch(Exception ex)
       //     {
       //         return InternalServerError(ex);
       //     }
       //}

        // GET api/Account/Profile
        [Route("Profile/{userId}")]
        public IHttpActionResult GetProfile(string userId)
        {
            try
            {
                var datacontext = new DatabaseContext();

                var user = datacontext.Users.SingleOrDefault(x => x.Id == userId);

                if (user == null) return BadRequest();

                var following = datacontext.Follows.Count(x => x.UserId == userId);

                var followers = datacontext.Follows.Count(x => x.FollowingId == userId);

                var tags = datacontext.TagUsers.Count(x => x.UserId == userId);

                var eventCounts = datacontext.EventHeaders
                .Count(x =>
                    (x.Archived == false && (x.End.CompareTo(DateTime.UtcNow) >= 0)) &&
                    (
                        // All events that I'm attending
                        x.Attendees.Any(n => n.UserId == userId) ||
                        // All of my events I posted
                        x.EventFooter.UserId == userId
                    )
                );

                var profile = new UserModel
                {
                    Email = user.Email,
                    ImageURL = user.ImageURL,
                    Quote = user.Quote,
                    UserId = user.Id,
                    UserName = user.UserName,
                    FollowersCount = followers,
                    FollowingCount = following,
                    TagCount = tags,
                    EventCount = eventCounts
                };

                return Ok(profile);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/Account/Profile
        [Route("Profile")]
        public IHttpActionResult GetProfile()
        {
            try
            {
                String userId = User.Identity.GetUserId();

                var datacontext = new DatabaseContext();

                var user = datacontext.Users.SingleOrDefault(x => x.Id == userId);

                if (user == null) return BadRequest();

                var following = datacontext.Follows.Count(x => x.UserId == userId);

                var followers = datacontext.Follows.Count(x => x.FollowingId == userId);

                var tags = datacontext.TagUsers.Count(x => x.UserId == userId);

                var attendedEvents = datacontext.Attendances.Count(x => x.UserId == userId);

                var postedEvents = datacontext.EventFooters.Count(x => x.UserId == userId);

                var profile = new UserModel
                {
                    Email = user.Email,
                    ImageURL = user.ImageURL,
                    Quote = user.Quote,
                    UserId = user.Id,
                    UserName = user.UserName,
                    FollowersCount = followers,
                    FollowingCount = following,
                    TagCount = tags,
                    EventCount = attendedEvents + postedEvents
                };

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/Account/Profile
        [Route("Profile/UserId")]
        public IHttpActionResult GetUserId()
        {
            try
            {
                String userId = User.Identity.GetUserId();

                return Ok(new { UserId = userId });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }

        [AllowAnonymous, Route("Contact")]
        public IHttpActionResult AddContact(ContactBindingModel model)
        {
            try
            {
                var db = new DatabaseContext();

                db.Contacts.Add(new Contact { Email = model.Email, Name = model.Name });

                db.SaveChanges();

                return Ok();
               
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route("UpdatePassword")]
        public IHttpActionResult UpdatePassword(UpdatePasswordModel model)
        {
            try
            {
                var token = _userManager.GeneratePasswordResetToken(User.Identity.GetUserId());

                _userManager.ResetPassword(User.Identity.GetUserId(), token, model.Password);

                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [AllowAnonymous, Route("ForgotPassword")]
        public IHttpActionResult ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                var user = UserManager.FindByEmail(model.SendTo);

                if (user == null) { return BadRequest(); }

                var items = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
                var ran = new Random();
                var ranArray = new char[10];

                for (int i = 0; i < ranArray.Length; i++ )
                {
                    ranArray[i] = items[ran.Next(items.Length)];
                }

                var tempPassword = new String(ranArray);

                if (tempPassword.Length != 10) { return BadRequest(); }

                //var token = UserManager.GeneratePasswordResetToken(user.Id);

                var emailService = new EmailService();

                try
                {
                    emailService.SendEmail(user.Email, tempPassword);

                    var repository = new DatabaseContext();

                    var resetToken = repository.ResetTokens.SingleOrDefault(x => x.UserId == user.Id);

                    if (resetToken != null) 
                    {
                        repository.ResetTokens.Remove(resetToken);

                        repository.SaveChanges();
                    }
                    
                    repository.ResetTokens.Add(
                        new ResetToken
                        {
                            LongToken = UserManager.GeneratePasswordResetToken(user.Id),
                            ShortToken = tempPassword,
                            UserId = user.Id
                        });

                    repository.SaveChanges();

                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [AllowAnonymous, Route("ResetPassword")]
        public IHttpActionResult ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (model.EmailAddress == "" || model.EmailAddress == null) { return BadRequest(); }

                if (model.Token == "" || model.Token == null) { return BadRequest(); }

                if (model.NewPassword.Length < 6) { return BadRequest(); }

                var user = UserManager.FindByEmail(model.EmailAddress);

                if (user == null) { return BadRequest(); }

                var repo = new DatabaseContext();
                
                var resetToken = repo.ResetTokens.SingleOrDefault(x => x.UserId == user.Id && x.ShortToken == model.Token);

                if (resetToken == null) { return BadRequest(); }

                repo.ResetTokens.Remove(resetToken);

                repo.SaveChanges();

                var result = UserManager.ResetPassword(user.Id, resetToken.LongToken, model.NewPassword);

                if (result.Succeeded)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
