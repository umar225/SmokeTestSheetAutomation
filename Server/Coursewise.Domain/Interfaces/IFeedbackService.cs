using Coursewise.Common.Models;
using Coursewise.Domain.Models;

namespace Coursewise.Domain.Interfaces
{
    public interface IFeedbackService
    {
        CourseQuestionTypeResult GetQuestionType(ResponseDto response);
    }
}