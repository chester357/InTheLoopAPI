using InTheLoopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using InTheLoopAPI.Models.Database;

namespace InTheLoopAPI.Migrations.Seeds
{
    public static class UserSeed
    {
        public static void Run(DatabaseContext context)
        {
            var password = "Password123!";

            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);

            #region Users
            var users = new List<User>
            {
                new User 
                { 
                    Email = "chester@gmail.com", 
                    UserName = "chester357",
                    Image = "TempUserImage.jpg",
                    Quote = "Just going with the flow",
                    Events = new List<EventHeader>
                    {
                        new EventHeader
                        {
                            BaseEvent = new EventFooter 
                            { 
                                AgeGroup = AgeGroup.TwentyOnePlus,
                                Category = Category.NightLife,
                                Description = "Fundraising party for help fight cancer please come and support",
                                Logo = "TempLogo.jpg",
                                Title = "Redcross Fundraiser",
                                Website = "www.redcross.com",
                            },
                            City = "New Orleans",
                            End = DateTime.Now.AddHours(5),
                            Latitude = 29.9500,
                            Longitude = -90.0667,
                            Start = DateTime.Now,
                            State = State.LA,
                            ZipCode = 70131
                        }
                    }
                },
                 new User 
                { 
                    Email = "james@gmail.com", 
                    UserName = "jamestheman",
                    Image = "TempUserImage.jpg",
                    Quote = "I know the best events",
                    Events = new List<EventHeader>
                    {
                        new EventHeader
                        {
                            BaseEvent = new EventFooter 
                            { 
                                AgeGroup = AgeGroup.EighteenPlus,
                                Category = Category.Music,
                                Description = "Deftones and Incubus live. Long awaited tour!",
                                Logo = "TempLogo.jpg",
                                Title = "Deftones Concert",
                                Website = "www.ticketmaster.com",
                            },
                            City = "New Orleans",
                            End = DateTime.Now.AddHours(10),
                            Latitude = 29.9500,
                            Longitude = -90.0667,
                            Start = DateTime.Now.AddHours(5),
                            State = State.LA,
                            ZipCode = 70131
                        }
                    }
                },
                new User 
                { 
                    Email = "desiree@gmail.com", 
                    UserName = "dezzybaby",
                    Image = "TempUserImage.jpg",
                    Quote = "Keeping it real 24/7",
                    Events = new List<EventHeader>
                    {
                        new EventHeader
                        {
                            BaseEvent = new EventFooter
                            {
                                AgeGroup = AgeGroup.All,
                                Category = Category.Food,
                                Description = @"We serve the best taco out of a truck!. Seriously this tacos a authentic mexican and are one o
                                of a kind please come check them out.",
                                Logo = "TempLogo.jpg",
                                Title = "Juans Flying Taco Truck",
                                Website = "www.juanstacotruck.com"
                            },
                            City = "New Orleans",
                            End = DateTime.Now.AddHours(19),
                            Latitude = 29.9500,
                            Longitude = -90.0667,
                            Start = DateTime.Now,
                            State = State.LA,
                            ZipCode = 70131
                        }
                    }
                },
                new User 
                { 
                    Email = "mike@gmail.com", 
                    UserName = "mikethetiger",
                    Image = "TempUserImage.jpg",
                    Quote = "LSU is the coolest",
                    Events = new List<EventHeader>
                    {
                        new EventHeader
                        {
                            BaseEvent = new EventFooter 
                            { 
                                AgeGroup = AgeGroup.TwentyOnePlus,
                                Category = Category.Bar,
                                Description = "The best deal in town. A must do for every LSU alum!",
                                Logo = "TempLogo.jpg",
                                Title = "Free drinks",
                                Website = "www.fredsbr.com",
                            },
                            City = "Baton Rouge",
                            End = DateTime.Now.AddHours(7),
                            Latitude = 30.4500,
                            Longitude = -91.1400,
                            Start = DateTime.Now.AddHours(3),
                            State = State.LA,
                            ZipCode = 70816
                        }
                    }
                },
                new User 
                { 
                    Email = "jackie@gmail.com", 
                    UserName = "jackiesgarage",
                    Image = "TempUserImage.jpg",
                    Quote = "I just wanna go fast",
                    Events = new List<EventHeader>
                    {
                        new EventHeader
                        {
                            BaseEvent = new EventFooter 
                            { 
                                AgeGroup = AgeGroup.All,
                                Category = Category.Automotive,
                                Description = "Some out and show off your sweet ride. Great fun for every so please bring the family",
                                Logo = "TempLogo.jpg",
                                Title = "Speed Club Car Show",
                                Website = "www.classiccarshows.com",
                            },
                            City = "New Orleans",
                            End = DateTime.Now.AddHours(19),
                            Latitude = 29.9500,
                            Longitude = -90.0667,
                            Start = DateTime.Now,
                            State = State.LA,
                            ZipCode = 70131
                        }
                    }
                },
                new User
                {
                    Email = "courney@gmail.com", 
                    UserName = "Number1Party",
                    Image = "TempUserImage.jpg",
                    Quote = "Going to the hottest parties"
                },
                new User
                {
                    Email = "anthony@gmail.com", 
                    UserName = "anthony12",
                    Image = "TempUserImage.jpg",
                    Quote = "Not much to say"
                }
            };
            #endregion

            foreach (var user in users)
            {
                if (!context.Users.Any(x => x.Email == user.Email))
                    userManager.Create(user, password);
            }
        }
    }
}