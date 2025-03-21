// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => CommercialControllerTests.cs
// Created : 2024/03/26 - 17:24
// Updated : 2024/03/26 - 17:24

using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;

namespace RecyOsTests.ControllersTests;

[Collection("CommercialTestsCollection")]
public class CommercialControllerTests
{
    private readonly CommercialController _commercialController;
    private readonly Mock<ICommercialBaseService> _commercialServiceMock;
    private readonly Mock<IDataValidationService> _dataValidationServiceMock;

    public CommercialControllerTests()
    {
        _commercialServiceMock = new Mock<ICommercialBaseService>();
        _dataValidationServiceMock = new Mock<IDataValidationService>();
        _commercialController = new CommercialController(_commercialServiceMock.Object, _dataValidationServiceMock.Object);
    }
    
    /********** Getters **********/
    [Fact]
    public async Task GetList_ReturnsOk()
    {
        // Arrange
        _commercialServiceMock.Setup(x => x.GetListAsync()).ReturnsAsync(new List<CommercialDto>());

        // Act
        var result = await _commercialController.GetList();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task GetById_WithValidId_ReturnsOk()
    {
        // Arrange
        var commercialDto = new CommercialDto();
        _commercialServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(commercialDto);

        // Act
        var result = await _commercialController.GetById(1);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task GetById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _commercialServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((CommercialDto)null);

        // Act
        var result = await _commercialController.GetById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task GetClientsByCommercialId_WithValidId_ReturnsOk()
    {
        // Arrange
        var clientsList = new GridData<object>();
        _commercialServiceMock.Setup(x => x.GetClientsByCommercialIdAsync(It.IsAny<int>(), It.IsAny<ClientByCommercialFilter>()))
            .ReturnsAsync(clientsList);

        // Act
        var result = await _commercialController.GetClientsByCommercialId(1, new ClientByCommercialFilter());

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task GetClientsByCommercialId_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _commercialServiceMock.Setup(x => x.GetClientsByCommercialIdAsync(It.IsAny<int>(), It.IsAny<ClientByCommercialFilter>()))
            .ReturnsAsync((GridData<object>)null);

        // Act
        var result = await _commercialController.GetClientsByCommercialId(1, new ClientByCommercialFilter());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    
    public async Task GetFilteredList_ReturnsOk()
    {
        // Arrange
        _commercialServiceMock.Setup(x => x.GetFilteredListAsync(It.IsAny<CommercialFilter>())).ReturnsAsync(new GridData<CommercialDto>());

        // Act
        var result = await _commercialController.GetFilteredList(new CommercialFilter());

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    /********** Create **********/

    [Fact]
    public async Task CreateCommercial_WithValidData_ReturnsOk()
    {
        // Arrange
        var commercialDtoCreate = new CommercialDto
        {
            Firstname = "John",
            Lastname = "Doe",
            Phone = "1234567890",
            Email = "johndoe@gmail.com",
            CodeMkgt = "DD"
        };
        _dataValidationServiceMock.Setup(x => x.ValidatePhoneNumber(commercialDtoCreate.Phone)).Returns(true);
        _dataValidationServiceMock.Setup(x => x.ValidateEmailAddress(commercialDtoCreate.Email)).Returns(true);
        _commercialServiceMock.Setup(x => x.CreateAsync(commercialDtoCreate)).ReturnsAsync(new CommercialDto());

        // Act
        var result = await _commercialController.CreateCommercial(commercialDtoCreate);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CreateCommercial_WithInvalidPhone_ReturnsBadRequest()
    {
        // Arrange
        var commercialDtoCreate = new CommercialDto
        {
            Firstname = "John",
            Lastname = "Doe",
            Phone = "123456789",
            Email = "johndoe@gmail.com",
            CodeMkgt = "DD"
        };
        _dataValidationServiceMock.Setup(x => x.ValidatePhoneNumber(commercialDtoCreate.Phone)).Returns(false);
        _dataValidationServiceMock.Setup(x => x.ValidateEmailAddress(commercialDtoCreate.Email)).Returns(true);
        
        // Act
        var result = await _commercialController.CreateCommercial(commercialDtoCreate);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateCommercial_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var commercialDtoCreate = new CommercialDto
        {
            Firstname = "John",
            Lastname = "Doe",
            Phone = "1234567890",
            Email = "johndoegmail",
            CodeMkgt = "DD"
        };
        _dataValidationServiceMock.Setup(x => x.ValidatePhoneNumber(commercialDtoCreate.Phone)).Returns(true);
        _dataValidationServiceMock.Setup(x => x.ValidateEmailAddress(commercialDtoCreate.Email)).Returns(false);
        
        // Act
        var result = await _commercialController.CreateCommercial(commercialDtoCreate);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateCommercial_IfNull_ReturnsNotFound()
    {
        // Arrange
        var commercialDtoCreate = new CommercialDto
        {
            Firstname = "John",
            Lastname = "Doe",
            Phone = "1234567890",
            Email = "johndoe@gmail.com",
            CodeMkgt = "EE"
        };
        _dataValidationServiceMock.Setup(x => x.ValidatePhoneNumber(commercialDtoCreate.Phone)).Returns(true);
        _dataValidationServiceMock.Setup(x => x.ValidateEmailAddress(commercialDtoCreate.Email)).Returns(true);
        _commercialServiceMock.Setup(x => x.CreateAsync(commercialDtoCreate)).ReturnsAsync((CommercialDto)null);

        // Act
        var result = await _commercialController.CreateCommercial(commercialDtoCreate);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    /********** Update **********/
    

    [Fact]
    public async Task Update_WithValidData_ReturnsOk()
    {
        // Arrange
        var commercialDto = new CommercialDto
        {
            Firstname = "Jane",
            Lastname = "Doe",
            Phone = "1234567890",
            Email = "janedoe@gmail.com",
            CodeMkgt = "DD"
        };
        _dataValidationServiceMock.Setup(x => x.ValidatePhoneNumber(commercialDto.Phone)).Returns(true);
        _dataValidationServiceMock.Setup(x => x.ValidateEmailAddress(commercialDto.Email)).Returns(true);
        _commercialServiceMock.Setup(x => x.UpdateAsync(It.IsAny<int>(), commercialDto))
            .ReturnsAsync(new CommercialDto());

        // Act
        var result = await _commercialController.UpdateCommercial(1, commercialDto);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task Update_WithNullData_ReturnsBadRequest()
    {
        // Arrange
        // Act
        var result = await _commercialController.UpdateCommercial(1, null);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Update_WithInvalidPhone_ReturnsBadRequest()
    {
        // Arrange
        var commercialDto = new CommercialDto
        {
            Firstname = "Jane",
            Lastname = "Doe",
            Phone = "123456789",
            Email = "janedoe@gmail.com",
            CodeMkgt = "DD"
        };
        _dataValidationServiceMock.Setup(x => x.ValidatePhoneNumber(commercialDto.Phone)).Returns(false);
        _dataValidationServiceMock.Setup(x => x.ValidateEmailAddress(commercialDto.Email)).Returns(true);
        _commercialServiceMock.Setup(x => x.UpdateAsync(It.IsAny<int>(), commercialDto))
            .ReturnsAsync(new CommercialDto());

        // Act
        var result = await _commercialController.UpdateCommercial(1, commercialDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Update_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var commercialDto = new CommercialDto
        {
            Firstname = "Jane",
            Lastname = "Doe",
            Phone = "1234567890",
            Email = "janedoegmail",
            CodeMkgt = "DD"
        };
        _dataValidationServiceMock.Setup(x => x.ValidatePhoneNumber(commercialDto.Phone)).Returns(true);
        _dataValidationServiceMock.Setup(x => x.ValidateEmailAddress(commercialDto.Email)).Returns(false);
        _commercialServiceMock.Setup(x => x.UpdateAsync(It.IsAny<int>(), commercialDto))
            .ReturnsAsync(new CommercialDto());

        // Act
        var result = await _commercialController.UpdateCommercial(1, commercialDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Update_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var commercialDto = new CommercialDto
        {
            Firstname = "Jane",
            Lastname = "Doe",
            Phone = "123456789",
            Email = "janedoegmail",
            CodeMkgt = "DD"
        };
        _dataValidationServiceMock.Setup(x => x.ValidatePhoneNumber(commercialDto.Phone)).Returns(false);
        _dataValidationServiceMock.Setup(x => x.ValidateEmailAddress(commercialDto.Email)).Returns(false);
        _commercialServiceMock.Setup(x => x.UpdateAsync(It.IsAny<int>(), commercialDto))
            .ReturnsAsync(new CommercialDto());

        // Act
        var result = await _commercialController.UpdateCommercial(1, commercialDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Update_IfNull_ReturnsNotFound()
    {
        // Arrange
        var commercialDto = new CommercialDto
        {
            Firstname = "Jane",
            Lastname = "Doe",
            Phone = "123456789",
            Email = "janedoegmail",
            CodeMkgt = "DD"
        };
        _dataValidationServiceMock.Setup(x => x.ValidatePhoneNumber(commercialDto.Phone)).Returns(true);
        _dataValidationServiceMock.Setup(x => x.ValidateEmailAddress(commercialDto.Email)).Returns(true);
        _commercialServiceMock.Setup(x => x.UpdateAsync(It.IsAny<int>(), commercialDto))
            .ReturnsAsync((CommercialDto)null);

        // Act
        var result = await _commercialController.UpdateCommercial(1, commercialDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    /********** Delete **********/
    [Fact]
    public async Task DeleteById_WithValidId_ReturnsOk()
    {
        // Arrange
        _commercialServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);

        // Act
        var result = await _commercialController.DeleteById(1);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Theory]
    [InlineData(999)]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DeleteById_WithInvalidId_ReturnsNotFound(int id)
    {
        // Arrange
        _commercialServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);

        // Act
        var result = await _commercialController.DeleteById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}