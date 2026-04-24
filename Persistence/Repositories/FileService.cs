
using Domain.Interfaces;

namespace Persistence.Repositories
{
    public class FileService : IFileService
    {
        public void Delete(string path)
        {
            try
            {
                var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    path.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
                );

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch
            {
                // optional: log
            }
        }

        public async Task<string> SaveAsync(Stream stream, string fileName, string folder)
        {
            var extension = Path.GetExtension(fileName);
            var newFileName = $"{Guid.NewGuid()}{extension}";

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                folder
            );

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fullPath = Path.Combine(path, newFileName);

            using var fileStream = new FileStream(fullPath, FileMode.Create);
            await stream.CopyToAsync(fileStream);

            return $"/images/{folder}/{newFileName}";
        }
    }
}
