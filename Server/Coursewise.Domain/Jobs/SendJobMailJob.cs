using Coursewise.Data;
using Coursewise.Domain.Interfaces;
using Coursewise.Logging;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Jobs
{
    public class SendJobMailJob : IJob
    {
        private readonly IJobEmailService _jobEmailService;
        private readonly IJobServiceContainer<SendJobMailJob> _serviceContainer;
        public SendJobMailJob(IJobEmailService jobEmailService,
            IJobServiceContainer<SendJobMailJob> serviceContainer)
        {
            _jobEmailService = jobEmailService;
            _serviceContainer = serviceContainer;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var workerName = GetType().Name;

                _serviceContainer.WriteRow(workerName, $"Job notification service started at {DateTime.Now:dd/MM/yyyy - HH:mm}");
                var response = await _jobEmailService.SendEmailToCustomers();
                if (!response.success)
                {
                    _serviceContainer.WriteErrorRow(workerName, response.message);
                }
                _serviceContainer.WriteRow(workerName, $"Job notification service completed at {DateTime.Now:dd/MM/yyyy - HH:mm}");
            }
            catch (Exception ex)
            {
                _serviceContainer.Logger.Exception(ex);
            }
        }
    }
}
