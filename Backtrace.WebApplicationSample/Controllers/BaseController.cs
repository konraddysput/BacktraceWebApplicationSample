using Backtrace.Model;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Web.Mvc;

namespace Backtrace.WebApplicationSample.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var backtraceAttributes = new Dictionary<string, object>()
            {
                {"statuCode", filterContext.HttpContext.Response.StatusCode },
                {"formData", filterContext.HttpContext.Request.Form },
                {"url", filterContext.HttpContext.Request.Url.ToString() },
                { "queryParameters", filterContext.HttpContext.Request.QueryString.ToString() },
                {"userName", filterContext.HttpContext.User?.Identity?.Name}
            };

            string hostUrl = ConfigurationManager.AppSettings["hostUrl"];
            string token = ConfigurationManager.AppSettings["token"];

            var backtraceCredentials = new BacktraceCredentials(
                backtraceHostUrl: hostUrl,
                accessToken: token);

            BacktraceClient backtraceClient = new BacktraceClient(backtraceCredentials);
            var backtraceResult = backtraceClient.Send(filterContext.HttpContext.Timestamp.ToString(), backtraceAttributes);
            Debug.WriteLine($"Backtrace Server result: {backtraceResult.Status}");

            base.OnActionExecuted(filterContext);
        }
    }
}