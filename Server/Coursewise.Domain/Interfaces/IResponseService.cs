using Coursewise.Domain.Models;

namespace Coursewise.Domain.Interfaces
{
    public interface IResponseService
    {
        Task<ResponseDto> GetResponseById(string responseId);
    }
}