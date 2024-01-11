using Coursewise.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Jobs
{
    public interface IJobServiceContainer<T> where T : class
    {
        ICoursewiseLogger<T> Logger { get; set; }
        void WriteRow(string workerName, string operation);
        void WriteErrorRow(string workerName, string operation);
    }
    public class JobServiceContainer<T> : IJobServiceContainer<T> where T : class
    {
        private readonly ICoursewiseLogger<T> _logger;

        public ICoursewiseLogger<T> Logger { get; set; }
        public JobServiceContainer(ICoursewiseLogger<T> logger
            )
        {
            _logger = logger;
            Logger = _logger;            
        }
        public void WriteRow(string workerName, string operation)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("-----------------------------------------------------------");
            string result = $"|| {workerName} || {operation} ||";
            Console.WriteLine(result);
            Console.WriteLine("-----------------------------------------------------------");
            _logger.Info(result);
        }

        public void WriteErrorRow(string workerName, string operation)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("-----------------------------------------------------------");
            string result = $"|| {workerName} || {operation} ||";
            Console.WriteLine(result);
            Console.WriteLine("-----------------------------------------------------------");
            _logger.Error(result);
        }
    }
}
