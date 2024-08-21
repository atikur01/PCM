using CloudinaryDotNet.Actions; 
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace PCM.Services
{
    // This service class handles uploading and deleting images to and from Cloudinary, a cloud-based image and video management service.
    public class CloudinaryUploader
    {
        private readonly Cloudinary _cloudinary; // Private field to hold the Cloudinary instance

        // Constructor that initializes the Cloudinary instance using credentials from the configuration
        public CloudinaryUploader(IConfiguration configuration)
        {
            // Retrieving Cloudinary credentials from the configuration (typically stored in appsettings.json or environment variables)
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            // Creating an Account object using the retrieved credentials
            Account account = new Account(cloudName, apiKey, apiSecret);

            // Initializing the Cloudinary instance with the Account object
            _cloudinary = new Cloudinary(account);
        }

        // Method to upload an image file to Cloudinary asynchronously
        public async Task<string> UploadFileAsync(IFormFile formFile)
        {
            try
            {
                // Setting up the parameters for the image upload, including the file and a unique public ID
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(formFile.FileName, formFile.OpenReadStream()),
                    PublicId = Guid.NewGuid().ToString() // Generating a unique public ID for the image
                };

                // Uploading the image file to Cloudinary asynchronously
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                // Checking if the upload was successful by verifying the status code
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Returning the secure URL of the uploaded image
                    return uploadResult.SecureUrl.ToString();
                }

                // Returning null if the upload was not successful
                return null;
            }
            catch (Exception e)
            {
                // Logging the exception to the console in case of an error
                Console.WriteLine("Error encountered while uploading to Cloudinary. Message:'{0}'", e.Message);
                return null;
            }
        }

        // Method to remove an image from Cloudinary asynchronously based on its public ID
        public async Task<bool> RemoveImageAsync(string publicId)
        {
            try
            {
                // Setting up the parameters for deleting the image, using the public ID
                var deletionParams = new DeletionParams(publicId);

                // Deleting the image from Cloudinary asynchronously
                var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

                // Returning true if the deletion was successful (result is "ok"), otherwise false
                return deletionResult.Result == "ok";
            }
            catch (Exception e)
            {
                // Logging the exception to the console in case of an error
                Console.WriteLine("Error encountered while deleting from Cloudinary. Message:'{0}'", e.Message);
                return false;
            }
        }
    }
}
