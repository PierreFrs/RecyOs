// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => FileValidationServiceTests.cs
// Created : 2023/12/28 - 16:37
// Updated : 2023/12/28 - 16:37

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service;

namespace RecyOsTests.ORMTests.ServiceTests;

public class FileValidationServiceTests
{
    private readonly Mock<ICurrentContextProvider> _mockContextProvider;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IEtablissementClientRepository<EtablissementClient>> _mockEtablissementClientRepository;
        private readonly Mock<IClientEuropeRepository<ClientEurope>> _mockClientEurope;
        private readonly Mock<ITypeDocumentPdfRepository<TypeDocumentPdf>> _mockTypeDocumentPdfRepository;
        private readonly FileValidationService _fileValidationService;

        public FileValidationServiceTests()
        {
            _mockContextProvider = new Mock<ICurrentContextProvider>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockEtablissementClientRepository = new Mock<IEtablissementClientRepository<EtablissementClient>>();
            _mockClientEurope = new Mock<IClientEuropeRepository<ClientEurope>>();
            _mockTypeDocumentPdfRepository = new Mock<ITypeDocumentPdfRepository<TypeDocumentPdf>>();

            _fileValidationService = new FileValidationService(
                _mockContextProvider.Object,
                _mockConfiguration.Object,
                _mockEtablissementClientRepository.Object,
                _mockClientEurope.Object,
                _mockTypeDocumentPdfRepository.Object);
        }

        [Fact]
        public async Task ValidateEtablissementClientId_ThrowsArgumentNullException_IfIdIsZero()
        {
            int? invalidId = 0;

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _fileValidationService.ValidateEtablissementClientId(invalidId));
        }

        [Fact]
        public async Task ValidateEtablissementClientId_ThrowsNoException_IfValidId()
        {
            int? validId = 1;
            _mockEtablissementClientRepository.Setup(repo => repo.Get(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
                                              .ReturnsAsync(new EtablissementClient());

            await _fileValidationService.ValidateEtablissementClientId(validId);
        }
        
        [Fact]
        public async Task ValidateEtablissementClientId_ThrowsArgumentNullException_IfExistingEtablissementClientIsNull()
        {
            int? validId = 1;
            
            _mockEtablissementClientRepository.Setup(repo => repo.Get(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
                .ReturnsAsync((EtablissementClient?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _fileValidationService.ValidateEtablissementClientId(validId));
        }

        [Fact]
        public async Task ValidateEtablissementClientEuropeId_ThrowsArgumentNullException_IfIdIsZero()
        {
            int? invalidId = 0;

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _fileValidationService.ValidateEtablissementClientEuropeId(invalidId));
        }

        [Fact]
        public async Task ValidateEtablissementClientEuropeId_ThrowsNoException_IfValidId()
        {
            int? validId = 1;
            _mockClientEurope.Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
                .ReturnsAsync(new ClientEurope());

            await _fileValidationService.ValidateEtablissementClientEuropeId(validId);
        }
        
        [Fact]
        public async Task ValidateEtablissementClientEuropeId_ThrowsArgumentNullException_IfExistingEtablissementClientIsNull()
        {
            int? validId = 1;
            
            _mockEtablissementClientRepository.Setup(repo => repo.Get(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
                .ReturnsAsync((EtablissementClient?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _fileValidationService.ValidateEtablissementClientEuropeId(validId));
        }

        [Fact]
        public async Task ValidateTypeDocumentPdfId_ThrowArgumentNullException_IfIdEqualOrLessThanZeroAndIsUpdateIsFalse()
        {
            int? typeDocumentPdfId = 0;
            bool isUpdate = false;

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _fileValidationService.ValidateTypeDocumentPdfId(typeDocumentPdfId, isUpdate));
        }
        
        [Fact]
        public async Task ValidateTypeDocumentPdfId_KeyNotFoundException_IfExistingTypeIsNullAndTypeDocumentPdfIsMoreThanZero()
        {
            // Arrange
            int? typeDocumentPdfId = 1;
            bool isUpdate = false;

            _mockTypeDocumentPdfRepository
                .Setup(repo => repo.GetByIdAsync(typeDocumentPdfId.Value, new ContextSession(), isUpdate))
                .ReturnsAsync((TypeDocumentPdf)null);
            
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _fileValidationService.ValidateTypeDocumentPdfId(typeDocumentPdfId, isUpdate));
        }
        
        [Fact]
        public async Task ValidateTypeDocumentPdfId_ThrowArgumentNullException_IfTypeDocumentIdIsNullAndIsUpdateIsFalse()
        {
            int? typeDocumentPdfId = null;
            bool isUpdate = false;
            
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _fileValidationService.ValidateTypeDocumentPdfId(typeDocumentPdfId, isUpdate));
        }
        
        [Fact]
        public async Task ValidateTypeDocumentPdfId_ThrowNoException_IfTypeDocumentIsValid()
        {
            int? typeDocumentPdfId = 1;
            bool isUpdate = true;
            
            _fileValidationService.ValidateTypeDocumentPdfId(typeDocumentPdfId, isUpdate);
        }
        
        [Fact]
        public async Task ValidateTypeDocumentPdfId_ThrowsNoException_IfTypeDocumentIsValidAndIsUpdateFalse()
        {
            // Arrange
            int? typeDocumentPdfId = 1;
            bool isUpdate = false;

            var mockTypeDocumentPdf = new TypeDocumentPdf
            {
                Id = 1,
                Label = "mockLabel"
            };

            _mockTypeDocumentPdfRepository
                .Setup(repo => repo.GetByIdAsync(1, It.IsAny<ContextSession>(), isUpdate))
                .ReturnsAsync(mockTypeDocumentPdf);

            // Act & Assert
            await _fileValidationService.ValidateTypeDocumentPdfId(typeDocumentPdfId, isUpdate);
        }
        
        [Fact]
        public void ValidateFile_InvalidSize_ThrowsArgumentException()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.Length).Returns(1025 * 1024 * 1024); // Setting file size greater than 1 MB for this example

            _mockConfiguration.Setup(config => config["FileValidation:FileSizeLimit"]).Returns("1"); // 1 MB limit

            Assert.Throws<ArgumentException>(() => _fileValidationService.ValidateFile(fileMock.Object));
        }

        [Fact]
        public void ValidateFile_InvalidExtension_ThrowsArgumentException()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.FileName).Returns("file.txt"); // Invalid file extension
            fileMock.Setup(_ => _.Length).Returns(100 * 1024); // Valid file size

            _mockConfiguration.Setup(config => config["FileValidation:AllowedExtensions"]).Returns(".pdf");
            _mockConfiguration.Setup(config => config["FileValidation:FileSizeLimit"]).Returns((1024 * 1024).ToString()); // 1 MB limit

            Assert.Throws<ArgumentException>(() => _fileValidationService.ValidateFile(fileMock.Object));
        }

        [Fact]
        public void ValidateFile_ValidFile_NoException()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.FileName).Returns("file.pdf");
            fileMock.Setup(_ => _.Length).Returns(1024);

            _mockConfiguration.Setup(config => config["FileValidation:AllowedExtensions"]).Returns(".pdf");
            _mockConfiguration.Setup(config => config["FileValidation:FileSizeLimit"]).Returns((1024 * 1024).ToString());

            _fileValidationService.ValidateFile(fileMock.Object);
        }
    }