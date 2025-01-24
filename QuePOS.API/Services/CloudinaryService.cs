using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace QuePOS.API.Services
{
    public class CloudinaryService
    {
        private IConfiguration _configuration;
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            _configuration = configuration;
            var account = new Account(
                configuration["CloudinarySettings:CloudName"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
        }
        public async Task<ImageUploadResult> UploadImageAsync(string base64string, string fileName)
        {
            var uploadResult = new ImageUploadResult();

            // Validate input
            if (string.IsNullOrEmpty(base64string))
                throw new ArgumentException("Base64 string cannot be null or empty.", nameof(base64string));

            try
            {
                // Decode Base64 string to a byte array
                byte[] fileBytes = Convert.FromBase64String(base64string);

                if (fileBytes.Length > 0)
                {
                    // Create a memory stream from the byte array
                    using var stream = new MemoryStream(fileBytes);

                    // Set up Cloudinary upload parameters
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(fileName, stream),
                    };

                    // Upload the image to Cloudinary
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Invalid Base64 string format.", ex);
            }

            return uploadResult;
        }
    }
}
