using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using Nimbus.Business.Common;
using Nimbus.Business.Models;
using Nimbus.Business.Services;
using Nimbus.Persistance.Data;

namespace Nimbus.Tests.Business;

public class FolderServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
    private readonly FolderService _folderService;
    private readonly Fixture _fixture;
    public FolderServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _appSettingsMock = new Mock<IOptions<AppSettings>>();
        _appSettingsMock.Setup(a => a.Value).Returns(new AppSettings { RootFolderId = Guid.NewGuid() });
        _folderService = new FolderService(_unitOfWorkMock.Object, _appSettingsMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CreateFolderAsync_ShouldCallAddFolderAndCommit()
    {
        // Arrange
        var folderName = "New Folder";
        var createFolderRequest = new CreateFolderRequest
        {
            Name = folderName,
            ParentFolderId = Guid.NewGuid(),
        };
        var folderEntity = createFolderRequest.ToEntity();

        // Act
        await _folderService.CreateFolderAsync(createFolderRequest, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.AddFolderAsync(It.Is<Persistance.Entities.File>(f => f.FileName == folderName), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SoftDeleteFolderAsync_ShouldSoftDeleteFolderAndChildren()
    {
        // Arrange
        var folderId = Guid.NewGuid();
        var folder = new Persistance.Entities.File
        {
            Id = folderId,
            IsFolder = true,
            FileName = "Folder to Delete"
        };

        _unitOfWorkMock.Setup(u => u.GetFileByIdAsync(folderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(folder);

        _unitOfWorkMock.Setup(u => u.GetFileList(folderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Persistance.Entities.File> { new Persistance.Entities.File { IsFolder = false, DeletedDateTime = null } });

        // Act
        await _folderService.SoftDeleteFolderAsync(folderId, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.GetFileByIdAsync(folderId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.GetFileList(folderId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        // Ensure no folder is added
        _unitOfWorkMock.Verify(u => u.AddFolderAsync(It.IsAny<Persistance.Entities.File>(), It.IsAny<CancellationToken>()), Times.Never);
        // Ensure the folder's deleted timestamp is set
        Assert.NotNull(folder.DeletedDateTime);
    }

    [Fact]
    public async Task SoftDeleteFolderAsync_ShouldNotDeleteRootFolder()
    {
        // Arrange
        var folderId = _appSettingsMock.Object.Value.RootFolderId;
        var rootFolder = new Persistance.Entities.File
        {
            Id = folderId,
            IsFolder = true,
            FileName = "Root Folder"
        };

        _unitOfWorkMock.Setup(u => u.GetFileByIdAsync(folderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rootFolder);

        _unitOfWorkMock.Setup(u => u.GetFileList(folderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Persistance.Entities.File>());

        // Act
        await _folderService.SoftDeleteFolderAsync(folderId, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Null(rootFolder.DeletedDateTime); // Root folder should not have a deleted timestamp
    }

    [Fact]
    public async Task GetFolderTreeAsync_ShouldReturnFolderTreeRecursively()
    {
        // Arrange
        var rootFolderId = Guid.NewGuid();
        var subFolderId = Guid.NewGuid();
        var childFileId = Guid.NewGuid();

        var rootFolderName = _fixture.Create<string>();
        var subFolderName = _fixture.Create<string>();
        var childFileName = _fixture.Create<string>();

        var rootFolder = new Persistance.Entities.File
        {
            Id = rootFolderId,
            IsFolder = true,
            FileName = rootFolderName,
            CreatedDateTime = DateTime.UtcNow
        };

        var subFolder = new Persistance.Entities.File
        {
            Id = subFolderId,
            ParentFolderId = rootFolderId,
            IsFolder = true,
            FileName = subFolderName,
            CreatedDateTime = DateTime.UtcNow
        };

        var childFile = new Persistance.Entities.File
        {
            Id = childFileId,
            ParentFolderId = subFolderId,
            IsFolder = false,
            FileName = childFileName,
            CreatedDateTime = DateTime.UtcNow
        };


        _unitOfWorkMock.Setup(u => u.GetFileByIdAsync(rootFolderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rootFolder);


        _unitOfWorkMock.Setup(u => u.GetFileByIdAsync(subFolderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subFolder);

        _unitOfWorkMock.Setup(u => u.GetFileByIdAsync(childFileId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(childFile);

        _unitOfWorkMock.Setup(u => u.GetFileList(rootFolderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Persistance.Entities.File> { subFolder });

        _unitOfWorkMock.Setup(u => u.GetFileList(subFolderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Persistance.Entities.File> { childFile });


        // Act
        var result = await _folderService.GetFolderTreeAsync(rootFolderId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(rootFolderName, result.FileName);

        // check if there is one subfolder
        var subfolder = Assert.Single(result.Subfolders);
        Assert.Equal(subFolderName, subfolder.FileName);

        // check if there is one file
        var file = Assert.Single(subfolder.Files);
        Assert.Equal(childFileName, file.FileName);
    }



}

