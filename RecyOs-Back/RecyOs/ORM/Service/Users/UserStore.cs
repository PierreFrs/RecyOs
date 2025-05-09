// /** UserStore.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 24/01/2021
//  * Fichier Modifié le : 24/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;
#pragma warning disable S2436 // Types and methods should not have too many generic parameters
public class UserStore<TUser, TRole, TUserRole, TUserClaim> :
        IUserEmailStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserRoleStore<TUser>,
        IUserClaimStore<TUser>
        where TUser : User, new()
        where TRole : Role, new()
        where TUserRole : UserRole, new()
        where TUserClaim : UserClaim, new()
    {
        protected ContextSession session;

        protected readonly IIdentityUserRepository<TUser> userRepository;
        protected readonly IUserRoleRepository<TUserRole> userRoleRepository;
        protected readonly IUserClaimRepository<TUserClaim> userClaimRepository;
        protected readonly IRoleStore<TRole> roleStore;

        public UserStore(
            ICurrentContextProvider contextProvider,
            IRoleStore<TRole> roleStore,
            IIdentityUserRepository<TUser> userRepository,
            IUserRoleRepository<TUserRole> userRoleRepository,
            IUserClaimRepository<TUserClaim> userClaimRepository
            )
        {
            session = contextProvider.GetCurrentContext();

            this.userRepository = userRepository;
            this.roleStore = roleStore;
            this.userRoleRepository = userRoleRepository;
            this.userClaimRepository = userClaimRepository;
        }

        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await userRepository.Edit(user, session);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await userRepository.Delete(user.Id, session);

            return IdentityResult.Success;
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            int.TryParse(userId, out var id);

            if (id > 0)
            {
                return await userRepository.GetById(id, session, true);
            }

            return null;
        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await userRepository.GetByLogin(normalizedUserName, session, true);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {

            return await CreateAsync(user, cancellationToken);
        }

        public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await userRepository.GetByEmail(normalizedEmail, session, true);
        }

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Password != null);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
        }

        public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var roleEntity = await roleStore.FindByNameAsync(roleName, cancellationToken);
            if (roleEntity != null)
            {
                var userRole = new TUserRole() { UserId = user.Id, RoleId = roleEntity.Id };
                var newRole = await userRoleRepository.Add(userRole, session);
                user.UserRoles.Add(newRole);
            }
        }

        public async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var roleEntity = await roleStore.FindByNameAsync(roleName, cancellationToken);
            if (roleEntity != null)
            {
                var roleId = roleEntity.Id;
                var userId = user.Id;
                var userRole = await userRoleRepository.Get(userId, roleId, session);
                if (userRole != null)
                {
                    await userRoleRepository.Delete(userId, roleId, session);
                }
            }
        }

        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userId = user.Id;
            return await userRoleRepository.GetByUserId(userId, session);
        }

        public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var role = await roleStore.FindByNameAsync(roleName, cancellationToken);
            if (role != null)
            {
                var userId = user.Id;
                var roleId = role.Id;
                var userRole = await userRoleRepository.Get(userId, roleId, session);
                if (userRole != null)
                    return true;
            }

            return false;
        }

        public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var role = await roleStore.FindByNameAsync(roleName, cancellationToken);

            if (role != null)
            {
                return await userRepository.GetUsersByRole(role.Id, session, true);
            }

            return new List<TUser>();
        }

        public async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user.Claims == null || user.Claims.Count == 0)
            {
                var claims = await userClaimRepository.GetByUserId(user.Id, session);
                return claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
            }

            return user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        }

        public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userClaims = claims.Select(c => new TUserClaim { UserId = user.Id, ClaimType = c.Type, ClaimValue = c.Value });

            foreach (var userClaim in userClaims)
            {
                var newClaim = await userClaimRepository.Add(userClaim, session);
                user.Claims.Add(newClaim);
            }
        }

        public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var matchedClaims = await userClaimRepository.GetList(user.Id, claim.Value, claim.Type, session);

            foreach (var matchedClaim in matchedClaims)
            {
                matchedClaim.ClaimValue = newClaim.Value;
                matchedClaim.ClaimType = newClaim.Type;
            }

            var updatedClaims = await userClaimRepository.EditMany(matchedClaims, session);

            foreach (var userClaim in user.Claims)
            {
                foreach (var updatedClaim in updatedClaims)
                {
                    if (userClaim.ClaimType == updatedClaim.ClaimType && userClaim.ClaimValue == updatedClaim.ClaimValue)
                    {
                        userClaim.ClaimValue = updatedClaim.ClaimValue;
                        userClaim.ClaimType = updatedClaim.ClaimType;
                    }
                }
            }
        }

        public async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
           
            cancellationToken.ThrowIfCancellationRequested();

            var userClaims = claims.Select(c => new TUserClaim { UserId = user.Id, ClaimType = c.Type, ClaimValue = c.Value });

            foreach (var userClaim in userClaims)
            {
                await userClaimRepository.Delete(user.Id, userClaim.ClaimType, userClaim.ClaimValue, session);

                var uc = user.Claims.FirstOrDefault(x => x.ClaimType == userClaim.ClaimType && x.ClaimValue == userClaim.ClaimValue);
                user.Claims.Remove(uc);
            }
        }

        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await userRepository.GetUsersByClaim(claim.Type, claim.Value, session, true);
        }
    }
    #pragma warning restore S2436 // Types and methods should not have too many generic parameters