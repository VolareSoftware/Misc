using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace Extensions
{
    public static class ExceptionExtensions
    {
        public static void AddToModelState(this Exception ex, ModelStateDictionary modelStateDictionary)
        {
            if (ex is DbEntityValidationException)
            {
                var dbEx = ((DbEntityValidationException) ex);
                foreach (var validationError in dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors))
                {
                    modelStateDictionary.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                }
            }
            else
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                modelStateDictionary.AddModelError("", ex.Message);
            }
        }
    }
}