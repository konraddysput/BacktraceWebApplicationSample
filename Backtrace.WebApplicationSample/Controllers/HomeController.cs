using Backtrace.Model;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Backtrace.WebApplicationSample.Controllers
{
    /// <summary>
    /// Simple example how to use Backtrace Client in ASP.NET MVC
    /// 1.[Home Controller] Simple example how to send data via BacktraceClient from Controller. (You're here)
    /// 2.[Middleware] Simple example how to use middlewares to provide usefull analytics to Backtrace API
    /// 3.[Dependency Injection] Example how to use our client with Autofac
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> About()
        {
            try
            {
                var random = new Random();
                if (random.Next(0, 10) < 5)
                {
                    System.IO.File.ReadAllBytes("path to not existing file");
                }
            }
            catch (FileNotFoundException exception)
            {
                string hostUrl = ConfigurationManager.AppSettings["hostUrl"];
                string token = ConfigurationManager.AppSettings["token"];

                var backtraceCredentials = new BacktraceCredentials(
                    backtraceHostUrl: hostUrl,
                    accessToken: token);

                BacktraceClient backtraceClient = new BacktraceClient(backtraceCredentials);
                var backtraceResult = await backtraceClient.SendAsync(exception);
                Debug.WriteLine($"Backtrace Server result: {backtraceResult.Status}");
            }

            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}