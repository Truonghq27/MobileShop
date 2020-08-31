using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.Admin.Models
{
    public class CustomersAutherize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //check Login
            if (HttpContext.Current.Request.Cookies["InfoCustomer"] == null)
            {
                return false;
            }
            return true;
        }
    }
}