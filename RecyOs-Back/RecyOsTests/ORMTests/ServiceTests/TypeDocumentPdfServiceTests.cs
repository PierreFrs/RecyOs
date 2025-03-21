using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOsTests.ORMTests.ServiceTests;
[Collection("TypeDocumentPdfTests")]
public  class TypeDocumentPdfServiceTests 

{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<ITypeDocumentPdfRepository<TypeDocumentPdf>> _mockRepository;
    private readonly IMapper _mapper;
    private readonly Mock<ITokenInfoService> _mockTokenInfoService;
    private readonly TypeDocumentPdfService<TypeDocumentPdf, TypeDocumentPdfDto> _service;

    public TypeDocumentPdfServiceTests()
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockRepository = new Mock<ITypeDocumentPdfRepository<TypeDocumentPdf>>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TypeDocumentPdf, TypeDocumentPdfDto>();
        });
        _mapper = mapperConfig.CreateMapper();
        _mockTokenInfoService = new Mock<ITokenInfoService>();
        _service = new TypeDocumentPdfService<TypeDocumentPdf, TypeDocumentPdfDto>(
            _mockCurrentContextProvider.Object, 
            _mockRepository.Object, 
            _mapper, 
            _mockTokenInfoService.Object);
    }
    
    [Fact]
    public async Task CreateTypeAsync_ReturnsTypeDocumentPdfDto_WhenCreationIsSuccessful()
    {
        var label = "SampleLabel";
        var mockUser = "TestUser";
        var mockTypeDocumentPdf = new TypeDocumentPdf
        {
            Label = label,
            CreateDate = DateTime.Now,
            CreatedBy = mockUser
        };
        
        // Arrange
        _mockTokenInfoService.Setup(service => service.GetCurrentUserName()).Returns(mockUser);
        _mockRepository.Setup(repo => repo.CreateTypeAsync(It.IsAny <TypeDocumentPdf>()))
            .ReturnsAsync(mockTypeDocumentPdf);
        
        // Act
        var result = await _service.CreateTypeAsync(label);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TypeDocumentPdfDto>(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsTypeDocumentPdfDtoList_WhenRequestIsSuccessful()
    {
        // Arrange
        var label = "SampleLabel";
        var mockUser = "TestUser";
        
        var mockTypeDocuments = new List<TypeDocumentPdf>
        {
            new TypeDocumentPdf
            {
                Label = label,
                CreateDate = DateTime.Now,
                CreatedBy = mockUser
            },
            new TypeDocumentPdf
            {
                Label = label,
                CreateDate = DateTime.Now,
                CreatedBy = mockUser
            },
        };
        
        _mockRepository.Setup((repo => repo.GetListAsync(It.IsAny<ContextSession>(), It.IsAny<bool>()))).ReturnsAsync(mockTypeDocuments);
        
        var mockTypeDocumentDtos = _mapper.Map<List<TypeDocumentPdfDto>>(mockTypeDocuments);
        
        // Act
        var result = await _service.GetAllAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<TypeDocumentPdfDto>>(result);
        Assert.Equal(mockTypeDocumentDtos.Count, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTypeDocumentDto_WhenRequestIsSuccessful()
    {
        // Arrange
        var label = "SampleLabel";
        var mockUser = "TestUser";
        var id = 1;
        var mockTypeDocument = new TypeDocumentPdf
        {
            Label = label,
            CreateDate = DateTime.Now,
            CreatedBy = mockUser
        };
        
        _mockRepository.Setup(repo => repo.GetByIdAsync(id, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(mockTypeDocument);
        
        // Act
        var result = await _service.GetByIdAsync(id);
        var dtoResult = Assert.IsType<TypeDocumentPdfDto>(result);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<TypeDocumentPdfDto>(result);
        Assert.Equal(mockTypeDocument.Label, dtoResult.Label);
        Assert.Equal(mockTypeDocument.CreateDate, dtoResult.CreateDate);
        Assert.Equal(mockTypeDocument.CreatedBy, dtoResult.CreatedBy);
    }

    [Fact]
    public async Task GetByLabelAsync_ReturnsTypeDocumentDto_WhenRequestIsSuccessful()
    {
        var label = "SampleLabel";
        var mockUser = "TestUser";
        var queryLabel = "SampleLabel";
        var mockTypeDocument = new TypeDocumentPdf
        {
            Label = label,
            CreateDate = DateTime.Now,
            CreatedBy = mockUser
        };

        // Arrange
        _mockRepository.Setup(repo => repo.GetByLabelAsync(queryLabel, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(mockTypeDocument);
        
        // Act
        var result = await _service.GetByLabelAsync(queryLabel);
        var dtoResult = Assert.IsType<TypeDocumentPdfDto>(result);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<TypeDocumentPdfDto>(result);
        Assert.Equal(queryLabel, mockTypeDocument.Label);
        Assert.Equal(mockTypeDocument.Label, dtoResult.Label);
        Assert.Equal(mockTypeDocument.CreateDate, dtoResult.CreateDate);
        Assert.Equal(mockTypeDocument.CreatedBy, dtoResult.CreatedBy);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsUpdatedTypeDocumentDto_WhenItemExists()
    {
        // Arrange
        var id = 1;
        var newLabel = "UdatedLabel";
        var mockUser = "TestUser";
        var mockTypeDocument = new TypeDocumentPdf
        {
            Label = "Label",
            CreateDate = DateTime.Now,
            CreatedBy = mockUser
        };
        var updatedMockTypeDocument = new TypeDocumentPdf
        {
            Label = newLabel,
            CreateDate = DateTime.Now,
            CreatedBy = mockUser
        };
        
        _mockTokenInfoService.Setup(service => service.GetCurrentUserName()).Returns(mockUser);
        _mockRepository.Setup(repo => repo.GetByIdAsync(id, It.IsAny<ContextSession>(), It.IsAny<bool>())).ReturnsAsync(mockTypeDocument);
        _mockRepository.Setup(repo => repo.UpdateAsync(mockTypeDocument, It.IsAny<ContextSession>()))
            .ReturnsAsync(updatedMockTypeDocument);
        
        // Act
        var result = await _service.UpdateAsync(id, newLabel);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<TypeDocumentPdfDto>(result);
        Assert.Equal(newLabel, result.Label);
    }

    [Fact]
    public async Task UpdateAsync_returnsNull_WhenItemDoesNotExist()
    {
        // Arrange
        var id = 1;
        var newLabel = "UpdatedLabel";
        
        _mockRepository.Setup(repo => repo.GetByIdAsync(id, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync((TypeDocumentPdf?)null);
        
        // Act
        var result = await _service.UpdateAsync(id, newLabel);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_returnsTrue_WhenRequestIsSuccessful()
    {
        // Arrange
        var id = 1;
        var mockTypeDocument = new TypeDocumentPdf
        {
            Label = "Label",
            CreateDate = DateTime.Now,
            CreatedBy = "mockUser"
        };
        
        _mockRepository.Setup(repo => repo.GetByIdAsync(id, It.IsAny<ContextSession>(), It.IsAny<bool>())).ReturnsAsync(mockTypeDocument);
        _mockRepository.Setup(repo => repo.DeleteAsync(id, It.IsAny<ContextSession>())).ReturnsAsync(true);
        
        // Act
        var result = await _service.DeleteAsync(id);
        
        // Assert
        Assert.True(result);
        _mockRepository.Verify(repo => repo.DeleteAsync(id, It.IsAny<ContextSession>()), Times.Once());
    }
    
    [Fact]
    public async Task DeleteAsync_returnsFalse_WhenItemDoesNotExist()
    {
        // Arrange
        var id = 1;
        
        _mockRepository.Setup(repo => repo.DeleteAsync(id, It.IsAny<ContextSession>())).ReturnsAsync(false);
        
        // Act
        var result = await _service.DeleteAsync(id);
        
        // Assert
        Assert.False(result);
        _mockRepository.Verify(repo => repo.DeleteAsync(id, It.IsAny<ContextSession>()), Times.Never);
    }
}
