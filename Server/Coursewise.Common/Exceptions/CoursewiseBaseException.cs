using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Common.Exceptions
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class CoursewiseBaseException : Exception
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public CoursewiseBaseException(string message) : base(message)
        {

        }
        private CoursewiseBaseException()
        {

        }
    }
}
