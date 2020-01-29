﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIS.HTTP
{
    public class HttpRequest
    {
        public HttpRequest(string httpRequestAsString)
        {
            this.Headers = new List<Header>();

            var lines = httpRequestAsString.Split(new string[] { GlobalConstants.NewLine}, StringSplitOptions.None);

            var httpInfoHeader = lines[0];

            var infoHeaderParts = httpInfoHeader.Split(' ');

            if (infoHeaderParts.Length != 3)
            {
                throw new HttpServerException("Invalid http header line.");
            }

            var httpMethod = infoHeaderParts[0];

            this.HttpMethod = httpMethod switch
            {
                "POST" => HttpMethodType.Post,
                "GET" => HttpMethodType.Get,
                "PUT" => HttpMethodType.Put,
                "DELETE" => HttpMethodType.Delete,
                _ => HttpMethodType.Unknown
            };

            this.Path = infoHeaderParts[1];
            var httpVersion = infoHeaderParts[2];
            this.HttpVersion = httpVersion switch
            {
                "HTTP/1.0" => HttpVersionType.Http10,
                "HTTP/1.1" => HttpVersionType.Http11,
                "HTTP/2.0" => HttpVersionType.Http20,
                _ => HttpVersionType.Http11
            };

            bool isInHeader = true;

            var bodyBuilder = new StringBuilder();

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    isInHeader = false;
                    continue;
                }
                if (isInHeader)
                {
                    var headerParts = line.Split(new string[] { ": " },2 ,StringSplitOptions.None);

                    if (headerParts.Length != 2)
                    {
                        throw new HttpServerException($"Invalid Header: {line}");
                    }

                    var header = new Header(headerParts[0], headerParts[1]);

                    this.Headers.Add(header);
                }
                else
                {
                    bodyBuilder.AppendLine(line);
                }
            }
        }
        public HttpMethodType HttpMethod { get; set; }

        public string Path { get; set; }

        public HttpVersionType HttpVersion { get; set; }

        public IList<Header> Headers { get; set; }

        public string Body { get; set; }

    }
}
