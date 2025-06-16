using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace FakeStore.Infrastructure.Storage
{
    public class AzureBlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName = "container1"; 

        public AzureBlobStorageService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<string> UploadImageAsync(byte[] imageBytes, string fileName)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);

            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = GetContentType(fileName)  // Definindo o tipo de conteúdo
            };

            using var stream = new MemoryStream(imageBytes);

            await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders
            });

            return blobClient.Uri.ToString();
        }
        public async Task DeleteImageAsync(string imageUrl)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            var fileName = Path.GetFileName(new Uri(imageUrl).LocalPath);
            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.DeleteIfExistsAsync();
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"  // Padrão caso seja outro tipo
            };
        }


    }
}
