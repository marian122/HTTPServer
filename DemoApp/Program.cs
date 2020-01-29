using SIS.HTTP;
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
            var content = "<h1>this is home page</h1>";
            byte[] fileContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpStatusCode.Ok, fileContent);
            response.Headers.Add(new Header("Content-Type", "text/html"));

            return response;
        }

        public static HttpResponse Contact(HttpRequest request)
        {
            var content = "<h1>this is contact page</h1>";
            byte[] fileContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpStatusCode.Ok, fileContent);
            response.Headers.Add(new Header("Content-Type", "text/html"));

            return response;
        }

        public static HttpResponse Login(HttpRequest request)
        {
            var content = "<h1>this is login page</h1>";
            byte[] fileContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpStatusCode.Ok, fileContent);
            response.Headers.Add(new Header("Content-Type", "text/html"));

            return response;
        }
        
        public static HttpResponse DoLogin(HttpRequest request)
        {
            var content = "<h1>this is random page</h1>";
            byte[] fileContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpStatusCode.Ok, fileContent);

            return response;
        }
    }

    
}
