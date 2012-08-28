using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using Should;

namespace Extensions
{
    public static class TestExtensions
    {
        public static void ShouldShowDefaultView(this ActionResult actionResult)
        {
            actionResult.ShouldBeType<ViewResult>();
            ((ViewResult) actionResult).ViewName.ShouldEqual(string.Empty); 
        }

        public static void ShouldShowView(this ActionResult actionResult, string viewName)
        {
            actionResult.ShouldBeType<ViewResult>();
            ((ViewResult) actionResult).ViewName.ShouldEqual(viewName);
        }

        public static void ShouldRedirectToAction(this ActionResult actionResult, string actionName)
        {
            actionResult.ShouldBeType<RedirectToRouteResult>();

            var actualValues = ((RedirectToRouteResult) actionResult).RouteValues;
            var expectedValues = new RouteValueDictionary
                                     {
                                         {"action", actionName}
                                     };
            foreach (var key in expectedValues.Keys)
            {
                actualValues[key].ShouldEqual(expectedValues[key]);
            }
        }

        public static void ShouldRedirectToActionController(this ActionResult actionResult, string actionName, string controllerName)
        {
            actionResult.ShouldBeType<RedirectToRouteResult>();

            var actualValues = ((RedirectToRouteResult) actionResult).RouteValues;
            var expectedValues = new RouteValueDictionary
                                     {
                                         {"action", actionName},
                                         {"controller", controllerName}
                                     };
            foreach (var key in expectedValues.Keys)
            {
                actualValues[key].ShouldEqual(expectedValues[key]);
            }
        }

        public static void ShouldRedirectToActionControllerArea(this ActionResult actionResult, string actionName, string controllerName, string areaName)
        {
            actionResult.ShouldBeType<RedirectToRouteResult>();

            var actualValues = ((RedirectToRouteResult)actionResult).RouteValues;
            var expectedValues = new RouteValueDictionary
                                     {
                                         {"action", actionName},
                                         {"controller", controllerName},
                                         {"area", areaName}
                                     };
            foreach (var key in expectedValues.Keys)
            {
                actualValues[key].ShouldEqual(expectedValues[key]);
            }
        }

        public static void ShouldHaveModelStateError(this ActionResult actionResult, string key,
                                                     string errorMessage)
        {
            actionResult.ShouldBeType<ViewResult>();
            ((ViewResult) actionResult).ViewData.ModelState[key].Errors.Single().ErrorMessage.ShouldEqual(errorMessage);
        }

        public static object ModelFor<TModel>(this ActionResult actionResult,
                                              Expression<Func<TModel, object>> expression)
        {
            actionResult.ShouldBeType<ViewResult>();
            var viewResult = (ViewResult) actionResult;
            var func = expression.Compile();
            return func((TModel) viewResult.ViewData.Model);
        }
    }
}