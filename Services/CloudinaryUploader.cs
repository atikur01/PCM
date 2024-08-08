using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace PCM.Services
{
    public class CloudinaryUploader
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryUploader(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            Account account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadFileAsync(IFormFile formFile)
        {
            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(formFile.FileName, formFile.OpenReadStream()),
                    PublicId = Guid.NewGuid().ToString()
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return uploadResult.SecureUrl.ToString();
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered while uploading to Cloudinary. Message:'{0}'", e.Message);
                return null;
            }
        }

        public async Task<bool> RemoveImageAsync(string publicId)
        {
            try
            {
                var deletionParams = new DeletionParams(publicId);
                var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

                return deletionResult.Result == "ok";
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered while deleting from Cloudinary. Message:'{0}'", e.Message);
                return false;
            }
        }

    }
}
