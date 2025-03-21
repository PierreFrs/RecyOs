using System.Reflection;
using RecyOs.Engine.Mappers;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.ORM.DTO.hub;

namespace RecyOsTests.EngineTests.Mappers;

public class MkgtEuropeClientProfileTests
{
     
    [Theory]
    [InlineData("12345678901234", "12345678901234")]
    [InlineData(null, "")]
    public void formatAdress_string_ReturnsExpectedValue(string input, string expected)
    {
        // Arrange
        var myClassInstance = new MkgtEuropeClientProfile(); // Remplacer MyClass par le nom de votre classe

        // Act
        MethodInfo? methodInfo = typeof(MkgtEuropeClientProfile).GetMethod("formatAdress", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            var result = (string?) methodInfo.Invoke(myClassInstance, new object[] {input});

            // Assert
            Assert.Equal(expected, result);
        }
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
    public void TransposeReglement_ValidInputs_ReturnsExpectedValue(int conditionReglement, int modeReglement, string expected)
    {
        // Arrange
        var myClassInstance = new MkgtEuropeClientProfile(); // Remplacer MyClass par le nom de votre classe
        ClientEuropeDto etablissementClientDto = new ClientEuropeDto
        {
            Id = 1,
            ConditionReglement = conditionReglement,
            ModeReglement = modeReglement,
            DelaiReglement = 30,
            FactorClientEuropeBus = new List<FactorClientEuropeBuDto>()
        };
        
        // Act
        MethodInfo? methodInfo = typeof(MkgtEuropeClientProfile).GetMethod("transposeReglement", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            var result = (string?) methodInfo.Invoke(myClassInstance, new object[] {etablissementClientDto});

            // Assert
            Assert.Equal(expected, result);
        }
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
    public void TransposeReglement_ValidInputsWithFactor_ReturnsExpectedValue(int conditionReglement, int modeReglement, string expected)
    {
        // Arrange
        var myClassInstance = new MkgtEuropeClientProfile(); // Remplacer MyClass par le nom de votre classe
        FactorClientEuropeBuDto factorClientEuropeBuDto = new FactorClientEuropeBuDto
        {
            IdBu = 1,
            IdClient = 1,
            IsDeleted = false
        };
        
        ClientEuropeDto etablissementClientDto = new ClientEuropeDto
        {
            Id = 1,
            ConditionReglement = conditionReglement,
            ModeReglement = modeReglement,
            DelaiReglement = 30,
            FactorClientEuropeBus = new List<FactorClientEuropeBuDto>()
        };
        etablissementClientDto.FactorClientEuropeBus.Add(factorClientEuropeBuDto);
        
        // Act
        MethodInfo? methodInfo = typeof(MkgtEuropeClientProfile).GetMethod("transposeReglement", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            var result = (string?) methodInfo.Invoke(myClassInstance, new object[] {etablissementClientDto});

            // Assert
            Assert.Equal(expected, result);
        }
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
    public void TransposeReglement_ValidInputsWithDeletedFactor_ReturnsExpectedValue(int conditionReglement, int modeReglement, string expected)
    {
        // Arrange
        var myClassInstance = new MkgtEuropeClientProfile(); // Remplacer MyClass par le nom de votre classe
        FactorClientEuropeBuDto factorClientEuropeBuDto = new FactorClientEuropeBuDto
        {
            IdBu = 1,
            IdClient = 1,
            IsDeleted = true
        };
        ClientEuropeDto etablissementClientDto = new ClientEuropeDto
        {
            Id = 1,
            ConditionReglement = conditionReglement,
            ModeReglement = modeReglement,
            DelaiReglement = 30,
            FactorClientEuropeBus = new List<FactorClientEuropeBuDto>()
        };
        etablissementClientDto.FactorClientEuropeBus.Add(factorClientEuropeBuDto);
        
        // Act
        MethodInfo? methodInfo = typeof(MkgtEuropeClientProfile).GetMethod("transposeReglement", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            var result = (string?) methodInfo.Invoke(myClassInstance, new object[] {etablissementClientDto});

            // Assert
            Assert.Equal(expected, result);
        }
    }
    
    [Theory]
    [InlineData(0.0, "non")]
    [InlineData(10.0, "oui")]
    [InlineData(20.0, "oui")]
    public void IsClientTVA_ValidInputs_ReturnsExpectedValue(decimal tauxTva, string expectedValue)
    {
        // Arrange
        var myClassInstance = new MkgtEuropeClientProfile(); // Remplacer MyClass par le nom de votre classe
        ClientEuropeDto etablissementMkgtDto = new ClientEuropeDto
        {
            TauxTva = tauxTva
        };
        
        // Act
        MethodInfo? methodInfo = typeof(MkgtEuropeClientProfile).GetMethod("isClientTVA", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            var result = (string?) methodInfo.Invoke(myClassInstance, new object[] {etablissementMkgtDto});

            // Assert
            Assert.Equal(expectedValue, result);
        }
    }
    
    [Theory]
    [InlineData("BELGIQUE", "BE")]
    [InlineData("ITALIE", "IT")]
    [InlineData("ESPAGNE", "ES")]
    [InlineData("PAYS-BAS", "NL")]
    [InlineData("LUXEMBOURG", "LU")]
    [InlineData("FRANCE", "12")]
    public void TransposeSecteur_ValidInputs_ReturnsExpectedValue(string paysFacturation, string expectedValue)
    {
        // Arrange
        var myClassInstance = new MkgtEuropeClientProfile(); // Remplacer MyClass par le nom de votre classe
        ClientEuropeDto etablissementMkgtDto = new ClientEuropeDto
        {
            PaysFacturation = paysFacturation,
            CodePostalFacturation = "12345"
        };
        
        // Act
        MethodInfo? methodInfo = typeof(MkgtEuropeClientProfile).GetMethod("transposeSecteur", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            var result = (string?) methodInfo.Invoke(myClassInstance, new object[] {etablissementMkgtDto});

            // Assert
            Assert.Equal(expectedValue, result);
        }
    }
    
    [Theory]
    [InlineData("AUSTRALIE", "AUS")]
    [InlineData("AUTRICHE", "AUT")]
    [InlineData("BELGIQUE", "BEL")]
    [InlineData("BOSNIE-HERZEGOVINE", "BIH")]
    [InlineData("BRESIL", "BRA")]
    [InlineData("CANADA", "CAN")]
    [InlineData("SUISSE", "CHE")]
    [InlineData("CHINE", "CHN")]
    [InlineData("COTE D'IVOIRE", "CIV")]
    [InlineData("REPUBLIQUE CHEQUE", "CZE")]
    [InlineData("ALLEMAGNE", "DEU")]
    [InlineData("DANEMARK", "DNK")]
    [InlineData("ALGERIE", "DZA")]
    [InlineData("ECOSSE", "ECO")]
    [InlineData("ESPAGNE", "ESP")]
    [InlineData("ESTONIE", "EST")]
    [InlineData("FINLANDE", "FIN")]
    [InlineData("FRANCE", "FRA")]
    [InlineData("ROYAUME-UNI", "GBR")]
    [InlineData("HONG KONG", "HKG")]
    [InlineData("INDONESIE", "IDN")]
    [InlineData("INDE", "IND")]
    [InlineData("IRLANDE", "IRL")]
    [InlineData("ITALIE", "ITA")]
    [InlineData("LUXEMBOURG", "LUX")]
    [InlineData("MAROC", "MAR")]
    [InlineData("MALAISIE", "MYS")]
    [InlineData("NOUVELLE CALEDONIE", "NCL")]
    [InlineData("PAYS-BAS", "NLD")]
    [InlineData("NORVEGE", "NOR")]
    [InlineData("PAKISTAN", "PAK")]
    [InlineData("POLOGNE", "POL")]
    [InlineData("PORTUGAL", "PRT")]
    [InlineData("ARABIE SAOUDITE", "SAU")]
    [InlineData("SENEGAL", "SEN")]
    [InlineData("SINGAPORE", "SGP")]
    [InlineData("SLOVENIE", "SVN")]
    [InlineData("SUEDE", "SWE")]
    [InlineData("TAHITI", "TAH")]
    [InlineData("TOGO", "TGO")]
    [InlineData("TURQUIE", "TUR")]
    [InlineData("TAIWAN", "TWN")]
    [InlineData("ETATS-UNIS", "USA")]
    [InlineData("VIETNAM", "VNM")]
    [InlineData("AFRIQUE DU SUD", "FRA")]
    public void GetCountryCode_ValidInputs_ReturnsExpectedValue(string paysFacturation, string expectedValue)
    {
        // Arrange
        var myClassInstance = new MkgtEuropeClientProfile(); // Remplacer MyClass par le nom de votre classe
        ClientEuropeDto etablissementMkgtDto = new ClientEuropeDto
        {
            PaysFacturation = paysFacturation
        };
        
        // Act
        MethodInfo? methodInfo = typeof(MkgtEuropeClientProfile).GetMethod("getCountryCode", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            var result = (string?) methodInfo.Invoke(myClassInstance, new object[] {etablissementMkgtDto});

            // Assert
            Assert.Equal(expectedValue, result);
        }
    }
}