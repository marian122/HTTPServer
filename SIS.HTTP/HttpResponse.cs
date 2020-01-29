using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP
{
    public class HttpResponse
    {
        public HttpVersionType Version { get; set; }

        public HttpStatusCode StatusCode { get; set; }


    }
}
