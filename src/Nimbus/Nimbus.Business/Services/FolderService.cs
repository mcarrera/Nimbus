using Microsoft.Extensions.Options;
using Nimbus.Business.Common;
using Nimbus.Business.Models;
using Nimbus.Persistance.Data;

namespace Nimbus.Business.Services
{

    public class FolderService(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings) : IFolderService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly AppSettings _appSettings = appSettings.Value;

        public async Task CreateFolderAsync(CreateFolderRequest request, CancellationToken cancellationToken)
        {
            var folder = request.ToEntity();

            await _unitOfWork.AddFolderAsync(folder, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task SoftDeleteFolderAsync(Guid folderId, CancellationToken cancellationToken)
        {
            var folder = await _unitOfWork.GetFileByIdAsync(folderId, cancellationToken);

            if (folder?.IsFolder ?? false)
            {
                // delete all the children recursively 
                await SoftDeleteChildFilesAndSubfoldersAsync(folderId, cancellationToken);

                // do not delete the root folder!
                if (folderId != _appSettings.RootFolderId)
                {
                    folder.DeletedDateTime = DateTime.UtcNow;
                }
                await _unitOfWork.CommitAsync(cancellationToken);
            }
        }

        private async Task SoftDeleteChildFilesAndSubfoldersAsync(Guid parentFolderId, CancellationToken cancellationToken)
        {
            var childrens = await _unitOfWork.GetFileList(parentFolderId, cancellationToken);

            foreach (var child in childrens)
            {
                child.DeletedDateTime = DateTime.UtcNow;

                // recursively delete its children
                if (child.IsFolder)
                {
                    await SoftDeleteChildFilesAndSubfoldersAsync(child.Id, cancellationToken);
                }
            }
        }
    }
}