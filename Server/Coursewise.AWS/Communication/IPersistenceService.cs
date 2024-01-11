using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.AWS.Communication
{
    public interface IPersistenceService
    {
        /// <summary>
        /// To read file from persistence source
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<Stream> ReadFile(string fileName);

        /// <summary>
        /// To write file on persistence source
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
        Task<object> WriteFile(string uploadAs, Stream fileStream, IDictionary<string, object>? parameters = null);

        
        /// <summary>
        /// Get a PreSignedURL for PUTing objects into S3
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        string GetUploadPreSignedUrl(string keyName);

        /// <summary>
        /// To delete file from persistence source.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<bool> DeleteFile(string fileName, bool keyFolder = false);

        /// <summary>
        /// To change ACL (Access Control List)
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="parameters">
        /// <para>S3 IDictionary { string, object } (StringComparer.CurrentCultureIgnoreCase)</para>
        /// <para>Supported keys:</para>
        /// <para>cannedACL: S3CannedACL,
        ///     accessControlList: S3AccessControlList </para>
        /// </param>
        /// <returns></returns>
        Task<bool> ChangeACL(string keyName, IDictionary<string, object>? parameters = null);

        /// <summary>
        /// To find bucket/container on persistence source
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="parameters">
        /// <para>S3 IDictionary { string, object } (StringComparer.CurrentCultureIgnoreCase)</para>
        /// <para>Supported keys:</para>
        /// <para>    client: IAmazonS3</para>
        /// </param>
        /// <returns></returns>
        Task<string> FindBucketLocation(string bucketName, IDictionary<string, object>? parameters = null);

        /// <summary>
        /// To create bucket/container on persistence source
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="parameters">
        /// <para>S3 IDictionary { string, object } (StringComparer.CurrentCultureIgnoreCase)</para>
        /// <para>Supported keys:</para>
        /// <para>    regionEndpoint: RegionEndpoint,
        ///     filePermission: S3CannedACL</para>
        /// </param>
        /// <returns></returns>
        Task<string> CreateBucket(string? bucketName = null, IDictionary<string, object>? parameters = null);
    }
}
