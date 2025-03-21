// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => BaseGenericDocumentPdfServiceTests.cs
// Created : 2023/12/18 - 14:11
// Updated : 2023/12/18 - 14:11

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;
using RecyOsTests.TestFixtures;
using RecyOsTests.TestsHelpers;

namespace RecyOsTests.ORMTests.ServiceTests;

[Collection("DocumentPdfTests")]
public class DocumentPdfServiceTests : IClassFixture<DocumentPdfServiceFixture>
{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<IDocumentPdfRepository<DocumentPdf>> _mockDocumentPdfRepository;
    private readonly IMapper _mapper;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<ITokenInfoService> _mockTokenInfoService;
    private readonly DocumentPdfService<DocumentPdf, DocumentPdfDto> _service;
    private readonly DocumentPdfTestsHelpers _documentPdfTestsHelpers;
    private readonly Mock<IFileSystem> _mockFileSystem;
    
    public DocumentPdfServiceTests(DocumentPdfServiceFixture fixture)
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockDocumentPdfRepository = new Mock<IDocumentPdfRepository<DocumentPdf>>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DocumentPdf, DocumentPdfDto>();
            cfg.CreateMap<DocumentPdfDto, DocumentPdf>();
        });
        _mapper = mapperConfig.CreateMapper();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockTokenInfoService = new Mock<ITokenInfoService>();
        _documentPdfTestsHelpers = new DocumentPdfTestsHelpers();
        _mockFileSystem = new Mock<IFileSystem>();
        _service = new DocumentPdfService<DocumentPdf, DocumentPdfDto>(
            _mockCurrentContextProvider.Object, 
            _mockDocumentPdfRepository.Object, 
            _mapper, 
            _mockConfiguration.Object,
            _mockTokenInfoService.Object,
            _mockFileSystem.Object);
    }
    
    [Fact]
    public async Task UploadPdfAsync_ShouldReturnADocumentDto_IfDetailsAreValid()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        var expectedfileSize = 1024;
        var expectedFileName = "testFile.pdf";
        string mockPdfFolder = "testFolder";
        string filePath = Path.Combine(mockPdfFolder, expectedFileName);
        string mockUser = "TestUser";
        int etablissementClientId = 1;
        int typeDocumentPdfId = 1;
        
        mockFile.Setup(f => f.Length).Returns(expectedfileSize);
        mockFile.Setup(f => f.FileName).Returns(expectedFileName);
        mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        var expectedDocumentPdf = new DocumentPdf()
        {
            FileSize = expectedfileSize,
            FileName = expectedFileName,
            FileLocation = filePath,
            TypeDocumentPdfId = typeDocumentPdfId,
            EtablissementClientId = etablissementClientId,
            CreateDate = DateTime.Now,
            CreatedBy = mockUser
        };
        
        _mockTokenInfoService.Setup(service => service.GetCurrentUserName()).Returns(mockUser);
        _mockConfiguration.Setup(config => config["FileRepository:PdfFolder"])
            .Returns(mockPdfFolder);
        _mockDocumentPdfRepository.Setup(repo => repo.CreateAsync(It.IsAny<DocumentPdf>()))
            .ReturnsAsync(expectedDocumentPdf);
        
        _mockFileSystem.Setup(fs => fs.DirectoryExists(It.IsAny<string>())).Returns(false);
        _mockFileSystem.Setup(fs => fs.CreateDirectory(It.IsAny<string>())).Verifiable();
        _mockFileSystem.Setup(fs => fs.CreateFileStream(It.IsAny<string>(), FileMode.Create))
            .Returns(new MemoryStream());

        // Act
        var result = await _service.UploadPdfAsync(mockFile.Object, etablissementClientId, typeDocumentPdfId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedFileName, result.FileName);
        Assert.Equal(expectedfileSize, result.FileSize);
        }
    [Fact]
    public async Task UploadPdfAsync_CreatesDirectory_IfItDoesNotExist()
    {
    // Arrange
    var mockFile = new Mock<IFormFile>();
    var expectedFileSize = 1024;
    var expectedFileName = "testFile.pdf";

    mockFile.Setup(f => f.Length).Returns(expectedFileSize);
    mockFile.Setup(f => f.FileName).Returns(expectedFileName);
    mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

    string mockUser = "TestUser";
    string mockPdfFolder = "testFolder";
    int etablissementClientId = 1;
    int typeDocumentPdfId = 1;

    _mockTokenInfoService.Setup(service => service.GetCurrentUserName()).Returns(mockUser);
    _mockConfiguration.Setup(config => config["FileRepository:PdfFolder"])
        .Returns(mockPdfFolder);

    _mockFileSystem.Setup(fs => fs.DirectoryExists(It.IsAny<string>())).Returns(false);
    _mockFileSystem.Setup(fs => fs.CreateDirectory(It.IsAny<string>())).Verifiable();
    _mockFileSystem.Setup(fs => fs.CreateFileStream(It.IsAny<string>(), FileMode.Create))
        .Returns(new MemoryStream());

    var expectedDocumentPdf = new DocumentPdf()
    {
        FileSize = expectedFileSize,
        FileName = expectedFileName,
        FileLocation = mockPdfFolder,
        TypeDocumentPdfId = typeDocumentPdfId,
        EtablissementClientId = etablissementClientId,
        CreateDate = DateTime.Now,
        CreatedBy = mockUser
    };

    _mockDocumentPdfRepository.Setup(repo => repo.CreateAsync(It.IsAny<DocumentPdf>()))
        .ReturnsAsync(expectedDocumentPdf);
    
    // Act
    await _service.UploadPdfAsync(mockFile.Object, etablissementClientId, typeDocumentPdfId);

    // Assert
    _mockFileSystem.Verify(fs => fs.DirectoryExists(mockPdfFolder), Times.Once);
    _mockFileSystem.Verify(fs => fs.CreateDirectory(mockPdfFolder), Times.Once);
}
    [Fact]
    public async Task UploadPdfAsync_DoesNotCreateDirectory_IfItExists()
    {
    // Arrange
    var mockFile = new Mock<IFormFile>();
    var expectedFileSize = 1024;
    var expectedFileName = "testFile.pdf";

    mockFile.Setup(f => f.Length).Returns(expectedFileSize);
    mockFile.Setup(f => f.FileName).Returns(expectedFileName);
    mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

    string mockUser = "TestUser";
    string mockPdfFolder = "testFolder";
    int etablissementClientId = 1;
    int typeDocumentPdfId = 1;

    _mockTokenInfoService.Setup(service => service.GetCurrentUserName()).Returns(mockUser);
    _mockConfiguration.Setup(config => config["FileRepository:PdfFolder"])
        .Returns(mockPdfFolder);

    _mockFileSystem.Setup(fs => fs.DirectoryExists(It.IsAny<string>())).Returns(true);
    _mockFileSystem.Setup(fs => fs.CreateFileStream(It.IsAny<string>(), FileMode.Create))
        .Returns(new MemoryStream());

    _mockFileSystem.Setup(fs => fs.DirectoryExists(It.IsAny<string>())).Returns(true);

    // Act
    await _service.UploadPdfAsync(mockFile.Object, etablissementClientId, typeDocumentPdfId);

    // Assert
    _mockFileSystem.Verify(fs => fs.DirectoryExists(It.IsAny<string>()), Times.Once);
    _mockFileSystem.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Never);
}
    [Fact]
    public async Task DownloadPdfAsync_ShouldReturnAFileStreamAndAnOriginalFileName_WhenDetailsAreValid()
    {
        // Arrange
        int testDocumentId = 2;
        string expectedFileName = "testFile.pdf";
        string testFileGuid = Guid.NewGuid().ToString();
        string testFilePath = Path.Combine("TestFiles", $"{testFileGuid}_{expectedFileName}");
        string currentDirectory = Directory.GetCurrentDirectory();
        string fullTestFilePath = Path.Combine(currentDirectory, testFilePath);
        
        var testDocument = new DocumentPdf
        {
            FileName = expectedFileName,
            FileLocation = testFilePath,
            FileSize = 1024,
            TypeDocumentPdfId = 1,
            EtablissementClientId = 1
        };
        
        _mockDocumentPdfRepository.Setup(repo => repo.GetByIdAsync(testDocumentId, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(testDocument);
        _mockFileSystem.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(true);

        
        FileStream? fileStream = null;
        
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullTestFilePath));
            await File.WriteAllTextAsync(fullTestFilePath, "Test Content");
            
            // Act
            (fileStream, var originalFileName) = await _service.DownloadPdfAsync(testDocumentId);

            // Assert
            Assert.NotNull((fileStream));
            Assert.Equal(expectedFileName, originalFileName);
        }
        finally
        {
            fileStream?.Dispose();
            if (File.Exists(fullTestFilePath))
            {
                File.Delete(fullTestFilePath);
            }
        }
    }
    [Fact]
    public async Task DownloadPdfAsync_ReturnsNull_WhenDocumentDoesNotExist()
    {
        // Arrange
        int nonExistentDocumentId = 1;

        _mockDocumentPdfRepository.Setup(repo => repo.GetByIdAsync(nonExistentDocumentId, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync((DocumentPdf?)null);

        // Act
        var (fileStream, originalFileName) = await _service.DownloadPdfAsync(nonExistentDocumentId);

        // Assert
        Assert.Null(fileStream);
        Assert.Null(originalFileName);
        _mockDocumentPdfRepository.Verify(repo => repo.GetByIdAsync(nonExistentDocumentId, It.IsAny<ContextSession>(), It.IsAny<bool>()), Times.Once);
    }
    [Fact]
    public async Task DownloadPdfAsync_ReturnsNull_WhenFileDoesNotExist()
    {
        // Arrange
        int existingDocumentId = 1;
        var existingDocument = new DocumentPdf
        {
            FileName = "fileName",
            FileLocation = "testFolder",
            FileSize = 1024,
            TypeDocumentPdfId = 1,
            EtablissementClientId = 1
        };

        _mockDocumentPdfRepository.Setup(repo => repo.GetByIdAsync(existingDocumentId, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(existingDocument);

        _mockFileSystem.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(false); // Simulate file does not exist

        // Act
        var (fileStream, originalFileName) = await _service.DownloadPdfAsync(existingDocumentId);

        // Assert
        Assert.Null(fileStream);
        Assert.Null(originalFileName);
        _mockFileSystem.Verify(fs => fs.FileExists(It.IsAny<string>()), Times.Once); // Verify file existence check
    }
    [Fact]
    public async Task GetListAsync_ShouldReturnAListOfDocumentPdfDtos_WhenDetailsAreValid()
    {
        // Arrange
        string currentDirectory = Directory.GetCurrentDirectory();
        var mockUser = "TestUser";
        
        var mockDocuments = new List<DocumentPdf>
        {
            new DocumentPdf
            {
                FileName = "Test file 1",
                FileLocation = _documentPdfTestsHelpers.CreateTestFilePath(currentDirectory, "Test file 1"),
                FileSize = 1024,
                TypeDocumentPdfId = 1,
                EtablissementClientId = 1,
                CreateDate = DateTime.Now,
                CreatedBy = mockUser
                
            },
            new DocumentPdf
            {
                FileName = "Test file 2",
                FileLocation = _documentPdfTestsHelpers.CreateTestFilePath(currentDirectory, "Test file 2"),
                FileSize = 1024,
                TypeDocumentPdfId = 1,
                EtablissementClientId = 1,
                CreateDate = DateTime.Now,
                CreatedBy = mockUser
            },
        };

        
        _mockDocumentPdfRepository.Setup((repo => repo.GetListAsync(It.IsAny<ContextSession>(), It.IsAny<bool>()))).ReturnsAsync(mockDocuments);
        
        var mockDocumentDtos = _mapper.Map<List<DocumentPdfDto>>(mockDocuments);
        
        // Act
        var result = await _service.GetListAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<DocumentPdfDto>>(result);
        Assert.Equal(mockDocumentDtos.Count, result.Count);
    }
    [Fact]
    public async Task GetByIdAsync_ShouldReturnADocumentPdfDto_WhenIdIsValid()
    {
        // Arrange
        int testDocumentId = 2;
        string expectedFileName = "testFile.pdf";
        string testFileGuid = Guid.NewGuid().ToString();
        string testFilePath = Path.Combine("TestFiles", $"{testFileGuid}_{expectedFileName}");
        string currentDirectory = Directory.GetCurrentDirectory();
        string fullTestFilePath = Path.Combine(currentDirectory, testFilePath);

        var testDocument = new DocumentPdf
        {
            FileName = expectedFileName,
            FileLocation = fullTestFilePath,
            FileSize = 1024,
            TypeDocumentPdfId = 1,
            EtablissementClientId = 1
        };

        _mockDocumentPdfRepository.Setup(repo => repo.GetByIdAsync(testDocumentId, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(testDocument);
        
        // Act
        var result = await _service.GetByIdAsync(testDocumentId);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<DocumentPdfDto>(result);
        Assert.Equal(testDocument.FileName, result.FileName);
        Assert.Equal(testDocument.FileLocation, result.FileLocation);
        Assert.Equal(testDocument.FileSize, result.FileSize);
        Assert.Equal(testDocument.TypeDocumentPdfId, result.TypeDocumentPdfId);
        Assert.Equal(testDocument.EtablissementClientId, result.EtablissementClientID);
        
    }
    
    [Fact]
    public async Task GetByClientIdAsync_ShouldReturnAListOfDocumentPdfDto_WhenIdIsValid()
    {
        // Arrange
        string currentDirectory = Directory.GetCurrentDirectory();
        var mockUser = "TestUser";
        var etablissementClientId = 1;
        
        List<DocumentPdf> mockDocuments = new List<DocumentPdf>
        {
            new DocumentPdf
            {
                FileName = "Test file 1",
                FileLocation = _documentPdfTestsHelpers.CreateTestFilePath(currentDirectory, "Test file 1"),
                FileSize = 1024,
                TypeDocumentPdfId = 1,
                EtablissementClientId = etablissementClientId,
                CreateDate = DateTime.Now,
                CreatedBy = mockUser
                
            },
            new DocumentPdf
            {
                FileName = "Test file 2",
                FileLocation = _documentPdfTestsHelpers.CreateTestFilePath(currentDirectory, "Test file 2"),
                FileSize = 1024,
                TypeDocumentPdfId = 1,
                EtablissementClientId = etablissementClientId,
                CreateDate = DateTime.Now,
                CreatedBy = mockUser
            },
        };

        
        _mockDocumentPdfRepository.Setup((repo => repo.GetByClientIdAsync(etablissementClientId, It.IsAny<ContextSession>(), It.IsAny<bool>()))).ReturnsAsync(mockDocuments);
        
        var mockDocumentDtos = _mapper.Map<List<DocumentPdfDto>>(mockDocuments);
        
        // Act
        var result = await _service.GetByClientIdAsync(etablissementClientId);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<DocumentPdfDto>>(result);
        Assert.Equal(mockDocumentDtos.Count, result.Count);
        
        for (int i = 0; i < mockDocumentDtos.Count; i++)
        {
            Assert.Equal(mockDocumentDtos[i].FileName, result[i].FileName);
            Assert.Equal(mockDocumentDtos[i].FileLocation, result[i].FileLocation);
            Assert.Equal(mockDocumentDtos[i].FileSize, result[i].FileSize);
            Assert.Equal(mockDocumentDtos[i].TypeDocumentPdfId, result[i].TypeDocumentPdfId);
        }
    }
    [Fact]
    public async Task UpdatePdfAsync_ShouldReturnAnUpdatedDocumentPdfDto_WhenDetailsAreValid()
    {
        // Arrange
        
        var id = 1;
        var originalFileName = "originalFileName.pdf";
        var newFileName = "newFileName.pdf";
        var currentDirectory = Directory.GetCurrentDirectory();
        var originalLocation = _documentPdfTestsHelpers.CreateTestFilePath(currentDirectory, "original name");
        var newLocation = _documentPdfTestsHelpers.CreateTestFilePath(currentDirectory, "new name");
        var originalMockFile = new Mock<IFormFile>();
        var newMockFile = new Mock<IFormFile>();
        var originalFileSize = 1024;
        var newFileSize = 2048;
        
        originalMockFile.Setup(f => f.Length).Returns(originalFileSize);
        originalMockFile.Setup(f => f.FileName).Returns(originalFileName);
        originalMockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        newMockFile.Setup(f => f.Length).Returns(newFileSize);
        newMockFile.Setup(f => f.FileName).Returns(newFileName);
        newMockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        string mockUser = "TestUser";
        string updateUser = "updateUser";
        string mockPdfFolder = "testFolder";
        int etablissementClientId = 1;
        int originalFileTypeId = 1;
        int newFileTypeId = 2;
        
        var originalDocumentPdf = new DocumentPdf()
        {
            FileSize = originalFileSize,
            FileName = originalFileName,
            FileLocation = originalLocation,
            TypeDocumentPdfId = originalFileTypeId,
            EtablissementClientId = etablissementClientId,
            CreateDate = DateTime.Now,
            CreatedBy = mockUser
        };
        
        var newDocumentPdf = new DocumentPdf()
        {
            FileSize = newFileSize,
            FileName = newFileName,
            FileLocation = newLocation,
            TypeDocumentPdfId = newFileTypeId,
            EtablissementClientId = etablissementClientId,
            CreateDate = DateTime.Now,
            CreatedBy = updateUser
        };

        _mockDocumentPdfRepository.Setup(repo => repo.GetByIdAsync(id, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(originalDocumentPdf);
        _mockDocumentPdfRepository.Setup(repo => repo.UpdateAsync(It.IsAny<DocumentPdf>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(newDocumentPdf);
        _mockConfiguration.Setup(config => config["FileRepository:PdfFolder"])
            .Returns(mockPdfFolder);
        _mockTokenInfoService.Setup(service => service.GetCurrentUserName()).Returns(mockUser);
        
        _mockFileSystem.Setup(fs => fs.DirectoryExists(It.IsAny<string>())).Returns(false);
        _mockFileSystem.Setup(fs => fs.CreateDirectory(It.IsAny<string>())).Verifiable();
        _mockFileSystem.Setup(fs => fs.CreateFileStream(It.IsAny<string>(), FileMode.Create))
            .Returns(new MemoryStream());

        // Act
        var result = await _service.UpdatePdfAsync(id, newMockFile.Object, newFileTypeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newFileName, result.FileName);
        Assert.Equal(newFileSize, result.FileSize);
        Assert.Equal(newFileTypeId, result.TypeDocumentPdfId);
        _mockDocumentPdfRepository.Verify(repo => repo.UpdateAsync(It.IsAny<DocumentPdf>(), It.IsAny<ContextSession>()), Times.Once);
    }
    [Fact]
    public async Task UpdatePdfAsync_ReturnsNull_WhenDocumentDoesNotExist()
    {
        // Arrange
        int nonExistentDocumentId = 1;
        Mock<IFormFile> mockFile = new Mock<IFormFile>();
        int? typeDocumentPdfId = 0;

        _mockDocumentPdfRepository.Setup(repo => repo.GetByIdAsync(nonExistentDocumentId, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync((DocumentPdf? )null);

        // Act
        var result = await _service.UpdatePdfAsync(nonExistentDocumentId, mockFile.Object, typeDocumentPdfId);

        // Assert
        Assert.Null(result);
        _mockDocumentPdfRepository.Verify(repo => repo.GetByIdAsync(nonExistentDocumentId, It.IsAny<ContextSession>(), It.IsAny<bool>()), Times.Once);
    }
    [Fact]
    public async Task UpdatePdfAsync_DeletesOldFile_IfItExists()
    {
        // Arrange
        int existingDocumentId = 1;
        string mockPdfFolder = "oldFilePath";
        var existingDocument = new DocumentPdf
        {
            Id = existingDocumentId,
            FileSize = 1024,
            FileName = "oldFileName",
            FileLocation = mockPdfFolder,
            TypeDocumentPdfId = 1,
            EtablissementClientId = 1,
            CreateDate = DateTime.Now,
            CreatedBy = "mockUser"
        };

        // Mock the repository to return an existing document
        _mockDocumentPdfRepository.Setup(repo => repo.GetByIdAsync(existingDocumentId, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(existingDocument);

        // Mock the file system to simulate that the old file exists and can be deleted
        _mockConfiguration.Setup(config => config["FileRepository:PdfFolder"])
            .Returns(mockPdfFolder);
        _mockFileSystem.Setup(fs => fs.FileExists("oldFilePath")).Returns(true);
        _mockFileSystem.Setup(fs => fs.DeleteFile("oldFilePath")).Verifiable();
        

        var mockNewFile = new Mock<IFormFile>();
        mockNewFile.Setup(f => f.FileName).Returns("newFile.pdf");
        mockNewFile.Setup(f => f.Length).Returns(2048);

        // Act
        await _service.UpdatePdfAsync(existingDocumentId, mockNewFile.Object, It.IsAny<int?>());

        // Assert
        _mockFileSystem.Verify(fs => fs.DeleteFile("oldFilePath"), Times.Once);
    }
    [Fact]
    public async Task DeleteAsync_returnsTrue_WhenRequestIsSuccessful()
    {
            // Arrange
            int testDocumentId = 1;
            string expectedFileName = "testFile.pdf";
            string testFileGuid = Guid.NewGuid().ToString();
            string testFilePath = Path.Combine("TestFiles", $"{testFileGuid}_{expectedFileName}");
            string currentDirectory = Directory.GetCurrentDirectory();
            string fullTestFilePath = Path.Combine(currentDirectory, testFilePath);

            var testDocument = new DocumentPdf
            {
                FileName = expectedFileName,
                FileLocation = fullTestFilePath,
                FileSize = 1024,
                TypeDocumentPdfId = 1,
                EtablissementClientId = 1
            };
        
            _mockDocumentPdfRepository.Setup(repo => repo.GetByIdAsync(testDocumentId, It.IsAny<ContextSession>(), It.IsAny<bool>())).ReturnsAsync(testDocument);
            _mockDocumentPdfRepository.Setup(repo => repo.DeleteAsync(testDocumentId, It.IsAny<ContextSession>())).ReturnsAsync(true);
        
            // Act
            var result = await _service.DeleteAsync(testDocumentId);
        
            // Assert
            Assert.True(result);
            _mockDocumentPdfRepository.Verify(repo => repo.DeleteAsync(testDocumentId, It.IsAny<ContextSession>()), Times.Once());
    }
    [Fact]
    public async Task DeleteAsync_returnsFalse_WhenItemDoesNotExist()
    {
        // Arrange
        var id = 1;
        
        _mockDocumentPdfRepository.Setup(repo => repo.DeleteAsync(id, It.IsAny<ContextSession>())).ReturnsAsync(false);
        
        // Act
        var result = await _service.DeleteAsync(id);
        
        // Assert
        Assert.False(result);
        _mockDocumentPdfRepository.Verify(repo => repo.DeleteAsync(id, It.IsAny<ContextSession>()), Times.Never);
    }
}