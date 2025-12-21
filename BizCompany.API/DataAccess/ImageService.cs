namespace BizCompany.API.DataAccess
{
    public class ImageService(IHostEnvironment _env) : IImageService
    {
        // Save Image method
        public async Task<string> SaveImageAsync(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null or empty.", nameof(file));

            var extension = Path.GetExtension(file.FileName);
            var newFileName = Guid.NewGuid() + extension;

            var absolutePath = Path.Combine(_env.ContentRootPath, "wwwroot", "images", subFolder, newFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);

            using (var stream = new FileStream(absolutePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("/images", subFolder, newFileName).Replace("\\", "/");
        }

        // Delete Image method
        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return false;

            // Normalize path
            var relativePath = imageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
            var fullPath = Path.Combine(_env.ContentRootPath, "wwwroot", relativePath);

            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
                return true;
            }

            return false;
        }
    }

}
