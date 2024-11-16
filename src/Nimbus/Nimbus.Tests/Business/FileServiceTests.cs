using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Nimbus.Business.Common;
using Nimbus.Business.Models;
using Nimbus.Business.Services;
using Nimbus.Persistance.Data;

namespace Nimbus.Tests.Business;

public class FileServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly FileService _fileService;
    private readonly IOptions<AppSettings> _appSettingsMock;

    public FileServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var appSettings = new AppSettings { RootFolderId = Guid.NewGuid() };
        _appSettingsMock = Options.Create(appSettings);

        _fileService = new FileService(_unitOfWorkMock.Object, _appSettingsMock);
    }

    [Fact]
    public async Task GetFileListAsync_WithValidFolderId_ReturnsFiles()
    {
        // Arrange
        var folderId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        _unitOfWorkMock.Setup(u => u.GetFileList(folderId, cancellationToken))
                       .ReturnsAsync(new List<Nimbus.Persistance.Entities.File> { new Nimbus.Persistance.Entities.File() });

        // Act
        var result = await _fileService.GetFileListAsync(folderId, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Value);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task GetFileListAsync_WithEmptyFolderId_UsesRootFolderId()
    {
        // Arrange
        var rootFolderId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        var appSettingsMock = Options.Create(new AppSettings { RootFolderId = rootFolderId });

        var appSettings = new AppSettings { RootFolderId = rootFolderId };
        var serviceWithSettings = new FileService(_unitOfWorkMock.Object, appSettingsMock);

        _unitOfWorkMock.Setup(u => u.GetFileList(rootFolderId, cancellationToken))
                       .ReturnsAsync(new List<Nimbus.Persistance.Entities.File>());

        // Act
        var result = await serviceWithSettings.GetFileListAsync(Guid.Empty, cancellationToken);

        // Assert: unit of work gets called with the root folder Id
        _unitOfWorkMock.Verify(u => u.GetFileList(rootFolderId, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task PersistFileAsync_WithValidFile_AddsFile()
    {
        // Arrange
        var fileEntity = new Nimbus.Persistance.Entities.File
        {
            Id = Guid.NewGuid(),
            FileName = "SampleFile.jpg",
            FileContent = new byte[] { 1, 2, 3 }
        };

        // Create a mock IFormFile
        var mockFile = new Mock<IFormFile>();
        var fileContent = new byte[] { 1, 2, 3 }; // Simulated content
        var fileName = "SampleFile.jpg";
        var ms = new MemoryStream(fileContent);
        mockFile.Setup(_ => _.OpenReadStream()).Returns(ms);
        mockFile.Setup(_ => _.FileName).Returns(fileName);
        mockFile.Setup(_ => _.Length).Returns(fileContent.Length);

        // Create FileUploadRequest
        var fileUploadRequest = new FileUploadRequest
        {
            FileName = fileName,
            File = mockFile.Object,
            FolderId = Guid.NewGuid()
        };

        // Mock unit of work behavior
        _unitOfWorkMock.Setup(u => u.PersistFileAsync(It.IsAny<Nimbus.Persistance.Entities.File>(), CancellationToken.None))
                       .Returns(Task.CompletedTask);

        // Act
        await _fileService.SaveFileAsync(fileUploadRequest, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(u => u.PersistFileAsync(It.IsAny<Nimbus.Persistance.Entities.File>(), CancellationToken.None), Times.Once);
    }


}
