using Coursewise.Test.Mock.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Test.Mock
{
    public interface ISetupMocks
    {
        CourseServiceMock CourseServiceMock { get; }
        FeedbackServiceMock FeedbackServiceMock { get; }
        ApplyJobServiceMock ApplyJobServiceMock { get; }
        ResourceServiceMock ResourceServiceMock { get; }
    }
    internal class SetupMocks : ISetupMocks
    {
        public CourseServiceMock CourseServiceMock => new CourseServiceMock();

        public FeedbackServiceMock FeedbackServiceMock => new FeedbackServiceMock();
        public ApplyJobServiceMock ApplyJobServiceMock => new ApplyJobServiceMock();
        public ResourceServiceMock ResourceServiceMock => new ResourceServiceMock();
    }

    public sealed class GetSetupMocks
    {
        private GetSetupMocks()
        {
        }
        private static readonly Lazy<ISetupMocks> lazy = new Lazy<ISetupMocks>(() => new SetupMocks());
        public static ISetupMocks Instance
        {
            get
            {
                return lazy.Value;
            }
        }
    }
}
