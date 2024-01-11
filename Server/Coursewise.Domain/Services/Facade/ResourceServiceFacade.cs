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
    public interface IResourceServiceFacade
    {
        IResourceRepository ResourceRepository { get; }
        IArtifactService ArtifactService { get; }
        ICustomerService _customerService { get; }
    }
    public class ResourceServiceFacade : IResourceServiceFacade
    {
        public IResourceRepository ResourceRepository { get; private set; }
        public IArtifactService ArtifactService { get; private set; }
        public ICustomerService _customerService { get; private set; }

        public ResourceServiceFacade(
            IResourceRepository resourceRepository,
            IArtifactService artifactService,
            ICustomerService customerService
            )
        {
            ResourceRepository = resourceRepository;
            ArtifactService = artifactService;
            _customerService = customerService;
        }
    }
}
