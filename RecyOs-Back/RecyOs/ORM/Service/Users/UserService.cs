// /** UserService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;
using AutoMapper;

namespace RecyOs.ORM.Service;

public class UserService<TUser> : BaseService, IUserService where TUser : User, new()
    {
        protected readonly IUserRepository<TUser> UserRepository;
        private readonly IMapper _mapper;

        public UserService(ICurrentContextProvider contextProvider, IUserRepository<TUser> userRepository, IMapper mapper) : base(contextProvider)
        {
            UserRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> Delete(int id)
        {
            await UserRepository.Delete(id, Session);
            return true;
        }

        public async Task<UserDto> Edit(UserDto dto)
        {
            var user = _mapper.Map<TUser>(dto);
            await UserRepository.Edit(user, Session);
            return _mapper.Map<UserDto>(user);
        }

 

        public async Task<UserDto> GetById(int id, bool includeDeleted = false)
        {
            var user = await UserRepository.Get(id, Session, includeDeleted);
            var result = _mapper.Map<UserDto>(user);
            if (user.UserRoles.Count > 0)
            {
                result.Roles = new List<RoleDto>();
            }
            foreach (var role in user.UserRoles)
            {
                result.Roles.Add(_mapper.Map<RoleDto>(role.Role));
            }

            return result;
        }

        public async Task<UserDto> GetByLogin(string login, bool includeDeleted = false)
        {
            var user = await UserRepository.GetByLogin(login, Session, includeDeleted);
            var result = _mapper.Map<UserDto>(user);
            if (user.UserRoles.Count > 0)
            {
                result.Roles = new List<RoleDto>();
            }
            foreach (var role in user.UserRoles)
            {
                result.Roles.Add(_mapper.Map<RoleDto>(role.Role));
            }

            return result;
        }

        public async Task<GridData<UserDto>> GetDataForGrid(UsersGridFilter filter, bool includeDeleted = false)
        {
            var tuple = await UserRepository.GetFilteredListWithTotalCount(filter, Session, includeDeleted);
            var ratio = tuple.Item2 / (double)filter.PageSize;
            var begin = filter.PageNumber  * filter.PageSize;

            return new GridData<UserDto>
            {
                Items = _mapper.Map<IEnumerable<UserDto>>(tuple.Item1),
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
        
        public async Task<UserDto> GetByEmail(string mail, bool includeDeleted = false)
        {
            var user = await UserRepository.GetByEmail(mail, Session, includeDeleted);
            var result = _mapper.Map<UserDto>(user);
            if (user == null)
            {
                return null;
            }
            
            if (user.UserRoles.Count > 0)
            {
                result.Roles = new List<RoleDto>();
            }
            foreach (var role in user.UserRoles)
            {
                result.Roles.Add(_mapper.Map<RoleDto>(role.Role));
            }

            return result;
        }
        
        public async Task<UserDto> CreateByEmail(string mail, bool includeDeleted = false)
        {
            var hasher = new PasswordHasher<User>();
            var user = new UserDto()
            {
                Email = mail,
                UserName = mail
            };
            var usr = _mapper.Map<TUser>(user);
            usr.Password = hasher.HashPassword(null, mail);
            var result = await UserRepository.Edit(usr, Session);
            return _mapper.Map<UserDto>(result);
        }
    }