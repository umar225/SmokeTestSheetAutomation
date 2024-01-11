using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Logging
{
    public interface ICoursewiseLogger<LoggerType>
    {
        void Debug(string message, params object[] args);
        void Error(string message, params object[] args);
        void Critical(string message, params object[] args);
        void Info(string message, params object[] args);
        void Info(string message, Dictionary<string, object> args);
        void Trace(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Exception(Exception exception);
    }
}
