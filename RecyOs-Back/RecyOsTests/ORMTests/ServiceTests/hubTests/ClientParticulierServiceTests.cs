// ClientParticulierServiceTests.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 08/10/2024
// Fichier Modifié le : 08/10/2024
// Code développé pour le projet : RecyOsTests

using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.MapProfile;
using RecyOs.ORM.Models.DTO.hub;
using RecyOs.ORM.Service.hub;

namespace RecyOsTests.ORMTests.hubTests;

[Collection("ClientParticulierTestsCollection")]
public class ClientParticulierServiceTests
{
    private readonly ClientParticulierService _clientParticulierService;
    private readonly Mock<IClientParticulierRepository> _clientParticulierRepositoryMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<ClientParticulierService>> _loggerMock;
    private readonly Mock<ICurrentContextProvider> _contextProviderMock;  // Mock for context provider


    public ClientParticulierServiceTests()
    {
        _clientParticulierRepositoryMock = new Mock<IClientParticulierRepository>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClientParticulierProfile>()).CreateMapper();
        _loggerMock = new Mock<ILogger<ClientParticulierService>>();
        _contextProviderMock = new Mock<ICurrentContextProvider>();  // Mock context provider
        _clientParticulierService =
            new ClientParticulierService(
                _contextProviderMock.Object,
                _clientParticulierRepositoryMock.Object, 
                _mapper, 
                _loggerMock.Object);
    }

    /********** Geters **********/

    [Fact]
    public async Task GetClientParticulierByIdAsync_ShouldReturnClientParticulierDto_IfItExists()
    {
        // Arrange
        var clientParticulierService =
            new ClientParticulierService(
                _contextProviderMock.Object,
                _clientParticulierRepositoryMock.Object, 
                _mapper, 
                _loggerMock.Object);
        var id = 1;
        var client = new ClientParticulier
        {
            Id = id,
            Nom = "TestName",
            Prenom = "TestPrenom"
        };

        _clientParticulierRepositoryMock
            .Setup(x => x.GetClientParticulierByIdAsync(id, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(client);

        // Act
        var result = await clientParticulierService.GetClientParticulierByIdAsync(id);

        // Assert
        Assert.IsType<ClientParticulierDto>(result);
    }

    [Fact]
    public async Task GetClientParticulierByIdAsync_ShouldThrowError_IfClientParticulierDoesNotExist()
    {
        // Arrange
        var id = 1;
        var includeDeleted = false;

        // Simulate an InvalidOperationException being thrown from the repository
        _clientParticulierRepositoryMock
            .Setup(repo =>
                repo.GetClientParticulierByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ThrowsAsync(new EntityNotFoundException("Test exception"));

        // Act & Assert: Ensure that the InvalidOperationException is thrown
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await _clientParticulierService.GetClientParticulierByIdAsync(id, includeDeleted);
        });
    }

    [Fact]
    public async Task GetFilteredListWithCountAsync_ShouldReturnClientParticulierDto_IfItExists()
    {
        // Arrange
        var filter = new ClientParticulierGridFilter();
        var includeDeleted = false;

        // Act
        var result = await _clientParticulierService.GetFilteredListWithCountAsync(filter, includeDeleted);

        // Assert
        Assert.IsType<GridData<ClientParticulierDto>>(result);
    }

    [Fact]
    public async Task GetFilteredListWithCountAsync_ShouldThrowError_IfClientParticulierDoesNotExist()
    {
        // Arrange
        var filter = new ClientParticulierGridFilter();
        var includeDeleted = false;

        // Simulate an InvalidOperationException being thrown from the repository
        _clientParticulierRepositoryMock
            .Setup(repo => repo.GetFilteredListWithCountAsync(It.IsAny<ClientParticulierGridFilter>(),
                It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ThrowsAsync(new InvalidOperationException("Test exception"));

        // Act & Assert: Ensure that the InvalidOperationException is thrown
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _clientParticulierService.GetFilteredListWithCountAsync(filter, includeDeleted);
        });
    }

    [Fact]
    public async Task GetClientParticulierByNameAndCityAsync_ShouldReturnClientParticulierDto_IfItExists()
    {
        // Arrange
        var clientParticulierService =
            new ClientParticulierService(
                _contextProviderMock.Object,
                _clientParticulierRepositoryMock.Object,
                _mapper,
                _loggerMock.Object);
        var id = 1;
        var client = new ClientParticulier
        {
            Id = id,
            Nom = "TestName",
            Prenom = "TestPrenom",
            VilleFacturation = "TestVille"
        };

        _clientParticulierRepositoryMock
            .Setup(x => x.GetClientParticulierByNameAndCityAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(client);

        // Act
        var result = await clientParticulierService.GetClientParticulierByNameAndCityAsync("test", "test", "test");

        // Assert
        Assert.IsType<ClientParticulierDto>(result);
    }

    [Fact]
    public async Task GetClientParticulierByNameAndCityAsync_ShouldThrowError_IfClientParticulierDoesNotExist()
    {
        // Arrange
        _clientParticulierRepositoryMock
            .Setup(repo =>
                repo.GetClientParticulierByNameAndCityAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ThrowsAsync(new EntityNotFoundException("Test exception"));

        // Act & Assert: Ensure that the InvalidOperationException is thrown
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await _clientParticulierService.GetClientParticulierByNameAndCityAsync("test", "test", "test");
        });
    }

    [Fact]
    public async Task GetClientParticulierByCodeMkgtAsync_ShouldReturnClientParticulierDto_IfItExists()
    {
        // Arrange
        var clientParticulierService =
            new ClientParticulierService(
                _contextProviderMock.Object,
                _clientParticulierRepositoryMock.Object,
                _mapper,
                _loggerMock.Object);
        var id = 1;
        var client = new ClientParticulier
        {
            Id = id,
            Nom = "TestName",
            Prenom = "TestPrenom",
            VilleFacturation = "TestVille"
        };

        _clientParticulierRepositoryMock
            .Setup(x => x.GetClientParticulierByCodeMkgtAsync(It.IsAny<string>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(client);

        // Act
        var result = await clientParticulierService.GetClientParticulierByCodeMkgtAsync("test");

        // Assert
        Assert.IsType<ClientParticulierDto>(result);
    }

    [Fact]
    public async Task GetClientParticulierByCodeMkgtAsync_ShouldThrowError_IfClientParticulierDoesNotExist()
    {
        // Arrange
        _clientParticulierRepositoryMock
            .Setup(repo =>
                repo.GetClientParticulierByCodeMkgtAsync(It.IsAny<string>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ThrowsAsync(new EntityNotFoundException("Test exception"));

        // Act & Assert: Ensure that the InvalidOperationException is thrown
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await _clientParticulierService.GetClientParticulierByCodeMkgtAsync("test");
        });
    }

    /********** Creators **********/

    [Fact]
    public async Task CreateClientParticulierAsync_ShouldReturnClientParticulierDto_IfItExists()
    {
        // Arrange
        var clientParticulierDto = new ClientParticulierDto
        {
            Nom = "TestName",
            Prenom = "TestPrenom"
        };
        var client = new ClientParticulier
        {
            Nom = "TestName",
            Prenom = "TestPrenom"
        };

        _clientParticulierRepositoryMock
            .Setup(x => x.CreateClientParticulierAsync(It.IsAny<ClientParticulier>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(client);

        // Act
        var result = await _clientParticulierService.CreateClientParticulierAsync(clientParticulierDto);

        // Assert
        Assert.IsType<ClientParticulierDto>(result);
    }

    [Fact]
    public async Task CreateClientParticulierAsync_ShouldThrowError_IfClientParticulierDoesNotExist()
    {
        // Arrange
        var clientParticulierDto = new ClientParticulierDto
        {
            Nom = "TestName",
            Prenom = "TestPrenom"
        };

        // Simulate an InvalidOperationException being thrown from the repository
        _clientParticulierRepositoryMock
            .Setup(repo => repo.CreateClientParticulierAsync(It.IsAny<ClientParticulier>(), It.IsAny<ContextSession>()))
            .ThrowsAsync(new InvalidOperationException("Test exception"));

        // Act & Assert: Ensure that the InvalidOperationException is thrown
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _clientParticulierService.CreateClientParticulierAsync(clientParticulierDto);
        });
    }

    /********** Updaters **********/

    [Fact]
    public async Task UpdateClientParticulierAsync_ShouldReturnClientParticulierDto_IfItExists()
    {
        // Arrange
        var clientParticulierService =
            new ClientParticulierService(
                _contextProviderMock.Object,
                _clientParticulierRepositoryMock.Object, 
                _mapper, 
                _loggerMock.Object);
        var clientParticulierDto = new ClientParticulierDto
        {
            Id = 1,
            Nom = "TestName",
            Prenom = "TestPrenom"
        };
        var client = new ClientParticulier
        {
            Id = 1,
            Nom = "TestName",
            Prenom = "TestPrenom"
        };

        _clientParticulierRepositoryMock
            .Setup(x => x.UpdateClientParticulierAsync(It.IsAny<int>(), It.IsAny<ClientParticulier>(),
                It.IsAny<ContextSession>()))
            .ReturnsAsync(client);

        // Act
        var result =
            await clientParticulierService.UpdateClientParticulierAsync(clientParticulierDto.Id, clientParticulierDto);

        // Assert
        Assert.IsType<ClientParticulierDto>(result);
    }

    [Fact]
    public async Task UpdateClientParticulierAsync_ShouldThrowError_IfClientParticulierDoesNotExist()
    {
        // Arrange
        var clientParticulierDto = new ClientParticulierDto
        {
            Id = 1,
            Nom = "TestName",
            Prenom = "TestPrenom"
        };

        // Simulate an InvalidOperationException being thrown from the repository
        _clientParticulierRepositoryMock
            .Setup(repo =>
                repo.UpdateClientParticulierAsync(It.IsAny<int>(), It.IsAny<ClientParticulier>(),
                    It.IsAny<ContextSession>()))
            .ThrowsAsync(new EntityNotFoundException("Test exception"));

        // Act & Assert: Ensure that the InvalidOperationException is thrown
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await _clientParticulierService.UpdateClientParticulierAsync(clientParticulierDto.Id,
                clientParticulierDto);
        });
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ShouldReturnClientParticulierDto_IfItExists()
    {
        // Arrange
        var clientParticulierService =
            new ClientParticulierService(
                _contextProviderMock.Object,
                _clientParticulierRepositoryMock.Object,
                _mapper,
                _loggerMock.Object);
        var id = 1;
        var client = new ClientParticulier
        {
            Id = id,
            Nom = "TestName",
            Prenom = "TestPrenom"
        };

        _clientParticulierRepositoryMock
            .Setup(x => x.DeleteErpCodeAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(client);

        // Act
        var result = await clientParticulierService.DeleteErpCodeAsync(id, "test");

        // Assert
        Assert.IsType<ClientParticulierDto>(result);
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ShouldReturnNull_IfClientParticulierDoesNotExist()
    {
        // Arrange
        var id = 1;

        // Simulate an EntityNotFoundException being thrown from the repository
        _clientParticulierRepositoryMock
            .Setup(repo => repo.DeleteErpCodeAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ContextSession>()))
            .ReturnsAsync((ClientParticulier)null);

        // Act
        var result = await _clientParticulierService.DeleteErpCodeAsync(id, "test");

        // Assert
        Assert.Null(result);
    }

    /********** Deleters **********/

    [Fact]
    public async Task SoftDeleteClientParticulierAsync_ShouldReturnTrue_IfItExists()
    {
        // Arrange
        var id = 1;

        _clientParticulierRepositoryMock
            .Setup(x => x.SoftDeleteClientParticulierAsync(id, It.IsAny<ContextSession>()))
            .ReturnsAsync(true);

        // Act
        var result = await _clientParticulierService.SoftDeleteClientParticulierAsync(id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SoftDeleteClientParticulierAsync_ShouldThrowError_IfClientParticulierDoesNotExist()
    {
        // Arrange
        var id = 1;

        // Simulate an EntityNotFoundException being thrown from the repository
        _clientParticulierRepositoryMock
            .Setup(repo => repo.SoftDeleteClientParticulierAsync(It.IsAny<int>(), It.IsAny<ContextSession>()))
            .ThrowsAsync(new EntityNotFoundException("Test exception"));

        // Act & Assert: Ensure that the EntityNotFoundException is thrown
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await _clientParticulierService.SoftDeleteClientParticulierAsync(id);
        });
    }

    [Fact]
    public async Task SoftDeleteClientParticulierAsync_ShouldThrowError_IfClientParticulierIsNotDeleted()
    {
        // Arrange
        var id = 1;

        // Simulate an EntityNotFoundException being thrown from the repository
        _clientParticulierRepositoryMock
            .Setup(repo => repo.SoftDeleteClientParticulierAsync(It.IsAny<int>(), It.IsAny<ContextSession>()))
            .ThrowsAsync(new ArgumentException("Test exception"));

        // Act & Assert: Ensure that the EntityNotFoundException is thrown
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _clientParticulierService.SoftDeleteClientParticulierAsync(id);
        });
    }

    [Fact]
    public async Task HardDeleteClientParticulierAsync_ShouldReturnTrue_IfItExists()
    {
        // Arrange
        var id = 1;

        _clientParticulierRepositoryMock
            .Setup(x => x.HardDeleteClientParticulierAsync(id, It.IsAny<ContextSession>()))
            .ReturnsAsync(true);

        // Act
        var result = await _clientParticulierService.HardDeleteClientParticulierAsync(id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task HardDeleteClientParticulierAsync_ShouldThrowError_IfClientParticulierDoesNotExist()
    {
        // Arrange
        var id = 1;

        // Simulate an EntityNotFoundException being thrown from the repository
        _clientParticulierRepositoryMock
            .Setup(repo => repo.HardDeleteClientParticulierAsync(It.IsAny<int>(), It.IsAny<ContextSession>()))
            .ThrowsAsync(new EntityNotFoundException("Test exception"));

        // Act & Assert: Ensure that the EntityNotFoundException is thrown
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await _clientParticulierService.HardDeleteClientParticulierAsync(id);
        });
    }
    
    [Fact]
    public async Task HardDeleteClientParticulierAsync_ShouldThrowError_IfClientParticulierIsNotDeleted()
    {
        // Arrange
        var id = 1;

        // Simulate an EntityNotFoundException being thrown from the repository
        _clientParticulierRepositoryMock
            .Setup(repo => repo.HardDeleteClientParticulierAsync(It.IsAny<int>(), It.IsAny<ContextSession>()))
            .ThrowsAsync(new ArgumentException("Test exception"));

        // Act & Assert: Ensure that the EntityNotFoundException is thrown
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _clientParticulierService.HardDeleteClientParticulierAsync(id);
        });
    }
}