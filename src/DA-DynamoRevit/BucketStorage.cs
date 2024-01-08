using Autodesk.Forge;
using Autodesk.Forge.Model;

namespace DA_DynamoRevit;

public class BucketStorage
{
    public BucketStorage()
    {
    }

    public dynamic CreateBucket(string token, string bucketName, string region = "US",PostBucketsPayload.PolicyKeyEnum Policy=PostBucketsPayload.PolicyKeyEnum.Transient)
    {
        BucketsApi bucketsApi = new BucketsApi();
        bucketsApi.Configuration.AccessToken = token;
        PostBucketsPayload postBuckets = new PostBucketsPayload(bucketName, null, Policy);
        dynamic bucket = bucketsApi.CreateBucket(postBuckets, region);
        return bucket;
    }

    public dynamic UploadFileToBucket(string token, string bucketKey, string filePath)
    {
        // check if bucket exists
        BucketsApi bucketsApi = new BucketsApi();
        bucketsApi.Configuration.AccessToken = token;
        dynamic buckets = bucketsApi.GetBuckets();
        bool bucketExist = false;
        foreach (KeyValuePair<string, dynamic> bucket in new DynamicDictionaryItems(buckets.items))
        {
            if (bucket.Value.bucketKey == bucketKey)
            {
                bucketExist = true;
                break;
            }
        }
        if (!bucketExist)
        {
            CreateBucket(token, bucketKey);
        }
        ObjectsApi objectsApi = new ObjectsApi();
        objectsApi.Configuration.AccessToken = token;
        dynamic file = objectsApi.UploadObject(bucketKey, Path.GetFileName(filePath),
            (int) new FileInfo(filePath).Length, new FileStream(filePath, FileMode.Open));
        return file;
    }
    public string GetFileSignedUrl(string token,string bucketKey,string fileName)
    {
        ObjectsApi objectsApi = new ObjectsApi();
        objectsApi.Configuration.AccessToken = token;
        dynamic signedUrl = objectsApi.CreateSignedResource(bucketKey, fileName, new PostBucketsSigned(10));
        return signedUrl.signedUrl;
    }
}