using Microsoft.Extensions.Logging;
using System.Text;

namespace Coursewise.Logging
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
    public class GrayLoggerService<LoggerType> : ICoursewiseLogger<LoggerType>
    {
        private readonly ILogger<LoggerType> _logger;
        public GrayLoggerService(ILogger<LoggerType> logger)
        {

            _logger = logger;
        }
        public void Critical(string message, params object[] args)
        {
            message = message.Replace('\n', '_').Replace('\r', '_');

            _logger.LogCritical(message, args);
        }

        public void Debug(string message, params object[] args)
        {
            message = message.Replace('\n', '_').Replace('\r', '_');

            _logger.LogDebug(message, args);
        }

        public void Error(string message, params object[] args)
        {
            message = message.Replace('\n', '_').Replace('\r', '_');

            _logger.LogError(message, args);
        }

        public void Info(string message, params object[] args)
        {
            message = message.Replace('\n', '_').Replace('\r', '_');
            _logger.LogInformation(message, args);

        }
        public void Info(string message, Dictionary<string, object> args)
        {
            using (_logger.BeginScope(args))
            {
                message = message.Replace('\n', '_').Replace('\r', '_');
                _logger.LogInformation(message, args);
            }
        }
        public void Trace(string message, params object[] args)
        {
            message = message.Replace('\n', '_').Replace('\r', '_');
            _logger.LogTrace(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            message = message.Replace('\n', '_').Replace('\r', '_');
            _logger.LogWarning(message, args);
        }

        public void Exception(Exception exception)
        {
            var exceptionBuilder = new StringBuilder();
            if (exception.InnerException != null)
            {
                exceptionBuilder.Append($"Message {exception.InnerException.Message}");
                exceptionBuilder.AppendLine();
                exceptionBuilder.Append($"Stack trace {exception.InnerException.StackTrace}");
            }
            else
            {
                exceptionBuilder.Append($"Message {exception.Message}");
                exceptionBuilder.AppendLine();
                exceptionBuilder.Append($"Stack trace {exception.StackTrace}");
            }
            Error(exceptionBuilder.ToString());
        }
    }
}
