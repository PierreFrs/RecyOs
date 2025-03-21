// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => DocumentPdfEuropeEuropeControllerTests.cs
// Created : 2023/12/26 - 12:13
// Updated : 2023/12/26 - 12:13

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Controllers;
using RecyOs.Helpers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Interfaces;

namespace RecyOsTests.ControllersTests;

[Collection("DocumentsPdfEuropefEuropeTests")]
public class DocumentPdfEuropeControllerTests
{
    private readonly Mock<IDocumentPdfEuropeService<DocumentPdfEuropeDto>> _mockDocumentPdfEuropeService;
    private readonly Mock<IFileValidationService> _mockFileValidationService;
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly DocumentPdfEuropeController _controller;
    public DocumentPdfEuropeControllerTests()
    { 
        _mockDocumentPdfEuropeService = new Mock<IDocumentPdfEuropeService<DocumentPdfEuropeDto>>();
        _mockFileValidationService = new Mock<IFileValidationService>();
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockConfiguration = new Mock<IConfiguration>();
        _controller = new DocumentPdfEuropeController(_mockDocumentPdfEuropeService.Object, _mockFileValidationService.Object, _mockCurrentContextProvider.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task UploadDocumentPdf_ShouldReturnError_IfFileIsNull()
    {
        // Arrange
        IFormFile? mockFile = null;
        int mockEtablissementClientId = 1;
        int mockTypeDocumentPdfEuropeId = 1;
        
        // Act
        var result = await _controller.UploadDocumentPdf(mockFile, mockEtablissementClientId, mockTypeDocumentPdfEuropeId);
        
        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal("file not selected", contentResult.Content);
    }
    
    [Fact]
    public async Task UploadDocumentPdf_ShouldReturnError_IfLengthEqualsZero()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.FileName).Returns("testfile.pdf");
        mockFile.Setup(_ => _.Length).Returns(0);
        int mockEtablissementClientId = 1;
        int mockTypeDocumentPdfEuropeId = 1;
        
        // Act
        var result = await _controller.UploadDocumentPdf(mockFile.Object, mockEtablissementClientId, mockTypeDocumentPdfEuropeId);
        
        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal("file not selected", contentResult.Content);
    }
    
    [Fact]
    public async Task UploadDocumentPdf_ShouldCallValidationAndReturnOk_WhenFileIsNotNull()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.FileName).Returns("testfile.pdf");
        mockFile.Setup(_ => _.Length).Returns(1024);
        int typeDocumentPdfEuropeId = 2;
        int etablissementClientId = 1;
        
        var mockDocumentDtoEurope = new DocumentPdfEuropeDto();

        _mockFileValidationService.Setup(f => f.ValidateFile(It.IsAny<IFormFile>()));
        _mockFileValidationService.Setup(f => f.ValidateEtablissementClientId(It.IsAny<int>()));
        _mockFileValidationService.Setup(f => f.ValidateTypeDocumentPdfId(It.IsAny<int>(), It.IsAny<bool>()));
        _mockDocumentPdfEuropeService.Setup(s => s.UploadPdfAsync(It.IsAny<IFormFile>(), It.IsAny<int>(),It.IsAny<int>()))
            .ReturnsAsync(mockDocumentDtoEurope);

        // Act
        var result = await _controller.UploadDocumentPdf(mockFile.Object, etablissementClientId, typeDocumentPdfEuropeId);

        // Assert
        _mockFileValidationService.Verify(f => f.ValidateFile(mockFile.Object), Times.Once);
        _mockFileValidationService.Verify(f => f.ValidateEtablissementClientEuropeId(etablissementClientId), Times.Once);
        _mockFileValidationService.Verify(f => f.ValidateTypeDocumentPdfId(typeDocumentPdfEuropeId, false), Times.Once);
        _mockDocumentPdfEuropeService.Verify(s => s.UploadPdfAsync(mockFile.Object, etablissementClientId, typeDocumentPdfEuropeId), Times.Once);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockDocumentDtoEurope, okResult.Value);
    }

    
    [Fact]
    public async Task UploadDocumentPdf_ShouldReturnOk_IfDocumentIsValid()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.FileName).Returns("testfile.pdf");
        mockFile.Setup(_ => _.Length).Returns(1024);
        int mockEtablissementClientId = 1;
        int mockTypeDocumentPdfEuropeId = 1;

        var mockDocumentPdfEuropeDto = new DocumentPdfEuropeDto();
        _mockDocumentPdfEuropeService.Setup(service => service.UploadPdfAsync(It.IsAny<IFormFile>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(mockDocumentPdfEuropeDto);
        
        // Act
        var result = await _controller.UploadDocumentPdf(mockFile.Object, mockEtablissementClientId, mockTypeDocumentPdfEuropeId);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockDocumentPdfEuropeDto, okResult.Value);
    }
    
    [Fact]
    async Task UploadDocumentPdf_ShouldReturnNotFound_WhenClientEuropeIdIsInvalid()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.FileName).Returns("testfile.pdf");
        mockFile.Setup(_ => _.Length).Returns(1024);
        int typeDocumentPdfId = 2;
        int etablissementClientId = 1;

        string expectedErrorMessage = "Entity not found";
        _mockFileValidationService.Setup(f => f.ValidateFile(It.IsAny<IFormFile>()));
        _mockFileValidationService.Setup(f => f.ValidateEtablissementClientEuropeId(It.IsAny<int>()))
            .Throws(new EntityNotFoundException(expectedErrorMessage));

        // Act
        var result = await _controller.UploadDocumentPdf(mockFile.Object, etablissementClientId, typeDocumentPdfId);

        // Assert
        _mockFileValidationService.Verify(f => f.ValidateFile(mockFile.Object), Times.Once);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(expectedErrorMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task DownloadDocumentPdf_ShouldReturnNotFound_IfFileStreamIsNull()
    {
        // Arrange
        _mockDocumentPdfEuropeService.Setup(service => service.DownloadPdfAsync(It.IsAny<int>()))
            .ReturnsAsync(new ValueTuple<FileStream, string>(null, "filepath"));

        // Act
        var result = await _controller.DownloadDocumentPdf(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DownloadDocumentPdfEurope_ShouldReturnNotFound_IfFilePathIsNull()
    {
        // Arrange
        string tempFileName = Path.GetTempFileName();
        FileStream mockFileStream = File.Create(tempFileName);
        _mockDocumentPdfEuropeService.Setup(service => service.DownloadPdfAsync(It.IsAny<int>())).ReturnsAsync((mockFileStream, null));

        // Act
        var result = await _controller.DownloadDocumentPdf(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        
        // Cleanup
        mockFileStream.Dispose();
        File.Delete(tempFileName);
    }

    [Fact]
    public async Task DownloadDocumentPdf_ShouldReturnOk_IfFileDetailsAreValid()
    {
        // Arrange
        string tempFileName = Path.GetTempFileName();
        string expectedMimeType = "application/pdf";

        _mockConfiguration.Setup(config => config["FileValidation:MimeType"]).Returns(expectedMimeType);
        FileStream mockFileStream = null;

        try
        {
            using (var stream = File.Create(tempFileName))

            _mockDocumentPdfEuropeService.Setup(service => service.DownloadPdfAsync(It.IsAny<int>())).ReturnsAsync(() =>
            {
                mockFileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                return (mockFileStream, "filepath");
            });

            // Act
            var result = await _controller.DownloadDocumentPdf(1);

            // Assert
            var fileStreamResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(expectedMimeType, fileStreamResult.ContentType);

        }
        finally
        {
            // Cleanup
                    mockFileStream?.Dispose();
                    File.Delete(tempFileName);
        }
    }

    [Fact]
    public async Task GetList_ShouldReturnDocumentPdfEuropeList()
    {
        // Arrange
        
        int fileSize = 1024;
        string fileName = "test";
        string fileLocation = "filePath";
        int typeDocumentPdfEuropeId = 1;

        var mockDocumentPdfEuropes = new List<DocumentPdfEuropeDto>
        {
            new DocumentPdfEuropeDto
            {
                FileSize = fileSize,
                FileName = fileName,
                FileLocation = fileLocation,
                TypeDocumentPdfId = typeDocumentPdfEuropeId
            },
            new DocumentPdfEuropeDto
            {
                FileSize = fileSize,
                FileName = fileName,
                FileLocation = fileLocation,
                TypeDocumentPdfId = typeDocumentPdfEuropeId
            }
        };

        _mockDocumentPdfEuropeService.Setup(service => service.GetListAsync()).ReturnsAsync(mockDocumentPdfEuropes);

        // Act
        var result = await _controller.GetList();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDocuments = Assert.IsAssignableFrom<List<DocumentPdfEuropeDto>>(okResult.Value);
        Assert.Equal(mockDocumentPdfEuropes.Count, returnedDocuments.Count);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_IfServiceReturnsNull()
    {
        // Arrange
        int id = 1;
        _mockDocumentPdfEuropeService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync((DocumentPdfEuropeDto?)null);
        
        // Act
        var result = await _controller.GetById(id);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_IfServiceFindsDocument() 
    {
        // Arrange
        int id = 1;
        int fileSize = 1024;
        string fileName = "test";
        string fileLocation = "filePath";
        int typeDocumentPdfEuropeId = 1;

        var mockDocumentPdfEurope = new DocumentPdfEuropeDto
        {
            FileSize = fileSize,
            FileName = fileName,
            FileLocation = fileLocation,
            TypeDocumentPdfId = typeDocumentPdfEuropeId
        };
        
        _mockDocumentPdfEuropeService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync(mockDocumentPdfEurope);
        
        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDocument = Assert.IsAssignableFrom<DocumentPdfEuropeDto>(okResult.Value);
        Assert.Equal(mockDocumentPdfEurope.FileSize, returnedDocument.FileSize);
        Assert.Equal(mockDocumentPdfEurope.FileName, returnedDocument.FileName);
        Assert.Equal(mockDocumentPdfEurope.FileLocation, returnedDocument.FileLocation);
        Assert.Equal(mockDocumentPdfEurope.TypeDocumentPdfId, returnedDocument.TypeDocumentPdfId);
    }

    [Fact]
    public async Task GetByClientId_ShouldReturnOk_IfClientIdIsValid()
    {
    // Arrange
    int fileSize = 1024;
    string fileName = "test";
    string fileLocation = "filePath";
    int typeDocumentPdfEuropeId = 1;
    int etablissementClientId = 1;

    var mockDocumentPdfEuropes = new List<DocumentPdfEuropeDto>
    {
        new DocumentPdfEuropeDto
        {
            FileSize = fileSize,
            FileName = fileName,
            FileLocation = fileLocation,
            TypeDocumentPdfId = typeDocumentPdfEuropeId
        },
        new DocumentPdfEuropeDto
        {
            FileSize = fileSize,
            FileName = fileName,
            FileLocation = fileLocation,
            TypeDocumentPdfId = typeDocumentPdfEuropeId
        }
    };
        
    _mockDocumentPdfEuropeService.Setup(service => service.GetByClientIdAsync(etablissementClientId)).ReturnsAsync(mockDocumentPdfEuropes);
        
    // Act
    var result = await _controller.GetByClientId(etablissementClientId);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var returnedDocuments = Assert.IsAssignableFrom<List<DocumentPdfEuropeDto>>(okResult.Value);
    Assert.Equal(mockDocumentPdfEuropes.Count, returnedDocuments.Count);
    }
    
    [Fact]
    public async Task UpdateDocumentPdfEurope_ShouldCallValidationAndReturnOk_WhenFileIsNotNull()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        int id = 1;
        int typeDocumentPdfEuropeId = 2;
        var mockDocumentDtoEurope = new DocumentPdfEuropeDto(); // Assuming DocumentPdfEuropeDto is your DTO class

        _mockFileValidationService.Setup(f => f.ValidateFile(It.IsAny<IFormFile>()));
        _mockFileValidationService.Setup(f => f.ValidateTypeDocumentPdfId(It.IsAny<int?>(), It.IsAny<bool>()));
        _mockDocumentPdfEuropeService.Setup(s => s.UpdatePdfAsync(It.IsAny<int>(), It.IsAny<IFormFile>(), It.IsAny<int?>()))
            .ReturnsAsync(mockDocumentDtoEurope);

        // Act
        var result = await _controller.UpdateDocumentPdf(mockFile.Object, id, typeDocumentPdfEuropeId);

        // Assert
        _mockFileValidationService.Verify(f => f.ValidateFile(mockFile.Object), Times.Once);
        _mockFileValidationService.Verify(f => f.ValidateTypeDocumentPdfId(typeDocumentPdfEuropeId, true), Times.Once);
        _mockDocumentPdfEuropeService.Verify(s => s.UpdatePdfAsync(id, mockFile.Object, typeDocumentPdfEuropeId), Times.Once);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockDocumentDtoEurope, okResult.Value);
    }
    
    [Fact]
    public async Task DeleteById_ReturnsNotFound_IfDeleteFails()
    {
        // Arrange
        var id = 1;
        _mockDocumentPdfEuropeService.Setup(service => service.DeleteAsync(id)).ReturnsAsync(false);
        
        // Act
        var result = await _controller.DeleteById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task DeleteById_ReturnsOk_IfDeleteSucceeds()
    {
        // Arrange
        var id = 1;
        _mockDocumentPdfEuropeService.Setup(service => service.DeleteAsync(id)).ReturnsAsync(true);
        
        // Act
        var result = await _controller.DeleteById(id);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}