using NexAsset.Application.Common.Interfaces;

namespace NexAsset.Infrastructure.Services;

public sealed class LocalFileStorageService : IFileStorageService
{
    private readonly string _storageRoot;

    public LocalFileStorageService()
    {
        _storageRoot = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "projects");
        if (!Directory.Exists(_storageRoot))
        {
            Directory.CreateDirectory(_storageRoot);
        }
    }

    public async Task<string> SaveAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(fileName);
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var relativePath = Path.Combine("uploads", "projects", uniqueFileName);
        var fullPath = Path.Combine(_storageRoot, uniqueFileName);

        using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await content.CopyToAsync(fileStream, cancellationToken);

        return relativePath;
    }

    public Task<(Stream Content, string ContentType, string FileName)?> GetAsync(string fileReference, CancellationToken cancellationToken)
    {
        var fileName = Path.GetFileName(fileReference);
        var fullPath = Path.Combine(_storageRoot, fileName);

        if (!File.Exists(fullPath))
        {
            return Task.FromResult<(Stream Content, string ContentType, string FileName)?>(null);
        }

        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var contentType = GetContentType(fileName);

        return Task.FromResult<(Stream Content, string ContentType, string FileName)?>(
            (stream, contentType, fileName));
    }

    public Task DeleteAsync(string fileReference, CancellationToken cancellationToken)
    {
        var fileName = Path.GetFileName(fileReference);
        var fullPath = Path.Combine(_storageRoot, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    private static string GetContentType(string fileName) => Path.GetExtension(fileName).ToLower() switch
    {
        ".pdf" => "application/pdf",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        _ => "application/octet-stream"
    };
}
