using AutoMapper;
using Moq;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Services;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.MapProfile;

namespace RecyOsTests.EngineTests;

/// <summary>
/// Unit tests for the EngineEuropeClientService class.
/// </summary>
public class EngineEuropeClientServiceTests
{
    private readonly Mock<IEngineEuropeClientRepository> _mockRepository;
    private readonly IMapper Mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="EngineEuropeClientServiceTests"/> class.
    /// </summary>
    public EngineEuropeClientServiceTests()
    {
        _mockRepository = new Mock<IEngineEuropeClientRepository>();
        Mapper = new MapperConfiguration(cfg => { cfg.AddProfile(new ClientEuropeProfile()); }).CreateMapper();
    }

    /// <summary>
    /// Verifies that the GetCreatedItems method calls the GetCreatedEntities method in the repository.
    /// </summary>
    [Fact]
    public void GetCreatedItems_ShouldCallRepositoryMethod()
    {
        // Arrange
        var service = new EngineEuropeClientService(_mockRepository.Object, Mapper);
        var moduleName = "ModuleTest";

        // Act
        service.GetCreatedItems(moduleName);

        // Assert
        _mockRepository.Verify(repo => repo.GetCreatedEntities(moduleName), Times.Once);
    }

    /// <summary>
    /// This method tests the behavior of the GetUpdatedItems method in the EngineEuropeClientService.
    /// It verifies that the method calls the GetUpdatedEntities method in the repository once.
    /// </summary> 
    [Fact]
    public void GetUpdatedItems_ShouldCallRepositoryMethod()
    {
        // Arrange
        var service = new EngineEuropeClientService(_mockRepository.Object, Mapper);
        var moduleName = "ModuleTest";

        // Act
        service.GetUpdatedItems(moduleName);

        // Assert
        _mockRepository.Verify(repo => repo.GetUpdatedEntities(moduleName), Times.Once);
    }
    
    /// <summary>
    /// This method tests the behavior of the CallBackDestIdCreation method in the EngineEuropeClientService.
    /// It verifies that the method calls the CallBackDestIdCreation method in the repository once.
    /// </summary>
    [Fact]
    public void CallBackDestIdCreation_ShouldCallRepositoryMethod()
    {
        // Arrange
        var service = new EngineEuropeClientService(_mockRepository.Object, Mapper);
        var moduleName = "ModuleTest";
        var items = new List<ClientEuropeDto>();

        // Act
        service.CallBackDestIdCreation(moduleName, items);

        // Assert
        _mockRepository.Verify(repo => repo.CallBackDestIdCreation(moduleName, It.IsAny<IList<ClientEurope>>()), Times.Once);
    }
    
}