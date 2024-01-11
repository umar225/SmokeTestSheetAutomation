namespace Coursewise.Typeform.Models
{
    public class TypeformConfig
    {
        public string Token { get; set; }
        public string FormId { get; set; }
        public Questions Questions { get; set; }
    }

    public class Questions
    {
        public string WantToAchieveQuestionId { get; set; }
        public string LevelOfTrainingQuestionId { get; set; }
    }
}