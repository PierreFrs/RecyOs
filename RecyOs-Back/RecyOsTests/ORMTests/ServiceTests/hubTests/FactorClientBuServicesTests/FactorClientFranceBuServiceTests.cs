// <copyright file="FactorClientFranceBuServiceTests.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using AutoMapper;
using Moq;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service.hub;

namespace RecyOsTests.ORMTests.hubTests.FactorClientBuServicesTests;

[Collection("FactorClientBuTestsCollection")]

public class FactorClientFranceBuServiceTests
{
    private readonly IFactorClientFranceBuService _factorClientFranceBuService;
    private readonly Mock<IFactorClientFranceBuRepository> _factorClientFranceBuRepositoryMock;
    private readonly IMapper _mapper;
    private readonly Mock<ITokenInfoService> _tokenInfoService;
    
    public FactorClientFranceBuServiceTests()
    {
        _factorClientFranceBuRepositoryMock = new Mock<IFactorClientFranceBuRepository>();
        _tokenInfoService = new Mock<ITokenInfoService>();
        
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<FactorClientFranceBu, FactorClientFranceBuDto>().ReverseMap();
        });
        _mapper = mapperConfig.CreateMapper();
        _factorClientFranceBuService = new FactorClientFranceBuService(
            _factorClientFranceBuRepositoryMock.Object,
            _mapper,
            _tokenInfoService.Object);
    }
    
    /* Getters */
    
    [Fact]
    public async Task GetListAsync_ReturnsListOfFactorClientFranceBuDto_WhenGetListIsSuccessful()
    {
        // Arrange
        var factorClientFranceBuList = new List<FactorClientFranceBu>
        {
            new FactorClientFranceBu
            {
                IdClient = 1,
                IdBu = 1,
                CreateDate = DateTime.Now
            },
            new FactorClientFranceBu
            {
                IdClient = 1,
                IdBu = 2,
                CreateDate = DateTime.Now
            }
        };
        _factorClientFranceBuRepositoryMock
            .Setup(x => x.GetListAsync(It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(factorClientFranceBuList);
        
        // Act
        var result = await _factorClientFranceBuService.GetListAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(factorClientFranceBuList.Count, result.Count);
    }
    
    [Fact]
    public async Task GetByClientIdAsync_ReturnsListOfFactorClientFranceBuDto_WhenGetByClientIdIsSuccessful()
    {
        // Arrange
        var clientId = 1;
        var factorClientFranceBuList = new List<FactorClientFranceBu>
        {
            new FactorClientFranceBu
            {
                IdClient = 1,
                IdBu = 1,
                CreateDate = DateTime.Now
            },
            new FactorClientFranceBu
            {
                IdClient = 1,
                IdBu = 2,
                CreateDate = DateTime.Now
            }
        };
        _factorClientFranceBuRepositoryMock
            .Setup(x => x.GetByClientIdAsync(It.IsAny<ContextSession>(), It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(factorClientFranceBuList);
        
        // Act
        var result = await _factorClientFranceBuService.GetByClientIdAsync(clientId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(factorClientFranceBuList.Count, result.Count);
    }
    
    [Fact]
    public async Task GetByBuIdAsync_ReturnsListOfFactorClientFranceBuDto_WhenGetByBuIdIsSuccessful()
    {
        // Arrange
        var buId = 1;
        var factorClientFranceBuList = new List<FactorClientFranceBu>
        {
            new FactorClientFranceBu
            {
                IdClient = 1,
                IdBu = 1,
                CreateDate = DateTime.Now
            },
            new FactorClientFranceBu
            {
                IdClient = 2,
                IdBu = 1,
                CreateDate = DateTime.Now
            }
        };
        _factorClientFranceBuRepositoryMock
            .Setup(x => x.GetByBuIdAsync(It.IsAny<ContextSession>(), It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(factorClientFranceBuList);
        
        // Act
        var result = await _factorClientFranceBuService.GetByBuIdAsync(buId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(factorClientFranceBuList.Count, result.Count);
    }
    
    /* Setters */
    
    [Fact]
    public async Task CreateAsync_ReturnsFactorClientFranceBuDto_WhenCreateIsSuccessful()
    {
        // Arrange
        DateTime createDate = DateTime.Now;
        var factorClientFranceBuDto = new FactorClientFranceBuDto
        {
            IdClient = 1,
            IdBu = 1,
            CreateDate = createDate
        };
        var factorClientFranceBu = new FactorClientFranceBu
        {
            IdClient = 1,
            IdBu = 1,
            CreateDate = createDate
        };
        _factorClientFranceBuRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<FactorClientFranceBu>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(factorClientFranceBu);
        
        // Act
        var result = await _factorClientFranceBuService.CreateAsync(factorClientFranceBuDto);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(factorClientFranceBuDto.IdClient, result.IdClient);
        Assert.Equal(factorClientFranceBuDto.IdBu, result.IdBu);
        Assert.Equal(factorClientFranceBuDto.CreateDate, result.CreateDate);
    }
    
    /* Updaters */
    
    [Fact]
public async Task UpdateBatchAsync_ShouldAddAndRemoveCorrectBUs()
{
    // Arrange
    var clientId = 1;
    var existingBus = new List<FactorClientFranceBuDto>
    {
        new FactorClientFranceBuDto { IdClient = clientId, IdBu = 1, CreateDate = DateTime.Now },
        new FactorClientFranceBuDto { IdClient = clientId, IdBu = 2, CreateDate = DateTime.Now },
        new FactorClientFranceBuDto { IdClient = clientId, IdBu = 3, CreateDate = DateTime.Now }
    };

    var newBuIds = new List<int> { 2, 3, 4 };

    var request = new FactorBatchRequest
    {
        ClientId = clientId,
        BuIds = newBuIds
    };

    _factorClientFranceBuRepositoryMock
        .Setup(x => x.GetByClientIdAsync(It.IsAny<ContextSession>(), clientId, It.IsAny<bool>()))
        .ReturnsAsync(existingBus.Select(b => _mapper.Map<FactorClientFranceBu>(b)).ToList());

    var createdEntities = new List<FactorClientFranceBu>();
    _factorClientFranceBuRepositoryMock
        .Setup(x => x.CreateAsync(It.IsAny<FactorClientFranceBu>(), It.IsAny<ContextSession>()))
        .ReturnsAsync((FactorClientFranceBu entity, ContextSession session) =>
        {
            createdEntities.Add(entity);
            return entity;
        });

    var deletedIds = new List<int>();
    _factorClientFranceBuRepositoryMock
        .Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ContextSession>()))
        .Callback<int, int, ContextSession>((clientId, buId, session) => deletedIds.Add(buId))
        .ReturnsAsync(true);

    // Act
    var result = await _factorClientFranceBuService.UpdateBatchAsync(request);

    // Assert
    var addedBuIds = createdEntities.Select(e => e.IdBu).ToList();
    
    Assert.NotNull(result);
    Assert.Contains(4, addedBuIds);
    Assert.DoesNotContain(1, addedBuIds);

    _factorClientFranceBuRepositoryMock.Verify(x => x.CreateAsync(It.Is<FactorClientFranceBu>(b => b.IdBu == 4), It.IsAny<ContextSession>()), Times.Once);
    _factorClientFranceBuRepositoryMock.Verify(x => x.DeleteAsync(clientId, 1, It.IsAny<ContextSession>()), Times.Once);
    _factorClientFranceBuRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<FactorClientFranceBu>(), It.IsAny<ContextSession>()), Times.Once);
    _factorClientFranceBuRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ContextSession>()), Times.Once);
}


    
    
    /* Deleters */
    
    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenDeleteIsSuccessful()
    {
        // Arrange
        var clientId = 1;
        var buId = 1;
        _factorClientFranceBuRepositoryMock
            .Setup(x => x.DeleteAsync(clientId, buId, It.IsAny<ContextSession>()))
            .ReturnsAsync(true);

        // Act
        var result = await _factorClientFranceBuService.DeleteAsync(clientId, buId);

        // Assert
        Assert.True(result);
    }
}