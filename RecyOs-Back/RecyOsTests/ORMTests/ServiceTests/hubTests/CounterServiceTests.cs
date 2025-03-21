using AutoMapper;
using Moq;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service.hub;

namespace RecyOsTests.ORMTests.hubTests;

public class CounterServiceTests
{
   
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    
    
    public CounterServiceTests()
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
    }

    [Fact]
    public async Task GetCounterById_WhenCalled_ReturnsCounter()
    {
        // Arrange
        var counter = new Counter { Id = 1, Name = "TestCounter", Value = 0 };
        var counterDto = new CounterDto { Id = 1, Name = "TestCounter", Value = 0 };
        var mockRepository = new Mock<ICounterRepository<Counter>>();
        var mockMapper = new Mock<IMapper>();
        mockRepository.Setup(x => x.GetCounterById(It.IsAny<int>(), It.IsAny<ContextSession>())).ReturnsAsync(counter);
        mockMapper.Setup(x => x.Map<CounterDto>(It.IsAny<Counter>())).Returns(counterDto);
        var service = new CounterService<Counter>(_mockCurrentContextProvider.Object, mockRepository.Object,
            mockMapper.Object);
        // Act
        var result = await service.GetCounterById(1);
        // Assert
        Assert.NotNull(result);
        Assert.IsType<CounterDto>(result);
        Assert.Equal(counterDto.Id, result.Id);
        Assert.Equal(counterDto.Name, result.Name);
        Assert.Equal(counterDto.Value, result.Value);
    }
    
    [Fact]
    public async Task GetCounterByName_WhenCalled_ReturnsCounter()
    {
        // Arrange
        var counter = new Counter { Id = 1, Name = "TestCounter", Value = 0 };
        var counterDto = new CounterDto { Id = 1, Name = "TestCounter", Value = 0 };
        var mockRepository = new Mock<ICounterRepository<Counter>>();
        var mockMapper = new Mock<IMapper>();
        mockRepository.Setup(x => x.GetCounterByName(It.IsAny<string>(), It.IsAny<ContextSession>())).ReturnsAsync(counter);
        mockMapper.Setup(x => x.Map<CounterDto>(It.IsAny<Counter>())).Returns(counterDto);
        var service = new CounterService<Counter>(_mockCurrentContextProvider.Object, mockRepository.Object,
            mockMapper.Object);
        // Act
        var result = await service.GetCounterByName("TestCounter");
        // Assert
        Assert.NotNull(result);
        Assert.IsType<CounterDto>(result);
        Assert.Equal(counterDto.Id, result.Id);
        Assert.Equal(counterDto.Name, result.Name);
        Assert.Equal(counterDto.Value, result.Value);
    }
    
    [Fact]
    public async Task IncrementCounterByName_WhenCalled_ReturnsCounter()
    {
        // Arrange
        var counter = new Counter { Id = 1, Name = "TestCounter", Value = 0 };
        var counterDto = new CounterDto { Id = 1, Name = "TestCounter", Value = 1 };
        var mockRepository = new Mock<ICounterRepository<Counter>>();
        var mockMapper = new Mock<IMapper>();
        mockRepository.Setup(x => x.IncrementCounterByName(It.IsAny<string>(), It.IsAny<ContextSession>())).ReturnsAsync(counter);
        mockMapper.Setup(x => x.Map<CounterDto>(It.IsAny<Counter>())).Returns(counterDto);
        var service = new CounterService<Counter>(_mockCurrentContextProvider.Object, mockRepository.Object,
            mockMapper.Object);
        // Act
        var result = await service.IncrementCounterByName("TestCounter");
        // Assert
        Assert.NotNull(result);
        Assert.IsType<CounterDto>(result);
        Assert.Equal(counterDto.Id, result.Id);
        Assert.Equal(counterDto.Name, result.Name);
        Assert.Equal(counterDto.Value, result.Value);
    }

}