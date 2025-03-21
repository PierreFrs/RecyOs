using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules;
using RecyOs.Engine.Modules.Odoo;
using RecyOs.OdooDB.DTO;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;

namespace RecyOsTests.EngineTests.modules;

public class BaseModuleTests
{
    private readonly Mock<IOdooClientService> dataService;
    private readonly Mock<IEngineSyncStatusService> engineSyncStatus;
    private readonly Mock<ILogger<TestModule>> logger;
    private readonly IMapper _mapper;

    public BaseModuleTests()
    {
        dataService = new Mock<IOdooClientService>();
        engineSyncStatus = new Mock<IEngineSyncStatusService>();
        logger = new Mock<ILogger<TestModule>>();
        
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new OdooClientProfile());
        }).CreateMapper();
        
        var engineSync = new EngineSyncStatusDto()
        {
            ModuleName = "TestModule",
            LastCreate = DateTime.Now,
            LastUpdate = DateTime.Now,
            SyncCre = true,
            SyncUpd = true
        };
        engineSyncStatus.Setup(x => x.GetByModuleName("TestModule")).Returns(engineSync);
    }

    [Fact]
    public void SyncData_Senario1()
    {
        // Arrange
        var hubservice = new Mock<IEngineEtablissementClientService>();
        var list = new List<EtablissementClientDto>();
        var obj = new EtablissementClientDto()
        {
            Id = 100,
            Nom = "Test",
            Siret = "123456789",
            Siren = "987654321",
            AdresseFacturation1 = "Rue des tests",
            EntrepriseBase = new EntrepriseBaseDto()
            {
                Id = 100,
                Siren = "987654321",
                NumeroTvaIntracommunautaire = "FR458564893967"
            }
        };
        list.Add(obj);
        hubservice.Setup(x => x.GetCreatedItems("TestModule")).Returns(list);
        hubservice.Setup(x => x.GetUpdatedItems("TestModule")).Returns(list);
        TestModule baseModule = new TestModule(dataService.Object, hubservice.Object, _mapper, engineSyncStatus.Object, logger.Object);
        // Act
        bool result = baseModule.SyncData();
        // Assert
        Assert.True(result);
        dataService.Verify(ser => ser.AddItems(It.Is<IList<ResPartnerDto>>(
            e => e.Count == 1 && e[0].Siret == "123456789"
            && e[0].Name == "Test"
            && e[0].Id == 0
            && e[0].Street == "Rue des tests"
            && e[0].Vat == "FR458564893967")), Times.Once);
    }

    [Fact]
    public void SyncData_Senario2()
    {
        // Arrange
        var hubservice = new Mock<IEngineEtablissementClientService>();
        var list = new List<EtablissementClientDto>();
        var obj = new EtablissementClientDto()
        {
            Id = 100,
            IdOdoo = "707",
            Nom = "Test",
            Siret = "123456789",
            Siren = "987654321",
            AdresseFacturation1 = "Rue des tests",
            EntrepriseBase = new EntrepriseBaseDto()
            {
                Id = 100,
                Siren = "987654321",
                NumeroTvaIntracommunautaire = "FR458564893967"
            }
        };
        list.Add(obj);
        hubservice.Setup(x => x.GetCreatedItems("TestModule")).Returns(new List<EtablissementClientDto>());
        hubservice.Setup(x => x.GetUpdatedItems("TestModule")).Returns(list);
        TestModule baseModule = new TestModule(dataService.Object, hubservice.Object, _mapper, engineSyncStatus.Object, logger.Object);
        // Act
        bool result = baseModule.SyncData();
        // Assert
        Assert.True(result);
        dataService.Verify(ser => ser.UpdateItems(It.Is<IList<ResPartnerDto>>(
            e => e.Count == 1 && e[0].Siret == "123456789"
                              && e[0].Name == "Test"
                              && e[0].Street == "Rue des tests"
                              && e[0].Vat == "FR458564893967"
                              && e[0].Id == 707)), Times.Once);
        
    }
}