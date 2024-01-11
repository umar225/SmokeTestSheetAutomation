using Coursewise.AWS.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Services.Facade
{
    public interface IAwsServiceFacade
    {
        IPersistenceService PersistenceService { get; }
        IEmailService EmailService { get; }
    }
    public class AwsServiceFacade : IAwsServiceFacade
    {
        public IPersistenceService PersistenceService { get; private set; }

        public IEmailService EmailService { get; private set; }
        public AwsServiceFacade(
            IPersistenceService persistenceService,
            IEmailService emailService)           
        {
            PersistenceService = persistenceService;
            EmailService = emailService;
        }
    }

}
