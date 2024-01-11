using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models
{
    public class BoardwiseUserDto
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("nicename")]
        public string Nicename { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
    public class BoardwiseUserWpResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public BoardwiseUserDto? User { get; set; }
    }
}
