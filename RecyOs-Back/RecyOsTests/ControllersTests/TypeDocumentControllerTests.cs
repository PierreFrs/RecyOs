using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Interfaces;

namespace RecyOsTests.ControllersTests;

[Collection("TypeDocumentPdfTests")]
public class TypeDocumentControllerTests
{
    private readonly Mock<ITypeDocumentPdfService<TypeDocumentPdfDto>> _mockTypeDocumentPdfService;
    private readonly TypeDocumentController _controller;
    public TypeDocumentControllerTests()
    {
        _mockTypeDocumentPdfService = new Mock<ITypeDocumentPdfService<TypeDocumentPdfDto>>();
        _controller = new TypeDocumentController(_mockTypeDocumentPdfService.Object);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task CreateType_ReturnsNotFound_WhenTypeIsNullOrEmpty(string typeLabel)
    {
        // Arrange
        _mockTypeDocumentPdfService.Setup(service => service.CreateTypeAsync(typeLabel))
            .ReturnsAsync((TypeDocumentPdfDto?)null);
        
        // Act
        var result = await _controller.CreateType(typeLabel);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateType_ShouldReturnOk_IfCategoryIsValid()
    {
        // Arrange
        string testType = "testType";

        var typeDocumentDto = new TypeDocumentPdfDto();
        _mockTypeDocumentPdfService.Setup(service => service.CreateTypeAsync(It.IsAny<string>()))
            .ReturnsAsync(typeDocumentDto);
        
        // Act
        var result = await _controller.CreateType(testType);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(typeDocumentDto, okResult.Value);
    }
    
    [Fact]
    public async Task Get_ReturnsTypeDocumentsList()
    {
        
        // Arrange
        var label = "SampleLabel";
        var mockUser = "TestUser";
        var mockTypeDocuments = new List<TypeDocumentPdfDto>
        {
            new TypeDocumentPdfDto
            {
                Label = label,
                CreateDate = DateTime.Now,
                CreatedBy = mockUser
            },
            new TypeDocumentPdfDto
            {
                Label = label,
                CreateDate = DateTime.Now,
                CreatedBy = mockUser
            },
        };
        _mockTypeDocumentPdfService.Setup((service => service.GetAllAsync())).ReturnsAsync(mockTypeDocuments);
        
        // Act
        var result = await _controller.Get();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedTypes = Assert.IsAssignableFrom<List<TypeDocumentPdfDto>>(okResult.Value);
        Assert.Equal(mockTypeDocuments.Count, returnedTypes.Count);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_IfTypeIsNull()
    {
        // Arrange
        var id = 1;
        _mockTypeDocumentPdfService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync((TypeDocumentPdfDto?)null);
        
        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnOk_IfTypeIsValid()
    {
        // Arrange
        int typeDocumentId = 1;
        string typeDocumentLabel = "testType";

        var mockTypeDocumentDto = new TypeDocumentPdfDto()
        {
            Id = typeDocumentId,
            Label = typeDocumentLabel
        };
        
        _mockTypeDocumentPdfService.Setup(service => service.GetByIdAsync(typeDocumentId))
            .ReturnsAsync(mockTypeDocumentDto);
        
        // Act
        var result = await _controller.GetById(typeDocumentId);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsAssignableFrom<TypeDocumentPdfDto>(okResult.Value);
        Assert.Equal(typeDocumentLabel, returnedDto.Label);
    }
    
    [Fact]
    public async Task GetByLabel_ReturnsNotFound_IfTypeIsNull()
    {
        // Arrange
        var label = "test type";
        _mockTypeDocumentPdfService.Setup(service => service.GetByLabelAsync(label)).ReturnsAsync((TypeDocumentPdfDto?)null);
        
        // Act
        var result = await _controller.GetByLabel(label);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task GetByLabel_ShouldReturnOk_IfTypeIsValid()
    {
        // Arrange
        int typeDocumentId = 1;
        string typeDocumentLabel = "testType";

        var mockTypeDocumentDto = new TypeDocumentPdfDto()
        {
            Id = typeDocumentId,
            Label = typeDocumentLabel
        };
        
        _mockTypeDocumentPdfService.Setup(service => service.GetByLabelAsync(typeDocumentLabel))
            .ReturnsAsync(mockTypeDocumentDto);
        
        // Act
        var result = await _controller.GetByLabel(typeDocumentLabel);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsAssignableFrom<TypeDocumentPdfDto>(okResult.Value);
        Assert.Equal(typeDocumentLabel, returnedDto.Label);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_IfTypeIsNull()
    {
        // Arrange
        var label = "test type";
        var id = 1;
        _mockTypeDocumentPdfService.Setup(service => service.UpdateAsync(id, label)).ReturnsAsync((TypeDocumentPdfDto?)null);
        
        // Act
        var result = await _controller.Update(id, label);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Update_ShouldReturnOk_IfTypeIsValid()
    {
        // Arrange
        int typeDocumentId = 1;
        string typeDocumentLabel = "testType";

        var newMockTypeDocumentDto = new TypeDocumentPdfDto()
        {
            Id = typeDocumentId,
            Label = typeDocumentLabel
        };
        
        _mockTypeDocumentPdfService.Setup(service => service.UpdateAsync(typeDocumentId, typeDocumentLabel))
            .ReturnsAsync(newMockTypeDocumentDto);
        
        // Act
        var result = await _controller.Update(typeDocumentId, typeDocumentLabel);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsAssignableFrom<TypeDocumentPdfDto>(okResult.Value);
        Assert.Equal(typeDocumentLabel, returnedDto.Label);
    }
    
    [Fact]
    public async Task DeleteById_ReturnsNotFound_IfDeleteFails()
    {
        // Arrange
        var id = 1;
        _mockTypeDocumentPdfService.Setup(service => service.DeleteAsync(id)).ReturnsAsync(false);
        
        // Act
        var result = await _controller.DeleteById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task DeleteById_ReturnsOk_IfDeleteSucceeds()
    {
        // Arrange
        var id = 1;
        _mockTypeDocumentPdfService.Setup(service => service.DeleteAsync(id)).ReturnsAsync(true);
        
        // Act
        var result = await _controller.DeleteById(id);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}