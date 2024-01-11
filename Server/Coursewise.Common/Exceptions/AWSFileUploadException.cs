using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Common.Exceptions
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class AwsFileUploadException : CoursewiseBaseException
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public AwsFileUploadException() : base("File could not be uploaded.")
        {

        }
        public AwsFileUploadException(string message) : base(message)
        {
        }
    }
}
