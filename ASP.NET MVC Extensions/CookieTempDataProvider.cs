using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;

namespace Extensions
{
    public class CookieTempDataProvider : ITempDataProvider
    {
        private const string _cookieName = "TempData";

        private readonly IFormatter _formatter;

        public CookieTempDataProvider()
        {
            _formatter = new BinaryFormatter();
        }

        public IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
        {
            var cookie = controllerContext.HttpContext.Request.Cookies[_cookieName];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                var bytes = Convert.FromBase64String(cookie.Value);
                using (var stream = new MemoryStream(bytes))
                {
                    return _formatter.Deserialize(stream) as IDictionary<string, object>;
                }
            }

            return new Dictionary<string, object>();
        }

        public void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
            try
            {
                var cookie = new HttpCookie(_cookieName)
                                 {
                                     HttpOnly = true
                                 };

                if (values.Count == 0)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    cookie.Value = string.Empty;
                    controllerContext.HttpContext.Response.Cookies.Set(cookie);

                    return;
                }

                using (var stream = new MemoryStream())
                {
                    _formatter.Serialize(stream, values);
                    var bytes = stream.ToArray();

                    cookie.Value = Convert.ToBase64String(bytes);
                }
                controllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            catch
            {
                // Do nothing, since we are not concerned if a temp data cookie write fails.
            }
        }
    }
}