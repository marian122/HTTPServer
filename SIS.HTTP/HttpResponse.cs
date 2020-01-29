using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP
{
    public class HttpResponse
    {
        public HttpResponse(HttpStatusCode statusCode, byte[] body)
        {
            this.Version = HttpVersionType.Http11;
            this.StatusCode = statusCode;
            this.Headers = new List<Header>();
            this.Body = body;
            this.Cookies = new List<ResponseCookie>();

            if (body?.Length > 0)
            {
                this.Headers.Add(new Header("Content-Length", body.Length.ToString()));
            }

        }
        public HttpVersionType Version { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public IList<Header> Headers { get; set; }

        public IList<ResponseCookie> Cookies { get; set; }

        public byte[] Body { get; set; }

        public override string ToString()
        {
            var responseAsString = new StringBuilder();
            var httpVersionAsString = this.Version switch
            {
                HttpVersionType.Http10 => "HTTP/1.0",
                HttpVersionType.Http11 => "HTTP/1.1",
                HttpVersionType.Http20 => "HTTP/2.0",
                _ => "HTTP/1.1"
            };

            responseAsString.Append($"{httpVersionAsString} {(int)this.StatusCode} {this.StatusCode}" + GlobalConstants.NewLine);


            foreach (var header in this.Headers)
            {
                responseAsString.Append(header.ToString() + GlobalConstants.NewLine);
            }

            foreach (var cookie in this.Cookies)
            {
                responseAsString.Append($"Set-Cookie: " + cookie.ToString() + GlobalConstants.NewLine);

            }

            responseAsString.Append(GlobalConstants.NewLine);

            return responseAsString.ToString();
        }

    }
}
