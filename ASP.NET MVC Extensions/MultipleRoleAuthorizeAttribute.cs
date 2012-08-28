using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Extensions
{
    public class MultipleRoleAuthorizeAttribute : AjaxAwareAuthorizeAttribute
    {
        private string[] _authorizedRoles;

        public string[] AuthorizedRoles
        {
            get { return _authorizedRoles ?? new string[0]; }
            set { _authorizedRoles = value; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            // If no roles on the attribute, just check that the user is logged in and keep going
            if (AuthorizedRoles.Length == 0)
            {
                return true;
            }

            // If any of the authorized roles fits any assigned roles, keep going
            if (AuthorizedRoles.Any(httpContext.User.IsInRole))
            {
                return true;
            }

            return false;
        }
    }
}