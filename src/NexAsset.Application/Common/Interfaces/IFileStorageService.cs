namespace NexAsset.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken);
    Task<Stream?> GetAsync(string fileReference, CancellationToken cancellationToken);
    Task DeleteAsync(string fileReference, CancellationToken cancellationToken);
}
