using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Common
{
    public class AwsS3Helper
    {
        String AWSUniqueDbKey = ConfigurationData.AWSUniqueDbKey;
        String AWSAccessKey = ConfigurationData.AWSAccessKey;
        String AWSSecrteKey = ConfigurationData.AWSSecrteKey;
        static AmazonS3Client s3Client;
        public AwsS3Helper()
        {
            AmazonS3Config config = new AmazonS3Config();
            config.RegionEndpoint = RegionEndpoint.USWest1;
            s3Client = new AmazonS3Client(
                       AWSAccessKey,
                       AWSSecrteKey,
                       config
                       );
        }

        #region S3Bucket
        public PutBucketResponse CreateBucket(String bucketName)
        {
            PutBucketRequest request = new PutBucketRequest();
            request.BucketName = AWSUniqueDbKey + bucketName;
            return s3Client.PutBucket(request);
        }

        public ListBucketsResponse GetBuckets()
        {
            ListBucketsResponse response = s3Client.ListBuckets();
            return response;
        }

        public bool IsExistBucket(String BucketName)
        {
            bool isBucketExist = false;
            ListBucketsResponse response = GetBuckets();
            foreach (S3Bucket bucket in response.Buckets)
            {
                if (bucket.BucketName == AWSUniqueDbKey + BucketName)
                {
                    isBucketExist = true;
                    break;
                }
            }
            return isBucketExist;
        }

        #endregion S3Bucket

        #region S3Object
        public PutObjectResponse CreateObject(String FileKey, String BucketName, String FileType, Stream inputStream)
        {
            //Stream stream = new MemoryStream(inputBytes);
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = AWSUniqueDbKey + BucketName;
            request.Key = FileKey;
            request.InputStream = inputStream;
            request.CannedACL = S3CannedACL.Private;
            return s3Client.PutObject(request);
        }

        public GetObjectResponse DownloadObject(String FileKey, String BucketName)
        {
            GetObjectRequest request = new GetObjectRequest();
            request.BucketName = AWSUniqueDbKey + BucketName;
            request.Key = FileKey;
            return s3Client.GetObject(request);
        }

        public DeleteObjectResponse DeleteObject(String FileKey, String BucketName)
        {
            DeleteObjectRequest request = new DeleteObjectRequest();
            request.BucketName = AWSUniqueDbKey + BucketName;
            request.Key = FileKey;
            return s3Client.DeleteObject(request);

          
        }

        #endregion S3Object

    }

}
