using Newtonsoft.Json;
using System.Text;

namespace Coursewise.Typeform.Extensions
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