using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Commands;
using RecyOs.MKGT_DB.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using Moq;
using RecyOs.Engine.Mappers;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service.pappers;

namespace RecyOsTests;

public class ImportFcliTest
{
    private readonly ImportFcli _sut;
    private readonly Mock<IFCliService> _fCliServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IEtablissementClientService> _etablissementClientServiceMock;
    private readonly Mock<ILogger<ImportFcli>> _loggerMock;

    public ImportFcliTest()
    {
        _fCliServiceMock = new Mock<IFCliService>();
        _mapperMock = new Mock<IMapper>();
        _etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        _loggerMock = new Mock<ILogger<ImportFcli>>();

        _sut = new ImportFcli(
            _fCliServiceMock.Object,
            _mapperMock.Object,
            _etablissementClientServiceMock.Object,
            null,_loggerMock.Object
        );
    }
    
    [Theory]
    [InlineData("BadSiret", null)]
    [InlineData("73282932000074", "73282932000074")]
    [InlineData("440 120 222 00045", "44012022200045")]
    [InlineData("440 120 222", null)]
    [InlineData("440120222", null)]
    [InlineData("123456789", null)]
    [InlineData("", null)]
    public void CheckSiret_ValidatesSiretNumbersCorrectly(string input, string? expectedOutput)
    {
        // Act
        MethodInfo? methodInfo = typeof(ImportFcli).GetMethod("checkSiret", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            string? result = (string?) methodInfo.Invoke(_sut, new object[] {input});
            // Assert
            Assert.Equal(expectedOutput, result);
        }

    }
    
    [Theory]
    [InlineData("+33612345678", "+33 6 12 34 56 78")]
    [InlineData("0033612345678", "+33 6 12 34 56 78")]
    [InlineData("0612345678", "+33 6 12 34 56 78")]
    [InlineData("01.02.03.04.05", "+33 1 02 03 04 05")]
    [InlineData("01 02 03 04 05", "+33 1 02 03 04 05")]
    [InlineData("01 02 03 04 05 06", "+33 1 02 03 04 05")]
    public void NormalizeTel_ValidInput_ReturnsFormattedPhoneNumber(string input, string expected)
    {
        MethodInfo? methodInfo = typeof(ImportFcli).GetMethod("NormalizeTel", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            string? result = (string?) methodInfo.Invoke(_sut, new object[] {input});

            // Assert
            Assert.Equal(expected, result);  
        }
    }

    [Theory]
    [InlineData("InvalidInput")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("12345678")]
    [InlineData("+3312345")]
    [InlineData("003312345")]
    [InlineData("012345")]
    public void NormalizeTel_InvalidInput_ReturnsNull(string input)
    {
        // Arrange
        MethodInfo? methodInfo = typeof(ImportFcli).GetMethod("NormalizeTel", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            string? result = (string?) methodInfo.Invoke(_sut, new object[] {input});

            // Assert
            Assert.Null(result);
        }
    }
    
    [Theory]
    [InlineData("user@example.com", "user@example.com")]
    [InlineData("user@example.co.uk", "user@example.co.uk")]
    [InlineData("user.name@example.com", "user.name@example.com")]
    [InlineData("user+tag@example.com", "user+tag@example.com")]
    [InlineData("invalid_email@example", null)]
    [InlineData("invalid@example..com", null)]
    [InlineData("invalid@.example.com", null)]
    [InlineData("", null)]
    [InlineData(null, null)]
    [InlineData("EmailNonValide", null)]
    public void CheckEmail_ValidAndInvalidInputs_ReturnsValidEmailOrNull(string? input, string? expected)
    {
        MethodInfo? methodInfo = typeof(ImportFcli).GetMethod("checkEmail", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        string? result = (string?) methodInfo.Invoke(_sut, new object?[] {input});

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void Normalize_ValidInput_ReturnsNormalizedEtablissementMkgtDTO()
    {
        // Arrange
        EtablissementMkgtDto input = new EtablissementMkgtDto
        {
            siret = "12345678901234",
            t2 = "0612345678",
            t3 = "0712345678",
            ptb2 = "0812345678",
            ptb3 = "0912345678",
            email1 = "user@example..com",
            email2 = "@example.com"
        };

        EtablissementMkgtDto expected = new EtablissementMkgtDto
        {
            siret = null,
            t2 = "+33 6 12 34 56 78",
            t3 = "+33 7 12 34 56 78",
            ptb2 = "+33 8 12 34 56 78",
            ptb3 = "+33 9 12 34 56 78",
            email1 = null,
            email2 = null
        };

        // Act
        var methodInfo = typeof(ImportFcli).GetMethod("normalize", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        var result = (EtablissementMkgtDto?)methodInfo.Invoke(_sut, new object[] { input });
        Assert.NotNull(result);

        // Assert
        Assert.Equal(expected.siret, result.siret);
        Assert.Equal(expected.t2, result.t2);
        Assert.Equal(expected.t3, result.t3);
        Assert.Equal(expected.ptb2, result.ptb2);
        Assert.Equal(expected.ptb3, result.ptb3);
        Assert.Equal(expected.email1, result.email1);
        Assert.Equal(expected.email2, result.email2);
    }
    
    [Fact]
    public void Import_ValidInput_CallsCreateAndMapWithNormalizedEtablissementMkgtDTO()
    {
        // Arrange
        var fCliServiceMock = new Mock<IFCliService>();
        IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MkgtClientProfile())).CreateMapper();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var papersServiceMock = new Mock<IPappersUtilitiesService>();

        var etablissementMkgtDTO = new EtablissementMkgtDto
        {
            siret = "82846599700033", intl2 = "robert", intl3 = "DUPOND", code = "TARTEMPHAUB59", cptFac = "234",
            t2 = "0612345678", t3 = "07 12 34 56 78", ptb2 = "08.12.34.56.78", ptb3 = "0033912345678", 
            email1 = "valide@domaine.uk", email2 = "invalide@domaine.com / invalide2@domaine", modReg = "V60", 
            tva = 20.0m, encours = 2000, nom = "TEST", adr1 = "L1", adr2 = "L2", adr3 = "L3", cp = "12345",
            ville = "VILLE", pays = "FRANCE", smTva = "oui", secteur = "59", codPay = "FRA", tpSoc = "SARL",
        };
        var etablissementClientDTO = new EtablissementClientDto
        {
            Siret = "82846599700033", Siren = "828465997", ContactFacturation = "robert", ContactAlternatif = "DUPOND",
            CodeMkgt = "TARTEMPHAUB59", IdOdoo = "234", TelephoneFacturation = "+33 6 12 34 56 78", 
            TelephoneAlternatif = "+33 7 12 34 56 78", PortableFacturation = "+33 8 12 34 56 78",
            PortableAlternatif = "+33 9 12 34 56 78", EmailFacturation = "valide@domaine.uk", EmailAlternatif = null,
            ModeReglement = 0, DelaiReglement = 60, ConditionReglement = 0, TauxTva = 20.0m, EncoursMax = 2000, 
            Nom = "TEST", AdresseFacturation1 = "L1", AdresseFacturation2 = "L2", AdresseFacturation3 = "L3",
            CodePostalFacturation = "12345", PaysFacturation = "FRANCE", VilleFacturation = "VILLE", Radie = false,
        };

        var etablissementFicheDto = new EtablissementFicheDto
        {
            EtablissementCesse = true, Siret = "82846599700033"
        };

        fCliServiceMock.Setup(service => service.GetValidsClients())
            .ReturnsAsync(new List<EtablissementMkgtDto> { etablissementMkgtDTO });

        etablissementClientServiceMock.Setup(service => service.Create(It.IsAny<EtablissementClientDto>()))
            .ReturnsAsync(new EtablissementClientDto(etablissementClientDTO));
        
        etablissementClientServiceMock.Setup(service => service.GetBySiret(It.Is((string s) => s == etablissementMkgtDTO.siret), It.IsAny<bool>()))
            .ReturnsAsync(new EtablissementClientDto(etablissementClientDTO));
        
        papersServiceMock.Setup(service => service.CreateEntrepriseBySiret(It.Is((string s) => s == etablissementMkgtDTO.siret)))
            .ReturnsAsync(etablissementFicheDto);

        var sut = new ImportFcli(fCliServiceMock.Object, mapper, etablissementClientServiceMock.Object, papersServiceMock.Object, _loggerMock.Object);

        // Act
        sut.Import();

        // Assert
        fCliServiceMock.Verify(service => service.GetValidsClients(), Times.Once);
        etablissementClientServiceMock.Verify(service => service.Create(It.Is<EtablissementClientDto>(
            e => e.Siret == etablissementClientDTO.Siret
                 && e.Siren == etablissementClientDTO.Siren
                 && e.ContactFacturation == etablissementClientDTO.ContactFacturation
                 && e.ContactAlternatif == etablissementClientDTO.ContactAlternatif
                 && e.CodeMkgt == etablissementClientDTO.CodeMkgt
                 && e.IdOdoo == etablissementClientDTO.IdOdoo
                 && e.TelephoneFacturation == etablissementClientDTO.TelephoneFacturation
                 && e.TelephoneAlternatif == etablissementClientDTO.TelephoneAlternatif
                 && e.PortableFacturation == etablissementClientDTO.PortableFacturation
                 && e.PortableAlternatif == etablissementClientDTO.PortableAlternatif
                 && e.EmailFacturation == etablissementClientDTO.EmailFacturation
                 && e.EmailAlternatif == etablissementClientDTO.EmailAlternatif
                 && e.ModeReglement == etablissementClientDTO.ModeReglement
                 && e.ConditionReglement ==etablissementClientDTO.ConditionReglement
                 && e.DelaiReglement == etablissementClientDTO.DelaiReglement
                 && e.TauxTva == etablissementClientDTO.TauxTva
                 && e.EncoursMax == etablissementClientDTO.EncoursMax
                 && e.Nom == etablissementClientDTO.Nom
                 && e.AdresseFacturation1 == etablissementClientDTO.AdresseFacturation1
                 && e.AdresseFacturation2 == etablissementClientDTO.AdresseFacturation2
                 && e.AdresseFacturation3 == etablissementClientDTO.AdresseFacturation3
                 && e.CodePostalFacturation == etablissementClientDTO.CodePostalFacturation
                 && e.VilleFacturation == etablissementClientDTO.VilleFacturation
                 && e.PaysFacturation == etablissementClientDTO.PaysFacturation)), Times.Once);
        
        etablissementClientServiceMock.Verify(
            service => service.GetBySiret(It.Is((string s) => s == etablissementMkgtDTO.siret), 
                It.IsAny<bool>()), Times.Once);
        
        papersServiceMock.Verify(
            service => service.CreateEntrepriseBySiret(It.Is((string s) => s == etablissementMkgtDTO.siret)),
            Times.Exactly(2));
        
        etablissementClientServiceMock.Verify(
            service => service.Edit(It.Is<EtablissementClientDto>(
                e => e.Siret == etablissementMkgtDTO.siret
                && e.Radie == true)), Times.Once);
    }

    [Fact]
    public void ImportWithString_ValidInput_CallsCreateAndMapWithNormalizedEtablissementMkgtDTO()
    {
        // Arrange
        var fCliServiceMock = new Mock<IFCliService>();
        IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MkgtClientProfile())).CreateMapper();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var papersServiceMock = new Mock<IPappersUtilitiesService>();

        var etablissementMkgtDTO = new EtablissementMkgtDto
        {
            siret = "82846599700033", intl2 = "robert", intl3 = "DUPOND", code = "TARTEMPHAUB59", cptFac = "234",
            t2 = "0612345678", t3 = "07 12 34 56 78", ptb2 = "08.12.34.56.78", ptb3 = "0033912345678",
            email1 = "valide@domaine.uk", email2 = "invalide@domaine.com / invalide2@domaine", modReg = "V60",
            tva = 20.0m, encours = 2000, nom = "TEST", adr1 = "L1", adr2 = "L2", adr3 = "L3", cp = "12345",
            ville = "VILLE", pays = "FRANCE", smTva = "oui", secteur = "59", codPay = "FRA", tpSoc = "SARL",

        };
        
        var etablissementClientDTO = new EtablissementClientDto
        {
            Siret = "82846599700033", Siren = "828465997", ContactFacturation = "robert", ContactAlternatif = "DUPOND",
            CodeMkgt = "TARTEMPHAUB59", IdOdoo = "234", TelephoneFacturation = "+33 6 12 34 56 78", 
            TelephoneAlternatif = "+33 7 12 34 56 78", PortableFacturation = "+33 8 12 34 56 78",
            PortableAlternatif = "+33 9 12 34 56 78", EmailFacturation = "valide@domaine.uk", EmailAlternatif = null,
            ModeReglement = 0, DelaiReglement = 60, ConditionReglement = 0, TauxTva = 20.0m, EncoursMax = 2000, 
            Nom = "TEST", AdresseFacturation1 = "L1", AdresseFacturation2 = "L2", AdresseFacturation3 = "L3",
            CodePostalFacturation = "12345", PaysFacturation = "FRANCE", VilleFacturation = "VILLE", Radie = false,
        };

        var code = etablissementMkgtDTO.code;
        
        fCliServiceMock.Setup(service => service.GetClient(It.Is((string s) => s == code)))
            .ReturnsAsync(etablissementMkgtDTO);
        
        // Act
        var sut = new ImportFcli(fCliServiceMock.Object, mapper, etablissementClientServiceMock.Object, papersServiceMock.Object, _loggerMock.Object);
        sut.Import(code);
        
        // Assert
        fCliServiceMock.Verify(service => service.GetClient(It.Is((string s) => s == code)), Times.Once);
        etablissementClientServiceMock.Verify( service => service.Create(It.Is<EtablissementClientDto>(
            e => e.Siret == etablissementClientDTO.Siret 
                 && e.Siren == etablissementClientDTO.Siren
                 && e.ContactFacturation == etablissementClientDTO.ContactFacturation
                 && e.ContactAlternatif == etablissementClientDTO.ContactAlternatif
                 && e.CodeMkgt == etablissementClientDTO.CodeMkgt
                 && e.IdOdoo == etablissementClientDTO.IdOdoo
                 && e.TelephoneFacturation == etablissementClientDTO.TelephoneFacturation
                 && e.TelephoneAlternatif == etablissementClientDTO.TelephoneAlternatif
                 && e.PortableFacturation == etablissementClientDTO.PortableFacturation
                 && e.PortableAlternatif == etablissementClientDTO.PortableAlternatif
                 && e.EmailFacturation == etablissementClientDTO.EmailFacturation
                 && e.EmailAlternatif == etablissementClientDTO.EmailAlternatif
                 && e.ModeReglement == etablissementClientDTO.ModeReglement
                 && e.ConditionReglement ==etablissementClientDTO.ConditionReglement
                 && e.DelaiReglement == etablissementClientDTO.DelaiReglement
                 && e.TauxTva == etablissementClientDTO.TauxTva
                 && e.EncoursMax == etablissementClientDTO.EncoursMax
                 && e.Nom == etablissementClientDTO.Nom
                 && e.AdresseFacturation1 == etablissementClientDTO.AdresseFacturation1
                 && e.AdresseFacturation2 == etablissementClientDTO.AdresseFacturation2
                 && e.AdresseFacturation3 == etablissementClientDTO.AdresseFacturation3
                 && e.CodePostalFacturation == etablissementClientDTO.CodePostalFacturation
                 && e.VilleFacturation == etablissementClientDTO.VilleFacturation
                 && e.PaysFacturation == etablissementClientDTO.PaysFacturation)), Times.Once);
        
        papersServiceMock.Verify(
            service => service.CreateEntrepriseBySiret(It.Is((string s) => s == etablissementMkgtDTO.siret)),
            Times.Once);
        
    }

    [Fact]
    public async Task ImportWithString_InvalidCode_returnFalse()
    {
        // Arrange
        var fCliServiceMock = new Mock<IFCliService>();
        IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MkgtClientProfile())).CreateMapper();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        
        var code = "invalidCode";
        fCliServiceMock.Setup(service => service.GetClient(It.Is((string s) => s == code)))
            .ReturnsAsync((EtablissementMkgtDto?)null);
        
        // Act
        var sut = new ImportFcli(fCliServiceMock.Object, mapper, etablissementClientServiceMock.Object, null, null);
        var result = await sut.Import(code);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ImportWithString_ValidInput_BadSiretText_logAndReturnFalse()
    {
        // Arrange
        var fCliServiceMock = new Mock<IFCliService>();
        IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MkgtClientProfile())).CreateMapper();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        
        var etablissementMkgtDTO = new EtablissementMkgtDto
        {
            siret = "BadSiret", intl2 = "robert", intl3 = "DUPOND", code = "TARTEMPHAUB59", cptFac = "234",
            t2 = "0612345678", t3 = "07 12 34 56 78", ptb2 = "08.12.34.56.78", ptb3 = "0033912345678",
            email1 = "valide@domaine.uk", email2 = "invalide@domaine.com / invalide2@domaine", modReg = "V60",
            tva = 20.0m, encours = 2000, nom = "TEST", adr1 = "L1", adr2 = "L2", adr3 = "L3", cp = "12345",
            ville = "VILLE", pays = "FRANCE", smTva = "oui", secteur = "59", codPay = "FRA", tpSoc = "SARL",

        };
        
        var code = etablissementMkgtDTO.code;
        
        fCliServiceMock.Setup(service => service.GetClient(It.Is((string s) => s == code)))
            .ReturnsAsync(etablissementMkgtDTO);
        
        // Act
        var sut = new ImportFcli(fCliServiceMock.Object, mapper, etablissementClientServiceMock.Object, null, _loggerMock.Object);
        var result = await sut.Import(code);
        
        // Assert
        Assert.False(result);
        _loggerMock.Verify(
            logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals("Etablissement invalide", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task ImportWithString_ValidInput_BadSiretDigits_logAndReturnFalse()
    {
        // Arrange
        var fCliServiceMock = new Mock<IFCliService>();
        IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MkgtClientProfile())).CreateMapper();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();

        var etablissementMkgtDTO = new EtablissementMkgtDto
        {
            siret = "123456789", intl2 = "robert", intl3 = "DUPOND", code = "TARTEMPHAUB59", cptFac = "234",
            t2 = "0612345678", t3 = "07 12 34 56 78", ptb2 = "08.12.34.56.78", ptb3 = "0033912345678",
            email1 = "valide@domaine.uk", email2 = "invalide@domaine.com / invalide2@domaine", modReg = "V60",
            tva = 20.0m, encours = 2000, nom = "TEST", adr1 = "L1", adr2 = "L2", adr3 = "L3", cp = "12345",
            ville = "VILLE", pays = "FRANCE", smTva = "oui", secteur = "59", codPay = "FRA", tpSoc = "SARL",

        };

        var code = etablissementMkgtDTO.code;

        fCliServiceMock.Setup(service => service.GetClient(It.Is((string s) => s == code)))
            .ReturnsAsync(etablissementMkgtDTO);

        // Act
        var sut = new ImportFcli(fCliServiceMock.Object, mapper, etablissementClientServiceMock.Object, null,
            _loggerMock.Object);
        var result = await sut.Import(code);

        // Assert
        Assert.False(result);
        _loggerMock.Verify(
            logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals("Etablissement invalide", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }

    [Fact]
    public async Task ImportWithString_ValidInput_EmptySiret_logAndReturnFalse()
    {
        // Arrange
        var fCliServiceMock = new Mock<IFCliService>();
        IMapper mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MkgtClientProfile())).CreateMapper();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();

        var etablissementMkgtDTO = new EtablissementMkgtDto
        {
            siret = null, intl2 = "robert", intl3 = "DUPOND", code = "TARTEMPHAUB59", cptFac = "234",
            t2 = "0612345678", t3 = "07 12 34 56 78", ptb2 = "08.12.34.56.78", ptb3 = "0033912345678",
            email1 = "valide@domaine.uk", email2 = "invalide@domaine.com / invalide2@domaine", modReg = "V60",
            tva = 20.0m, encours = 2000, nom = "TEST", adr1 = "L1", adr2 = "L2", adr3 = "L3", cp = "12345",
            ville = "VILLE", pays = "FRANCE", smTva = "oui", secteur = "59", codPay = "FRA", tpSoc = "SARL",

        };

        var code = etablissementMkgtDTO.code;

        fCliServiceMock.Setup(service => service.GetClient(It.Is((string s) => s == code)))
            .ReturnsAsync(etablissementMkgtDTO);

        // Act
        var sut = new ImportFcli(fCliServiceMock.Object, mapper, etablissementClientServiceMock.Object, null,
            _loggerMock.Object);
        var result = await sut.Import(code);

        // Assert
        
        Assert.False(result);
        _loggerMock.Verify(
            logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals("Etablissement invalide", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
    }
}