using AutoMapper;
using Moq;
using RecyOs.Engine.Alerts.DTO;
using RecyOs.Engine.Alerts.Entities;
using RecyOs.Engine.Alerts.Interfaces;
using RecyOs.Engine.Alerts.Repositories;
using RecyOs.Engine.Alerts.Services;
using TaskStatus = RecyOs.Engine.Alerts.Entities.TaskStatus;

namespace RecyOsTests.EngineTests.Services;

public class EngineMessageMailServiceTests
{
    private readonly Mock<IEngineMessageMailRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EngineMessageMailService _service;

    public EngineMessageMailServiceTests()
    {
        _repositoryMock = new Mock<IEngineMessageMailRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new EngineMessageMailService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetPendingMessagesAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var messages = new List<MessageMail> { new() { Id = 1, Status = TaskStatus.Pending } };
        var expectedDtos = new List<MessageMailDto> { new() { Id = 1, Status = TaskStatus.Pending } };
        
        _repositoryMock.Setup(r => r.GetPendingMessagesAsync()).ReturnsAsync(messages);
        _mapperMock.Setup(m => m.Map<IEnumerable<MessageMailDto>>(messages)).Returns(expectedDtos);

        // Act
        var result = await _service.GetPendingMessagesAsync();

        // Assert
        Assert.Equal(expectedDtos, result);
        _repositoryMock.Verify(r => r.GetPendingMessagesAsync(), Times.Once());
    }

    [Fact]
    public async Task GetByStatusAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var status = TaskStatus.Completed;
        var messages = new List<MessageMail> { new() { Status = status } };
        var expectedDtos = new List<MessageMailDto> { new() { Status = status } };
        
        _repositoryMock.Setup(r => r.GetByStatusAsync(status)).ReturnsAsync(messages);
        _mapperMock.Setup(m => m.Map<IEnumerable<MessageMailDto>>(messages)).Returns(expectedDtos);

        // Act
        var result = await _service.GetByStatusAsync(status);

        // Assert
        Assert.Equal(expectedDtos, result);
        _repositoryMock.Verify(r => r.GetByStatusAsync(status), Times.Once());
    }

    [Fact]
    public async Task GetFailedMessagesAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var messages = new List<MessageMail> { new() { Status = TaskStatus.Failed } };
        var expectedDtos = new List<MessageMailDto> { new() { Status = TaskStatus.Failed } };
        
        _repositoryMock.Setup(r => r.GetFailedMessagesAsync()).ReturnsAsync(messages);
        _mapperMock.Setup(m => m.Map<IEnumerable<MessageMailDto>>(messages)).Returns(expectedDtos);

        // Act
        var result = await _service.GetFailedMessagesAsync();

        // Assert
        Assert.Equal(expectedDtos, result);
        _repositoryMock.Verify(r => r.GetFailedMessagesAsync(), Times.Once());
    }

    [Fact]
    public async Task UpdateStatusAsync_ShouldCallRepository()
    {
        // Arrange
        var id = 1;
        var status = TaskStatus.Completed;

        // Act
        await _service.UpdateStatusAsync(id, status);

        // Assert
        _repositoryMock.Verify(r => r.UpdateStatusAsync(id, status), Times.Once());
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnMappedDto()
    {
        // Arrange
        var dto = new MessageMailDto();
        var entity = new MessageMail();
        var createdEntity = new MessageMail();
        var expectedDto = new MessageMailDto();

        _mapperMock.Setup(m => m.Map<MessageMail>(dto)).Returns(entity);
        _repositoryMock.Setup(r => r.CreateAsync(entity)).ReturnsAsync(createdEntity);
        _mapperMock.Setup(m => m.Map<MessageMailDto>(createdEntity)).Returns(expectedDto);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.Equal(expectedDto, result);
        _repositoryMock.Verify(r => r.CreateAsync(entity), Times.Once());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedDto()
    {
        // Arrange
        var id = 1;
        var entity = new MessageMail();
        var expectedDto = new MessageMailDto();

        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<MessageMailDto>(entity)).Returns(expectedDto);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.Equal(expectedDto, result);
        _repositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once());
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepository()
    {
        // Arrange
        var id = 1;

        // Act
        await _service.DeleteAsync(id);

        // Assert
        _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once());
    }

    [Fact]
    public async Task GetUnsentEmails_ShouldReturnPendingMessages()
    {
        // Arrange
        var messages = new List<MessageMail> { new() { Status = TaskStatus.Pending } };
        var expectedDtos = new List<MessageMailDto> { new() { Status = TaskStatus.Pending } };
        
        _repositoryMock.Setup(r => r.GetByStatusAsync(TaskStatus.Pending)).ReturnsAsync(messages);
        _mapperMock.Setup(m => m.Map<IEnumerable<MessageMailDto>>(messages)).Returns(expectedDtos);

        // Act
        var result = await _service.GetUnsentEmails();

        // Assert
        Assert.Equal(expectedDtos, result);
        _repositoryMock.Verify(r => r.GetByStatusAsync(TaskStatus.Pending), Times.Once());
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnMappedDto()
    {
        // Arrange
        var dto = new MessageMailDto();
        var entity = new MessageMail();
        var updatedEntity = new MessageMail();
        var expectedDto = new MessageMailDto();

        _mapperMock.Setup(m => m.Map<MessageMail>(dto)).Returns(entity);
        _repositoryMock.Setup(r => r.UpdateAsync(entity)).ReturnsAsync(updatedEntity);
        _mapperMock.Setup(m => m.Map<MessageMailDto>(updatedEntity)).Returns(expectedDto);

        // Act
        var result = await _service.UpdateAsync(dto);

        // Assert
        Assert.Equal(expectedDto, result);
        _repositoryMock.Verify(r => r.UpdateAsync(entity), Times.Once());
    }
}
