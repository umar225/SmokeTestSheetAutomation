using Coursewise.Common.Models;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Models;
using Coursewise.Logging;
using Coursewise.Typeform.Models;
using Microsoft.Extensions.Options;

namespace Coursewise.Domain.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly TypeformConfig _typeformConfig;

        public FeedbackService(IOptionsSnapshot<TypeformConfig> typeformOptions)
        {
            _typeformConfig = typeformOptions.Value;            
        }

        private static bool isProjectManagementQualifications(Answer? wantToAchieveAnswer)
        {
            if (wantToAchieveAnswer != null && wantToAchieveAnswer.Choice.Label == "Project Management Qualifications")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private static bool isNEDQualifications(Answer? wantToAchieveAnswer)
        {
            if (wantToAchieveAnswer != null && wantToAchieveAnswer.Choice.Label == "NED Training ")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private static bool isLeadershipManagementQualifications(Answer? wantToAchieveAnswer, Answer? levelOfTrainingAnswer)
        {
            if (wantToAchieveAnswer != null && levelOfTrainingAnswer != null && wantToAchieveAnswer.Choice.Label == "Leadership and Management Qualifications")
            {
                    return true;
                

            }

            return false;


        }
        private static bool isCoachingQualifications(Answer? wantToAchieveAnswer,Answer? levelOfTrainingAnswer)
        {
            if (wantToAchieveAnswer != null&&levelOfTrainingAnswer!=null && wantToAchieveAnswer.Choice.Label == "Coaching Qualifications")
            {
               
                    return true;
                
                
            }
           
                return false;
            

        }
        public CourseQuestionTypeResult GetQuestionType(ResponseDto response)
        {
           
                foreach (var item in response.Items.Select(i => i.Answers))
                {
                var levelOfTrainingAnswer = item.Find(a => a.Field.Ref == _typeformConfig.Questions.LevelOfTrainingQuestionId);

                var wantToAchieveAnswer = item.Find(a => a.Field.Ref == _typeformConfig.Questions.WantToAchieveQuestionId);
                        if (isProjectManagementQualifications(wantToAchieveAnswer))
                        {
                            return new CourseQuestionTypeResult
                            {
                                Type = "Project Management Qualifications"
                            };
                        }
                        if (levelOfTrainingAnswer!=null&&isCoachingQualifications(wantToAchieveAnswer, levelOfTrainingAnswer))
                        {
                            
                                        return new CourseQuestionTypeResult
                                        {
                                            Type = "Coaching Qualifications",
                                            Level = levelOfTrainingAnswer.Choice.Label

                                        };
                            
                        }
                        if (levelOfTrainingAnswer != null && isLeadershipManagementQualifications(wantToAchieveAnswer, levelOfTrainingAnswer))
                        {

                            return new CourseQuestionTypeResult
                            {
                                Type = "Leadership and Management Qualifications",
                                Level = levelOfTrainingAnswer.Choice.Label
                            };


                        }
                
                        if (isNEDQualifications(wantToAchieveAnswer))
                        {
                            return new CourseQuestionTypeResult
                            {
                                Type = "NED Training"
                            };
                        }
                    



                }
            
            return new CourseQuestionTypeResult();
        }
    }
}