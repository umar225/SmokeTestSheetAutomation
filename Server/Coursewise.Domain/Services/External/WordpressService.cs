using Coursewise.Common.Models;
using Coursewise.Common.Utilities;
using Coursewise.Domain.Interfaces.External;
using Coursewise.Domain.Models;
using Coursewise.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services.External
{
    public class WordpressService: IWordpressService
    {
        private readonly string _boardwiseWPUrl;
        private readonly ICoursewiseLogger<WordpressService> _logger;

        public WordpressService(
            IConfiguration configuration,
            ICoursewiseLogger<WordpressService> logger)
        {
            _boardwiseWPUrl = configuration["Nova:BoardwiseWPUrl"];
            _logger = logger;
        }

        public async Task<BaseModel> GetUserInfo(string token)
        {
            using (var httpClient = new HttpClient())
            {
                Uri uri = new Uri($"{_boardwiseWPUrl}/wp-json/wp/v2/users/me?context=edit");
                var headers = new Dictionary<string, string>();
                headers.Add("Authorization", $"Bearer {token}");
                var request = HttpRequestMessageBuilder.Build(uri, HttpMethod.Get, null, headers);
                var response = await httpClient.SendAsync(request);
                var isResponseRight = await IsResponseRight(response, "GetUserInfo");
                if (!isResponseRight.success)
                {
                    return isResponseRight;
                }

                var boardwiseUserWpResponse = JsonConvert.DeserializeObject<BoardwiseWordpressUser>(isResponseRight.message);
                return BaseModel.Success(new {user= boardwiseUserWpResponse });

            }
        }

        public async Task <BaseModel> IsPaidUser(string token)
        {
            return await Task.FromResult(BaseModel.Success());
              
        }

        public async Task<(BaseModel,List<BoardwiseWordpressUserBasic>)> GetAllUsers(string token,int page,int perPage=100, string context="edit")
        {
            using (var httpClient = new HttpClient())
            {
                var list = new List<BoardwiseWordpressUserBasic>();
                Uri uri = new Uri($"{_boardwiseWPUrl}/wp-json/wp/v2/users?context={context}&per_page={perPage}&page={page}");
                var headers = new Dictionary<string, string>();
                headers.Add("Authorization", $"Bearer {token}");
                var request = HttpRequestMessageBuilder.Build(uri, HttpMethod.Get, null, headers);
                var response = await httpClient.SendAsync(request);
                var isResponseRight = await IsUserResponseRight(response, "GetAllUsers");
                if (!isResponseRight.success)
                {
                    return (isResponseRight, list);
                }

                list = JsonConvert.DeserializeObject<List<BoardwiseWordpressUserBasic>>(isResponseRight.message);
                return (BaseModel.Success(), list!);
            }
        }

        public async Task<(BaseModel, List<BoardwiseWordpressUserBasic>)> GetAllUsers(string token)
        {
            var list = new List<BoardwiseWordpressUserBasic>();
            bool getMoreUser = true;
            int page = 1;
            while (getMoreUser)
            {
                var response = await GetAllUsers(token,page);
                if (!response.Item1.success)
                {
                    return (BaseModel.Error(response.Item1.message), list);
                }
                if (response.Item2.Any())
                {
                    list.AddRange(response.Item2);
                }
                else
                {
                    getMoreUser = false;
                }
                page++;
            }
            return (BaseModel.Success(), list);
        }

        public async Task<BaseModel> IsResponseRight(HttpResponseMessage response,string methodName,string? message=null)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.Error($"{methodName} is not working due to wordpress server error");
                return BaseModel.Error($"{(string.IsNullOrEmpty(message)? "There is some issue on our end":message)}");
            }

            var responseInString = await response.Content.ReadAsStringAsync();
            var boardwiseUserWpResponse = JsonConvert.DeserializeObject<BoardwiseWordpressBaseModel>(responseInString);
            if (boardwiseUserWpResponse is not null && boardwiseUserWpResponse.statusCode>0 && !boardwiseUserWpResponse.success)
            {
                _logger.Error($"Token validity: statusCode {boardwiseUserWpResponse.statusCode} - message {boardwiseUserWpResponse.message} - code {boardwiseUserWpResponse.code}");
                return BaseModel.Error(boardwiseUserWpResponse.message);
            }
            return BaseModel.Success(responseInString);
        }
        public async Task<BaseModel> IsUserResponseRight(HttpResponseMessage response, string methodName, string? message = null)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                _logger.Error($"{methodName} is not working due to wordpress server error");
                return BaseModel.Error($"{(string.IsNullOrEmpty(message) ? "There is some issue on our end" : message)}");
            }

            var responseInString = await response.Content.ReadAsStringAsync();            
            return BaseModel.Success(responseInString);
        }
    }
}
