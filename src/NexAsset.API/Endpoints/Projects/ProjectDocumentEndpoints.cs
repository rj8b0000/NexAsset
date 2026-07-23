using MediatR;
using Microsoft.AspNetCore.Mvc;
using NexAsset.API.Authorization;
using NexAsset.Application.Common.Interfaces;
using NexAsset.Application.Features.ProjectDocuments.Commands.DeleteDocument;
using NexAsset.Application.Features.ProjectDocuments.Commands.ReplaceDocument;
using NexAsset.Application.Features.ProjectDocuments.Commands.UploadDocument;
using NexAsset.Application.Features.ProjectDocuments.Queries.GetDocuments;
using NexAsset.Application.Features.ProjectDocuments.Queries.GetDocumentVersionHistory;
using NexAsset.Domain.Enums;

namespace NexAsset.API.Endpoints.Projects;

public static class ProjectDocumentEndpoints
{
    public static IEndpointRouteBuilder MapProjectDocumentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects/{projectId:guid}/documents")
            .WithTags("Project Documents")
            .RequireAuthorization()
            .AddEndpointFilter<PermissionEnforcementFilter>();

        group.MapGet("/", async (Guid projectId, [FromQuery] DocumentCategory? category, [FromQuery] string? search, [FromQuery] int pageNumber, [FromQuery] int pageSize, ISender sender) =>
        {
            var query = new GetDocumentsQuery
            {
                ProjectId = projectId,
                Category = category,
                Search = search,
                PageNumber = pageNumber > 0 ? pageNumber : 1,
                PageSize = pageSize > 0 ? pageSize : 10
            };
            var result = await sender.Send(query);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapPost("/upload", async (Guid projectId, IFormFile file, [FromForm] DocumentCategory category, [FromForm] string? description, [FromForm] string? remarks, [FromForm] Guid? uploadedByEmployeeId, IFileStorageService storageService, ISender sender, CancellationToken cancellationToken) =>
        {
            if (file == null || file.Length == 0)
            {
                return Results.BadRequest("File content is required.");
            }

            using var stream = file.OpenReadStream();
            var fileRef = await storageService.SaveAsync(stream, file.FileName, file.ContentType, cancellationToken);

            var command = new UploadDocumentCommand(
                projectId,
                uploadedByEmployeeId,
                file.FileName,
                category,
                description,
                fileRef,
                remarks,
                null);

            var result = await sender.Send(command, cancellationToken);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Created($"/api/projects/{projectId}/documents/{result.Value!.Id}", result.Value);
        });

        group.MapGet("/{documentId:guid}/download", async (Guid projectId, Guid documentId, ISender sender, IFileStorageService storageService, CancellationToken cancellationToken) =>
        {
            var docResult = await sender.Send(new GetDocumentsQuery { ProjectId = projectId, PageSize = 100 }, cancellationToken);
            if (docResult.IsFailure) return Results.BadRequest(docResult.Error);

            var doc = docResult.Value!.Items.FirstOrDefault(x => x.Id == documentId);
            if (doc == null) return Results.NotFound("Document not found.");

            var fileData = await storageService.GetAsync(doc.FileReference, cancellationToken);
            if (fileData == null) return Results.NotFound("File storage reference not found.");

            return Results.File(fileData.Value.Content, fileData.Value.ContentType, fileData.Value.FileName);
        });

        group.MapPost("/{documentId:guid}/replace", async (Guid projectId, Guid documentId, IFormFile file, [FromForm] string? remarks, [FromForm] Guid? uploadedByEmployeeId, IFileStorageService storageService, ISender sender, CancellationToken cancellationToken) =>
        {
            if (file == null || file.Length == 0)
            {
                return Results.BadRequest("File content is required.");
            }

            using var stream = file.OpenReadStream();
            var fileRef = await storageService.SaveAsync(stream, file.FileName, file.ContentType, cancellationToken);

            var command = new ReplaceDocumentCommand(documentId, fileRef, remarks, uploadedByEmployeeId);
            var result = await sender.Send(command, cancellationToken);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        group.MapDelete("/{documentId:guid}", async (Guid projectId, Guid documentId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteDocumentCommand(documentId));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
        });

        group.MapGet("/history", async (Guid projectId, [FromQuery] string documentName, ISender sender) =>
        {
            var result = await sender.Send(new GetDocumentVersionHistoryQuery(projectId, documentName));
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
        });

        return app;
    }
}
