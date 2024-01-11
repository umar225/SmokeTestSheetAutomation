using Coursewise.Data.Interfaces;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Interfaces.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services.Facade
{
    public interface IApplyJobServiceFacade
    {
         IApplyJobRepository ApplyJobRepository { get; }
         IWordpressService WordpressService { get;  }
         IJobService JobService { get; }
         IArtifactService ArtifactService { get; }
         IEmailTemplateService EmailTemplateService { get; }
         ICustomerService CustomerService { get; }
    }
    public class ApplyJobServiceFacade: IApplyJobServiceFacade
    {
        public ApplyJobServiceFacade(
            IApplyJobRepository applyJobRepository,
            IWordpressService wordpressService,
            IJobService jobService,
            IArtifactService artifactService,
            IEmailTemplateService emailTemplateService,
            ICustomerService customerService)
        {
            ApplyJobRepository = applyJobRepository;
            WordpressService = wordpressService;
            JobService = jobService;
            ArtifactService = artifactService;
            EmailTemplateService = emailTemplateService;
            CustomerService = customerService;
        }

        public IApplyJobRepository ApplyJobRepository { get; private set; }
        public IWordpressService WordpressService { get; private set; }
        public IJobService JobService { get; private set; }
        public IArtifactService ArtifactService { get; private set; }
        public IEmailTemplateService EmailTemplateService { get; private set; }
        public ICustomerService CustomerService { get; private set; }
    }
}
