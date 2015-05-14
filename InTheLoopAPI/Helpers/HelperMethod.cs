using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;

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
            catch 
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
    }
}