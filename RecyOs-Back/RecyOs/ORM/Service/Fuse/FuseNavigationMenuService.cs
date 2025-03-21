// /** FuseNavigationMenuService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/05/2023
//  * Fichier Modifié le : 23/05/2023
//  * Code développé pour le projet : Fuse Framework
//  */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class FuseNavigationMenuService<TFuseNavigationMenu> : BaseService, IFuseNavigationMenuService
    where TFuseNavigationMenu : FuseNavigationItem, new()
{
    protected readonly IFuseNavigationMenuRepository<TFuseNavigationMenu> _repository;
    private readonly IMapper _mapper;
    private readonly IUserRepository<User> _userRepository;

    public FuseNavigationMenuService(ICurrentContextProvider contextProvider, IFuseNavigationMenuRepository<TFuseNavigationMenu> repository, 
        IMapper mapper, IUserRepository<User> userRepository) : base(contextProvider)
    {
        _repository = repository;
        _mapper = mapper;
        _userRepository = userRepository;
    }
    
    public async Task<GridData<FuseNavigationItemDto>> GetDataForGrid(FuseNavigationMenuFilter filter, bool includeDeleted = false)
    {
        var tuple = await _repository.GetFiltredListWithCount(filter, Session, includeDeleted);
        var ratio = tuple.Item2 / (double)filter.PageSize;
        var begin = filter.PageNumber  * filter.PageSize;

        return new GridData<FuseNavigationItemDto>
        {
            Items = _mapper.Map<IEnumerable<FuseNavigationItemDto>>(tuple.Item1),
            Paginator = new Pagination()
            {
                length = tuple.Item2,
                size = filter.PageSize,
                page = filter.PageNumber,
                lastPage = (int)Math.Max(Math.Ceiling(ratio),1.0),
                startIndex = begin,
            }
        };
    }
    
    public async Task<IEnumerable<FuseNavigationItemDto>> GetMenu(bool includeDeleted = false, int menuId = 1)
    {
        var user = await _userRepository.Get(Session.UserId, Session);
        List<int> roles = new List<int>();
        foreach (var userRole in user.UserRoles)
        {
            roles.Add(userRole.RoleId);
        }
        var fuseNavigationMenu = await _repository.GetMenu(Session, roles,includeDeleted, menuId); 
        return _mapper.Map<IEnumerable<FuseNavigationItemDto>>(fuseNavigationMenu);
    }

    public async Task<FuseNavigationItemDto> GetById(int id, bool includeDeleted = false)
    {
        var fuseNavigationMenu = await _repository.Get(id, Session, includeDeleted);
        return _mapper.Map<FuseNavigationItemDto>(fuseNavigationMenu);
    }
    
    public async Task<FuseNavigationItemDto> GetByNom(string nom, bool includeDeleted = false)
    {
        var fuseNavigationMenu = await _repository.GetByName(nom, Session, includeDeleted);
        return _mapper.Map<FuseNavigationItemDto>(fuseNavigationMenu);
    }
    
    public async Task<bool> Delete(int id)
    {
        await _repository.Delete(id, Session);
        return true;
    }
    
    public async Task<FuseNavigationItemDto> Edit(FuseNavigationItemDto dto)
    {
        var fuseNavigationMenu = _mapper.Map<TFuseNavigationMenu>(dto);
        fuseNavigationMenu = await _repository.Update(fuseNavigationMenu, Session);
        return _mapper.Map<FuseNavigationItemDto>(fuseNavigationMenu);
    }
}