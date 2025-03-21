using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using RecyOs.Commands;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces.hub;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace RecyOsTests.CommandsTests;

[Collection("CouvertureTests")]
public class ImportCouvertureTests
{
    private readonly Mock<IEntrepriseCouvertureService> _mockEntrepriseCouvertureService;
    private readonly Mock<IEntrepriseBaseService> _mockEntrepriseBaseService;
    private readonly ImportCouverture _importCouverture;
    private readonly Mock<ILogger<ImportCouverture>> _mockLogger;

    public ImportCouvertureTests()
    {
        _mockEntrepriseCouvertureService = new Mock<IEntrepriseCouvertureService>();
        _mockEntrepriseBaseService = new Mock<IEntrepriseBaseService>();
        _mockLogger = new Mock<ILogger<ImportCouverture>>();
        _importCouverture = new ImportCouverture(_mockEntrepriseCouvertureService.Object, _mockEntrepriseBaseService.Object, _mockLogger.Object);
    }
    
    // Import method tests
    
    [Fact]
    public async Task Import_ShouldReturnFalse_IfFileIsNull()
    {
        // Arrange
        IFormFile? file = null;

        // Act
        var result = await _importCouverture.Import(file);

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
        var result = await _importCouverture.Import(mockFile.Object);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task Import_ShouldReturnTrue_IfRealExcelFileIsValid()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // Arrange
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "couverture_test_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var formFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));
        

        // Act
        var result = await _importCouverture.Import(formFile);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task Import_ShouldReturnFalse_IfRealExcelFileIsNotValid()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // Arrange
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "couverture_test_false_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var falseFormFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));
        

        // Act
        var result = await _importCouverture.Import(falseFormFile);

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
        var result = await _importCouverture.Import(formFile);

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
        var result = await _importCouverture.Import(formFile);

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
        var result = await _importCouverture.Import(formFile);

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
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "couverture_test_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var formFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));
    
        var fakeSiren = "123456789";
        _mockEntrepriseBaseService.Setup(service => service.GetBySiren(fakeSiren, It.IsAny<bool>())).ReturnsAsync((EntrepriseBaseDto?)null);
        
        

        // Act
        var result = await _importCouverture.Import(formFile);

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
    public async Task Import_ShouldCreateCouverture_WhenSirenExists()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "couverture_test_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var formFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));
        
        var validSiren = "123456789";
        var mockDto = new EntrepriseBaseDto(); // Assuming such a DTO exists
        _mockEntrepriseBaseService.Setup(service => service.GetBySiren(validSiren, It.IsAny<bool>()))
            .ReturnsAsync(mockDto);

        // Act
        var result = await _importCouverture.Import(formFile);

        // Assert
        Assert.True(result);
        _mockEntrepriseCouvertureService.Verify(service => service.Create(It.IsAny<EntrepriseCouvertureDto>()), Times.Once);
    }

    [Fact]
    public async Task Import_ShouldLogError_IfCatchesException()
    {
        // Arrange
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "couverture_test_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var invalidFormFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));

        var fakeException = new Exception("Test exception");

        // Setup a dependency of the Import method to throw an exception
        _mockEntrepriseBaseService
            .Setup(service => service.GetBySiren(It.IsAny<string>(), It.IsAny<bool>()))
            .ThrowsAsync(fakeException);

        // Act
        var result = await _importCouverture.Import(invalidFormFile);

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
        var result = await _importCouverture.CheckFormat(file);

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
        var result = await _importCouverture.CheckFormat(mockFile.Object);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task CheckFormat_ShouldReturnTrue_IfRealExcelFileIsValid()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // Arrange
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "couverture_test_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var formFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));
        

        // Act
        var result = await _importCouverture.CheckFormat(formFile);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task CheckFormat_ShouldReturnFalse_IfRealExcelFileIsNotValid()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // Arrange
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData", "couverture_test_false_data.xlsx");
        using var stream = new FileStream(filePath, FileMode.Open);
        var falseFormFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(filePath));
        

        // Act
        var result = await _importCouverture.CheckFormat(falseFormFile);

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
        var result = await _importCouverture.CheckFormat(formFile);

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
        var result = await _importCouverture.CheckFormat(formFile);

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
        var result = await _importCouverture.CheckFormat(formFile);

        // Assert
        Assert.False(result);
        
        // Clean up: Delete the temporary file
        fileStream.Close();
        File.Delete(tempFileName);
    }
}