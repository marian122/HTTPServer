using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SIS.HTTP
{
    public class HttpServer : IHttpServer
    {

        private readonly TcpListener tcpListener;

        //TODO: actions
        public HttpServer(int port)
        {
            this.tcpListener = new TcpListener(IPAddress.Loopback, port);

        }

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

                var content = "<h1>this is random page</h1>";

                if (request.Path == "/")
                {
                    content = "<h1>this is home page</h1>";
                }
                else if (request.Path == "/users/login")
                {
                    content = "<h1>this is login page</h1>";
                }

                byte[] fileContent = Encoding.UTF8.GetBytes(content);

                var response = new HttpResponse(HttpStatusCode.Ok, fileContent);

                response.Headers.Add(new Header("Server", "HomeOfficeServer/1.1"));
                response.Headers.Add(new Header("Content-Type", "text/html"));

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


            //using NetworkStream networkStream = tcpClient.GetStream();
            //try
            //{
            //    byte[] requestBytes = new byte[1000000]; // TODO: Use buffer
            //    int bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
            //    string requestAsString = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);

            //    var request = new HttpRequest(requestAsString);
            //    string newSessionId = null;
            //    var sessionCookie = request.Cookies.FirstOrDefault(x => x.Name == HttpConstants.SessionIdCookieName);
            //    if (sessionCookie != null && this.sessions.ContainsKey(sessionCookie.Value))
            //    {
            //        request.SessionData = this.sessions[sessionCookie.Value];
            //    }
            //    else
            //    {
            //        newSessionId = Guid.NewGuid().ToString();
            //        var dictionary = new Dictionary<string, string>();
            //        this.sessions.Add(newSessionId, dictionary);
            //        request.SessionData = dictionary;
            //    }

            //    Console.WriteLine($"{request.Method} {request.Path}");

            //    var route = this.routeTable.FirstOrDefault(
            //        x => x.HttpMethod == request.Method && x.Path == request.Path);
            //    HttpResponse response;
            //    if (route == null)
            //    {
            //        response = new HttpResponse(HttpResponseCode.NotFound, new byte[0]);
            //    }
            //    else
            //    {
            //        response = route.Action(request);
            //    }

            //    response.Headers.Add(new Header("Server", "SoftUniServer/1.0"));

            //    if (newSessionId != null)
            //    {
            //        response.Cookies.Add(
            //            new ResponseCookie(HttpConstants.SessionIdCookieName, newSessionId)
            //            { HttpOnly = true, MaxAge = 30 * 3600, });
            //    }

            //    byte[] responseBytes = Encoding.UTF8.GetBytes(response.ToString());
            //    await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            //    await networkStream.WriteAsync(response.Body, 0, response.Body.Length);
            //}
            //catch (Exception ex)
            //{
            //    var errorResponse = new HttpResponse(
            //        HttpResponseCode.InternalServerError,
            //        Encoding.UTF8.GetBytes(ex.ToString()));
            //    errorResponse.Headers.Add(new Header("Content-Type", "text/plain"));
            //    byte[] responseBytes = Encoding.UTF8.GetBytes(errorResponse.ToString());
            //    await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            //    await networkStream.WriteAsync(errorResponse.Body, 0, errorResponse.Body.Length);
            //}
        }
    }
}
