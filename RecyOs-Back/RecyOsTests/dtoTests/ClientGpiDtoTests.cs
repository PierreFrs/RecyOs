using System.Reflection;
using RecyOs.ORM.Hub.DTO;

namespace RecyOsTests.dtoTests;

public class ClientGpiDtoTests
{
    [Theory]
    [InlineData(0, "Virement")]
    [InlineData(1, "L.C.R.")]
    [InlineData(2, "Chèque")]
    [InlineData(4, "Carte bancaire")]
    [InlineData(10004, "Virement")]
    
    public void GetModeReglement_ShouldReturnString(int prmModeReglement, string prmExpected)
    {
        // Arrange
        var clientGpiDto = new ClientGpiDto();
        
        // Act
        var GetModeReglementMethod = clientGpiDto.GetType().GetMethod("GetModeReglement", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(GetModeReglementMethod);
        var result = GetModeReglementMethod.Invoke(clientGpiDto, new object[] {prmModeReglement});
        
        // Assert
        Assert.Equal(prmExpected, result);
    }
}