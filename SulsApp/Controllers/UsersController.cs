using SIS.HTTP;
using SIS.HTTP.Response;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SulsApp.Controllers
{
    public class UsersController : Controller
    {
        public HttpResponse Login (HttpRequest request)
        {
            var layout = File.ReadAllText("Views/Shared/_Layout.html");
            var html = File.ReadAllText("Views/Users/Login.html");
            var bodyWithLayout = layout.Replace("@RenderBody()", html);
            return new HtmlResponse(bodyWithLayout);
        }

        public HttpResponse Register(HttpRequest request)
        {
            var layout = File.ReadAllText("Views/Shared/_Layout.html");
            var html = File.ReadAllText("Views/Users/Register.html");
            var bodyWithLayout = layout.Replace("@RenderBody()", html);
            return new HtmlResponse(bodyWithLayout);
        }
    }
}
