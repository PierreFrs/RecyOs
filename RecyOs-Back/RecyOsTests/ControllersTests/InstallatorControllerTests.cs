using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers;
using RecyOs.Cron;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using RecyOs.ORM.Interfaces.ICron;

namespace RecyOsTests.ControllersTests;

public class InstallatorControllerTests
{
    private readonly Mock<ICommandImportFcli> _mockICommandImportFcli;
    private readonly Mock<ICommandImportCouverture> _mockImportCouverture;
    private readonly Mock<ICommandImportNDCover> _mockImportNDCover;
    private readonly Mock<ICommandExportSoumissionNDCoverService> _mockExportSoumissionNDCoverService;
    private readonly Mock<ISyncBalanceCron> _mockSyncBalanceCron;
    private readonly InstallatorControler _controller;
    public InstallatorControllerTests()
    {
        _mockICommandImportFcli = new Mock<ICommandImportFcli>();
        _mockImportCouverture = new Mock<ICommandImportCouverture>();
        _mockImportNDCover = new Mock<ICommandImportNDCover>();
        _mockExportSoumissionNDCoverService = new Mock<ICommandExportSoumissionNDCoverService>();
        _mockSyncBalanceCron = new Mock<ISyncBalanceCron>();
        _controller = new InstallatorControler(
            _mockICommandImportFcli.Object,
            _mockImportCouverture.Object,
            _mockImportNDCover.Object,
            _mockExportSoumissionNDCoverService.Object,
            _mockSyncBalanceCron.Object);
    }
    
    /********** FCLI **********/
    
    [Fact]
    public async Task ImportFcli_ShouldReturnOk_IfFileIsImported()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.Length).Returns(1024);
        _mockICommandImportFcli.Setup(service => service.Import()).Returns(Task.FromResult(true));
        // Act
        var result = await _controller.ImportFcli();
        
        // Assert
        Assert.IsType<OkResult>(result);
    }
    
    /********** Couverture Allianz Trade **********/
    
    [Fact]
    public async Task ImportCouverture_ShouldReturnOk_IfFileIsImported()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.Length).Returns(1024);
        _mockImportCouverture.Setup(service => service.Import(It.IsAny<IFormFile>())).Returns(Task.FromResult(true));
        // Act
        var result = await _controller.ImportCouverture(mockFile.Object);
        
        // Assert
        Assert.IsType<OkResult>(result);
    }
    
    [Fact]
    public async Task ImportCouverture_ShouldReturnError_IfFileIsNull()
    {
        // Arrange
        IFormFile? nullFile = null;
        
        // Act
        var result = await _controller.ImportCouverture(nullFile);
        
        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal("file not selected", contentResult.Content);
    }
    
    [Fact]
    public async Task ImportCouverture_ShouldReturnError_IfFileIsEmpty()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.Length).Returns(0);
        
        // Act
        var result = await _controller.ImportCouverture(mockFile.Object);
        
        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal("file not selected", contentResult.Content);
    }
    
    [Fact]
    public async Task CheckCouverture_ShouldReturnOk_IfFileIsConform()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.Length).Returns(1024);
        _mockImportCouverture.Setup(service => service.CheckFormat(It.IsAny<IFormFile>())).Returns(Task.FromResult(true));
        // Act
        var result = await _controller.CheckCouverture(mockFile.Object);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);

    }
    
    [Fact]
    public async Task CheckCouverture_ShouldReturnError_IfFileIsNull()
    {
        // Arrange
        IFormFile? nullFile = null;
        
        // Act
        var result = await _controller.CheckCouverture(nullFile);
        
        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal("file not selected", contentResult.Content);
    }
    
    [Fact]
    public async Task CheckCouverture_ShouldReturnError_IfFileIsEmpty()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.Length).Returns(0);
        
        // Act
        var result = await _controller.CheckCouverture(mockFile.Object);
        
        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal("file not selected", contentResult.Content);
    }
    
    /********** Couverture ND Cover **********/
    
    [Fact]
public async Task ImportNDCover_ShouldReturnOk_IfFileIsImported()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.Length).Returns(1024);
        _mockImportNDCover.Setup(service => service.Import(It.IsAny<IFormFile>())).Returns(Task.FromResult(true));
        // Act
        var result = await _controller.ImportNdCoverFile(mockFile.Object);
        
        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task ImportNDCover_ShouldReturnError_IfFileIsNull()
    {
        // Arrange
        IFormFile? nullFile = null;
        
        // Act
        var result = await _controller.ImportNdCoverFile(nullFile);
        
        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal("file not selected", contentResult.Content);
    }
    
    [Fact]
    public async Task ImportNDCover_ShouldReturnError_IfFileIsEmpty()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.Length).Returns(0);
        
        // Act
        var result = await _controller.ImportNdCoverFile(mockFile.Object);
        
        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal("file not selected", contentResult.Content);
    }
    
    [Fact]
    public async Task CheckNDCoverFile_ShouldReturnOk_IfFileIsConform()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.Length).Returns(1024);
        _mockImportNDCover.Setup(service => service.CheckFormat(It.IsAny<IFormFile>())).Returns(Task.FromResult(true));
        // Act
        var result = await _controller.CheckNdCoverFile(mockFile.Object);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task CheckNDCover_ShouldReturnError_IfFileIsNull()
    {
        // Arrange
        IFormFile? nullFile = null;
        
        // Act
        var result = await _controller.CheckNdCoverFile(nullFile);
        
        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal("file not selected", contentResult.Content);
    }
    
    [Fact]
    public async Task CheckNDCover_ShouldReturnError_IfFileIsEmpty()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.Length).Returns(0);
        
        // Act
        var result = await _controller.CheckNdCoverFile(mockFile.Object);
        
        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal("file not selected", contentResult.Content);
    }
    
    /********** Export ND Cover **********/
    
    [Fact]
    public async Task ExportNDCoverFileFrance_ShouldReturnExcelFile_IfFileIsExported()
    {
        // Arrange
        var workbook = new XLWorkbook();
        workbook.Worksheets.Add("Data");
        _mockExportSoumissionNDCoverService
            .Setup(service => service.ExportSoumissionNDCoverFranceAsync())
            .ReturnsAsync(workbook);
        
        // Act
        var result = await _controller.ExportNdCoverFileFrance();
        
        // Assert
        Assert.IsType<FileContentResult>(result);
    }
    
    /********** Import MKGT Client **********/
    
    [Fact]
    public async Task ImportMKGTClient_ShouldReturnOk_IfClientIsImported()
    {
        // Arrange
        var code = "test";
        _mockICommandImportFcli.Setup(service => service.Import(It.IsAny<string>())).Returns(Task.FromResult(true));
        // Act
        var result = await _controller.ImportMkgtClient(code);
        
        // Assert
        Assert.IsType<OkResult>(result);
    }
    
    [Fact]
    public async Task ImportMKGTClient_ShouldReturnNotFound_IfResultIsFalse()
    {
        // Arrange
        var code = "test";
        _mockICommandImportFcli.Setup(service => service.Import(It.IsAny<string>())).Returns(Task.FromResult(false));
        // Act
        var result = await _controller.ImportMkgtClient(code);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    /********** ImportNdCoverError **********/

    [Fact]
    public async Task ImportNdCoverError_ShouldReturnOk_IfFileIsImported()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(_ => _.Length).Returns(1024);
        _mockImportNDCover.Setup(service => service.ImportNdCoverErrorAsync(It.IsAny<IFormFile>())).Returns(Task.FromResult(true));
        // Act
        var result = await _controller.ImportNdCoverError(mockFile.Object);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task ImportNdCoverError_ShouldReturnError_IfFileIsNull()
    {
        // Arrange
        IFormFile? nullFile = null;

        // Act
        var result = await _controller.ImportNdCoverError(nullFile);

        // Assert
        var contentResult = Assert.IsType<ContentResult>(result);
        Assert.Equal("file not selected", contentResult.Content);
    }
}