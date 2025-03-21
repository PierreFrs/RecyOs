// <copyright file="FactorFileExportServiceTests.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service;

namespace RecyOsTests.ORMTests.hubTests.FactorClientBuServicesTests;
[Collection("FactorClientBuTestsCollection")]

public class FactorFileExportServiceTests
{
    private readonly Mock<IFactorFileExportRepository> _mockFactorFileExportRepository;
        private readonly Mock<IEtablissementClientRepository<EtablissementClient>> _mockEtablissementClientRepository;
        private readonly Mock<IClientEuropeRepository<ClientEurope>> _mockClientEuropeRepository;
        private readonly Mock<IBusinessUnitRepository<BusinessUnit>> _mockBusinessUnitRepository;
        private readonly FactorFileExportService _service;

        public FactorFileExportServiceTests()
        {
            _mockFactorFileExportRepository = new Mock<IFactorFileExportRepository>();
            _mockEtablissementClientRepository = new Mock<IEtablissementClientRepository<EtablissementClient>>();
            _mockClientEuropeRepository = new Mock<IClientEuropeRepository<ClientEurope>>();
            _mockBusinessUnitRepository = new Mock<IBusinessUnitRepository<BusinessUnit>>();

            _service = new FactorFileExportService(
                _mockFactorFileExportRepository.Object,
                _mockEtablissementClientRepository.Object,
                _mockClientEuropeRepository.Object,
                _mockBusinessUnitRepository.Object
            );
        }

        [Fact]
        public async Task ExportFactorFileAsync_ReturnsFileResult()
        {
            // Arrange
            var businessUnits = new List<BusinessUnit>
            {
                new BusinessUnit { Id = 1, Libelle = "BU1" },
                new BusinessUnit { Id = 2, Libelle = "BU2" }
            };

            var factorClientList = new List<FactorClientDto>
            {
                new FactorClientDto { ClientId = 1, Nom = "Client1", Siret = "SIRET1" },
                new FactorClientDto { ClientId = 2, Nom = "Client2" }
            };

            _mockBusinessUnitRepository.Setup(repo => repo.GetListAsync(It.IsAny<ContextSession>(), false))
                .ReturnsAsync(businessUnits);
            _mockFactorFileExportRepository.Setup(repo => repo.ExportFactorFileRepositoryAsync(It.IsAny<int>()))
                .ReturnsAsync(factorClientList);
            
            _mockFactorFileExportRepository.Setup(repo => repo.UpdateExportDateForFranceClientsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            _mockFactorFileExportRepository.Setup(repo => repo.UpdateExportDateForEuropeClientsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.ExportFactorFileAsync();

            // Assert
            Assert.IsType<FileContentResult>(result);
            var fileContentResult = result as FileContentResult;
            Assert.Equal("application/zip", fileContentResult.ContentType);
            Assert.Equal("clients_affacturage.zip", fileContentResult.FileDownloadName);
        }
}