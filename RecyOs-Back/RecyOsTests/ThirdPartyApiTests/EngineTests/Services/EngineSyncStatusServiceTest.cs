using AutoMapper;
using Moq;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.Odoo;
using RecyOs.Engine.Services;

using RecyOs.ORM.Entities;
using RecyOs.ORM.MapProfile;

namespace RecyOsTests.EngineTests.Services;

public class EngineSyncStatusServiceTest
{
    private readonly IMapper _mapper;

    public EngineSyncStatusServiceTest()
    {
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new EngineSyncStatusProfile());
        }).CreateMapper();
    }

    [Fact]
    public void GetByModuleName_ShouldCallAndReturnRepositoryValue()
    {
        // Arrange
        var moduleName = "testModule";
        var expectedStatus = new EngineSyncStatus()
        {
            ModuleName = "testModule",
            LastCreate = DateTime.Now,
            LastUpdate = DateTime.Now,
            SyncCre = true,
            SyncUpd = true
        };
        var repositoryMock = new Mock<IEngineSyncStatusRepository>();
        
        repositoryMock.Setup(r => r.GetByModuleName(moduleName)).Returns(expectedStatus);
        var service = new EngineSyncStatusService(repositoryMock.Object,_mapper);
        // Act
        var result = service.GetByModuleName(moduleName);
        // Assert
        repositoryMock.Verify( repo => repo.GetByModuleName(moduleName),Times.Once());
        Assert.Equal(expectedStatus.ModuleName, result.ModuleName);
        Assert.Equal(expectedStatus.LastCreate, result.LastCreate);
        Assert.Equal(expectedStatus.LastUpdate, result.LastUpdate);
        Assert.Equal(expectedStatus.SyncCre, result.SyncCre);
        Assert.Equal(expectedStatus.SyncUpd, result.SyncUpd);
    }

    [Fact]
    public void GetEnabledModules_ShouldCallAndReturnRepositoryValue()
    {
        // Arrange
        var expectedStatus = new EngineSyncStatus()
        {
            ModuleName = "testModule",
            LastCreate = DateTime.Now,
            LastUpdate = DateTime.Now,
            SyncCre = true,
            SyncUpd = true,
            ModuleEnabled = true
        };
        List<EngineSyncStatus> lst = new List<EngineSyncStatus>();
        lst.Add(expectedStatus);
        var repositoryMock = new Mock<IEngineSyncStatusRepository>();
        repositoryMock.Setup(r => r.GetEnabledModules()).Returns(lst);
        
        var service = new EngineSyncStatusService(repositoryMock.Object, _mapper);
        // Act
        var result = service.GetEnabledModules();
        // Assert
        repositoryMock.Verify(repo => repo.GetEnabledModules(), Times.Once());
        Assert.Equal(lst.Count, result.Count);
        for (int i = 0; i < lst.Count; i++)
        {
            Assert.Equal(lst[i].ModuleName, result[i].ModuleName);
            Assert.Equal(lst[i].LastCreate, result[i].LastCreate);
            Assert.Equal(lst[i].LastUpdate, result[i].LastUpdate);
            Assert.Equal(lst[i].SyncCre, result[i].SyncCre);
            Assert.Equal(lst[i].SyncUpd, result[i].SyncUpd);
            Assert.Equal(lst[i].ModuleEnabled, result[i].ModuleEnabled);
        }
    }
}