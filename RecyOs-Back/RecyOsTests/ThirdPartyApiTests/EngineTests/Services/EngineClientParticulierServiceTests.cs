using AutoMapper;
using Moq;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Services;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.MapProfile;
using RecyOs.ORM.Models.DTO.hub;

namespace RecyOsTests.ThirdPartyApiTests.EngineTests.Services;

public class EngineClientParticulierServiceTests
{
    private readonly Mock<IEngineClientParticulierRepository> _mockRepository;
    private readonly IMapper Mapper;
    
    public EngineClientParticulierServiceTests()
    {
        _mockRepository = new Mock<IEngineClientParticulierRepository>();
        Mapper = new MapperConfiguration(cfg => { cfg.AddProfile(new ClientParticulierProfile()); }).CreateMapper();
    }
    
    /// <summary>
    /// Verifies that the GetCreatedItems method calls the GetCreatedEntities method in the repository.
    /// </summary>
    [Fact]
    public void GetCreatedItems_ShouldCallRepositoryMethod()
    {
        // Arrange
        var service = new EngineClientParticulierService(_mockRepository.Object, Mapper);
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
        var service = new EngineClientParticulierService(_mockRepository.Object, Mapper);
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
        var service = new EngineClientParticulierService(_mockRepository.Object, Mapper);
        var moduleName = "ModuleTest";
        var items = new List<ClientParticulierDto>();
        
        // Act
        service.CallBackDestIdCreation(moduleName, items);
        
        // Assert
        _mockRepository.Verify(repo => repo.CallBackDestIdCreation(moduleName, It.IsAny<List<ClientParticulier>>()), Times.Once);
    }
}