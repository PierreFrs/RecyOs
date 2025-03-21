using Microsoft.Extensions.Logging;
using Moq;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.Engine.Modules.Mkgt.Services;
using RecyOs.MKGT_DB.Interfaces;
using RecyOs.ORM.DTO.hub;

namespace RecyOsTests.EngineTests;

public class MkgtClientServiceTests
{
    [Fact]
    public void AddItem_ShouldCallInsertEtablissementClientAndLog()
    {
        // Arrange
        var mockRepository = new Mock<IFCliService>();
        var loggerMock = new Mock<ILogger<MkgtClientService>>();
        var service = new MkgtClientService(mockRepository.Object, loggerMock.Object);
        var item = new EtablissementMkgtDto()
        {
            siret = "82846599700033", intl2 = "robert", intl3 = "DUPOND", code = "TARTEMPHAUB59", cptFac = "234",
            t2 = "0612345678", t3 = "07 12 34 56 78", ptb2 = "08.12.34.56.78", ptb3 = "0033912345678",
            email1 = "valide@domaine.uk", email2 = "invalide@domaine.com / invalide2@domaine", modReg = "V60",
            tva = 20.0m, encours = 2000, nom = "TEST", adr1 = "L1", adr2 = "L2", adr3 = "L3", cp = "12345",
            ville = "VILLE", pays = "FRANCE", smTva = "oui", secteur = "59", codPay = "FRA", tpSoc = "SARL",
            ape = "41.34Z", intracom = "FR76397849385"
        };

        // Act
        service.AddItem(item);

        // Assert
        mockRepository.Verify(repo => repo.InsertEtablissementClient(item), Times.Once);
        loggerMock.Verify(
            logger => logger.Log(
                LogLevel.Trace,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public void UpdateItem_ShouldCallUpdateEtablissementClientAndLog()
    {
        // Arrange
        var mockRepository = new Mock<IFCliService>();
        var loggerMock = new Mock<ILogger<MkgtClientService>>();
        var service = new MkgtClientService(mockRepository.Object, loggerMock.Object);
        var item = new EtablissementMkgtDto()
        {
            siret = "82846599700033", intl2 = "robert", intl3 = "DUPOND", code = "TARTEMPHAUB59", cptFac = "234",
            t2 = "0612345678", t3 = "07 12 34 56 78", ptb2 = "08.12.34.56.78", ptb3 = "0033912345678",
            email1 = "valide@domaine.uk", email2 = "invalide@domaine.com / invalide2@domaine", modReg = "V60",
            tva = 20.0m, encours = 2000, nom = "TEST", adr1 = "L1", adr2 = "L2", adr3 = "L3", cp = "12345",
            ville = "VILLE", pays = "FRANCE", smTva = "oui", secteur = "59", codPay = "FRA", tpSoc = "SARL",
        };
        
        mockRepository.Setup(repo => repo.UpdateEtablissementClient(item))
            .ReturnsAsync(item); // Simulate a write success.

        // Act
        service.UpdateItem(item);
        
        // Assert
        mockRepository.Verify(repo => repo.UpdateEtablissementClient(item), Times.Once);
        loggerMock.Verify(
            logger => logger.Log(
                LogLevel.Trace,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public void UpdateItem_withWriteError_ShouldLogAndReturnNull()
    {
        // Arrange
        var mockRepository = new Mock<IFCliService>();
        var loggerMock = new Mock<ILogger<MkgtClientService>>();
        var service = new MkgtClientService(mockRepository.Object, loggerMock.Object);
        var item = new EtablissementMkgtDto()
        {
            siret = "82846599700033", intl2 = "robert", intl3 = "DUPOND", code = "TARTEMPHAUB59", cptFac = "234",
            t2 = "0612345678", t3 = "07 12 34 56 78", ptb2 = "08.12.34.56.78", ptb3 = "0033912345678",
            email1 = "valide@domaine.uk", email2 = "invalide@domaine.com / invalide2@domaine", modReg = "V60",
            tva = 20.0m, encours = 2000, nom = "TEST", adr1 = "L1", adr2 = "L2", adr3 = "L3", cp = "12345",
            ville = "VILLE", pays = "FRANCE", smTva = "oui", secteur = "59", codPay = "FRA", tpSoc = "SARL",
        };

        mockRepository.Setup(repo => repo.UpdateEtablissementClient(item))
            .ReturnsAsync((EtablissementMkgtDto?)null); // Simulate a write error then return null.

        // Act
        var result = service.UpdateItem(item);

        // Assert
        mockRepository.Verify(repo => repo.UpdateEtablissementClient(item), Times.Once);
        loggerMock.Verify(
            logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
        Assert.Null(result);

    }
}