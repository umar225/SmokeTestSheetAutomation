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
    public interface IJobServiceFacade
    {
        IAwsServiceFacade AWSServiceFacade { get; }
        IJobRepository JobRepository { get; }
        
        IArtifactService ArtifactService { get; }
        ICustomerService _customerService { get; }
        IJobSelectionFacade JobSelectionFacade { get; }
    }

    public class JobServiceFacade: IJobServiceFacade
    {
        public JobServiceFacade(
            IJobRepository jobRepository,
            IJobSelectionFacade jobSelectionFacade,
            IArtifactService artifactService,
            ICustomerService customerService,
            IAwsServiceFacade awsServiceFacade
            )
        {
            JobRepository = jobRepository;
            ArtifactService = artifactService;
            _customerService = customerService;
            AWSServiceFacade = awsServiceFacade;
            JobSelectionFacade = jobSelectionFacade;
        }

        public IAwsServiceFacade AWSServiceFacade { get; private set; }
        public IJobRepository JobRepository { get; private set; }

        
        public IArtifactService ArtifactService { get; private set; }
        public ICustomerService _customerService { get; private set; }
        public IJobSelectionFacade JobSelectionFacade { get; private set; }
    }

    public interface IJobSelectionFacade
    {
        ISkillService SkillService { get; }
        ILocationService LocationService { get; }
        IIndustryService IndustryService { get; }
        ITitleService TitleService { get; }
    }
    public class JobSelectionFacade : IJobSelectionFacade
    {
        public JobSelectionFacade(ISkillService skillService,
            ILocationService locationService,
            IIndustryService industryService,
            ITitleService titleService
            )
        {
            SkillService = skillService;
            LocationService = locationService;
            IndustryService = industryService;
            TitleService = titleService;

        }
        public ISkillService SkillService { get; private set; }

        public ILocationService LocationService { get; private set; }

        public IIndustryService IndustryService { get; private set; }
        public ITitleService TitleService { get; private set; }
    }
}
