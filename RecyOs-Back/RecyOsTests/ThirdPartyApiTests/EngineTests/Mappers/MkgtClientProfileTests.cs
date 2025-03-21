using System.Globalization;
using System.Reflection;
using RecyOs.Engine.Mappers;
using RecyOs.Engine.Modules.Mkgt;

namespace RecyOsTests.EngineTests.Mappers;

public class MkgtClientProfileTests
{
    [Theory]
    [InlineData("20,0", "411103")]
    [InlineData("10,0", "411102")]
    [InlineData("5,5", "411101")]
    [InlineData("0,0", "411104")]
    [InlineData("19,1", "411103")]
    public void getCompteComptable_EtablissementMkgtDto_ReturnsExpectedValue(string inputStr, string expected)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
        // Arrange
        var myClassInstance = new MkgtClientProfile(); // Remplacer MyClass par le nom de votre classe

        // Act
        MethodInfo? methodInfo = typeof(MkgtClientProfile).GetMethod("getCompteComptable", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            decimal input = decimal.Parse(inputStr);
            var EtablissementMkgtDto = new EtablissementMkgtDto();
            EtablissementMkgtDto.tva = input;
            string? result = (string?) methodInfo.Invoke(myClassInstance, new object[] {EtablissementMkgtDto});

            // Assert
            Assert.Equal(expected, result);
        }
    }
    
    [Theory]
    [InlineData("12345678901234", "12345678901234")]
    [InlineData(null, "")]
    public void formatAdress_string_ReturnsExpectedValue(string input, string expected)
    {
        // Arrange
        var myClassInstance = new MkgtClientProfile(); // Remplacer MyClass par le nom de votre classe

        // Act
        MethodInfo? methodInfo = typeof(MkgtClientProfile).GetMethod("formatAdress", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            var result = (string?) methodInfo.Invoke(myClassInstance, new object[] {input});

            // Assert
            Assert.Equal(expected, result);
        }
    }
    
    [Theory]
    [InlineData("CHE", 0)]
    [InlineData("ESP", 0)]
    [InlineData("CB", 0)]
    [InlineData("V30", 30)]
    [InlineData("BadValue", 0)]
    public void getDelaiReglement_ReturnsExpectedValue(string input, int expected)
    {
        // Arrange
        var myClassInstance = new MkgtClientProfile(); // Remplacer MyClass par le nom de votre classe

        // Act
        MethodInfo? methodInfo = typeof(MkgtClientProfile).GetMethod("getDelaiReglement", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            var EtablissementMkgtDto = new EtablissementMkgtDto();
            EtablissementMkgtDto.modReg = input;
            var result = (int?) methodInfo.Invoke(myClassInstance, new object[] {EtablissementMkgtDto});

            // Assert
            Assert.Equal(expected, result);
        }
    }
    
    [Theory]
    [InlineData("CHE", 1)]
    [InlineData("ESP", 1)]
    [InlineData("CB", 1)]
    [InlineData("V30", 0)]
    [InlineData("BadValue", 1)]
    public void getModeReglement_ReturnsExpectedValue(string input, int expected)
    {
        // Arrange
        var myClassInstance = new MkgtClientProfile(); // Remplacer MyClass par le nom de votre classe

        // Act
        MethodInfo? methodInfo = typeof(MkgtClientProfile).GetMethod("getConditionReglement", BindingFlags.NonPublic | BindingFlags.Instance);
        if (methodInfo != null)
        {
            var EtablissementMkgtDto = new EtablissementMkgtDto();
            EtablissementMkgtDto.modReg = input;
            var result = (int?) methodInfo.Invoke(myClassInstance, new object[] {EtablissementMkgtDto});

            // Assert
            Assert.Equal(expected, result);
        }
    }
    
}