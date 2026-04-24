namespace Domain.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveAsync(Stream stream, string fileName, string folder);
        void Delete(string path);
    }
}
