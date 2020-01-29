using SIS.HTTP;
using SIS.HTTP.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    public class Program
    {
        static async Task Main()
        {
            var routeTable = new List<Route>();

            routeTable.Add(new Route(HttpMethodType.Get, "/", Index));
            routeTable.Add(new Route(HttpMethodType.Get, "/users/login", Login));
            routeTable.Add(new Route(HttpMethodType.Post, "/users/login", DoLogin));
            routeTable.Add(new Route(HttpMethodType.Get, "/contact", Contact));
            routeTable.Add(new Route(HttpMethodType.Get, "/favicon.ico", FavIcon));



            var httpServer = new HttpServer(80, routeTable);
            await httpServer.StartAsync();
        }

        private static HttpResponse FavIcon(HttpRequest request)
        {
            throw new NotImplementedException();
        }

        public static HttpResponse Index(HttpRequest request)
        {
            return new HtmlResponse("<h1> this is home page </h1>");
        }

        public static HttpResponse Contact(HttpRequest request)
        {
            return new HtmlResponse("<h1> this is contact page </h1>");
        }

        public static HttpResponse Login(HttpRequest request)
        {
            return new HtmlResponse("<h1> this is login page </h1>");
        }
        
        public static HttpResponse DoLogin(HttpRequest request)
        {
            return new HtmlResponse("<h1> this is doLogin page </h1>");
        }
    }

    
}
