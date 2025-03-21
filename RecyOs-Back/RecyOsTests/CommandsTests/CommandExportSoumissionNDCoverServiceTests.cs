// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => CommandExportSoumissionNDCoverServiceTests.cs
// Created : 2024/01/23 - 13:51
// Updated : 2024/01/23 - 13:51

using ClosedXML.Excel;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Commands;
using RecyOs.Controllers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;

namespace RecyOsTests.ORMTests.ServiceTests;

[Collection("CouvertureTests")]
public class CommandExportSoumissionNDCoverServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<ICommandExportSoumissionNDCoverRepository<EntrepriseBase>> _mockCommandExportSoumissionNDCoverRepository;
    private readonly Mock<ITokenInfoService> _mockTokenInfoService;
    private readonly CommandExportSoumissionNDCoverService _commandExportSoumissionNDCoverService;
    
    public CommandExportSoumissionNDCoverServiceTests()
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockCommandExportSoumissionNDCoverRepository = new Mock<ICommandExportSoumissionNDCoverRepository<EntrepriseBase>>();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockTokenInfoService = new Mock<ITokenInfoService>();
        _commandExportSoumissionNDCoverService = new CommandExportSoumissionNDCoverService(
            _mockConfiguration.Object,
            _mockCurrentContextProvider.Object, 
            _mockCommandExportSoumissionNDCoverRepository.Object,
            _mockTokenInfoService.Object);
    }
    
    [Fact]
    public async Task ExportSoumissionNDCoverFranceAsync_ShouldReturnXLWorkbook_WhenExportIsSuccessful()
    {
        // Arrange
        var mockEntrepriseBase = new EntrepriseBase
        {
            Siren = "123456789"
        };
        var mockEntrepriseBaseList = new List<EntrepriseBase> {mockEntrepriseBase};
        var mockConfigurationSection = new Mock<IConfigurationSection>();
        mockConfigurationSection.Setup(a => a.Value).Returns("TestValue");
        _mockConfiguration.Setup(a => a.GetSection(It.IsAny<string>())).Returns(mockConfigurationSection.Object);
        _mockCommandExportSoumissionNDCoverRepository.Setup(a => a.ExportSoumissionNDCoverFranceRepositoryAsync(It.IsAny<ContextSession>(), It.IsAny<bool>())).ReturnsAsync(mockEntrepriseBaseList);
        
        // Act
        var result = await _commandExportSoumissionNDCoverService.ExportSoumissionNDCoverFranceAsync();
        
        // Assert
        Assert.IsType<XLWorkbook>(result);
    }
}