using AutoMapper;
using Coursewise.Data.Generics;
using Coursewise.Data.Interfaces;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Interfaces.External;
using Coursewise.Domain.Services;
using Coursewise.Domain.Services.Facade;
using Coursewise.Logging;
using Coursewise.Typeform.Models;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Test.Mock
{

    public static class AllMocksGetter
    {
        public static IAllMocks AllMocks => new AllMocks();
    }
    public interface IAllMocks
    {
        #region Repo
        Mock<ICourseRepository> MockCourseRepository { get; }
        Mock<IApplyJobRepository> MockApplyJobRepository { get; }
        Mock<IResourceRepository> MockResourceRepository { get; }
        Mock<IUnitOfWork> MockUnitOfWork { get; }
        #endregion
        #region Services
        Mock<IPaymentService> MockPaymentService { get; }
        Mock<IFeedbackService> MockFeedbackService { get; }
        Mock<ICourseService> MockCourseService { get; }
        Mock<ICategoryService> MockCategoryService { get; }
        Mock<ILevelService> MockLevelService { get; }
        Mock<IWordpressService> MockWordpressService { get; }
        Mock<IArtifactService> MockArtifactService { get; }
        Mock<IJobService> MockJobService { get; }
        Mock<IEmailTemplateService> MockEmailTemplateService { get; }        
        Mock<IMapper> MockMapper { get; }
        Mock<ICoursewiseLogger<FeedbackService>> MockFeebackServiceLogger { get; }
        Mock<ICoursewiseLogger<CourseService>> MockCourseServiceLogger { get; }
        Mock<ICoursewiseLogger<ApplyJobService>> MockApplyJobServiceLogger { get; }
        Mock<ICoursewiseLogger<ResourceService>> MockResourceServiceLogger { get; }
        #endregion
        #region Facade
        Mock<IUtilityFacade> MockUtilityFacade { get; }
        Mock<IApplyJobServiceFacade> MockApplyJobServiceFacade { get; }
        Mock<IResourceServiceFacade> MockResourceServiceFacade { get; }
        #endregion


        Mock<IOptionsSnapshot<TypeformConfig>> MockTypeformConfigSnapshot { get; }
    }
    internal class AllMocks : IAllMocks
    {
        public AllMocks()
        {

        }
        #region Repo
        public Mock<ICourseRepository> MockCourseRepository  => new Mock<ICourseRepository>();
        public Mock<IApplyJobRepository> MockApplyJobRepository => new Mock<IApplyJobRepository>();
        public Mock<IResourceRepository> MockResourceRepository => new Mock<IResourceRepository>();
        public Mock<IUnitOfWork> MockUnitOfWork => new Mock<IUnitOfWork>();
        #endregion
        #region Services
        public Mock<IPaymentService> MockPaymentService => new Mock<IPaymentService>();

        public Mock<IFeedbackService> MockFeedbackService => new Mock<IFeedbackService>();

        public Mock<ICourseService> MockCourseService => new Mock<ICourseService>();
        public Mock<ICategoryService> MockCategoryService => new Mock<ICategoryService>();
        public Mock<ILevelService> MockLevelService => new Mock<ILevelService>();
        public Mock<IWordpressService> MockWordpressService => new Mock<IWordpressService>();
        public Mock<IArtifactService> MockArtifactService => new Mock<IArtifactService>();
        public Mock<IJobService> MockJobService => new Mock<IJobService>();
        public Mock<IEmailTemplateService> MockEmailTemplateService => new Mock<IEmailTemplateService>();
        public Mock<IMapper> MockMapper => new Mock<IMapper>();

        public Mock<ICoursewiseLogger<FeedbackService>> MockFeebackServiceLogger => new Mock<ICoursewiseLogger<FeedbackService>>();
        public Mock<ICoursewiseLogger<CourseService>> MockCourseServiceLogger => new Mock<ICoursewiseLogger<CourseService>>();
        public Mock<ICoursewiseLogger<ApplyJobService>> MockApplyJobServiceLogger => new Mock<ICoursewiseLogger<ApplyJobService>>();
        public Mock<ICoursewiseLogger<ResourceService>> MockResourceServiceLogger => new Mock<ICoursewiseLogger<ResourceService>>();
        public Mock<IOptionsSnapshot<TypeformConfig>> MockTypeformConfigSnapshot => new Mock<IOptionsSnapshot<TypeformConfig>>();
        #endregion
        #region Facade
        public Mock<IUtilityFacade> MockUtilityFacade => new Mock<IUtilityFacade>();
        public Mock<IApplyJobServiceFacade> MockApplyJobServiceFacade => new Mock<IApplyJobServiceFacade>();
        public Mock<IResourceServiceFacade> MockResourceServiceFacade => new Mock<IResourceServiceFacade>();
        #endregion
    }
}
