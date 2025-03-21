using System.Collections;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using RecyOs.Helpers;
using RecyOs.MKGT_DB.Entities;
using RecyOs.MKGT_DB.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Results;
using RecyOs.ORM.Service.hub;

namespace RecyOsTests.ORMTests.hubTests;

public class EtablissementClientServiceTests
{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<IEtablissementFicheRepository<EtablissementFiche>> _mockEtablissementFicheRepository;
    private readonly Mock<IFCliRepository<Fcli>> _mockFCliRepository;
    private readonly Mock<IPappersUtilitiesService> _mockPappersUtilitiesService;
    private readonly Mock<IEtablissementServiceUtilitaryMethods> _mockEtablissementServiceUtilitaryMethods;
    private readonly Mock<ITokenInfoService> _mockTokenInfoService;
    private readonly IMapper _mapper;
    private readonly Mock<IEtablissementClientRepository<EtablissementClient>> _mockRepository;
    private readonly Mock<IGroupRepository> _mockGroupRepository;
    public EtablissementClientServiceTests()
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockEtablissementFicheRepository = new Mock<IEtablissementFicheRepository<EtablissementFiche>>();
        _mockFCliRepository = new Mock<IFCliRepository<Fcli>>();
        _mockPappersUtilitiesService = new Mock<IPappersUtilitiesService>();
        _mockEtablissementServiceUtilitaryMethods = new Mock<IEtablissementServiceUtilitaryMethods>();
        _mockTokenInfoService = new Mock<ITokenInfoService>();
        _mockRepository = new Mock<IEtablissementClientRepository<EtablissementClient>>();
        _mockGroupRepository = new Mock<IGroupRepository>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<EtablissementClient, EtablissementClientDto>().ReverseMap();
            cfg.CreateMap<EntrepriseBase, EntrepriseBaseDto>().ReverseMap();
            cfg.CreateMap<EtablissementFiche, EtablissementFicheDto>().ReverseMap();
            cfg.CreateMap<Group, GroupDto>().ReverseMap();
        });
        _mapper = mapperConfig.CreateMapper();
    }
    
    private EtablissementClientService<EtablissementClient> CreateService()
    {
        return new EtablissementClientService<EtablissementClient>(
            _mockCurrentContextProvider.Object, 
            _mockRepository.Object,
            _mockEtablissementFicheRepository.Object,
            _mockFCliRepository.Object,
            _mockPappersUtilitiesService.Object,
            _mockEtablissementServiceUtilitaryMethods.Object,
            _mockTokenInfoService.Object,
            _mapper,
            _mockGroupRepository.Object
        );
    }
    
    /********** Getters **********/
    
    [Fact]
    public async Task GetDataForGrid_ShouldReturnGridData()
    {
        // Arrange
        var filter = new EtablissementClientGridFilter();
        var tuple = (new List<EtablissementClient>(), 0);
        _mockRepository.Setup(repo => repo.GetFiltredListWithCount(filter, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(tuple);
        var service = CreateService();
        
        // Act
        var result = await service.GetDataForGrid(filter);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<GridData<EtablissementClientDto>>(result);
        _mockRepository.Verify(repo => repo.GetFiltredListWithCount(It.IsAny<EtablissementClientGridFilter>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnEtablissementClientDto()
    {
        // Arrange
        var etablissementClient = new EtablissementClient();
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissementClient);
        var service = CreateService();
        
        // Act
        var result = await service.GetById(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementClientDto>(result);
        _mockRepository.Verify(repo => repo.GetById(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()), Times.Once);
    }
    
    [Fact]
    public async Task GetBySiret_ShouldReturnEtablissementClientDto()
    {
        // Arrange
        var etablissementClient = new EtablissementClient();
        _mockRepository.Setup(repo => repo.GetBySiret(It.IsAny<string>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(etablissementClient);
        var service = CreateService();
        
        // Act
        var result = await service.GetBySiret("123456789");
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementClientDto>(result);
        _mockRepository.Verify(repo => repo.GetBySiret(It.IsAny<string>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetBySiret_ShouldReturnEtablissementClientDto_WhenIncludeIsTrue()
    {
        // Arrange
        var etablissementClient = new EtablissementClient();
        _mockRepository.Setup(repo => repo.GetBySiret(It.IsAny<string>(), It.IsAny<ContextSession>(), true))
            .ReturnsAsync(etablissementClient);
        var service = CreateService();
        
        // Act
        var result = await service.GetBySiret("123456789", true);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementClientDto>(result);
        _mockRepository.Verify(repo => repo.GetBySiret(It.IsAny<string>(), It.IsAny<ContextSession>(), true), Times.Once);
    }
    
    [Fact]
    public async Task GetByCodeKerlog_ShouldReturnEtablissementClientDto()
    {
        // Arrange
        var etablissementClient = new EtablissementClient();
        _mockRepository.Setup(repo => repo.GetByCodeKerlog(It.IsAny<string>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(etablissementClient);
        var service = CreateService();
        
        // Act
        var result = await service.GetByCodeKerlog("123456789");
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementClientDto>(result);
        _mockRepository.Verify(repo => repo.GetByCodeKerlog(It.IsAny<string>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetByCodeKerlog_ShouldReturnEtablissementClientDto_WhenIncludeIsTrue()
    {
        // Arrange
        var etablissementClient = new EtablissementClient();
        _mockRepository.Setup(repo => repo.GetByCodeKerlog(It.IsAny<string>(), It.IsAny<ContextSession>(), true))
            .ReturnsAsync(etablissementClient);
        var service = CreateService();
        
        // Act
        var result = await service.GetByCodeKerlog("123456789", true);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementClientDto>(result);
        _mockRepository.Verify(repo => repo.GetByCodeKerlog(It.IsAny<string>(), It.IsAny<ContextSession>(), true), Times.Once);
    }
    
    [Fact]
    public async Task GetByCodeMkgt_ShouldReturnEtablissementClientDto()
    {
        // Arrange
        var etablissementClient = new EtablissementClient();
        _mockRepository.Setup(repo => repo.GetByCodeMkgt(It.IsAny<string>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(etablissementClient);
        var service = CreateService();
        
        // Act
        var result = await service.GetByCodeMkgt("123456789");
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementClientDto>(result);
        _mockRepository.Verify(repo => repo.GetByCodeMkgt(It.IsAny<string>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetByCodeMkgt_ShouldReturnEtablissementClientDto_WhenIncludeIsTrue()
    {
        // Arrange
        var etablissementClient = new EtablissementClient();
        _mockRepository.Setup(repo => repo.GetByCodeMkgt(It.IsAny<string>(), It.IsAny<ContextSession>(), true))
            .ReturnsAsync(etablissementClient);
        var service = CreateService();
        
        // Act
        var result = await service.GetByCodeMkgt("123456789", true);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementClientDto>(result);
        _mockRepository.Verify(repo => repo.GetByCodeMkgt(It.IsAny<string>(), It.IsAny<ContextSession>(), true), Times.Once);
    }
    
    [Fact]
    public async Task GetByIdOdoo_ShouldReturnEtablissementClientDto()
    {
        // Arrange
        var etablissementClient = new EtablissementClient();
        _mockRepository.Setup(repo => repo.GetByIdOdoo(It.IsAny<string>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(etablissementClient);
        var service = CreateService();
        
        // Act
        var result = await service.GetByIdOdoo("1");
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementClientDto>(result);
        _mockRepository.Verify(repo => repo.GetByIdOdoo(It.IsAny<string>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetByIdOdoo_ShouldReturnEtablissementClientDto_WhenIncludeIsTrue()
    {
        // Arrange
        var etablissementClient = new EtablissementClient();
        _mockRepository.Setup(repo => repo.GetByIdOdoo(It.IsAny<string>(), It.IsAny<ContextSession>(), true))
            .ReturnsAsync(etablissementClient);
        var service = CreateService();
        
        // Act
        var result = await service.GetByIdOdoo("1", true);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementClientDto>(result);
        _mockRepository.Verify(repo => repo.GetByIdOdoo(It.IsAny<string>(), It.IsAny<ContextSession>(), true), Times.Once);
    }
    
    [Fact]
    public async Task GetGroup_ShouldReturnGroupDto()
    {
        // Arrange
        var group = new Group();
        var etablissementClient = new EtablissementClient { GroupId = 1 };
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(etablissementClient);
        _mockGroupRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(group);
        var service = CreateService();
        
        // Act
        var result = await service.GetGroup(1);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<GroupDto>(result);
        _mockGroupRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), false), Times.Once);
    }

    [Fact]
    public async Task GetGroup_ShouldReturnNull_IfGroupIsNotFound()
    {
        // Arrange
        var etablissementClient = new EtablissementClient { GroupId = 1 };
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(etablissementClient);
        _mockGroupRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync((Group)null);
        var service = CreateService();
        
        // Act
        var result = await service.GetGroup(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetEtablissementGroupBySirenAsync_ShouldReturnEtablissementClientList_IfSirenIsValid()
    {
        // Arrange
        var etablissementClient = new EtablissementClient { Siren = "056800659" }; // Mock data
        var expectedEtablissements = new List<EtablissementClient> { etablissementClient };

        _mockRepository
            .Setup(repo => repo.GetEtablissementGroupBySirenAsync(It.IsAny<string>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(expectedEtablissements);
        var service = CreateService();

        // Act
        var result = await service.GetEtablissementGroupBySirenAsync("056800659");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("056800659", result.First().Siren);
    }
    
    /********** Create **********/
    
    [Fact]
    public async Task Create_ShouldReturnEtablissementClientDto()
    {
        // Arrange
        var etablissementClient = new EtablissementClient();
        _mockRepository.Setup(repo => repo.CreateIfDoesntExistAsync(It.IsAny<EtablissementClient>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(etablissementClient);
        var service = CreateService();
        
        // Act
        var result = await service.Create(new EtablissementClientDto());
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementClientDto>(result);
        _mockRepository.Verify(repo => repo.CreateIfDoesntExistAsync(It.IsAny<EtablissementClient>(), It.IsAny<ContextSession>()), Times.Once);
    }
    
    [Fact]
    public async Task CreateFull_ShouldReturnEtablissementClientDto()
    {
        // Arrange
        var etablissementClient = new EtablissementClient();
        var etablClientDto = new EtablissementClientDto();
        var entrepriseBaseDto = new EntrepriseBaseDto();
        var etablissementFicheDto = new EtablissementFicheDto();
        
        _mockRepository.Setup(repo => repo.Create(It.IsAny<EtablissementClient>(), It.IsAny<EntrepriseBase>(), It.IsAny<EtablissementFiche>(),It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissementClient);
        var service = CreateService();
        
        // Act
        var result = await service.Create(etablClientDto, entrepriseBaseDto, etablissementFicheDto, false);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementClientDto>(result);
        _mockRepository.Verify(repo => repo.Create(It.IsAny<EtablissementClient>(), It.IsAny<EntrepriseBase>(),
            It.IsAny<EtablissementFiche>(), It.IsAny<ContextSession>(), It.IsAny<bool>()));
    }

    [Fact]
    public async Task CreateFromScratchAsync_ShouldReturnEtablissementClientDto_IfEntityIsCreated()
    {
        // Arrange
        var siret = "12345678901234";
        var siren = siret.Substring(0, 9);

        var etablissementClient = new EtablissementClient
        {
            Siret = siret,
            Siren = siren
        };

        _mockRepository.Setup(repo => repo.Create(
            It.IsAny<EtablissementClient>(),
            It.IsAny<EntrepriseBase>(),
            It.IsAny<EtablissementFiche>(),
            It.IsAny<ContextSession>(),
            It.IsAny<bool>()
        )).ReturnsAsync(etablissementClient);

        var service = CreateService();

        // Act
        var result = await service.CreateFromScratchAsync(siret);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(siret, result.Siret);
        Assert.Equal(siren, result.Siren);
        _mockRepository.Verify(repo => repo.Create(
            It.Is<EtablissementClient>(ec => ec.Siret == siret && ec.Siren == siren),
            It.Is<EntrepriseBase>(eb => eb.Siren == siren),
            It.Is<EtablissementFiche>(ef => ef.Siret == siret),
            It.IsAny<ContextSession>(),
            It.IsAny<bool>()
        ), Times.Once);
    }

    [Fact]
    public async Task CreateFromScratchAsync_ShouldThrowException_WhenEntityIsNotCreated()
    {
        // Arrange
        var siret = "12345678901234";

        _mockRepository.Setup(repo => repo.Create(
            It.IsAny<EtablissementClient>(),
            It.IsAny<EntrepriseBase>(),
            It.IsAny<EtablissementFiche>(),
            It.IsAny<ContextSession>(),
            It.IsAny<bool>()
        )).ReturnsAsync((EtablissementClient)null);

        var service = CreateService();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => service.CreateFromScratchAsync(siret));

        Assert.Equal("Impossible de créer l'établissement.", exception.Message);
    }
    
    /********** Update **********/
    
    [Fact]
    public async Task Edit_ShouldReturnEtablissementClientDto()
    {
        // Arrange
        var etablissementClient = new EtablissementClient();
        _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<EtablissementClient>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(etablissementClient);
        var service = CreateService();
        
        // Act
        var result = await service.Edit(new EtablissementClientDto());
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementClientDto>(result);
        _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<EtablissementClient>(), It.IsAny<ContextSession>()), Times.Once);
    }
    
    [Fact]
    public async Task Edit_ShouldThrowException_WhenDbUpdateExceptionIsThrown()
    {
        // Arrange
        var service = CreateService();
        var dto = new EtablissementClientDto { /* Initialize properties */ };
        
        _mockRepository
            .Setup(repo => repo.UpdateAsync(It.IsAny<EtablissementClient>(), It.IsAny<ContextSession>()))
            .ThrowsAsync(new DbUpdateException());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => service.Edit(dto));

        Assert.Equal("Impossible de mettre à jour l'établissement client", exception.Message);
    }
    
    [Fact]
    public async Task ChangeSiretAsync_ShouldReturnSuccess_WhenAllStepsPass()
    {
        // Arrange
        var id = 1;
        var newSiret = "12345678901234";
        var session = new ContextSession();
        var oldEtablissementClient = new EtablissementClient { Id = id, Siret = "oldSiret" };
        var oldEtablissementFiche = new EtablissementFiche { Siret = "oldSiret" };
        var newEtablissementClientDto = new EtablissementClientDto { Siret = newSiret };
        var newEtablissementClient = new EtablissementClient { Siret = newSiret };
        var validationResult = new ServiceResult
        {
            Success = true,
            StatusCode = 200,
            Message = "Success",
            Data = new SiretUpdateEntitiesListResult
            {
                EtablissementClient = oldEtablissementClient,
                EtablissementFiche = oldEtablissementFiche
            }
        };
        
        var service = CreateService();
        
        // Mock the EntityChecks method in the service itself
       _mockEtablissementServiceUtilitaryMethods.Setup(x => x.EntityChecks(id, newSiret, session))
            .ReturnsAsync(validationResult);
        _mockRepository.Setup(repo => repo.GetById(id, session, false))
            .ReturnsAsync(oldEtablissementClient);
        _mockEtablissementFicheRepository.SetupSequence(repo => repo.GetBySiret(It.IsAny<string>(), session, false))
            .ReturnsAsync(oldEtablissementFiche).ReturnsAsync((EtablissementFiche)null);
        _mockPappersUtilitiesService.Setup(x => x.CreateEtablissementClientBySiret(newSiret, true, false, false))
            .ReturnsAsync(newEtablissementClientDto);
        _mockTokenInfoService.Setup(x => x.GetCurrentUserName())
            .Returns("TestUser");
        _mockRepository.Setup(repo => repo.DeleteErpCodeAsync(oldEtablissementClient.Id, "mkgt", session))
            .ReturnsAsync(oldEtablissementClient);
        _mockRepository.Setup(repo => repo.UpdateWithNewSiretAsync(It.IsAny<EtablissementClient>(), session))
            .ReturnsAsync(newEtablissementClient);
        _mockRepository.Setup(repo => repo.Delete(id, session))
            .Returns(Task.CompletedTask);
        _mockEtablissementFicheRepository.Setup(repo => repo.Delete(oldEtablissementFiche.Id, session))
            .Returns(Task.CompletedTask);
        _mockEtablissementServiceUtilitaryMethods.Setup(x => x.UpdateIdsInDependantEntities(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(new ServiceResult { Success = true, StatusCode = 200, Message = "success"});
        _mockRepository.Setup(repo => repo.GetBySiret(oldEtablissementClient.Siret, session, false))
            .ReturnsAsync((EtablissementClient)null);
        
        
        // Act
        var result = await service.ChangeSiretAsync(id, newSiret, true, false, session);

        // Assert
        Assert.True(result.Success);
        _mockRepository.Verify(repo => repo.UpdateWithNewSiretAsync(It.IsAny<EtablissementClient>(), session), Times.Once);
        _mockRepository.Verify(repo => repo.Delete(id, session), Times.Once);
        _mockEtablissementFicheRepository.Verify(repo => repo.Delete(oldEtablissementFiche.Id, session), Times.Once);
        _mockRepository.Verify(repo => repo.DeleteErpCodeAsync(oldEtablissementClient.Id, "mkgt", session), Times.Once);
    }
    
    [Fact]
    public async Task ChangeSiretAsync_ShouldReturnError_WhenSiretIsInvalid()
    {
        // Arrange
        var service = CreateService();
        _mockEtablissementServiceUtilitaryMethods.Setup(s => s.EntityChecks(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(new ServiceResult { Success = false, Message = "SIRET Invalide" });
        
        // Act
        var result = await service.ChangeSiretAsync(1, "invalidSiret", true, false, new ContextSession());
        
        // Assert
        Assert.False(result.Success);
        Assert.Equal("SIRET Invalide", result.Message);
    }

    [Fact]
    public async Task ChangeSiretAsync_ShouldReturnError_WhenOldEtablissementClientNotFound()
    {
        // Arrange
        var service = CreateService();
        var validationResult = new ServiceResult
        {
            Success = true,
            StatusCode = 200,
            Message = "Success",
            Data = new SiretUpdateEntitiesListResult
            {
                EtablissementClient = null,
                EtablissementFiche = null
            }
        };
        
        _mockEtablissementServiceUtilitaryMethods.Setup(s => s.EntityChecks(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(validationResult);
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync((EtablissementClient)null);

        // Act
        var result = await service.ChangeSiretAsync(1, "newSiret", true, false, new ContextSession());

        // Assert
        Assert.False(result.Success);
        Assert.Equal("L'établissement client n'a pas été trouvé.", result.Message);
    }

    [Fact]
    public async Task ChangeSiretAsync_ShouldReturnError_WhenOldEtablissementFicheNotFound()
    {
        // Arrange
        var id = 1;
        var newSiret = "validSiret";
        var session = new ContextSession();
        var oldEtablissementClient = new EtablissementClient { Id = id, Siret = "oldSiret" };
        var validationResult = new ServiceResult
        {
            Success = true,
            StatusCode = 200,
            Message = "Success",
            Data = new SiretUpdateEntitiesListResult
            {
                EtablissementClient = oldEtablissementClient,
                EtablissementFiche = null
            }
        };
        
        _mockEtablissementServiceUtilitaryMethods.Setup(s => s.EntityChecks(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(validationResult);
        _mockRepository.Setup(repo => repo.GetById(id, session, It.IsAny<bool>()))
            .ReturnsAsync(oldEtablissementClient);
        _mockEtablissementFicheRepository.Setup(repo => repo.GetBySiret(oldEtablissementClient.Siret, session, It.IsAny<bool>()))
            .ReturnsAsync((EtablissementFiche)null);

        var service = CreateService();

        // Act
        var result = await service.ChangeSiretAsync(id, newSiret, true, false, session);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("La fiche n'a pas été trouvée.", result.Message);
    }

    [Fact]
    public async Task ChangeSiretAsync_ShouldReturnError_WhenOldEtablissementClientOrFicheNotDeleted()
    {
        var id = 1;
            var newSiret = "12345678901234";
            var session = new ContextSession();
            var oldEtablissementClient = new EtablissementClient { Id = id, Siret = "oldSiret" };
            var oldEtablissementFiche = new EtablissementFiche { Siret = "oldSiret" };
            var newEtablissementClientDto = new EtablissementClientDto { Siret = newSiret };
            var newEtablissementClient = new EtablissementClient { Siret = newSiret };
            var validationResult = new ServiceResult
            {
                Success = true,
                StatusCode = 200,
                Message = "Success",
                Data = new SiretUpdateEntitiesListResult
                {
                    EtablissementClient = oldEtablissementClient,
                    EtablissementFiche = oldEtablissementFiche
                }
            };
            
            var service = CreateService();
            
            // Mock the EntityChecks method in the service itself
            _mockEtablissementServiceUtilitaryMethods.Setup(x => x.EntityChecks(id, newSiret, session))
                .ReturnsAsync(validationResult);
            _mockRepository.Setup(repo => repo.GetById(id, session, false))
                .ReturnsAsync(oldEtablissementClient);
            _mockEtablissementFicheRepository.Setup(repo => repo.GetBySiret(It.IsAny<string>(), session, false))
                .ReturnsAsync(oldEtablissementFiche);
            _mockPappersUtilitiesService.Setup(x => x.CreateEtablissementClientBySiret(newSiret, true, false, false))
                .ReturnsAsync(newEtablissementClientDto);
            _mockTokenInfoService.Setup(x => x.GetCurrentUserName())
                .Returns("TestUser");
            _mockRepository.Setup(repo => repo.DeleteErpCodeAsync(oldEtablissementClient.Id, "mkgt", session))
                .ReturnsAsync(oldEtablissementClient);
            _mockRepository.Setup(repo => repo.UpdateWithNewSiretAsync(It.IsAny<EtablissementClient>(), session))
                .ReturnsAsync(newEtablissementClient);
            _mockRepository.Setup(repo => repo.Delete(id, session))
                .Returns(Task.CompletedTask);
            _mockEtablissementFicheRepository.Setup(repo => repo.Delete(oldEtablissementFiche.Id, session))
                .Returns(Task.CompletedTask);
            _mockEtablissementServiceUtilitaryMethods.Setup(x => x.UpdateIdsInDependantEntities(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ContextSession>()))
                .ReturnsAsync(new ServiceResult { Success = true, StatusCode = 200, Message = "success"});
            _mockRepository.Setup(repo => repo.GetBySiret(oldEtablissementClient.Siret, session, false))
                .ReturnsAsync(oldEtablissementClient);
            
            // Act
            var result = await service.ChangeSiretAsync(id, newSiret, true, false, session);

            // Assert
            Assert.False(result.Success);
        Assert.Equal("L'ancien établissement client ou la fiche n'a pas été supprimé.", result.Message);
    }

    /********** Delete **********/
    
    [Fact]
    public async Task Delete_ShouldReturnTrue_IfEntityIsDeleted()
    {
        // Arrange
        var etablissementClient = new EtablissementClient();
        etablissementClient.Client = true;
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissementClient);
        _mockRepository.Setup(repo => repo.UpdateAsync(new EtablissementClient(), It.IsAny<ContextSession>()))
            .ReturnsAsync(new EtablissementClient());
        
        var service = CreateService();
        
        // Act
        var result = await service.DeleteAsync(It.IsAny<int>(), true, false);
        
        // Assert
        Assert.True(result);
        _mockRepository.Verify(repo => repo.Delete(It.IsAny<int>(), It.IsAny<ContextSession>()), Times.Once);
    }
}


