using Microsoft.Extensions.Options;
using Nimbus.Business.Common;
using Nimbus.Business.Models;
using Nimbus.Persistance.Data;

namespace Nimbus.Business.Services
{
    public class FileService(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings) : IFileService
    {
        private readonly AppSettings _appSettings = appSettings.Value;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<IEnumerable<FileDescriptor>>> GetFileListAsync(Guid? folderId, CancellationToken cancellationToken)
        {
            var folderIdValue = GetFolderIdOrDefault(folderId);
            var result = await _unitOfWork.GetFileList(folderIdValue, cancellationToken);

            return new Result<IEnumerable<FileDescriptor>>
            {
                Value = result.Select(FileDescriptor.FromEntity)
            };
        }

        public async Task SaveFileAsync(FileUploadRequest request, CancellationToken cancellationToken)
        {
            var folderIdValue = GetFolderIdOrDefault(request.FolderId);

            using var memoryStream = new MemoryStream();
            await request.File.CopyToAsync(memoryStream, cancellationToken);

            var fileEntity = new Persistance.Entities.File
            {
                Id = Guid.NewGuid(),
                FileName = request.FileName ?? request.File.FileName,
                FileContent = memoryStream.ToArray(),
                MimeType = request.File.ContentType,
                FileSize = request.File.Length,
                ParentFolderId = folderIdValue,
                CreatedDateTime = DateTime.UtcNow
            };


            await _unitOfWork.PersistFileAsync(fileEntity, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<FileResponse?> GetFileByIdAsync(Guid fileId, CancellationToken cancellationToken)
        {
            var fileEntity = await _unitOfWork.GetFileByIdAsync(fileId, cancellationToken);

            if (fileEntity == null)
            {
                return null;
            }

            return new FileResponse
            {
                Id = fileEntity.Id,
                FileName = fileEntity.FileName,
                ContentType = fileEntity.MimeType,
                FileSize = fileEntity.FileSize ?? 0,
                Base64Content = fileEntity.FileContent == null ? string.Empty : Convert.ToBase64String(fileEntity.FileContent)
            };
        }
        private Guid GetFolderIdOrDefault(Guid? folderId)
        {
            // if a folder Id is not provided, we default to the root folder Id (from the configuration)
            return folderId.GetValueOrDefault(Guid.Empty) == Guid.Empty || folderId == null ? _appSettings.RootFolderId : folderId.Value;
        }
    }

}
