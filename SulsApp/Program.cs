using SIS.HTTP;
using SulsApp.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SulsApp
{
    public static class Program
    {
        public static async Task Main()
        {
            var db = new ApplicationDbContext();

            db.Database.EnsureCreated();

            var routeTable = new List<Route>();

            routeTable.Add(new Route(HttpMethodType.Get, "/", new HomeController().Index));

            var httpServer = new HttpServer(80, routeTable);
            await httpServer.StartAsync();
        }
    }
}
