using SIS.HTTP;
using SIS.HTTP.Response;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SulsApp.Controllers
{
    public class HomeController : Controller
    {
        public HttpResponse Index(HttpRequest request)
        {
            var layout = File.ReadAllText("Views/Shared/_Layout.html");
            var html = File.ReadAllText("Views/Home/Index.html");
            var bodyWithLayout = layout.Replace("@RenderBody()", html);
            return new HtmlResponse(bodyWithLayout);
        }
    }
}
