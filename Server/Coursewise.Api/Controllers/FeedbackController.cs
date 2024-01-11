using Coursewise.Common.Models;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Models;
using Coursewise.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Coursewise.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly ICoursewiseLogger<FeedbackController> _logger;
        private readonly IResponseService _responseService;
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(ICoursewiseLogger<FeedbackController> logger,
            IResponseService responseService,
            IFeedbackService feedbackService)
        {
            _logger = logger;
            _responseService = responseService;
            _feedbackService = feedbackService;
        }

        [HttpGet("{responseId}")]
        public async Task<IActionResult> GetResponse(string responseId)
        {
            var response = await _responseService.GetResponseById(responseId);
            if (response == null)
            {
                _logger.Info($"Empty response from typeform response id {responseId}");
                return NotFound();
            }
            if (response.PageCount == 0 && response.TotalItems == 0)
            {
                response = await GetResponseWithDelay(responseId);
            }
            var result = _feedbackService.GetQuestionType(response);
            return Ok(BaseModel.Success(result));
        }

        private async Task<ResponseDto> GetResponseWithDelay(string responseId)
        {
            await Task.Delay(2000);
            return await _responseService.GetResponseById(responseId);
        }
    }
}