using System.Reflection;
using RecyOs.Engine.Mappers;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.ORM.DTO.hub;

namespace RecyOsTests;

public class MkgtClientProfileTest
{
    [Fact]
    public void TransposeSecteur_ReturnsExpectedResult()
    {
        // Arrange
        MkgtClientProfile profile = new MkgtClientProfile();
        EtablissementClientDto prm = new EtablissementClientDto
        {
            CodePostalFacturation = "75001"
        };
        string expected = "75";

        // Act
        MethodInfo? methodInfo = typeof(MkgtClientProfile).GetMethod("transposeSecteur", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            string? result = (string?)methodInfo.Invoke(profile, new object[] { prm });

            // Assert
            Assert.Equal(expected, result);
        }
    }
    
    [Fact]
    public void TransposeSecteur_ThrowsArgumentNullException_IfPrmIsNull()
    {
        // Arrange
        MkgtClientProfile profile = new MkgtClientProfile();
        EtablissementClientDto? prm = null;

        // Act & Assert
        MethodInfo? methodInfo = typeof(MkgtClientProfile).GetMethod("transposeSecteur", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(profile);
        Assert.NotNull(methodInfo);
        Assert.Throws<TargetInvocationException>(() => methodInfo.Invoke(profile, new object?[] { prm }));
    }
    
    [Theory]
    [InlineData(0, 0, "V30")]
    [InlineData(0, 1, "L30")]
    [InlineData(0, 5, "T30")]
    [InlineData(0, 99, "V30")]
    [InlineData(1, 2, "CHE")]
    [InlineData(1, 3, "ESP")]
    [InlineData(1, 4, "CB")]
    [InlineData(2, 0, "V30")]
    public void TransposeReglement_ValidInputs_ReturnsExpectedValue(int conditionReglement, int modeReglement, string expectedReglement)
    {
        // Arrange
        MkgtClientProfile sut = new MkgtClientProfile(); 
        EtablissementClientDto etablissementClientDto = new EtablissementClientDto
        {
            ConditionReglement = conditionReglement,
            ModeReglement = modeReglement,
            DelaiReglement = 30,
            FactorClientFranceBus = new List<FactorClientFranceBuDto>()
        };

        // Act
        Assert.NotNull(sut);
        Assert.NotNull(etablissementClientDto);
        var methodInfo = sut.GetType().GetMethod("transposeReglement", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        var result = methodInfo!.Invoke(sut, new object[] { etablissementClientDto });

        // Assert
        Assert.Equal(expectedReglement, result);
    }
    
    [Theory]
    [InlineData(0, 0, "FAC")]
    [InlineData(0, 1, "FAC")]
    [InlineData(0, 5, "FAC")]
    [InlineData(0, 99, "FAC")]
    [InlineData(1, 2, "FAC")]
    [InlineData(1, 3, "FAC")]
    [InlineData(1, 4, "FAC")]
    [InlineData(2, 0, "FAC")]
    public void TransposeReglement_ValidInputsWithFactor_ReturnsExpectedValue(int conditionReglement, int modeReglement, string expectedReglement)
    {
        // Arrange
        MkgtClientProfile sut = new MkgtClientProfile(); 
        FactorClientFranceBuDto factor = new FactorClientFranceBuDto
        {
            IdClient = 1,
            IdBu = 1,
            IsDeleted = false
        };
        EtablissementClientDto etablissementClientDto = new EtablissementClientDto
        {
            Id = 1,
            ConditionReglement = conditionReglement,
            ModeReglement = modeReglement,
            DelaiReglement = 30,
            FactorClientFranceBus = new List<FactorClientFranceBuDto>()
        };
        etablissementClientDto.FactorClientFranceBus.Add(factor);

        // Act
        Assert.NotNull(sut);
        Assert.NotNull(etablissementClientDto);
        var methodInfo = sut.GetType().GetMethod("transposeReglement", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        var result = methodInfo!.Invoke(sut, new object[] { etablissementClientDto });

        // Assert
        Assert.Equal(expectedReglement, result);
    }
    
    [Theory]
    [InlineData(0, 0, "V30")]
    [InlineData(0, 1, "L30")]
    [InlineData(0, 5, "T30")]
    [InlineData(0, 99, "V30")]
    [InlineData(1, 2, "CHE")]
    [InlineData(1, 3, "ESP")]
    [InlineData(1, 4, "CB")]
    [InlineData(2, 0, "V30")]
    public void TransposeReglement_ValidInputsWithDeletedFactor_ReturnsExpectedValue(int conditionReglement, int modeReglement, string expectedReglement)
    {
        // Arrange
        MkgtClientProfile sut = new MkgtClientProfile(); 
        FactorClientFranceBuDto factor = new FactorClientFranceBuDto
        {
            IdClient = 1,
            IdBu = 1,
            IsDeleted = true
        };
        
        EtablissementClientDto etablissementClientDto = new EtablissementClientDto
        {
            ConditionReglement = conditionReglement,
            ModeReglement = modeReglement,
            DelaiReglement = 30,
            FactorClientFranceBus = new List<FactorClientFranceBuDto>()
        };
        etablissementClientDto.FactorClientFranceBus.Add(factor);

        // Act
        Assert.NotNull(sut);
        Assert.NotNull(etablissementClientDto);
        var methodInfo = sut.GetType().GetMethod("transposeReglement", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        var result = methodInfo!.Invoke(sut, new object[] { etablissementClientDto });

        // Assert
        Assert.Equal(expectedReglement, result);
    }
    
    [Theory]
    [InlineData(0.0, "non")]
    [InlineData(10.0, "oui")]
    [InlineData(20.0, "oui")]
    public void IsClientTVA_ValidInputs_ReturnsExpectedValue(decimal tauxTva, string expectedValue)
    {
        // Arrange
        var sut = new MkgtClientProfile(); // Remplacez 'YourClass' par le nom de la classe qui contient la méthode 'isClientTVA'
        var etablissementClientDto = new EtablissementClientDto
        {
            TauxTva = tauxTva
        };

        // Act
        var methodInfo = sut.GetType().GetMethod("isClientTVA", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        var result = methodInfo!.Invoke(sut, new object[] { etablissementClientDto });
        
        // Assert
        Assert.Equal(expectedValue, result);
    }
    
    [Theory]
    [InlineData("1000", "E.I")]
    [InlineData("5202", "S.N.C")]
    [InlineData("5498", "E.U.R.L")]
    [InlineData("5499", "S.A.R.L")]
    [InlineData("5599", "S.A")]
    [InlineData("5710", "S.A.S")]
    [InlineData("5720", "S.A.S.U")]
    [InlineData("6540", "S.C.I")]
    [InlineData("6533", "G.A.E.C")]
    [InlineData("9220", "ASSOC")]
    public void GetFormeJuridique_ValidInputs_ReturnsExpectedValue(string categorieJuridique, string expectedFormeJuridique)
    {
        // Arrange
        var sut = new MkgtClientProfile(); // Remplacez 'YourClass' par le nom de la classe qui contient la méthode 'getFormeJuridique'
        
        // Act
        var methodInfo = sut.GetType().GetMethod("getFormeJuridique", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        var result = methodInfo.Invoke(sut, new object[] { categorieJuridique });

        // Assert
        Assert.Equal(expectedFormeJuridique, result);
    }
    
    [Fact]
    public void GetFormeJuridique_InvalidInput_ReturnsNull()
    {
        // Arrange
        var sut = new MkgtClientProfile(); // Remplacez 'YourClass' par le nom de la classe qui contient la méthode 'getFormeJuridique'

        // Act
        var methodInfo = sut.GetType().GetMethod("getFormeJuridique", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        var result = methodInfo.Invoke(sut, new object[] { "-1" });

        // Assert
        Assert.Null(result);
    }
    
    [Theory]
    [InlineData("France", "FRA")]
    [InlineData("United States", "UNI")]
    [InlineData("Australia", "AUS")]
    public void GetCountryCode_ValidInputs_ReturnsExpectedValue(string paysFacturation, string expectedCountryCode)
    {
        // Arrange
        var sut = new MkgtClientProfile(); // Remplacez 'YourClass' par le nom de la classe qui contient la méthode 'getCountryCode'
        var etablissementClientDto = new EtablissementClientDto
        {
            PaysFacturation = paysFacturation
        };

        // Act
        var methodInfo = sut.GetType().GetMethod("getCountryCode", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        var result = methodInfo.Invoke(sut, new object[] { etablissementClientDto });

        // Assert
        Assert.Equal(expectedCountryCode, result);
    }
    
    [Fact]
    public void GetCountryCode_ShortInput_ReturnsInputInUpperCase()
    {
        // Arrange
        var sut = new MkgtClientProfile(); // Remplacez 'YourClass' par le nom de la classe qui contient la méthode 'getCountryCode'
        var etablissementClientDto = new EtablissementClientDto
        {
            PaysFacturation = "Fr"
        };

        // Act
        var methodInfo = sut.GetType().GetMethod("getCountryCode", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        var result = methodInfo.Invoke(sut, new object[] { etablissementClientDto });

        // Assert
        Assert.Equal("FR", result);
    }
    
    [Fact]
    public void GetCountryCode_EmptyInput_ReturnsEmptyString()
    {
        // Arrange
        var sut = new MkgtClientProfile(); // Remplacez 'YourClass' par le nom de la classe qui contient la méthode 'getCountryCode'
        var etablissementClientDto = new EtablissementClientDto
        {
            PaysFacturation = ""
        };

        // Act
        var methodInfo = sut.GetType().GetMethod("getCountryCode", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        var result = methodInfo.Invoke(sut, new object[] { etablissementClientDto });

        // Assert
        Assert.Equal("", result);
    }
    
    [Theory]
    [InlineData("A12", true)]
    [InlineData("z99", true)]
    [InlineData("F00", true)]
    [InlineData("a1", false)]
    [InlineData("A123", false)]
    [InlineData("1A2", false)]
    [InlineData("AA12", false)]
    [InlineData("", false)]
    public void IsValidFormat_Input_ReturnsExpectedResult(string input, bool expectedResult)
    {
        // Arrange
        var sut = new MkgtClientProfile(); // Remplacez 'YourClass' par le nom de la classe qui contient la méthode 'IsValidFormat'

        // Act
        var isValidFormatMethod = sut.GetType().GetMethod("IsValidFormat", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(isValidFormatMethod);
        var result = (bool?)isValidFormatMethod.Invoke(null, new object[] { input });

        // Assert
        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData("CHE", 2)]
    [InlineData("ESP", 3)]
    [InlineData("CB", 4)]
    [InlineData("V12", 0)]
    [InlineData("L99", 1)]
    [InlineData("T00", 5)]
    [InlineData("C55", 2)]
    [InlineData("Z25", 0)]
    [InlineData("invalid", 0)]
    public void GetModeReglement_Input_ReturnsExpectedResult(string modReg, int expectedResult)
    {
        // Arrange
        var sut = new MkgtClientProfile(); // Remplacez 'YourClass' par le nom de la classe qui contient la méthode 'getModeReglement'
        var etablissementMkgtDto = new EtablissementMkgtDto
        {
            modReg = modReg
        };

        // Act
        var getModeReglementMethod = sut.GetType().GetMethod("getModeReglement", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(getModeReglementMethod);
        var result = (int?)getModeReglementMethod.Invoke(sut, new object[] { etablissementMkgtDto });

        // Assert
        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData("CHE", 0)]
    [InlineData("ESP", 0)]
    [InlineData("CB", 0)]
    [InlineData("V12", 12)]
    [InlineData("L99", 99)]
    [InlineData("T00", 0)]
    [InlineData("C55", 55)]
    [InlineData("Z25", 25)]
    [InlineData("invalid", 0)]
    public void GetDelaiReglement_Input_ReturnsExpectedResult(string modReg, int expectedResult)
    {
        // Arrange
        var sut = new MkgtClientProfile(); // Remplacez 'YourClass' par le nom de la classe qui contient la méthode 'getDelaiReglement'
        var etablissementMkgtDto = new EtablissementMkgtDto
        {
            modReg = modReg
        };

        // Act
        var getDelaiReglementMethod = sut.GetType().GetMethod("getDelaiReglement", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(getDelaiReglementMethod);
        var result = (int?)getDelaiReglementMethod.Invoke(sut, new object[] { etablissementMkgtDto });

        // Assert
        Assert.Equal(expectedResult, result);
    }
    
    [Theory]
    [InlineData("CHE", 1)]
    [InlineData("ESP", 1)]
    [InlineData("CB", 1)]
    [InlineData("V12", 0)]
    [InlineData("L99", 0)]
    [InlineData("T00", 0)]
    [InlineData("C55", 0)]
    [InlineData("Z25", 0)]
    [InlineData("FAC", 1)]
    public void GetConditionReglement_Input_ReturnsExpectedResult(string modReg, int expectedResult)
    {
        // Arrange
        var sut = new MkgtClientProfile(); // Remplacez 'YourClass' par le nom de la classe qui contient la méthode 'getConditionReglement'
        var etablissementMkgtDto = new EtablissementMkgtDto
        {
            modReg = modReg
        };

        // Act
        var getConditionReglementMethod = sut.GetType().GetMethod("getConditionReglement", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(getConditionReglementMethod);
        var result = (int?)getConditionReglementMethod.Invoke(sut, new object[] { etablissementMkgtDto });

        // Assert
        Assert.Equal(expectedResult, result);
    }
}