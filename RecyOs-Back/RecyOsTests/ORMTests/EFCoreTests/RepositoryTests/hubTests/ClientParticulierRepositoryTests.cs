// ClientParticulierRepositoryTests.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 08/10/2024
// Fichier Modifié le : 08/10/2024
// Code développé pour le projet : RecyOsTests

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOsTests.Interfaces;
using ILogger = NLog.ILogger;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests;

[Collection("RepositoryTests")]
public class ClientParticulierRepositoryTests
{
    private readonly DataContext _context;
    private readonly Mock<ITokenInfoService> _tokenInfoServiceMock;
    private readonly Mock<ILogger<ClientParticulierRepository>> _loggerMock;

    public ClientParticulierRepositoryTests(IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
        _tokenInfoServiceMock = new Mock<ITokenInfoService>();
        _loggerMock = new Mock<ILogger<ClientParticulierRepository>>();
    }
    
    /*********** Geters ***********/
    
    [Fact]
    public async Task GetClientParticulierByIdAsync_ShouldReturnClientParticulier_IfItExists()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context, 
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        // Act
        var client = await repository.GetClientParticulierByIdAsync(1, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<ClientParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.Equal("Octavianus", client.Nom);
    }

    [Fact]
    public async Task GetClientParticulierByIdAsync_ShouldThrowException_IfItDoesNotExist()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await repository.GetClientParticulierByIdAsync(0, new ContextSession());
        });
    }

    [Fact]
    public async Task GetClientParticulierByIdAsync_ShouldReturnDeletedClient_IfIncludeDeletedIsTrue()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context, 
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        var clientToSoftDelete = new ClientParticulier()
        {
            Id = 5,
            Nom = "Vespasianus",
            Prenom = "Titus Flavius Vespasianus",
            Titre = "M.",
            AdresseFacturation1 = "Via Appia",
            CodePostalFacturation = "00100",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "test@test.test",
            TelephoneFacturation = "0123456789"
        };
        
        _context.Add(clientToSoftDelete);
        await _context.SaveChangesAsync();
        
        await repository.SoftDeleteClientParticulierAsync(clientToSoftDelete.Id, new ContextSession());

        // Act
        var client = await repository.GetClientParticulierByIdAsync(clientToSoftDelete.Id, new ContextSession(), true);
        var trackedEntities = _context.ChangeTracker.Entries<ClientParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.Equal("Vespasianus", client.Nom);
    }
    
    [Fact]
    public async Task GetFilteredListWithCountAsync_ShouldReturnClientParticulierList_IfItExists()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        // Act
        var count = repository.GetFilteredListWithCountAsync(new ClientParticulierGridFilter(), new ContextSession()).Result.Item2;
        var trackedEntities = _context.ChangeTracker.Entries<ClientParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.Equal(3, count);
    }
    
    [Fact]
    public async Task GetFilteredListWithCountAsync_ShouldReturnInvalidOperationException_IfPageSizeOrPageNumberAreInvalid()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await repository.GetFilteredListWithCountAsync(new ClientParticulierGridFilter { PageSize = -1 }, new ContextSession());
        });
    }

    [Fact]
    public async Task GetClientParticulierByNameAndCityAsync_ShouldReturnClientParticulier_IfItExists()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        // Act
        var client = await repository.GetClientParticulierByNameAndCityAsync("Caius Julius Caesar", "Octavianus", "Rome", new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<ClientParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.Equal("Caius Julius Caesar", client.Prenom);
        Assert.Equal("Octavianus", client.Nom);
        Assert.Equal("Rome", client.VilleFacturation);
    }

    [Fact]
    public async Task GetClientParticulierByNameAndCityAsync_ShouldThrowException_IfItDoesNotExist()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await repository.GetClientParticulierByNameAndCityAsync("Marcus", "Aurelius", "Rome", new ContextSession());
        });
    }

    [Fact]
    public async Task GetClientParticulierByNameAndCityAsync_ShouldReturnDeletedClient_IfIncludeDeletedIsTrue()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        var clientToSoftDelete = new ClientParticulier()
        {
            Id = 5,
            Nom = "Vespasianus",
            Prenom = "Titus Flavius",
            Titre = "M.",
            AdresseFacturation1 = "Via Appia",
            CodePostalFacturation = "00100",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "test@test.test",
            TelephoneFacturation = "0123456789"
        };

        _context.Add(clientToSoftDelete);
        await _context.SaveChangesAsync();

        await repository.SoftDeleteClientParticulierAsync(clientToSoftDelete.Id, new ContextSession());

        // Act
        var client = await repository.GetClientParticulierByNameAndCityAsync("Titus Flavius", "Vespasianus", "Rome", new ContextSession(), true);
        var trackedEntities = _context.ChangeTracker.Entries<ClientParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.Equal("Titus Flavius", client.Prenom);
        Assert.Equal("Vespasianus", client.Nom);
        Assert.Equal("Rome", client.VilleFacturation);
    }

    [Fact]
    public async Task GetClientParticulierByCodeMkgtAsync_ShouldReturnClientParticulier_IfItExists()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        // Act
        var client = await repository.GetClientParticulierByCodeMkgtAsync("TESCLI0001", new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<ClientParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.Equal("Caius Julius Caesar", client.Prenom);
    }

    [Fact]
    public async Task GetClientParticulierByCodeMkgtAsync_ShouldThrowException_IfItDoesNotExist()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await repository.GetClientParticulierByCodeMkgtAsync("TESCLI0010", new ContextSession());
        });
    }

    [Fact]
    public async Task GetClientParticulierByCodeMkgtAsync_ShouldReturnDeletedClient_IfIncludeDeletedIsTrue()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        var clientToSoftDelete = new ClientParticulier()
        {
            Id = 5,
            Nom = "Vespasianus",
            Prenom = "Titus Flavius",
            CodeMkgt = "TESTCLI0011",
            Titre = "M.",
            AdresseFacturation1 = "Via Appia",
            CodePostalFacturation = "00100",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "test@test.test",
            TelephoneFacturation = "0123456789",
        };

        _context.Add(clientToSoftDelete);
        await _context.SaveChangesAsync();

        await repository.SoftDeleteClientParticulierAsync(clientToSoftDelete.Id, new ContextSession());

        // Act
        var client = await repository.GetClientParticulierByCodeMkgtAsync("TESTCLI0011", new ContextSession(), true);
        var trackedEntities = _context.ChangeTracker.Entries<ClientParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.Equal("Titus Flavius", client.Prenom);
        Assert.Equal("Vespasianus", client.Nom);
    }

    /*********** Adders ***********/

    [Fact]
    public async Task CreateClientParticulierAsync_ShouldAddClientParticulier_IfItDoesNotExist()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        var client = new ClientParticulier
        {
            Id = 4,
            Nom = "Claudius",
            Prenom = "Tiberius Claudius Caesar Augustus Germanicus",
            Titre = "M.",
            AdresseFacturation1 = "Via Appia",
            CodePostalFacturation = "00100",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "test@test.test",
            TelephoneFacturation = "0123456789",
        };

        // Act
        await repository.CreateClientParticulierAsync(client, new ContextSession());

        var list = await repository.GetFilteredListWithCountAsync(new ClientParticulierGridFilter(),
            new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<ClientParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.Equal(4, list.Item2);
        
        // Clean
        _context.ClientParticuliers.Remove(client);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateClientParticulierAsync_ShouldThrowException_IfClientParticulierAlreadyExists()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        var client = new ClientParticulier
        {
            Id = 1,
            Nom = "Octavianus",
            Prenom = "Gaius Octavius Thurinus",
            Titre = "M.",
            AdresseFacturation1 = "Via Appia",
            CodePostalFacturation = "00100",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "test@test.test",
            TelephoneFacturation = "0123456789"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await repository.CreateClientParticulierAsync(client, new ContextSession());
        });
    }
    
    /*********** Updaters ***********/
    
    [Fact]
    public async Task UpdateClientParticulierAsync_ShouldUpdateClientParticulier_IfItExists()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        var client = new ClientParticulier
        {
            Id = 6,
            Nom = "Nero",
            Prenom = "Nero Claudius Caesar Augustus Germanicus",
            Titre = "M.",
            AdresseFacturation1 = "Via Appia",
            CodePostalFacturation = "00100",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "test@test.test",
            TelephoneFacturation = "0123456789"
        };

        _context.Add(client);
        await _context.SaveChangesAsync();
        
        var modifiedClient = new ClientParticulier
        {
            Id = 6,
            Nom = "Galba",
            Prenom = "Servius Sulpicius Galba Caesar Augustus",
            Titre = "M.",
            AdresseFacturation1 = "Via Appia",
            CodePostalFacturation = "00100",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "test@test.test",
            TelephoneFacturation = "0123456789"
        };

        // Act
        await repository.UpdateClientParticulierAsync(6, modifiedClient, new ContextSession());

        var updatedClient = await repository.GetClientParticulierByIdAsync(6, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<ClientParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.Equal("Servius Sulpicius Galba Caesar Augustus", updatedClient.Prenom);
    }
    
    [Fact]
    public async Task UpdateClientParticulierAsync_ShouldThrowException_IfClientParticulierDoesNotExist()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        var client = new ClientParticulier
        {
            Id = 7,
            Nom = "Vitellius",
            Prenom = "Aulus Vitellius Germanicus",
            Titre = "M.",
            AdresseFacturation1 = "Via Appia",
            CodePostalFacturation = "00100",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "test@test.test",
            TelephoneFacturation = "0123456789"
        };

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await repository.UpdateClientParticulierAsync(7, client, new ContextSession());
        });
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ShouldReturnUpdatedMkgtClientParticulierData_IfItExists()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        var client = new ClientParticulier
        {
            Id = 8,
            Nom = "Otho",
            Prenom = "Marcus Salvius Otho",
            Titre = "M.",
            AdresseFacturation1 = "Via Appia",
            CodePostalFacturation = "00100",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "test@test.test",
            TelephoneFacturation = "0123456789",
            CodeMkgt = "TESTCLI0001",
            DateCreMkgt = DateTime.Now
        };

        _context.Add(client);
        await _context.SaveChangesAsync();

        // Act

        await repository.DeleteErpCodeAsync(8, "mkgt", new ContextSession());

        var updatedClient = await repository.GetClientParticulierByIdAsync(8, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<ClientParticulier>();

        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.Null(updatedClient.CodeMkgt);
        Assert.Null(updatedClient.DateCreMkgt);
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ShouldReturnUpdatedOdooClientParticulierData_IfItExists()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        var client = new ClientParticulier
        {
            Id = 9,
            Nom = "Otho",
            Prenom = "Marcus Salvius Otho",
            Titre = "M.",
            AdresseFacturation1 = "Via Appia",
            CodePostalFacturation = "00100",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "test@test.test",
            TelephoneFacturation = "0123456789",
            IdOdoo = "1",
            DateCreOdoo = DateTime.Now
        };

        _context.Add(client);
        await _context.SaveChangesAsync();

        // Act

        await repository.DeleteErpCodeAsync(9, "odoo", new ContextSession());

        var updatedClient = await repository.GetClientParticulierByIdAsync(9, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<ClientParticulier>();

        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.Null(updatedClient.IdOdoo);
        Assert.Null(updatedClient.DateCreOdoo);
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ShouldThrowException_IfClientParticulierDoesNotExist()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await repository.DeleteErpCodeAsync(0, "mkgt", new ContextSession());
        });
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ShouldThrowException_IfErpCodeIsInvalid()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await repository.DeleteErpCodeAsync(1, "invalid", new ContextSession());
        });
    }

    /*********** Deleters ***********/
    
    [Fact]
    public async Task SoftDeleteClientParticulierAsync_ShouldTurnIsDeletedToTrue_IfClientParticulierExists()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        var client = new ClientParticulier
        {
            Id = 8,
            Nom = "Otho",
            Prenom = "Marcus Salvius Otho",
            Titre = "M.",
            AdresseFacturation1 = "Via Appia",
            CodePostalFacturation = "00100",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "test@test.test",
            TelephoneFacturation = "0123456789"
        };

        _context.Add(client);
        await _context.SaveChangesAsync();

        // Act
        await repository.SoftDeleteClientParticulierAsync(8, new ContextSession());

        var deletedClient = await repository.GetClientParticulierByIdAsync(8, new ContextSession(), true);
        var trackedEntities = _context.ChangeTracker.Entries<ClientParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.True(deletedClient.IsDeleted);
    }
    
    [Fact]
    public async Task SoftDeleteClientParticulierAsync_ShouldThrowException_IfClientParticulierDoesNotExist()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await repository.SoftDeleteClientParticulierAsync(0, new ContextSession());
        });
    }
    
    [Fact]
    public async Task SoftDeleteClientParticulierAsync_ShouldThrowException_IfClientParticulierHasNotBeenDeleted()
    {
        // Arrange
       var client = new ClientParticulier
        {
            Id = 10,
            Nom = "TestClientFailure",
            Prenom = "TestPrenom",
            IsDeleted = false,
            Titre = "M.",
            AdresseFacturation1 = "Via Appia",
            CodePostalFacturation = "00100",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "test@test.test",
            TelephoneFacturation = "0123456789" // Initially, the client is not deleted
        };

        _context.Add(client);
        await _context.SaveChangesAsync();

        // Simulate that the soft delete will fail by manually ensuring IsDeleted remains false
        var mockRepository = new Mock<ClientParticulierRepository>(
            _context, 
            _tokenInfoServiceMock.Object, 
            _loggerMock.Object);
        
        mockRepository
            .Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<ContextSession>()))
            .Callback(() => 
            {
                // Simulate a failed delete by not changing IsDeleted to true
                var failedClient = _context.ClientParticuliers.FirstOrDefault(c => c.Id == client.Id);
                failedClient.IsDeleted = false;
            })
            .Returns(Task.CompletedTask);

        // Act & Assert: Ensure that the method throws an ArgumentException
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await mockRepository.Object.SoftDeleteClientParticulierAsync(client.Id, new ContextSession());
        });
    }

    [Fact]
    public async Task HardDeleteClientParticulierAsync_ShouldRemoveClientParticulier_IfItExists()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        var client = new ClientParticulier
        {
            Id = 9,
            Nom = "Vespasianus",
            Prenom = "Titus Flavius Vespasianus",
            Titre = "M.",
            AdresseFacturation1 = "Via Appia",
            CodePostalFacturation = "00100",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "test@test.test",
            TelephoneFacturation = "0123456789"
        };

        _context.Add(client);
        await _context.SaveChangesAsync();

        // Act
        await repository.HardDeleteClientParticulierAsync(9, new ContextSession());

        var deletedClient = await repository.GetClientParticulierByIdAsync(9, new ContextSession(), true);
        var trackedEntities = _context.ChangeTracker.Entries<ClientParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.Null(deletedClient);
    }
    
    [Fact]
    public async Task HardDeleteClientParticulierAsync_ShouldThrowException_IfClientParticulierDoesNotExist()
    {
        // Arrange
        var repository = new ClientParticulierRepository(
            _context,
            _tokenInfoServiceMock.Object,
            _loggerMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
        {
            await repository.HardDeleteClientParticulierAsync(0, new ContextSession());
        });
    }
}