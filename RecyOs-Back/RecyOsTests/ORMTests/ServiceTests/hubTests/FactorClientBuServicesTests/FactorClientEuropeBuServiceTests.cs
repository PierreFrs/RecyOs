// <copyright file="FactorClientEuropeBuServiceTests.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using AutoMapper;
using Moq;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;
using RecyOs.ORM.Service.hub;

namespace RecyOsTests.ORMTests.hubTests.FactorClientBuServicesTests;

[Collection("FactorClientBuTestsCollection")]

public class FactorClientEuropeBuServiceTests
{
    private readonly IFactorClientEuropeBuService _factorClientEuropeBuService;
    private readonly Mock<IFactorClientEuropeBuRepository> _factorClientEuropeBuRepositoryMock;
    private readonly IMapper _mapper;
    private readonly Mock<ITokenInfoService> _tokenInfoService;
    
    public FactorClientEuropeBuServiceTests()
    {
        _factorClientEuropeBuRepositoryMock = new Mock<IFactorClientEuropeBuRepository>();
        _tokenInfoService = new Mock<ITokenInfoService>();
        
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<FactorClientEuropeBu, FactorClientEuropeBuDto>().ReverseMap();
        });
        _mapper = mapperConfig.CreateMapper();
        _factorClientEuropeBuService = new FactorClientEuropeBuService(
            _factorClientEuropeBuRepositoryMock.Object,
            _mapper,
            _tokenInfoService.Object);
    }
    
    /* Getters */
    
    [Fact]
    public async Task GetListAsync_ReturnsListOfFactorClientEuropeBuDto_WhenGetListIsSuccessful()
    {
        // Arrange
        var factorClientEuropeBuList = new List<FactorClientEuropeBu>
        {
            new FactorClientEuropeBu
            {
                IdClient = 1,
                IdBu = 1,
                CreateDate = DateTime.Now
            },
            new FactorClientEuropeBu
            {
                IdClient = 1,
                IdBu = 2,
                CreateDate = DateTime.Now
            }
        };
        _factorClientEuropeBuRepositoryMock
            .Setup(x => x.GetListAsync(It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(factorClientEuropeBuList);
        
        // Act
        var result = await _factorClientEuropeBuService.GetListAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(factorClientEuropeBuList.Count, result.Count);
    }
    
    [Fact]
    public async Task GetByClientIdAsync_ReturnsListOfFactorClientEuropeBuDto_WhenGetByClientIdIsSuccessful()
    {
        // Arrange
        var clientId = 1;
        var factorClientEuropeBuList = new List<FactorClientEuropeBu>
        {
            new FactorClientEuropeBu
            {
                IdClient = 1,
                IdBu = 1,
                CreateDate = DateTime.Now
            },
            new FactorClientEuropeBu
            {
                IdClient = 1,
                IdBu = 2,
                CreateDate = DateTime.Now
            }
        };
        _factorClientEuropeBuRepositoryMock
            .Setup(x => x.GetByClientIdAsync(It.IsAny<ContextSession>(), It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(factorClientEuropeBuList);
        
        // Act
        var result = await _factorClientEuropeBuService.GetByClientIdAsync(clientId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(factorClientEuropeBuList.Count, result.Count);
    }
    
    [Fact]
    public async Task GetByBuIdAsync_ReturnsListOfFactorClientEuropeBuDto_WhenGetByBuIdIsSuccessful()
    {
        // Arrange
        var buId = 1;
        var factorClientEuropeBuList = new List<FactorClientEuropeBu>
        {
            new FactorClientEuropeBu
            {
                IdClient = 1,
                IdBu = 1,
                CreateDate = DateTime.Now
            },
            new FactorClientEuropeBu
            {
                IdClient = 2,
                IdBu = 1,
                CreateDate = DateTime.Now
            }
        };
        _factorClientEuropeBuRepositoryMock
            .Setup(x => x.GetByBuIdAsync(It.IsAny<ContextSession>(), It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(factorClientEuropeBuList);
        
        // Act
        var result = await _factorClientEuropeBuService.GetByBuIdAsync(buId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(factorClientEuropeBuList.Count, result.Count);
    }
    
    /* Setters */
    
    [Fact]
    public async Task CreateAsync_ReturnsFactorClientEuropeBuDto_WhenCreateIsSuccessful()
    {
        // Arrange
        DateTime createDate = DateTime.Now;
        var factorClientEuropeBuDto = new FactorClientEuropeBuDto
        {
            IdClient = 1,
            IdBu = 1,
            CreateDate = createDate
        };
        var factorClientEuropeBu = new FactorClientEuropeBu
        {
            IdClient = 1,
            IdBu = 1,
            CreateDate = createDate
        };
        _factorClientEuropeBuRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<FactorClientEuropeBu>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(factorClientEuropeBu);
        
        // Act
        var result = await _factorClientEuropeBuService.CreateAsync(factorClientEuropeBuDto);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(factorClientEuropeBuDto.IdClient, result.IdClient);
        Assert.Equal(factorClientEuropeBuDto.IdBu, result.IdBu);
        Assert.Equal(factorClientEuropeBuDto.CreateDate, result.CreateDate);
    }
    
    /* Updaters */
    
    [Fact]
    public async Task UpdateBatchAsync_ShouldAddAndRemoveCorrectBUs()
    {
        // Arrange
        var clientId = 1;
        var existingBus = new List<FactorClientEuropeBuDto>
        {
            new FactorClientEuropeBuDto { IdClient = clientId, IdBu = 1, CreateDate = DateTime.Now, CreatedBy = "test" },
            new FactorClientEuropeBuDto { IdClient = clientId, IdBu = 2, CreateDate = DateTime.Now, CreatedBy = "test" },
            new FactorClientEuropeBuDto { IdClient = clientId, IdBu = 3, CreateDate = DateTime.Now, CreatedBy = "test" }
        };

        var newBuIds = new List<int> { 2, 3, 4 };

        var request = new FactorBatchRequest
        {
            ClientId = clientId,
            BuIds = newBuIds
        };

        _factorClientEuropeBuRepositoryMock
            .Setup(x => x.GetByClientIdAsync(It.IsAny<ContextSession>(), clientId, It.IsAny<bool>()))
            .ReturnsAsync(existingBus.Select(b => _mapper.Map<FactorClientEuropeBu>(b)).ToList());

        _tokenInfoService
            .Setup(x => x.GetCurrentUserName())
            .Returns("test");
        
        var createdEntities = new List<FactorClientEuropeBu>();
        _factorClientEuropeBuRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<FactorClientEuropeBu>(), It.IsAny<ContextSession>()))
            .ReturnsAsync((FactorClientEuropeBu entity, ContextSession session) =>
            {
                createdEntities.Add(entity);
                return entity;
            });

        var deletedIds = new List<int>();
        _factorClientEuropeBuRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ContextSession>()))
            .Callback<int, int, ContextSession>((clientId, buId, session) => deletedIds.Add(buId))
            .ReturnsAsync(true);

        // Act
        var result = await _factorClientEuropeBuService.UpdateBatchAsync(request);

        // Assert
        var addedBuIds = createdEntities.Select(e => e.IdBu).ToList();
        
        Assert.NotNull(result);
        Assert.Contains(4, addedBuIds);
        Assert.DoesNotContain(1, addedBuIds);

        _factorClientEuropeBuRepositoryMock.Verify(x => x.CreateAsync(It.Is<FactorClientEuropeBu>(b => b.IdBu == 4), It.IsAny<ContextSession>()), Times.Once);
        _factorClientEuropeBuRepositoryMock.Verify(x => x.DeleteAsync(clientId, 1, It.IsAny<ContextSession>()), Times.Once);
        _factorClientEuropeBuRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<FactorClientEuropeBu>(), It.IsAny<ContextSession>()), Times.Once);
        _factorClientEuropeBuRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ContextSession>()), Times.Once);
    }
}