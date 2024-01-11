using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.AWS
{
    public  class AwsSettings
    {
        public string AWSAccessKeyID { get; set; }
        public string AWSSecretAccessKey { get; set; }
        public string AWSRegion { get; set; }
        public string FromEmail { get; set; }
        public string S3AccessKey { get; set; }
        public string S3SecretKey { get; set; }

        
            
        public string BucketName { get; set; }
        public int PreSignedUrlExpirationInDays { get; set; }
        public string KeyFolder { get; set; }
        public long SinglepartUploadSize { get; set; }
        public string S3Path { get; set; }
        public long S3MultipartUploadSize { get; set; }
    }
}
