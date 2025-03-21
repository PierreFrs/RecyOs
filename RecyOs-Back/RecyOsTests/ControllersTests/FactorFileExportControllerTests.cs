// <copyright file="FactorFileExportControllerTests.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers;
using RecyOs.ORM.Interfaces;

namespace RecyOsTests.ControllersTests;
[Collection("FactorClientBuTestsCollection")]

public class FactorFileExportControllerTests
{
    private readonly Mock<IFactorFileExportService> _mockFactorFileExportService;
    private readonly FactorFileExportController _controller;

    public FactorFileExportControllerTests()
    {
        _mockFactorFileExportService = new Mock<IFactorFileExportService>();
        _controller = new FactorFileExportController(_mockFactorFileExportService.Object);
    }

    [Fact]
    public async Task ExportFactorFile_ReturnsFileResult()
    {
        // Arrange
        var expectedFileResult = new FileContentResult(new byte[0], "application/zip");
        _mockFactorFileExportService.Setup(service => service.ExportFactorFileAsync())
            .ReturnsAsync(expectedFileResult);

        // Act
        var result = await _controller.ExportFactorFile();

        // Assert
        Assert.IsType<FileContentResult>(result);
        _mockFactorFileExportService.Verify(service => service.ExportFactorFileAsync(), Times.Once);
    }
}