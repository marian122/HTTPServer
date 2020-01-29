using SIS.HTTP;
using System;
using System.Threading.Tasks;

namespace DemoApp
{
    public class Program
    {
        static async Task Main()
        {
            var httpServer = new HttpServer(80);
            await httpServer.StartAsync();
        }


    }
}
