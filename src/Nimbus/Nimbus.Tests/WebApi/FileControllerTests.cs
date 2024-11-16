using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Nimbus.Business.Models;
using Nimbus.Business.Services;
using Nimbus.WebApi.Controllers;

namespace Nimbus.Tests.WebApi
{
    public class FileControllerTests
    {
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly FileController _controller;

        public FileControllerTests()
        {
            _fileServiceMock = new Mock<IFileService>();
            _controller = new FileController(_fileServiceMock.Object);
        }


        [Fact]
        public async Task UploadFile_ReturnsOk_WhenFileIsUploaded()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var fileName = "testfile.txt";
            var fileContent = "This is a test file content";

            var fileUploadRequest = new FileUploadRequest
            {
                File = fileMock.Object,
                FileName = fileName,
                FolderId = Guid.NewGuid()
            };
            var fileContentBytes = System.Text.Encoding.UTF8.GetBytes(fileContent);
            var fileStream = new System.IO.MemoryStream(fileContentBytes);

            fileMock.Setup(f => f.Length).Returns(fileContentBytes.Length);
            fileMock.Setup(f => f.OpenReadStream()).Returns(fileStream);
            fileMock.Setup(f => f.FileName).Returns(fileName);

            _fileServiceMock.Setup(f => f.SaveFileAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>()))
                            .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UploadFileAsync(fileUploadRequest, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task UploadFile_ReturnsBadRequest_WhenFileHasZeroLength()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var fileUploadRequest = new FileUploadRequest
            {
                File = fileMock.Object,
                FileName = "testfile.txt"
            };

            // Simulate an exception thrown when attempting to save the file
            _fileServiceMock.Setup(f => f.SaveFileAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.UploadFileAsync(fileUploadRequest, CancellationToken.None);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
            Assert.False(string.IsNullOrEmpty(objectResult?.Value?.ToString()));
        }
    }
}