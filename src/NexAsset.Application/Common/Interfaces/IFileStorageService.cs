namespace NexAsset.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken);
    Task<(Stream Content, string ContentType, string FileName)?> GetAsync(string fileReference, CancellationToken cancellationToken);
    Task DeleteAsync(string fileReference, CancellationToken cancellationToken);
}
