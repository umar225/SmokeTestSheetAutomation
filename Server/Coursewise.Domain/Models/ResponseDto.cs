using Newtonsoft.Json;

namespace Coursewise.Domain.Models
{
    public class ResponseDto
    {
        [JsonProperty("total_items")]
        public int TotalItems { get; set; }

        [JsonProperty("page_count")]
        public int PageCount { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("user_agent")]
        public string UserAgent { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("referer")]
        public string Referer { get; set; }

        [JsonProperty("network_id")]
        public string NetworkId { get; set; }

        [JsonProperty("browser")]
        public string Browser { get; set; }
    }

    public class Field
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }
    }

    public class Choices
    {
        [JsonProperty("labels")]
        public List<string> Labels { get; set; }
    }

    public class Choice
    {
        [JsonProperty("label")]
        public string Label { get; set; }
    }

    public class Answer
    {
        [JsonProperty("field")]
        public Field Field { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("boolean")]
        public bool? Boolean { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("number")]
        public int? Number { get; set; }

        [JsonProperty("choices")]
        public Choices Choices { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("choice")]
        public Choice Choice { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("file_url")]
        public string FileUrl { get; set; }
    }


    public class Calculated
    {
        [JsonProperty("score")]
        public int Score { get; set; }
    }

    public class Variable
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Item
    {
        [JsonProperty("landing_id")]
        public string LandingId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("landed_at")]
        public DateTime LandedAt { get; set; }

        [JsonProperty("submitted_at")]
        public DateTime SubmittedAt { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("answers")]
        public List<Answer> Answers { get; set; }

        

        [JsonProperty("calculated")]
        public Calculated Calculated { get; set; }

        [JsonProperty("variables")]
        public List<Variable> Variables { get; set; }
    }
}