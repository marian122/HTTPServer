﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SIS.HTTP
{
    public class HttpServer : IHttpServer
    {

        private readonly TcpListener tcpListener;
        private readonly IList<Route> routeTable;


        public HttpServer(int port, IList<Route> routeTable)
        {
            this.tcpListener = new TcpListener(IPAddress.Loopback, port);
            this.routeTable = routeTable;
        }

        public IList<Route> RouteTable { get; }

        public async Task ResetAsync()
        {
            this.Stop();
            await this.StartAsync();
        }

        public async Task StartAsync()
        {
            this.tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                await Task.Run(() => ProcessClientAsync(tcpClient));
            }
        }

        public void Stop()
        {
            this.tcpListener.Stop();
        }

        private async Task ProcessClientAsync(TcpClient tcpClient)
        {
            using NetworkStream networkStream = tcpClient.GetStream();
            try
            {
                byte[] requestBytes = new byte[1000000]; //TODO: Use buffer
                int bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
                string requestAsString = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);

                var request = new HttpRequest(requestAsString);

                Console.WriteLine($"{request.HttpMethod} {request.Path}");

                var route = this.routeTable.FirstOrDefault(x => x.HttpMethod == request.HttpMethod && x.Path == request.Path);

                HttpResponse response;

                if (route == null)
                {  
                    response = new HttpResponse(HttpStatusCode.NotFound, new byte[0]);
                }
                else
                {
                    response = route.Action(request);
                }

                response.Headers.Add(new Header("Server", "HomeOfficeServer/1.1"));

                response.Cookies.Add(new ResponseCookie("sid", Guid.NewGuid().ToString())
                { HttpOnly = true, MaxAge = 3600 });

                byte[] responseBytes = Encoding.UTF8.GetBytes(response.ToString());
                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                await networkStream.WriteAsync(response.Body, 0, response.Body.Length);

                Console.WriteLine(request);
                Console.WriteLine(new string('-', 60));
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponse(
                    HttpStatusCode.InternalServerError, 
                    Encoding.UTF8.GetBytes(ex.ToString()));

                errorResponse.Headers.Add(new Header("Content-Type", "text/plain"));
                byte[] responseBytes = Encoding.UTF8.GetBytes(errorResponse.ToString());

                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                await networkStream.WriteAsync(errorResponse.Body, 0, errorResponse.Body.Length);

            }

        }
    }
}
