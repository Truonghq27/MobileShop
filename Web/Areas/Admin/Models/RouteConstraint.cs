using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Web.Areas.Admin.Models
{
    public class GuidConstraint : IRouteConstraint
    {

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            object value;
            if (!values.TryGetValue(parameterName, out value)) return false;
            if (value is Guid) return true;

            var stringValue = Convert.ToString(value);
            if (string.IsNullOrWhiteSpace(stringValue)) return false;

            Guid guidValue;
            if (!Guid.TryParse(stringValue, out guidValue)) return false;
            if (guidValue == Guid.Empty) return false;

            return true;
        }
    }
}