//  RoleService.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2021
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class RoleService<TRole> : BaseService, IRoleService where TRole : Role, new()
{
    private readonly IRoleRepository<TRole> _roleRepository;
    private readonly IMapper _mapper;
    
    public RoleService(ICurrentContextProvider contextProvider, IRoleRepository<TRole> roleRepository, IMapper mapper) : base(contextProvider)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    public async Task<RoleDto> GetRoleById(int id)
    {
        var  role = await _roleRepository.Get(id, Session);
        return _mapper.Map<RoleDto>(role);
    }

    public async Task<List<RoleDto>> GetRoles()
    {
        var tuples = await _roleRepository.GetList(Session);
        return _mapper.Map<List<RoleDto>>(tuples);
    }
}