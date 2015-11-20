using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.ModelBinding;

namespace InTheLoopAPI.Helpers
{
    public static class HelperMethod
    {
        public static bool IsValidUrl(string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                return true;
            }
            catch(Exception ex)
            { 
                return false;
            }
        }

        public static string DisplayErrors(List<ValidationResult> errors)
        {
            string result = "";

            errors.ForEach(x => result += x.ErrorMessage + " " );

            return result;
        }

        public static string ToBase64(byte[] array)
        {
            return System.Convert.ToBase64String(array);
        }
    }
}