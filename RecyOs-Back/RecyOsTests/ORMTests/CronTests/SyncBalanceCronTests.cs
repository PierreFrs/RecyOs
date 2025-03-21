// // Created by : Pierre FRAISSE
// // RecyOs => RecyOsTests => SyncBalanceCronTests.cs
// // Created : 2024/03/19 - 14:30
// // Updated : 2024/03/19 - 14:30

using Moq;
using RecyOs.Cron;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using RecyOs.ORM.Models.DTO.hub;

namespace RecyOsTests.ORMTests.CronTests;

public class SyncBalanceCronTests
{
    private readonly Mock<IMoveAccountService> _mockMoveAccountService;
    private readonly Mock<IEtablissementClientService> _mockEtablissementClientService;
    private readonly Mock<IClientEuropeService> _mockClientEuropeService;
    private readonly Mock<IClientParticulierService> _mockClientParticulierService;
    private readonly Mock<ISocieteBaseService> _mockSocieteService;
    private readonly Mock<IBalanceFranceService> _mockBalanceFranceService;
    private readonly Mock<IAcountAccountService> _mockAccountAccountService;
    private readonly Mock<IBalanceEuropeService> _mockBalanceEuropeService;
    private readonly Mock<IBalanceParticulierService> _mockBalanceParticulierService;
    private readonly Mock<ITokenInfoService> _mockTokenInfoService;
    private readonly SyncBalanceCron _syncBalanceCron;

    public SyncBalanceCronTests()
    {
        _mockMoveAccountService = new Mock<IMoveAccountService>();
        _mockEtablissementClientService = new Mock<IEtablissementClientService>();
        _mockClientEuropeService = new Mock<IClientEuropeService>();
        _mockClientParticulierService = new Mock<IClientParticulierService>();
        _mockSocieteService = new Mock<ISocieteBaseService>();
        _mockBalanceFranceService = new Mock<IBalanceFranceService>();
        _mockAccountAccountService = new Mock<IAcountAccountService>();
        _mockBalanceEuropeService = new Mock<IBalanceEuropeService>();
        _mockBalanceParticulierService = new Mock<IBalanceParticulierService>();
        _mockTokenInfoService = new Mock<ITokenInfoService>();

        _syncBalanceCron = new SyncBalanceCron(
            _mockMoveAccountService.Object,
            _mockEtablissementClientService.Object,
            _mockClientEuropeService.Object,
            _mockClientParticulierService.Object,
            _mockSocieteService.Object,
            _mockBalanceFranceService.Object,
            _mockAccountAccountService.Object,
            _mockBalanceEuropeService.Object,
            _mockBalanceParticulierService.Object,
            _mockTokenInfoService.Object
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldProcessFranceClients()
    {
        // Arrange
        SetupBasicMocks();

        var etablissementClient = new EtablissementClientDto
        {
            Id = 1,
            IdOdoo = "123",
            Nom = "Test Client"
        };

        _mockEtablissementClientService.Setup(x => x.GetAll(false))
            .ReturnsAsync(new List<EtablissementClientDto> { etablissementClient });

        // Act
        await _syncBalanceCron.ExecuteAsync();

        // Assert
        _mockBalanceFranceService.Verify(
            x => x.CreateAsync(It.IsAny<BalanceDto>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldProcessEuropeClients()
    {
        // Arrange
        SetupBasicMocks();

        var europeClient = new ClientEuropeDto
        {
            Id = 1,
            IdOdoo = "123",
            Nom = "Test Europe Client"
        };

        _mockClientEuropeService.Setup(x => x.GetAll(false))
            .ReturnsAsync(new List<ClientEuropeDto> { europeClient });

        // Act
        await _syncBalanceCron.ExecuteAsync();

        // Assert
        _mockBalanceEuropeService.Verify(
            x => x.CreateAsync(It.IsAny<BalanceDto>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldProcessParticulierClients()
    {
        // Arrange
        SetupBasicMocks();

        var particulierClient = new ClientParticulierDto
        {
            Id = 1,
            IdOdoo = "123",
            Nom = "Test Particulier Client"
        };

        _mockClientParticulierService.Setup(x => x.GetAll(false))
            .ReturnsAsync(new List<ClientParticulierDto> { particulierClient });

        // Act
        await _syncBalanceCron.ExecuteAsync();

        // Assert
        _mockBalanceParticulierService.Verify(
            x => x.CreateAsync(It.IsAny<BalanceDto>()),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSkipClientsWithoutOdooId()
    {
        // Arrange
        SetupBasicMocks();

        var clientWithoutOdooId = new ClientParticulierDto
        {
            Id = 1,
            IdOdoo = "",
            Nom = "Test Client Without Odoo ID"
        };

        _mockClientParticulierService.Setup(x => x.GetAll(false))
            .ReturnsAsync(new List<ClientParticulierDto> { clientWithoutOdooId });

        // Act
        await _syncBalanceCron.ExecuteAsync();

        // Assert
        _mockBalanceParticulierService.Verify(
            x => x.CreateAsync(It.IsAny<BalanceDto>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldHandleAccountLineCalculations()
    {
        // Arrange
        SetupBasicMocks();

        var client = new ClientParticulierDto
        {
            Id = 1,
            IdOdoo = "123",
            Nom = "Test Client"
        };

        var accountLines = new List<AccountMoveLineOdooModel>
        {
            new() { AccountId = 1, Debit = 100, Credit = 50 },
            new() { AccountId = 1, Debit = 75, Credit = 25 }
        };

        _mockClientParticulierService.Setup(x => x.GetAll(false))
            .ReturnsAsync(new List<ClientParticulierDto> { client });

        _mockMoveAccountService.Setup(x => x.GetAccountLines(
                It.IsAny<long>(),
                It.IsAny<string>(),
                It.IsAny<long[]>()))
            .ReturnsAsync(accountLines);

        // Act
        await _syncBalanceCron.ExecuteAsync();

        // Assert
        _mockBalanceParticulierService.Verify(x => x.CreateAsync(
            It.Is<BalanceDto>(dto => Math.Abs(dto.Montant - 100) < 0.001m)), // Using decimal comparison
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldProcessAccountsForAllSocietes()
    {
        // Arrange
        var societes = new List<SocieteDto>
        {
            new() { Id = 1, IdOdoo = "1", Nom = "Societe 1" },
            new() { Id = 2, IdOdoo = "2", Nom = "Societe 2" }
        };

        _mockSocieteService.Setup(x => x.GetListAsync())
            .ReturnsAsync(societes);

        _mockAccountAccountService.Setup(x => x.GetCustomerAccountList(It.IsAny<long>()))
            .Returns((long id) => new List<AccountAccountOdooModel>
            {
                new() { Id = 1, Code = "411000" }
            });

        SetupBasicClientMocks();

        // Act
        await _syncBalanceCron.ExecuteAsync();

        // Assert
        _mockAccountAccountService.Verify(
            x => x.GetCustomerAccountList(It.IsAny<long>()),
            Times.Exactly(societes.Count));
    }

    private void SetupBasicMocks()
    {
        _mockSocieteService.Setup(x => x.GetListAsync())
            .ReturnsAsync(new List<SocieteDto>
            {
                new() { Id = 1, IdOdoo = "1", Nom = "Test Societe" }
            });

        _mockAccountAccountService.Setup(x => x.GetCustomerAccountList(It.IsAny<long>()))
            .Returns((long id) => new List<AccountAccountOdooModel>
            {
                new() { Id = 1, Code = "411000" }
            });

        _mockMoveAccountService.Setup(x => x.GetAccountLines(
            It.IsAny<long>(),
            It.IsAny<string>(),
            It.IsAny<long[]>()))
            .ReturnsAsync(new List<AccountMoveLineOdooModel>
            {
                new() { AccountId = 1, Debit = 0, Credit = 0 }
            });

        _mockEtablissementClientService.Setup(x => x.GetAll(false))
            .ReturnsAsync(new List<EtablissementClientDto>());
        _mockClientEuropeService.Setup(x => x.GetAll(false))
            .ReturnsAsync(new List<ClientEuropeDto>());
        _mockClientParticulierService.Setup(x => x.GetAll(false))
            .ReturnsAsync(new List<ClientParticulierDto>());

        _mockTokenInfoService.Setup(x => x.GetCurrentUserName())
            .Returns("TestUser");
    }

    private void SetupBasicClientMocks()
    {
        _mockEtablissementClientService.Setup(x => x.GetAll(false))
            .ReturnsAsync(new List<EtablissementClientDto>());
        _mockClientEuropeService.Setup(x => x.GetAll(false))
            .ReturnsAsync(new List<ClientEuropeDto>());
        _mockClientParticulierService.Setup(x => x.GetAll(false))
            .ReturnsAsync(new List<ClientParticulierDto>());
    }
}