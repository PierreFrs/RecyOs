// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => DataValidationServiceTests.cs
// Created : 2024/03/26 - 17:12
// Updated : 2024/03/26 - 17:12

using RecyOs.ORM.Service;

namespace RecyOsTests.ORMTests.ServiceTests;

[Collection("DataValidationServiceTests")]
public class DataValidationServiceTests
{
    [Fact]
    public void ValidatePhoneNumber_ValidPhoneNumber_ReturnsTrue()
    {
        // Arrange
        var dataValidationService = new DataValidationService();
        var phoneNumber = "+33 1 23 45 67 89";

        // Act
        var result = dataValidationService.ValidatePhoneNumber(phoneNumber);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidatePhoneNumber_InvalidPhoneNumber_ReturnsFalse()
    {
        // Arrange
        var dataValidationService = new DataValidationService();
        var phoneNumber = "012345678";

        // Act
        var result = dataValidationService.ValidatePhoneNumber(phoneNumber);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateEmailAddress_ValidEmailAddress_ReturnsTrue()
    {
        // Arrange
        var dataValidationService = new DataValidationService();
        var emailAddress = "toto@gmail.com";

        // Act
        var result = dataValidationService.ValidateEmailAddress(emailAddress);

        // Assert
        Assert.True(result);
    }
    
    [Theory]
    [InlineData("toto@gmail")]
    [InlineData("toto")]
    [InlineData("toto@")]
    [InlineData("@gmail.com")]
    [InlineData("toto@gmail.")]
    public void ValidateEmailAddress_InvalidEmailAddresses_ReturnsFalse(string emailAddress)
    {
        // Arrange
        var dataValidationService = new DataValidationService();

        // Act
        var result = dataValidationService.ValidateEmailAddress(emailAddress);

        // Assert
        Assert.False(result);
    }
}