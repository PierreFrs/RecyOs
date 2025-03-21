// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => BaseGenericCategorieClientServiceTests.cs
// Created : 2023/12/26 - 15:07
// Updated : 2023/12/26 - 15:07

using AutoMapper;
using Moq;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOsTests.ORMTests.ServiceTests;

[Collection("CategorieClientTests")]
public class CategorieClientServiceTests
{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<ICategorieClientRepository<CategorieClient>> _mockRepository;
    private readonly IMapper _mapper;
    private readonly Mock<ITokenInfoService> _mockTokenInfoService;
    private readonly CategorieClientService<CategorieClient, CategorieClientDto> _service;

    public CategorieClientServiceTests()
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockRepository = new Mock<ICategorieClientRepository<CategorieClient>>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CategorieClient, CategorieClientDto>();
        });
        _mapper = mapperConfig.CreateMapper();
        _mockTokenInfoService = new Mock<ITokenInfoService>();
        _service = new CategorieClientService<CategorieClient, CategorieClientDto>(
            _mockCurrentContextProvider.Object, 
            _mockRepository.Object, 
            _mapper, 
            _mockTokenInfoService.Object);
        
    }
    
    [Fact]
    public async Task CreateTypeAsync_ReturnsCategorieClientDto_WhenCreationIsSuccessful()
    {
        var categorieLabel = "SampleCategorieLabel";
        var mockUser = "TestUser";
        var mockCategorieClient = new CategorieClient
        {
            CategorieLabel = categorieLabel,
            CreateDate = DateTime.Now,
            CreatedBy = mockUser
        };
        
        // Arrange
        _mockTokenInfoService.Setup(service => service.GetCurrentUserName()).Returns(mockUser);
        _mockRepository.Setup(repo => repo.CreateCategoryAsync(It.IsAny <CategorieClient>()))
            .ReturnsAsync(mockCategorieClient);
        
        // Act
        var result = await _service.CreateCategoryAsync(categorieLabel);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CategorieClientDto>(result);
    }
[Fact]
    public async Task GetAllAsync_ReturnsCategorieClientDtoList_WhenRequestIsSuccessful()
    {
        var categorieLabel = "SampleCategorieLabel";
        var mockUser = "TestUser";
        
        // Arrange
        var mockTypeDocuments = new List<CategorieClient>
        {
            new CategorieClient
            {
                CategorieLabel = categorieLabel,
                CreateDate = DateTime.Now,
                CreatedBy = mockUser
            },
            new CategorieClient
            {
                CategorieLabel = categorieLabel,
                CreateDate = DateTime.Now,
                CreatedBy = mockUser
            },
        };
        
        _mockRepository.Setup((repo => repo.GetListAsync(It.IsAny<ContextSession>(), It.IsAny<bool>()))).ReturnsAsync(mockTypeDocuments);
        
        var mockTypeDocumentDtos = _mapper.Map<List<CategorieClientDto>>(mockTypeDocuments);
        
        // Act
        var result = await _service.GetListAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<CategorieClientDto>>(result);
        Assert.Equal(mockTypeDocumentDtos.Count, result.Count);
    }
[Fact]
    public async Task GetByIdAsync_ReturnsTypeDocumentDto_WhenRequestIsSuccessful()
    {
        var categorieLabel = "SampleCategorieLabel";
        var mockUser = "TestUser";
        var id = 1;
        var mockTypeDocument = new CategorieClient
        {
            CategorieLabel = categorieLabel,
            CreateDate = DateTime.Now,
            CreatedBy = mockUser
        };
        
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(id, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(mockTypeDocument);
        
        // Act
        var result = await _service.GetByIdAsync(id);
        var dtoResult = Assert.IsType<CategorieClientDto>(result);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<CategorieClientDto>(result);
        Assert.Equal(mockTypeDocument.CategorieLabel, dtoResult.CategorieLabel);
        Assert.Equal(mockTypeDocument.CreateDate, dtoResult.CreateDate);
        Assert.Equal(mockTypeDocument.CreatedBy, dtoResult.CreatedBy);
    }
[Fact]
    public async Task UpdateAsync_ReturnsUpdatedTypeDocumentDto_WhenItemExists()
    {
        // Arrange
        var id = 1;
        var newCategorieLabel = "UdatedCategorieLabel";
        var mockUser = "TestUser";
        var mockTypeDocument = new CategorieClient
        {
            CategorieLabel = "CategorieLabel",
            CreateDate = DateTime.Now,
            CreatedBy = mockUser
        };
        var updatedMockTypeDocument = new CategorieClient
        {
            CategorieLabel = newCategorieLabel,
            CreateDate = DateTime.Now,
            CreatedBy = mockUser
        };
        
        _mockTokenInfoService.Setup(service => service.GetCurrentUserName()).Returns(mockUser);
        _mockRepository.Setup(repo => repo.GetByIdAsync(id, It.IsAny<ContextSession>(), It.IsAny<bool>())).ReturnsAsync(mockTypeDocument);
        _mockRepository.Setup(repo => repo.UpdateAsync(mockTypeDocument, It.IsAny<ContextSession>()))
            .ReturnsAsync(updatedMockTypeDocument);
        
        // Act
        var result = await _service.UpdateCategorieClientAsync(id, newCategorieLabel);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CategorieClientDto>(result);
        Assert.Equal(newCategorieLabel, result.CategorieLabel);
    }
[Fact]
    public async Task UpdateAsync_returnsNull_WhenItemDoesNotExist()
    {
        // Arrange
        var id = 1;
        var newCategorieLabel = "UpdatedCategorieLabel";
        
        _mockRepository.Setup(repo => repo.GetByIdAsync(id, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync((CategorieClient?)null);
        
        // Act
        var result = await _service.UpdateCategorieClientAsync(id, newCategorieLabel);

        // Assert
        Assert.Null(result);
    }
[Fact]
    public async Task DeleteAsync_returnsTrue_WhenRequestIsSuccessful()
    {
        // Arrange
        var id = 1;
        var mockTypeDocument = new CategorieClient
        {
            CategorieLabel = "CategorieLabel",
            CreateDate = DateTime.Now,
            CreatedBy = "mockUser"
        };
        
        _mockRepository.Setup(repo => repo.GetByIdAsync(id, It.IsAny<ContextSession>(), It.IsAny<bool>())).ReturnsAsync(mockTypeDocument);
        _mockRepository.Setup(repo => repo.DeleteAsync(id, It.IsAny<ContextSession>())).ReturnsAsync(true);
        
        // Act
        var result = await _service.DeleteAsync(id);
        
        // Assert
        Assert.True(result);
        _mockRepository.Verify(repo => repo.DeleteAsync(id, It.IsAny<ContextSession>()), Times.Once());
    }
    
[Fact]
    public async Task DeleteAsync_returnsFalse_WhenItemDoesNotExist()
    {
        // Arrange
        var id = 1;
        
        _mockRepository.Setup(repo => repo.DeleteAsync(id, It.IsAny<ContextSession>())).ReturnsAsync(false);
        
        // Act
        var result = await _service.DeleteAsync(id);
        
        // Assert
        Assert.False(result);
        _mockRepository.Verify(repo => repo.DeleteAsync(id, It.IsAny<ContextSession>()), Times.Never);
        
    }
}