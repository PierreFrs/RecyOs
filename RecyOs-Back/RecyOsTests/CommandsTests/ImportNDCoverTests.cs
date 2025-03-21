// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => ImportNDCoverTests.cs
// Created : 2023/12/19 - 16:18
// Updated : 2023/12/19 - 16:18

using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using RecyOs.Commands;
using RecyOs.Controllers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOsTests.CommandsTests;

[Collection("NDCoverTests")]
public class ImportNDCoverTests
{
    private readonly Mock<IEntrepriseNDCoverService> _mockEntrepriseNDCoverService;
    private readonly Mock<IEntrepriseBaseService> _mockEntrepriseBaseService;
    private readonly Mock<IEtablissementClientService> _mockEtablissementService;
    private readonly Mock<IEtablissementFicheService> _mockEtablissementFicheService;
    private readonly Mock<ICommandExportSoumissionNDCoverRepository<EntrepriseBase> > _mockCommandExportSoumissionNDCoverRepository;
    private readonly Mock<INotificationRepository<Notification> > _mockNotificationRepository;
    private readonly ImportNDCover _importNDCover;
    private readonly Mock<ILogger<ImportNDCover>> _mockLogger;

    public ImportNDCoverTests()
    {
        _mockEntrepriseNDCoverService = new Mock<IEntrepriseNDCoverService>();
        _mockEntrepriseBaseService = new Mock<IEntrepriseBaseService>();
        _mockEtablissementService = new Mock<IEtablissementClientService>();
        _mockEtablissementFicheService = new Mock<IEtablissementFicheService>();
        _mockCommandExportSoumissionNDCoverRepository = new Mock<ICommandExportSoumissionNDCoverRepository<EntrepriseBase>>();
        _mockNotificationRepository = new Mock<INotificationRepository<Notification>>();
        _mockLogger = new Mock<ILogger<ImportNDCover>>();
        _importNDCover = new ImportNDCover(
            _mockEntrepriseNDCoverService.Object,
            _mockEntrepriseBaseService.Object,
            _mockEtablissementService.Object,
            _mockEtablissementFicheService.Object,
            _mockCommandExportSoumissionNDCoverRepository.Object,
            _mockNotificationRepository.Object,
            _mockLogger.Object);
    }
    
    // Import method tests
    
    [Fact]
    public async Task Import_ShouldReturnFalse_IfFileIsNull()
    {
        // Arrange
        IFormFile? file = null;

        // Act
        var result = await _importNDCover.Import(file);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task Import_ShouldReturnFalse_IfFileLengthIsZero()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(x => x.Length).Returns(0);

        // Act
        var result = await _importNDCover.Import(mockFile.Object);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task Import_ShouldReturnTrue_IfRealExcelFileIsValid()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // Arrange
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "ndcover_test_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var formFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));
        

        // Act
        var result = await _importNDCover.Import(formFile);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task Import_ShouldReturnFalse_IfRealExcelFileIsNotValid()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // Arrange
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "ndcover_test_false_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var falseFormFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));
        

        // Act
        var result = await _importNDCover.Import(falseFormFile);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task Import_ShouldReturnFalse_IfHeaderRowIsMissing()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");

        worksheet.Cell(1, 1).Value = "Data1";
        worksheet.Cell(2, 1).Value = "Data2";

        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);

        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", Path.GetFileName(tempFileName));
    
        // Act
        var result = await _importNDCover.Import(formFile);

        // Assert
        Assert.False(result);
        
        // Clean up: Delete the temporary file
        fileStream.Close();
        File.Delete(tempFileName);
    }
    
    [Fact]
    public async Task Import_ShouldReturnFalse_IfNoRowsInExcelFile()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");

        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);

        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", Path.GetFileName(tempFileName));

        // Act
        var result = await _importNDCover.Import(formFile);

        // Assert
        Assert.False(result);
    
        // Clean up: Delete the temporary file
        fileStream.Close();
        File.Delete(tempFileName);
    }
    
    [Fact]
    public async Task Import_ShouldHandleDuplicateColumnNamesCorrectly()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");
        
        worksheet.Cell(1, 1).Value = "ColumnName";
        worksheet.Cell(1, 2).Value = "UniqueColumn";
        worksheet.Cell(1, 3).Value = "ColumnName"; // Duplicate
        
        worksheet.Cell(2, 1).Value = "Data1";
        worksheet.Cell(2, 2).Value = "Data2";
        worksheet.Cell(2, 3).Value = "Data3";

        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);
        
        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", Path.GetFileName(tempFileName));
    
        // Act
        var result = await _importNDCover.Import(formFile);

        // Assert
        Assert.False(result);
        
        // Clean up: Delete the temporary file
        fileStream.Close();
        File.Delete(tempFileName);
    }

    [Fact]
    public async Task Import_ShouldLogError_WhenSirenDoesNotExist()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "ndcover_test_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var formFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));
    
        var fakeSiren = "123456789";
        _mockEntrepriseBaseService.Setup(service => service.GetBySiren(fakeSiren, It.IsAny<bool>())).ReturnsAsync((EntrepriseBaseDto?)null);
        
        

        // Act
        var result = await _importNDCover.Import(formFile);

        // Assert
        _mockLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()));
        Assert.True(result);
    }

    [Fact]
    public async Task Import_ShouldCreateNDCover_WhenSirenExists()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "ndcover_test_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var formFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));
        
        var validSiren = "123456789";
        var mockDto = new EntrepriseBaseDto(); // Assuming such a DTO exists
        _mockEntrepriseBaseService.Setup(service => service.GetBySiren(validSiren, It.IsAny<bool>()))
            .ReturnsAsync(mockDto);

        // Act
        var result = await _importNDCover.Import(formFile);

        // Assert
        Assert.True(result);
        _mockEntrepriseNDCoverService.Verify(service => service.Create(It.IsAny<EntrepriseNDCoverDto>()), Times.Once);
    }

    [Fact]
    public async Task Import_ShouldLogError_IfCatchesException()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "ndcover_test_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var invalidFormFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));

        var fakeException = new Exception("Test exception");

        // Setup a dependency of the Import method to throw an exception
        _mockEntrepriseBaseService
            .Setup(service => service.GetBySiren(It.IsAny<string>(), It.IsAny<bool>()))
            .ThrowsAsync(fakeException);

        // Act
        var result = await _importNDCover.Import(invalidFormFile);

        // Assert
        _mockLogger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()));
        Assert.False(result);
    }
    
    // CheckFormat method tests
    
    [Fact]
    public async Task CheckFormat_ShouldReturnFalse_IfFileIsNull()
    {
        // Arrange
        IFormFile? file = null;

        // Act
        var result = await _importNDCover.CheckFormat(file);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task CheckFormat_ShouldReturnFalse_IfFileLengthIsZero()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(x => x.Length).Returns(0);

        // Act
        var result = await _importNDCover.CheckFormat(mockFile.Object);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task CheckFormat_ShouldReturnTrue_IfRealExcelFileIsValid()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // Arrange
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "ndcover_test_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var formFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));
        

        // Act
        var result = await _importNDCover.CheckFormat(formFile);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task CheckFormat_ShouldReturnFalse_IfRealExcelFileIsNotValid()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // Arrange
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "ndcover_test_false_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var falseFormFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));
        

        // Act
        var result = await _importNDCover.CheckFormat(falseFormFile);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task CheckFormat_ShouldReturnFalse_IfHeaderRowIsMissing()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");

        worksheet.Cell(1, 1).Value = "Data1";
        worksheet.Cell(2, 1).Value = "Data2";

        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);

        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", Path.GetFileName(tempFileName));
    
        // Act
        var result = await _importNDCover.CheckFormat(formFile);

        // Assert
        Assert.False(result);
        
        // Clean up: Delete the temporary file
        fileStream.Close();
        File.Delete(tempFileName);
    }
    
    [Fact]
    public async Task CheckFormat_ShouldReturnFalse_IfNoRowsInExcelFile()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");

        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);

        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", Path.GetFileName(tempFileName));

        // Act
        var result = await _importNDCover.CheckFormat(formFile);

        // Assert
        Assert.False(result);
    
        // Clean up: Delete the temporary file
        fileStream.Close();
        File.Delete(tempFileName);
    }
    
    [Fact]
    public async Task CheckFormat_ShouldHandleDuplicateColumnNamesCorrectly()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");
        
        worksheet.Cell(1, 1).Value = "ColumnName";
        worksheet.Cell(1, 2).Value = "UniqueColumn";
        worksheet.Cell(1, 3).Value = "ColumnName"; // Duplicate
        
        worksheet.Cell(2, 1).Value = "Data1";
        worksheet.Cell(2, 2).Value = "Data2";
        worksheet.Cell(2, 3).Value = "Data3";

        var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
        workbook.SaveAs(tempFileName);
        
        using var fileStream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", Path.GetFileName(tempFileName));
    
        // Act
        var result = await _importNDCover.CheckFormat(formFile);

        // Assert
        Assert.False(result);
        
        // Clean up: Delete the temporary file
        fileStream.Close();
        File.Delete(tempFileName);
    }

    [Fact]
    public async Task ImportNdCoverErrorAsync_ShouldHandleAllErrorCodes()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "ndcover-error-mock.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var formFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));

        // Mock setup for GetSirenFromLineNumberAsync
        _mockCommandExportSoumissionNDCoverRepository
            .Setup(x => x.GetSirenFromLineNumberAsync(It.IsAny<int>()))
            .ReturnsAsync("123456789"); // Return a test SIREN for lines 4 and 5

        // Mock setup for entreprise base service
        _mockEntrepriseBaseService
            .Setup(x => x.GetBySiren(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(new EntrepriseBaseDto { Id = 1 });

        _mockEntrepriseBaseService
            .Setup(x => x.Edit(It.IsAny<EntrepriseBaseDto>()))
            .ReturnsAsync((EntrepriseBaseDto e) => e);

        // Mock setup for etablissements
        var etablissements = new List<EtablissementClientDto> 
        { 
            new() { 
                Id = 1,
                Commercial = new CommercialDto { 
                    Id = 1,
                    UserId = 1,
                    Username = "test"
                },
                EtablissementFiche = new EtablissementFicheDto {
                    Id = 1
                }
            } 
        };

        _mockEtablissementService
            .Setup(x => x.GetEtablissementGroupBySirenAsync(It.IsAny<string>()))
            .ReturnsAsync(etablissements);

        // Capture notifications
        var context = new ContextSession();
        var capturedNotifications = new List<Notification>();

        _mockNotificationRepository
            .Setup(x => x.CreateAsync(It.IsAny<Notification>(), It.IsAny<ContextSession>(), false))
            .Callback<Notification, ContextSession, bool>((n, c, i) => capturedNotifications.Add(n))
            .ReturnsAsync((Notification n, ContextSession c, bool i) => n);

        // Act
        await _importNDCover.ImportNdCoverErrorAsync(formFile);

        // Assert
        // Verify error code 124 (company closed)
        _mockEntrepriseBaseService.Verify(x => x.GetBySiren("309574911", It.IsAny<bool>()), Times.AtLeastOnce);
        _mockEtablissementService.Verify(x => x.Edit(It.Is<EtablissementClientDto>(e => e.Radie == true)), Times.Once);
        _mockEtablissementFicheService.Verify(x => x.Edit(It.Is<EtablissementFicheDto>(e => e.EtablissementCesse == true)), Times.Once);

        // Verify error code 122 (company not found)
        _mockEntrepriseBaseService.Verify(x => x.GetBySiren("312126824", It.IsAny<bool>()), Times.AtLeastOnce);

        // Verify error codes 253 and 254 (group company errors)
        _mockCommandExportSoumissionNDCoverRepository.Verify(x => x.GetSirenFromLineNumberAsync(4), Times.Exactly(2));
        _mockCommandExportSoumissionNDCoverRepository.Verify(x => x.GetSirenFromLineNumberAsync(5), Times.Exactly(2));

        // Verify total Edit calls
        _mockEntrepriseBaseService.Verify(x => x.Edit(It.IsAny<EntrepriseBaseDto>()), Times.Exactly(5));

        // Verify notifications
        Assert.Equal(2, capturedNotifications.Count);
        Assert.Contains(capturedNotifications, n => n.Description.Contains("309574911") && n.Icon == "error");
        Assert.Contains(capturedNotifications, n => n.Description.Contains("312126824") && n.Icon == "error");
    }

    [Fact]
    public async Task ImportNdCoverErrorAsync_ShouldReturnEarly_WhenFileIsNullOrEmpty()
    {
        // Arrange
        IFormFile? nullFile = null;
        var emptyFile = new FormFile(new MemoryStream(), 0, 0, "empty", "empty.xlsx");

        // Act & Assert
        await _importNDCover.ImportNdCoverErrorAsync(nullFile);
        await _importNDCover.ImportNdCoverErrorAsync(emptyFile);

        // Verify no processing occurred
        _mockEntrepriseBaseService.Verify(x => x.Edit(It.IsAny<EntrepriseBaseDto>()), Times.Never);
        _mockEtablissementService.Verify(x => x.Edit(It.IsAny<EtablissementClientDto>()), Times.Never);
        _mockEtablissementFicheService.Verify(x => x.Edit(It.IsAny<EtablissementFicheDto>()), Times.Never);
        _mockNotificationRepository.Verify(x => x.CreateAsync(It.IsAny<Notification>(), It.IsAny<ContextSession>(), false), Times.Never);
    }

    [Fact]
    public async Task ImportNdCoverErrorAsync_ShouldThrowException_WhenRequiredColumnsAreMissing()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "ndcover-error-24700.xlsx");
        
        // Create test file with missing columns
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Sheet1");
            worksheet.Cell(1, 1).Value = "Line";
            worksheet.Cell(1, 2).Value = "CoverId";
            worksheet.Cell(1, 3).Value = "Code";
            worksheet.Cell(1, 3).Value = "Error";

            worksheet.Cell(2, 1).Value = "1";
            worksheet.Cell(2, 2).Value = "";
            worksheet.Cell(2, 3).Value = "24700";
            worksheet.Cell(2, 3).Value = "We are already monitoring this buyer for you.";
            workbook.SaveAs(filePath);
        }

        using var stream = new FileStream(filePath, FileMode.Open);
        var formFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _importNDCover.ImportNdCoverErrorAsync(formFile));
        
        Assert.Equal("Le fichier Excel ne contient pas toutes les colonnes requises : Line, CoverId, Code, Error.", 
            exception.Message);

        // Cleanup
        File.Delete(filePath);
    }

    [Fact]
    public async Task ImportNdCoverErrorAsync_ShouldDisplayMessageFor24700_WhenRequiredColumnsAreMissing()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "ndcover-error-invalid-columns.xlsx");

        // Create test file with missing columns
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Sheet1");
            worksheet.Cell(1, 1).Value = "Line"; // Missing other required columns
            worksheet.Cell(1, 2).Value = "SomeOtherColumn";
            workbook.SaveAs(filePath);
        }

        using var stream = new FileStream(filePath, FileMode.Open);
        var formFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _importNDCover.ImportNdCoverErrorAsync(formFile));

        Assert.Equal("Le fichier Excel ne contient pas toutes les colonnes requises : Line, CoverId, Code, Error.",
            exception.Message);

        // Cleanup
        File.Delete(filePath);
    }
}