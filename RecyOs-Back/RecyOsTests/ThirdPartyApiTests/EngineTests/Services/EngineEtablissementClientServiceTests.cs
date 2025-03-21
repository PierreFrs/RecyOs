using Moq;
using Xunit;
using System.Collections.Generic;
using AutoMapper;
using RecyOs.Engine.Services;
using RecyOs.Engine.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.MapProfile;

namespace RecyOsTests
{
    public class EngineEtablissementClientServiceTests
    {
        private readonly Mock<IEngineEtablissementClientRepository> mockRepository;
        private readonly IMapper _mapper;
        public EngineEtablissementClientServiceTests()
        {
            mockRepository = new Mock<IEngineEtablissementClientRepository>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EtablissementClientProfile());
            }).CreateMapper();
        }

        [Fact]
        public void GetCreatedItems_ShouldCallRepositoryMethod()
        {
            // Arrange
            var service = new EngineEtablissementClientService(mockRepository.Object, _mapper);
            var moduleName = "ModuleTest";

            // Act
            service.GetCreatedItems(moduleName);

            // Assert
            mockRepository.Verify(repo => repo.GetCreatedEntities(moduleName), Times.Once);
        }

        [Fact]
        public void GetUpdatedItems_ShouldCallRepositoryMethod()
        {
            // Arrange
            var service = new EngineEtablissementClientService(mockRepository.Object, _mapper);
            var moduleName = "ModuleTest";

            // Act
            service.GetUpdatedItems(moduleName);

            // Assert
            mockRepository.Verify(repo => repo.GetUpdatedEntities(moduleName), Times.Once);
        }
        
        [Fact]
        public void CallBackDestIdCreation_ShouldCallRepositoryMethod()
        {
            // Arrange
            var service = new EngineEtablissementClientService(mockRepository.Object, _mapper);
            var moduleName = "ModuleTest";
            var items = new List<EtablissementClientDto>();

            // Act
            service.CallBackDestIdCreation(moduleName, items);

            // Assert
            mockRepository.Verify(repo => repo.CallBackDestIdCreation(moduleName, It.IsAny<List<EtablissementClient>>()), Times.Once);
        }
    }
}