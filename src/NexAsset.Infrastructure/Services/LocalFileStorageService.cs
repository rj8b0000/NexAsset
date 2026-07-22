using NexAsset.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace NexAsset.Infrastructure.Services;

public sealed class LocalFileStorageService : IFileStorageService
{
    private readonly string _rootPath;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _rootPath = configuration["FileStorage:RootPath"]
            ?? Path.Combine(AppContext.BaseDirectory, "uploads");
    }

    public async Task<string> SaveAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(Path.GetFileName(fileName));
        var relativePath = Path.Combine(DateTime.UtcNow.ToString("yyyy"), DateTime.UtcNow.ToString("MM"), $"{Guid.NewGuid():N}{extension}");
        var absolutePath = Path.Combine(_rootPath, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);
        await using var target = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, useAsync: true);
        await content.CopyToAsync(target, cancellationToken);
        return relativePath.Replace(Path.DirectorySeparatorChar, '/');
    }

    public Task<Stream?> GetAsync(string fileReference, CancellationToken cancellationToken)
    {
        var file = ResolvePath(fileReference);
        Stream? stream = File.Exists(file)
            ? new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, useAsync: true)
            : null;
        return Task.FromResult(stream);
    }

    public Task DeleteAsync(string fileReference, CancellationToken cancellationToken)
    {
        var file = ResolvePath(fileReference);
        if (File.Exists(file)) File.Delete(file);
        return Task.CompletedTask;
    }

    private string ResolvePath(string fileReference)
    {
        var relative = fileReference.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);
        var resolved = Path.GetFullPath(Path.Combine(_rootPath, relative));
        var root = Path.GetFullPath(_rootPath) + Path.DirectorySeparatorChar;
        if (!resolved.StartsWith(root, StringComparison.Ordinal)) throw new InvalidOperationException("Invalid file reference.");
        return resolved;
    }
}
