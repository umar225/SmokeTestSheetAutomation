using Coursewise.Domain.Models;
using Coursewise.Test.Mock;
using Coursewise.Test.Mock.Services;
using Coursewise.Test.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Test.UnitTests
{
    public class FeedbackServiceUnitTests
    {
        private readonly IFeedbackServiceMock _feedbackServiceMock;
        private readonly ICourseServiceMock _courseServiceMock;
        private readonly string _WantToAchieveQuestionId;
        private readonly string _LevelOfTrainingQuestionId;

        public FeedbackServiceUnitTests()
        {
            _feedbackServiceMock = GetSetupMocks.Instance.FeedbackServiceMock;
            _courseServiceMock = GetSetupMocks.Instance.CourseServiceMock;
            _WantToAchieveQuestionId = "1";
            _LevelOfTrainingQuestionId = "2";
        }

        [Test]
        public async Task Feedback_Get_PMP_Courses_Successfull()
        {
            // Arrange
            var model = new FeedbackModel() { WantToAchieveQuestionId = _WantToAchieveQuestionId };
            var courseType = "Project Management Qualifications";
            var feedbackService = _feedbackServiceMock.SetupSuccessfullFeedbackServiceMock(model);
            var responseDto = SetupResponseDto(_WantToAchieveQuestionId, courseType, null,null);
            // Act
            var result =  feedbackService.GetQuestionType(responseDto);

            // Assert
            Assert.AreSame(courseType, result.Type);
            var courseService = _courseServiceMock.SetupSuccessfullCourseServiceMock(new CourseModel() { Type = courseType });
            var pmpCourses = await courseService.GetPmpCourses();
            Assert.That(pmpCourses, Is.Not.Empty);
            Assert.That(pmpCourses[0].Type, Is.EqualTo(courseType));            
        }

        [Test]
        public async Task Feedback_Get_Coaching_Director_Courses_Successfull()
        {
            // Arrange
            var level = "Director +";
            var courseType = "Coaching Qualifications";
            var model = new FeedbackModel() { WantToAchieveQuestionId = _WantToAchieveQuestionId, LevelOfTrainingQuestionId = _LevelOfTrainingQuestionId };
            var feedbackService = _feedbackServiceMock.SetupSuccessfullFeedbackServiceMock(model);
            var responseDto = SetupResponseDto(_WantToAchieveQuestionId, courseType, level, _LevelOfTrainingQuestionId);
            // Act
            var result = feedbackService.GetQuestionType(responseDto);

            // Assert
            Assert.AreSame(courseType, result.Type);
            var courseService = _courseServiceMock.SetupSuccessfullCourseServiceMock(new CourseModel() { Type=courseType,Level=level});
            var baseModel = await courseService.GetCoachingCoursesByLevel(level);
            Assert.IsTrue(baseModel.success);
            var coachingCourses = baseModel.data as List<CourseDto>;
            Assert.That(coachingCourses, Is.Not.Empty);
            Assert.That(coachingCourses![0].Type, Is.EqualTo(courseType));
            Assert.That(coachingCourses[0].Level, Is.EqualTo(level));
        }

        [Test]
        public async Task Feedback_Get_Leadership_Supervisor_Courses_Successfull()
        {
            // Arrange
            var level = "Supervisor";
            var courseType = "Leadership and Management Qualifications";
            var model = new FeedbackModel() { WantToAchieveQuestionId = _WantToAchieveQuestionId, LevelOfTrainingQuestionId = _LevelOfTrainingQuestionId };
            var feedbackService = _feedbackServiceMock.SetupSuccessfullFeedbackServiceMock(model);
            var responseDto = SetupResponseDto(_WantToAchieveQuestionId, courseType, level, _LevelOfTrainingQuestionId);
            // Act
            var result = feedbackService.GetQuestionType(responseDto);

            // Assert
            Assert.AreSame(courseType, result.Type);
            var courseService = _courseServiceMock.SetupSuccessfullCourseServiceMock(new CourseModel() { Type = courseType, Level = level });
            var baseModel = await courseService.GetLeadershipCoursesByLevel(level);
            Assert.IsTrue(baseModel.success);
            var coachingCourses = baseModel.data as List<CourseDto>;
            Assert.That(coachingCourses, Is.Not.Empty);
            Assert.That(coachingCourses![0].Type, Is.EqualTo(courseType));
            Assert.That(coachingCourses[0].Level, Is.EqualTo(level));
        }

        private static ResponseDto SetupResponseDto(string questionRef,string courseType,string? level,string? levelQuestionRef)
        {
            var responseDto = new ResponseDto()
            {
                Items = new List<Item>()
                {
                    new Item()
                    {
                        Answers = new List<Answer>() {
                                    AddAnswer(courseType, questionRef)

                        }

                    }
                }
            };
            if (!string.IsNullOrEmpty(level) && !string.IsNullOrEmpty(levelQuestionRef))
            {
                responseDto.Items.FirstOrDefault()!.Answers.Add(AddAnswer(level, levelQuestionRef));
            }
            return responseDto;
        }

        private static Answer AddAnswer(string label, string questionRef)
        {
            var answer = new Answer()
            {
                Choice = new Choice()
                {
                    Label = label
                },
                Field = new Field()
                {
                    Ref = questionRef
                }
            };
            return answer;
        }

        
    }
}
