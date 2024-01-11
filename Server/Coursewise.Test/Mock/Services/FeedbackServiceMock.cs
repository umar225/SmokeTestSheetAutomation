using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Services;
using Coursewise.Logging;
using Coursewise.Test.Models;
using Coursewise.Typeform.Models;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Test.Mock.Services
{
   
    public interface IFeedbackServiceMock
    {
        FeedbackService SetupSuccessfullFeedbackServiceMock(FeedbackModel feedbackModel);
    }
    public class FeedbackServiceMock : IFeedbackServiceMock
    {
        private readonly IAllMocks _allMocks;
        private readonly Mock<IFeedbackService> _feedbackServiceMock;
        private readonly Mock<ICoursewiseLogger<FeedbackService>> _loggerFeedbackService;
        private readonly Mock<IOptionsSnapshot<TypeformConfig>> _mockTypeformConfigOptionSanpshot;

        public FeedbackServiceMock()
        {
            _allMocks = AllMocksGetter.AllMocks;
            _feedbackServiceMock = _allMocks.MockFeedbackService;
            _loggerFeedbackService= _allMocks.MockFeebackServiceLogger;
            _mockTypeformConfigOptionSanpshot= _allMocks.MockTypeformConfigSnapshot;
        }

        public FeedbackService SetupSuccessfullFeedbackServiceMock(FeedbackModel feedbackModel)
        {
            return GetFeedbackService(feedbackModel);
        }

        private FeedbackService GetFeedbackService(FeedbackModel feedbackModel)
        {
            _mockTypeformConfigOptionSanpshot.Setup(r => r.Value).Returns(() => new TypeformConfig()
            {
                Questions = new Questions() { WantToAchieveQuestionId = feedbackModel.WantToAchieveQuestionId, LevelOfTrainingQuestionId = feedbackModel.LevelOfTrainingQuestionId }
            });

            return new FeedbackService( _mockTypeformConfigOptionSanpshot.Object);

        }
    }
}
