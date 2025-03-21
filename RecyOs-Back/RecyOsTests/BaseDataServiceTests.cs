using RecyOs.Engine.Services;
using RecyOs.OdooDB.DTO;

namespace RecyOsTests;

public class BaseDataServiceTests
{
    private class DataServiceMock : BaseDataService<ResPartnerDto>
    {
        public override ResPartnerDto AddItem(ResPartnerDto item)
        {
            return item; // Just return the same item.
        }

        public override ResPartnerDto UpdateItem(ResPartnerDto item)
        {
            return item; // Assume update is always successful.
        }
    }
    
    [Fact]
    public void AddItems_ShouldReturnAddedItems()
    {
        // Arrange
        var items = new List<ResPartnerDto>
        {
            new ResPartnerDto { Id = 11 },
            new ResPartnerDto { Id = 22 }
        };
            
        var dataService = new DataServiceMock();

        // Act
        var result = dataService.AddItems(items);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(11, result[0].Id);
        Assert.Equal(22, result[1].Id);
    }
    
    [Fact]
    public void UpdateItems_ShouldReturnAddedItems()
    {
        // Arrange
        var items = new List<ResPartnerDto>
        {
            new ResPartnerDto { Id = 11 },
            new ResPartnerDto { Id = 22 }
        };
            
        var dataService = new DataServiceMock();

        // Act
        var result = dataService.UpdateItems(items);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(11, result[0].Id);
        Assert.Equal(22, result[1].Id);
    }
        
}