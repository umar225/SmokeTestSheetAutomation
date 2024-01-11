using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Models;
using Coursewise.Typeform.Extensions;
using Coursewise.Typeform.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Coursewise.Domain.Services
{
    public class ResponseService : IResponseService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly TypeformConfig _config;

        public ResponseService(IHttpClientFactory clientFactory, IOptionsSnapshot<TypeformConfig> options)
        {
            _clientFactory = clientFactory;
            _config = options.Value;
        }

        public async Task<ResponseDto> GetResponseById(string responseId)
        {
            Uri uri = new Uri($"https://api.typeform.com/forms/{_config.FormId}/responses?included_response_ids={responseId}");
            var client = _clientFactory.CreateClient();
            var headers = new Dictionary<string, string>();
            headers.Add("Authorization", $"Bearer {_config.Token}");
            var request = HttpRequestMessageBuilder.Build(uri, HttpMethod.Get, null, headers);
            var response = await client.SendAsync(request);
            var responseInString = await response.Content.ReadAsStringAsync();
            var formResponse = JsonConvert.DeserializeObject<ResponseDto>(responseInString)!;
            return formResponse;
        }
    }
}