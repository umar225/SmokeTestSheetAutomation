﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Common.Utilities
{
    public static class HttpRequestMessageBuilder
    {
        public static HttpRequestMessage Build(Uri uri, HttpMethod httpMethod, object? requestObject,
            Dictionary<string, string>? headers, string contentType = "application/json")
        {
            var request = new HttpRequestMessage(httpMethod, uri);
            request.Headers.Add("Accept", contentType);
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            if (requestObject != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, contentType);
            }

            return request;
        }
    }
}
