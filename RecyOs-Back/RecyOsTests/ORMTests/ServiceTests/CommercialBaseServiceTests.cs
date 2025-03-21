// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => CommercialServiceTests.cs
// Created : 2024/03/27 - 09:01
// Updated : 2024/03/27 - 09:01

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOsTests.ORMTests.ServiceTests;

[Collection("CommercialTestsCollection")]
public class CommercialBaseServiceTests
{
    private readonly CommercialBaseService _commercialBaseService;
    private readonly IMapper _mapper;
    private readonly Mock<ITokenInfoService> _mockTokenInfoService;
    private readonly Mock<ICurrentContextProvider> _mockContextProvider; 
    private readonly Mock<ICommercialBaseRepository> _mockCommercialRepository;
    public CommercialBaseServiceTests()
    {
        _mockContextProvider = new Mock<ICurrentContextProvider>();
        _mockCommercialRepository = new Mock<ICommercialBaseRepository>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Commercial, CommercialDto>().ReverseMap();
            cfg.CreateMap<EtablissementClient, EtablissementClientDto>();
            cfg.CreateMap<ClientEurope, ClientEuropeDto>();
        });
        _mapper = mapperConfig.CreateMapper();
        _mockTokenInfoService = new Mock<ITokenInfoService>();
        _commercialBaseService = new CommercialBaseService(
            _mockContextProvider.Object,
            _mockCommercialRepository.Object,
            _mapper,
            _mockTokenInfoService.Object);
    }
    
    /********** Getters **********/
    [Fact]
    public async Task GetListAsync_ReturnsList()
    {
        // Arrange
        var commercialList = new List<Commercial>
        {
            new Commercial(),
            new Commercial()
        };
        _mockCommercialRepository.Setup(x => x.GetListAsync(It.IsAny<bool>())).ReturnsAsync(commercialList);
        
        // Act
        var result = await _commercialBaseService.GetListAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task GetByIdAsync_ReturnsCommercial()
    {
        // Arrange
        var commercial = new Commercial();
        _mockCommercialRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>())).ReturnsAsync(commercial);
        
        // Act
        var result = await _commercialBaseService.GetByIdAsync(1);
        
        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetClientsByCommercialIdAsync_ReturnsClientCompositeDto()
    {
        // Arrange
        var clients = new List<object>();
        var count = 2;
        _mockCommercialRepository.Setup(x => x.GetClientsByCommercialIdAsyncWithCount(It.IsAny<int>(), It.IsAny<ClientByCommercialFilter>())).ReturnsAsync((clients, count));
        
        // Act
        var result = await _commercialBaseService.GetClientsByCommercialIdAsync(1, new ClientByCommercialFilter());
        
        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetClientBByCommercialId_ShouldMapToEtablissementClientDto_WhenClientIsEtablissementClient()
    {
        // Arrange
        var clients = new List<object>
        {
            new EtablissementClient()
        };
        var count = 1;
        _mockCommercialRepository.Setup(x => x.GetClientsByCommercialIdAsyncWithCount(It.IsAny<int>(), It.IsAny<ClientByCommercialFilter>())).ReturnsAsync((clients, count));
        
        // Act
        var result = await _commercialBaseService.GetClientsByCommercialIdAsync(1, new ClientByCommercialFilter());
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementClientDto>(result.Items.First());
    }
    
    [Fact]
    public async Task GetClientBByCommercialId_ShouldMapToClientEuropeDto_WhenClientIsClientEurope()
    {
        // Arrange
        var clients = new List<object>
        {
            new ClientEurope()
        };
        var count = 1;
        _mockCommercialRepository.Setup(x => x.GetClientsByCommercialIdAsyncWithCount(It.IsAny<int>(), It.IsAny<ClientByCommercialFilter>())).ReturnsAsync((clients, count));
        
        // Act
        var result = await _commercialBaseService.GetClientsByCommercialIdAsync(1, new ClientByCommercialFilter());
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<ClientEuropeDto>(result.Items.First());
    }
    
    [Fact]
    public async Task GetFilteredListAsync_ReturnsGridData()
    {
        // Arrange
        var commercialList = new List<Commercial>
        {
            new Commercial(),
            new Commercial()
        };
        var filter = new CommercialFilter();
        _mockCommercialRepository.Setup(x => x.GetFilteredListAsync(It.IsAny<CommercialFilter>(), It.IsAny<ContextSession>(), It.IsAny<bool>())).ReturnsAsync((commercialList, 2));
        
        // Act
        var result = await _commercialBaseService.GetFilteredListAsync(filter);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count());
    }
    
    /********** Create **********/
    [Fact]
    public async Task CreateAsync_ReturnsCommercial()
    {
        // Arrange
        var commercialDtoCreate = new CommercialDto();
        var commercial = new Commercial();
        _mockTokenInfoService.Setup(x => x.GetCurrentUserName()).Returns("Test");
        _mockCommercialRepository.Setup(x => x.CreateAsync(It.IsAny<Commercial>(), It.IsAny<ContextSession>())).ReturnsAsync(commercial);
        
        // Act
        var result = await _commercialBaseService.CreateAsync(commercialDtoCreate);
        
        // Assert
        Assert.NotNull(result);
    }
    
    /********** Update **********/
    [Fact]
    public async Task Update_ReturnsCommercial()
    {
        // Arrange
        var commercialDtoUpdate = new CommercialDto();
        var commercial = new Commercial();
        _mockTokenInfoService.Setup(x => x.GetCurrentUserName()).Returns("Test");
        _mockCommercialRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>())).ReturnsAsync(commercial);
        _mockCommercialRepository.Setup(x => x.UpdateAsync(It.IsAny<Commercial>(), It.IsAny<ContextSession>())).ReturnsAsync(commercial);

        // Act
        var result = await _commercialBaseService.UpdateAsync(1, commercialDtoUpdate);

        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task Update_ReturnsNull()
    {
        // Arrange
        var commercialDtoUpdate = new CommercialDto();
        _mockTokenInfoService.Setup(x => x.GetCurrentUserName()).Returns("Test");
        _mockCommercialRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>())).ReturnsAsync((Commercial)null);
        
        // Act
        var result = await _commercialBaseService.UpdateAsync(1, commercialDtoUpdate);
        
        // Assert
        Assert.Null(result);
    }
    
    /********** Delete **********/
    [Fact]
    public async Task Delete_ReturnsTrue()
    {
        // Arrange
        _mockCommercialRepository.Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<ContextSession>())).ReturnsAsync(true);
        
        // Act
        var result = await _commercialBaseService.DeleteAsync(1);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task Delete_ReturnsFalse()
    {
        // Arrange
        _mockCommercialRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>())).ReturnsAsync((Commercial)null);
        
        // Act
        var result = await _commercialBaseService.DeleteAsync(1);
        
        // Assert
        Assert.False(result);
    }
    
    
    
    

}