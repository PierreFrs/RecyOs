// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => DocumentPdfControllerTests.cs
// Created : 2023/12/18 - 10:02
// Updated : 2023/12/18 - 10:02

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Controllers;
using RecyOs.Helpers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Interfaces;

namespace RecyOsTests.ControllersTests;

[Collection("DocumentsPdfTests")]
public class DocumentPdfControllerTests
{
    private readonly Mock<IDocumentPdfService<DocumentPdfDto>> _mockDocumentPdfService;
    private readonly Mock<IFileValidationService> _mockFileValidationService;
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly DocumentPdfController _controller;
    public DocumentPdfControllerTests()
    { 
        _mockDocumentPdfService = new Mock<IDocumentPdfService<DocumentPdfDto>>();
        _mockFileValidationService = new Mock<IFileValidationService>();
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockConfiguration = new Mock<IConfiguration>();
        _controller = new DocumentPdfController(_mockDocumentPdfService.Object, _mockFileValidationService.Object, _mockCurrentContextProvider.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task UploadDocumentPdf_ShouldReturnError_IfFileIsNull()
    {
        // Arrange
        IFormFile? mockFile = null;
        int mockEtablissementClientId = 1;
        int mockTypeDocumentPdfId = 1;
        
        // Act
        var result = await _controller.UploadDocumentPdf(mockFile, mockEtablissementClientId, mockTypeDocumentPdfId);
        
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
        int mockTypeDocumentPdfId = 1;
        
        // Act
        var result = await _controller.UploadDocumentPdf(mockFile.Object, mockEtablissementClientId, mockTypeDocumentPdfId);
        
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
        int typeDocumentPdfId = 2;
        int etablissementClientId = 1;
        
        var mockDocumentDto = new DocumentPdfDto();

        _mockFileValidationService.Setup(f => f.ValidateFile(It.IsAny<IFormFile>()));
        _mockFileValidationService.Setup(f => f.ValidateEtablissementClientId(It.IsAny<int>()));
        _mockFileValidationService.Setup(f => f.ValidateTypeDocumentPdfId(It.IsAny<int>(), It.IsAny<bool>()));
        _mockDocumentPdfService.Setup(s => s.UploadPdfAsync(It.IsAny<IFormFile>(), It.IsAny<int>(),It.IsAny<int>()))
            .ReturnsAsync(mockDocumentDto);

        // Act
        var result = await _controller.UploadDocumentPdf(mockFile.Object, etablissementClientId, typeDocumentPdfId);

        // Assert
        _mockFileValidationService.Verify(f => f.ValidateFile(mockFile.Object), Times.Once);
        _mockFileValidationService.Verify(f => f.ValidateEtablissementClientId(etablissementClientId), Times.Once);
        _mockFileValidationService.Verify(f => f.ValidateTypeDocumentPdfId(typeDocumentPdfId, false), Times.Once);
        _mockDocumentPdfService.Verify(s => s.UploadPdfAsync(mockFile.Object, etablissementClientId, typeDocumentPdfId), Times.Once);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockDocumentDto, okResult.Value);
    }

    [Fact]
    async Task UploadDocumentPdf_ShouldReturnNotFound_WhenEtablissementClientIdIsInvalid()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.FileName).Returns("testfile.pdf");
        mockFile.Setup(_ => _.Length).Returns(1024);
        int typeDocumentPdfId = 2;
        int etablissementClientId = 1;

        string expectedErrorMessage = "Entity not found";
        _mockFileValidationService.Setup(f => f.ValidateFile(It.IsAny<IFormFile>()));
        _mockFileValidationService.Setup(f => f.ValidateEtablissementClientId(It.IsAny<int>()))
            .Throws(new EntityNotFoundException(expectedErrorMessage));

        // Act
        var result = await _controller.UploadDocumentPdf(mockFile.Object, etablissementClientId, typeDocumentPdfId);

        // Assert
        _mockFileValidationService.Verify(f => f.ValidateFile(mockFile.Object), Times.Once);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(expectedErrorMessage, notFoundResult.Value);
    }
    
    [Fact]
    public async Task UploadDocumentPdf_ShouldReturnOk_IfDocumentIsValid()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.FileName).Returns("testfile.pdf");
        mockFile.Setup(_ => _.Length).Returns(1024);
        int mockEtablissementClientId = 1;
        int mockTypeDocumentPdfId = 1;

        var mockDocumentPdfDto = new DocumentPdfDto();
        _mockDocumentPdfService.Setup(service => service.UploadPdfAsync(It.IsAny<IFormFile>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(mockDocumentPdfDto);
        
        // Act
        var result = await _controller.UploadDocumentPdf(mockFile.Object, mockEtablissementClientId, mockTypeDocumentPdfId);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockDocumentPdfDto, okResult.Value);
    }

    [Fact]
    public async Task DownloadDocumentPdf_ShouldReturnNotFound_IfFileStreamIsNull()
    {
        // Arrange
        _mockDocumentPdfService.Setup(service => service.DownloadPdfAsync(It.IsAny<int>()))
            .ReturnsAsync(new ValueTuple<FileStream, string>(null, "filepath"));

        // Act
        var result = await _controller.DownloadDocumentPdf(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DownloadDocumentPdf_ShouldReturnNotFound_IfFilePathIsNull()
    {
        // Arrange
        string tempFileName = Path.GetTempFileName();
        FileStream mockFileStream = File.Create(tempFileName);
        _mockDocumentPdfService.Setup(service => service.DownloadPdfAsync(It.IsAny<int>())).ReturnsAsync((mockFileStream, null));

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
        FileStream? mockFileStream = null;

        try
        {
            using (var stream = File.Create(tempFileName))

            _mockDocumentPdfService.Setup(service => service.DownloadPdfAsync(It.IsAny<int>())).ReturnsAsync(() =>
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
    public async Task GetList_ShouldReturnDocumentPdfList()
    {
        // Arrange
        
        int fileSize = 1024;
        string fileName = "test";
        string fileLocation = "filePath";
        int typeDocumentPdfId = 1;
        int etablissementClientId = 1;

        var mockDocumentPdfs = new List<DocumentPdfDto>
        {
            new DocumentPdfDto
            {
                FileSize = fileSize,
                FileName = fileName,
                FileLocation = fileLocation,
                TypeDocumentPdfId = typeDocumentPdfId,
                EtablissementClientID = etablissementClientId
            },
            new DocumentPdfDto
            {
                FileSize = fileSize,
                FileName = fileName,
                FileLocation = fileLocation,
                TypeDocumentPdfId = typeDocumentPdfId,
                EtablissementClientID = etablissementClientId
            }
        };

        _mockDocumentPdfService.Setup(service => service.GetListAsync()).ReturnsAsync(mockDocumentPdfs);

        // Act
        var result = await _controller.GetList();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDocuments = Assert.IsAssignableFrom<List<DocumentPdfDto>>(okResult.Value);
        Assert.Equal(mockDocumentPdfs.Count, returnedDocuments.Count);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_IfServiceReturnsNull()
    {
        // Arrange
        int id = 1;
        _mockDocumentPdfService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync((DocumentPdfDto?)null);
        
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
        int typeDocumentPdfId = 1;
        int etablissementClientId = 1;

        var mockDocumentPdf = new DocumentPdfDto
        {
            FileSize = fileSize,
            FileName = fileName,
            FileLocation = fileLocation,
            TypeDocumentPdfId = typeDocumentPdfId,
            EtablissementClientID = etablissementClientId
        };
        
        _mockDocumentPdfService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync(mockDocumentPdf);
        
        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDocument = Assert.IsAssignableFrom<DocumentPdfDto>(okResult.Value);
        Assert.Equal(mockDocumentPdf.FileSize, returnedDocument.FileSize);
        Assert.Equal(mockDocumentPdf.FileName, returnedDocument.FileName);
        Assert.Equal(mockDocumentPdf.FileLocation, returnedDocument.FileLocation);
        Assert.Equal(mockDocumentPdf.TypeDocumentPdfId, returnedDocument.TypeDocumentPdfId);
        Assert.Equal(mockDocumentPdf.EtablissementClientID, returnedDocument.EtablissementClientID);
    }

    [Fact]
    public async Task GetByClientId_ShouldReturnOk_IfClientIdIsValid()
    {
    // Arrange
    int fileSize = 1024;
    string fileName = "test";
    string fileLocation = "filePath";
    int typeDocumentPdfId = 1;
    int etablissementClientId = 1;

    var mockDocumentPdfs = new List<DocumentPdfDto>
    {
        new DocumentPdfDto
        {
            FileSize = fileSize,
            FileName = fileName,
            FileLocation = fileLocation,
            TypeDocumentPdfId = typeDocumentPdfId,
            EtablissementClientID = etablissementClientId
        },
        new DocumentPdfDto
        {
            FileSize = fileSize,
            FileName = fileName,
            FileLocation = fileLocation,
            TypeDocumentPdfId = typeDocumentPdfId,
            EtablissementClientID = etablissementClientId
        }
    };
        
    _mockDocumentPdfService.Setup(service => service.GetByClientIdAsync(etablissementClientId)).ReturnsAsync(mockDocumentPdfs);
        
    // Act
    var result = await _controller.GetByClientId(etablissementClientId);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var returnedDocuments = Assert.IsAssignableFrom<List<DocumentPdfDto>>(okResult.Value);
    Assert.Equal(mockDocumentPdfs.Count, returnedDocuments.Count);
    }
    
    [Fact]
    public async Task UpdateDocumentPdf_ShouldCallValidationAndReturnOk_WhenFileIsNotNull()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        int id = 1;
        int typeDocumentPdfId = 2;
        var mockDocumentDto = new DocumentPdfDto();

        _mockFileValidationService.Setup(f => f.ValidateFile(It.IsAny<IFormFile>()));
        _mockFileValidationService.Setup(f => f.ValidateTypeDocumentPdfId(It.IsAny<int?>(), It.IsAny<bool>()));
        _mockDocumentPdfService.Setup(s => s.UpdatePdfAsync(It.IsAny<int>(), It.IsAny<IFormFile>(), It.IsAny<int?>()))
            .ReturnsAsync(mockDocumentDto);

        // Act
        var result = await _controller.UpdateDocumentPdf(mockFile.Object, id, typeDocumentPdfId);

        // Assert
        _mockFileValidationService.Verify(f => f.ValidateFile(mockFile.Object), Times.Once);
        _mockFileValidationService.Verify(f => f.ValidateTypeDocumentPdfId(typeDocumentPdfId, true), Times.Once);
        _mockDocumentPdfService.Verify(s => s.UpdatePdfAsync(id, mockFile.Object, typeDocumentPdfId), Times.Once);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockDocumentDto, okResult.Value);
    }
    
    [Fact]
    public async Task DeleteById_ReturnsNotFound_IfDeleteFails()
    {
        // Arrange
        var id = 1;
        _mockDocumentPdfService.Setup(service => service.DeleteAsync(id)).ReturnsAsync(false);
        
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
        _mockDocumentPdfService.Setup(service => service.DeleteAsync(id)).ReturnsAsync(true);
        
        // Act
        var result = await _controller.DeleteById(id);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}