using Nimbus.Business.Models;
using Nimbus.Persistance.Data;

namespace Nimbus.Business.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<IEnumerable<FileDescriptor>>> GetFileListAsync(Guid folderId)
        {
            var result = await _unitOfWork.GetFileList(folderId);

            return new Result<IEnumerable<FileDescriptor>>
            {
                Value = result.Select(FileDescriptor.FromEntity)
            };
        }
    }

}
