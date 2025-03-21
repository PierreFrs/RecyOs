using System.Reflection;
using RecyOs.Engine.Modules.Odoo;

namespace RecyOsTests.EngineTests.Mappers;

public class OdooEuropeClientProfileTests
{
    [Theory]
    [InlineData("12345", 12345)]
    [InlineData("BadValue", 0)]
    public void ParseIdOdoo_ValidId_ReturnsExpectedValue(string inputStr, long expected)
    {
        // Arrange
        var myClassInstance = new OdooEuropeClientProfile(); // Remplacer MyClass par le nom de votre classe

        // Act
        var result = myClassInstance.ParseIdOdoo(inputStr);
        
        // Assert
        Assert.Equal(expected, result);

    }
    
    [Theory]
    [InlineData(0, 1)]
    [InlineData(30, 4)]
    [InlineData(45, 29)]
    [InlineData(60, 39)]
    [InlineData(null, 1)]
    [InlineData(15, 1)] // cas non spécifié, on s'attend à ce que cela renvoie la valeur par défaut, 1
    public void ParsePaymentTermId_ReturnsExpectedValue(int? input, int expected)
    {
        // Arrange
        var myClassInstance = new OdooEuropeClientProfile(); // Remplacer MyClass par le nom de votre classe

        // Act
        int result = myClassInstance.ParsePaymentTermId(input);

        // Assert
        Assert.Equal(expected, result);
    }
}