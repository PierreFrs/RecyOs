// /** ApplicationUserManager.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM;

public class ApplicationUserManager : UserManager<User>
{
    public ApplicationUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    public override async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
    {
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        var result = await UpdatePasswordHash(user, newPassword, true);
        if (!result.Succeeded)
        {
            return result;
        }

        return await UpdateUserAsync(user);
    }
}