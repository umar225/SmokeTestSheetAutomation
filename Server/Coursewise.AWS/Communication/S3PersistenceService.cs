using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Coursewise.AWS.Communication.Models;
using Coursewise.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.AWS.Communication
{
    public class S3PersistenceService: IPersistenceService
    {

        private readonly AwsSettings _awsSettings;
        private readonly ICoursewiseLogger<S3PersistenceService> _logger;

        public S3PersistenceService(IOptionsSnapshot<AwsSettings> awsSettings,
            ICoursewiseLogger<S3PersistenceService> logger
)
        {
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", awsSettings.Value.AWSAccessKeyID);
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", awsSettings.Value.AWSSecretAccessKey);
            Environment.SetEnvironmentVariable("AWS_REGION", awsSettings.Value.AWSRegion);
            _awsSettings = awsSettings.Value;
            
            _logger = logger;
        }


        /// <inheritdoc />
        /// <summary>
        /// Generate a PreSignedURL for PUTing objects into S3
        /// </summary>
        /// <param name="keyName">KeyName is storage location(path/filename)</param>
        /// <returns></returns>
        public string GetUploadPreSignedUrl(string keyName)
        {
            try
            {
                var expires = DateTime.Now.ToUniversalTime().AddDays(_awsSettings.PreSignedUrlExpirationInDays);
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _awsSettings.BucketName,
                    Key = keyName,
                    Expires = expires,
                    Verb = HttpVerb.GET
                };
                string? url = null;
                var client=GetS3Client();
                url = client.GetPreSignedURL(request);
                return url;
            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
                throw;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// To read file from a S3 location
        /// </summary>
        /// <param name="fileName">KeyName is storage location(path/filename)</param>
        /// <returns></returns>
        public async Task<Stream> ReadFile(string fileName)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = _awsSettings.BucketName,
                    Key = _awsSettings.KeyFolder + "/" + fileName,
                };
                var client = GetS3Client();

                var result = await client.GetObjectAsync(request);

                return result.ResponseStream;
            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
                throw;
            }

        }

        /// <inheritdoc />
        /// <summary>
        /// To write a file from Amazon S3
        /// </summary>
        /// <param name="uploadAs"></param>
        /// <param name="fileStream"></param>
        /// <param name="parameters">
        /// <para>S3 IDictionary { string, object } (StringComparer.CurrentCultureIgnoreCase)</para> 
        /// <para>Supported keys:</para>
        /// <para>bucketName:String,
        ///     regionEndpoint:RegionEndpoint,
        ///     filePermission:S3CannedACL,
        ///     storageType:S3StorageClass,
        ///     keyFolder:String </para>
        /// </param>
        /// <returns>Artifact as an object</returns>
        /// 
        private static void getS3SettingsToWriteFile(out string? bucketName, out S3CannedACL? filePermission,out S3StorageClass? storageType,out RegionEndpoint? regionEndpoint, out string? keyFolder, IDictionary<string, object>? parameters = null)
        {
             bucketName = null;
             filePermission = null;
             storageType = null;
             regionEndpoint = null;
             keyFolder = null;
            if (parameters != null && parameters.Count > 0 && parameters.ContainsKey("bucketName"))
            {
                bucketName = parameters["bucketName"].ToString();
            }
            if (parameters != null && parameters.Count > 0 && parameters.ContainsKey("filePermission"))
            {
                filePermission = (S3CannedACL)parameters["filePermission"];
            }
            if (parameters != null && parameters.Count > 0 && parameters.ContainsKey("storageType"))
            {
                storageType = (S3StorageClass)parameters["storageType"];
            }
            if (parameters != null && parameters.Count > 0 && parameters.ContainsKey("regionEndpoint"))
            {
                regionEndpoint = RegionEndpoint.EUWest2;
            }
            if (parameters != null && parameters.Count > 0 && parameters.ContainsKey("keyFolder"))
            {
                keyFolder = parameters["keyFolder"].ToString();
            }

        }
        public async Task<object> WriteFile(string uploadAs, Stream fileStream, IDictionary<string, object>? parameters = null)
        {
            
            try
            {

                getS3SettingsToWriteFile(out string? bucketName, out S3CannedACL? filePermission, out S3StorageClass? storageType, out RegionEndpoint? regionEndpoint, out string? keyFolder, parameters);
                IAmazonS3 client = GetS3Client();

                //Check if bucket is specified otherwise use default.
                if (String.IsNullOrEmpty(bucketName))
                    bucketName = _awsSettings.BucketName;


                if (storageType == null)
                    storageType = S3StorageClass.Standard;
                if (filePermission == null)
                    filePermission = S3CannedACL.PublicRead;
                var artifact = new Artifact();

                string key = keyFolder ?? _awsSettings.KeyFolder;
                key += "/" + uploadAs;


                if (fileStream.Length <= _awsSettings.SinglepartUploadSize)
                    await UploadSinglepart(fileStream, bucketName, client, filePermission, storageType, key);
                else
                    await UploadMultipart(fileStream, bucketName, client, key);


                artifact.StorageLocation = key;
                if (filePermission == S3CannedACL.PublicRead || filePermission == S3CannedACL.NoACL ||
                    filePermission == S3CannedACL.PublicReadWrite)
                    artifact.Url = _awsSettings.S3Path + bucketName + "/" + key;
                else
                    artifact.Url = GetUploadPreSignedUrl(key);

                artifact.FileType = uploadAs.Substring(uploadAs.LastIndexOf('.'));

                return artifact;
            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
                throw;
            }

        }

        
        private AmazonS3Client GetS3Client()
        {
            BasicAWSCredentials credentials = new BasicAWSCredentials(_awsSettings.S3AccessKey, _awsSettings.S3SecretKey);

            // Create a new Amazon S3 client
            AmazonS3Client s3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.EUWest2);

            return s3Client;
        }
        /// <inheritdoc />
        /// <summary>
        /// To delete a file from S3 location
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteFile(string fileName,bool keyFolder=false)
        {
            try
            {
                var fileToDelete = (keyFolder)?$"{_awsSettings.KeyFolder}/{fileName}" : fileName;
                var request = new DeleteObjectRequest { BucketName = _awsSettings.BucketName, Key = fileToDelete };
                var client = GetS3Client();
                var result = await client.DeleteObjectAsync(request);
                if (result.HttpStatusCode == System.Net.HttpStatusCode.OK || result.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                    return true;
                else
                    return false;
            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
                throw;
            }

        }

        /// <inheritdoc />
        /// <summary>
        /// To change Access control list (permissions)
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="parameters">
        /// <para>S3 IDictionary { string, object } (StringComparer.CurrentCultureIgnoreCase)</para>
        /// <para>Supported keys:</para>
        /// <para>cannedACL: S3CannedACL,
        ///     accessControlList: S3AccessControlList </para>
        /// </param>
        /// <returns></returns>

        public async Task<bool> ChangeACL(string keyName, IDictionary<string, object>? parameters = null)
        {
            try
            {
                if (parameters != null)
                {
                    S3CannedACL? cannedACL = null;
                    S3AccessControlList? accessControlList = null;

                    if (parameters.ContainsKey("cannedACL"))
                        cannedACL = (S3CannedACL)parameters["cannedACL"];

                    if (parameters.ContainsKey("accessControlList"))
                        accessControlList = (S3AccessControlList)parameters["accessControlList"];


                    var request = new PutACLRequest
                    {
                        BucketName = _awsSettings.BucketName,
                        Key = _awsSettings.KeyFolder + "/" + keyName,
                        CannedACL = cannedACL,
                        AccessControlList = accessControlList
                    };
                    var client = GetS3Client();
                    var result = await client.PutACLAsync(request);
                    if (result.HttpStatusCode == System.Net.HttpStatusCode.OK || result.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                        return true;
                    else
                        return false;
                }

                return false;
            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
                throw;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// To find a bucket location from AmazonS3
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="parameters">
        /// <para>S3 IDictionary { string, object } (StringComparer.CurrentCultureIgnoreCase)</para>
        /// <para>Supported keys:</para>
        /// <para>    client: IAmazonS3</para>
        /// </param>
        /// <returns></returns>

        public async Task<string> FindBucketLocation(string bucketName, IDictionary<string, object>? parameters = null)
        {
            try
            {
                IAmazonS3? client = null;
                if (parameters != null && parameters.ContainsKey("client"))
                {
                    client = (IAmazonS3)parameters["client"];
                }

                if (client == null)
                {
                    client = GetS3Client();
                }
                if (string.IsNullOrWhiteSpace(bucketName))
                {
                    bucketName = _awsSettings.BucketName;
                }
                var request = new GetBucketLocationRequest
                {
                    BucketName = bucketName
                };
                GetBucketLocationResponse response = await client.GetBucketLocationAsync(request);
                return response.Location.ToString();
            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
                throw;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// To create a bucket on Amazon
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="parameters">
        /// <para>S3 IDictionary { string, object } (StringComparer.CurrentCultureIgnoreCase)</para>
        /// <para>Supported keys:</para>
        /// <para>    regionEndpoint: RegionEndpoint,
        ///     filePermission: S3CannedACL</para>
        /// </param>
        /// <returns></returns>

        public async Task<string> CreateBucket(string? bucketName = null, IDictionary<string, object>? parameters = null)
        {

            RegionEndpoint? regionEndpoint = null;
            S3CannedACL? bucketPermission = null;

            try
            {

                if (parameters != null && parameters.ContainsKey("regionEndpoint"))
                {
                    regionEndpoint = (RegionEndpoint)parameters["regionEndpoint"];

                }
                if (parameters != null && parameters.ContainsKey("filePermission"))
                {
                    bucketPermission = (S3CannedACL)parameters["filePermission"];

                }
                
                IAmazonS3 client = GetS3Client();


                string bucketLocation;


                if (string.IsNullOrWhiteSpace(bucketName))
                {
                    bucketName = _awsSettings.BucketName;
                }
                var isBucketExistResult = await AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName);

                if (!isBucketExistResult)
                {
                    if (bucketPermission is not null)
                    {
                        await createBucket(bucketName, client, bucketPermission);
                    }
                    else
                    {
                        await createBucket(bucketName, client);
                    }
                }
                // Retrieve bucket location.
                bucketLocation = await FindBucketLocation(bucketName);

                return bucketLocation;

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    _logger.Error("Invalied AWS Credentials in CreateBucket method.");
                    throw;
                }
                _logger.Error("Error occurred. Message:" + amazonS3Exception.Message + "when writing an object");
                throw;

            }
            catch (Exception exception)
            {
                _logger.Exception(exception);
                throw;
            }

        }

        #region Private Methods

        private static async Task createBucket(string bucketName, IAmazonS3 awsClient, S3CannedACL? bucketPermission = null)
        {
            var putRequest = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true,
            };
            if (bucketPermission != null)
            {
                putRequest.CannedACL = bucketPermission;
            }

            await awsClient.PutBucketAsync(putRequest);
        }

#pragma warning disable CA1822 // Mark members as static
        private async Task UploadSinglepart(Stream fileStream, string bucketName, IAmazonS3 client, S3CannedACL filePermission, S3StorageClass storageType, string key)
#pragma warning restore CA1822 // Mark members as static
        {

            var request = new PutObjectRequest
            {
                Key = key,
                InputStream = fileStream,
                BucketName = bucketName,
                CannedACL = filePermission,
                StorageClass = storageType
            };

            await client.PutObjectAsync(request);
        }

        private async Task UploadMultipart(Stream fileStream, string bucketName, IAmazonS3 client, string keyName)
        {
            List<PartETag> partETags = new List<PartETag>();

            // List to store upload part responses.
            List<UploadPartResponse> uploadResponses = new List<UploadPartResponse>();

            // 1. Initialize.
            InitiateMultipartUploadRequest initiateRequest = new InitiateMultipartUploadRequest
            {
                BucketName = bucketName,
                Key = keyName
            };

            InitiateMultipartUploadResponse initResponse = await client.InitiateMultipartUploadAsync(initiateRequest);

            // 2. Upload Parts.
            long contentLength = fileStream.Length;

            long partSize = _awsSettings.S3MultipartUploadSize;

            try
            {
                long filePosition = 0;
                for (int i = 1; filePosition < contentLength; i++)
                {

                    // Create request to upload a part.
                    UploadPartRequest uploadRequest = new UploadPartRequest
                    {
                        BucketName = bucketName,
                        Key = keyName,
                        UploadId = initResponse.UploadId,
                        PartNumber = i,
                        PartSize = partSize,
                        InputStream = fileStream
                    };

                    // Upload part and add response to our list.

                    var uploadedResponse = await client.UploadPartAsync(uploadRequest);
                    uploadResponses.Add(uploadedResponse);

                    PartETag petag = new PartETag(uploadedResponse.PartNumber, uploadedResponse.ETag);

                    partETags.Add(petag);

                    filePosition += partSize;
                }

                // Step 3: complete.
                CompleteMultipartUploadRequest completeRequest = new CompleteMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    UploadId = initResponse.UploadId,
                    PartETags = partETags
                };

                await client.CompleteMultipartUploadAsync(completeRequest);

            }
            catch (Exception exception)
            {
                AbortMultipartUploadRequest abortMPURequest = new AbortMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    UploadId = initResponse.UploadId
                };
                await client.AbortMultipartUploadAsync(abortMPURequest);
                _logger.Exception(exception);
                throw;
            }
        }


        #endregion

    }
}
