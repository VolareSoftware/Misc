using System.Web.Mvc;

namespace Extensions
{
    public class AjaxAwareAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            HandleUnauthenticatedAjaxRequest(filterContext);
        }

        internal void HandleUnauthenticatedAjaxRequest(AuthorizationContext filterContext)
        {
            if (filterContext.Result is HttpUnauthorizedResult && filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.HttpContext.Response.End();
            }
        }
    }
}