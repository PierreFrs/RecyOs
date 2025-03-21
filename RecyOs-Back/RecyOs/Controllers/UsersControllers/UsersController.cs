//  UsersController.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using RecyOs.ORM;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("users")]
    public class UsersController : BaseApiController
    {
        protected readonly IUserService userService;
        protected readonly JwtManager jwtManager;
        protected readonly IAuthenticationService authService;
   

        public UsersController(IUserService userService, JwtManager jwtManager, IAuthenticationService authService)
        {
            this.userService = userService;
            this.jwtManager = jwtManager;
            this.authService = authService;
        }

        /// <summary>
        /// Permet d'obtenir un objet Utilisateur à partir de son identifiant
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur</param>
        /// <returns>Objet Utilisateur</returns>
        [SwaggerResponse(200, "Utilisateur trouvé", typeof(UserDto))]
        [SwaggerResponse(401, "Non autorisé")]
        [SwaggerResponse(404, "Utilisateur non trouvé")]
        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Policy = "SuperAdmin")]

        public async Task<IActionResult> Get(int id)
        {
            var user = await userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
        
        /// <summary>
        /// Permet d'obtenir la liste des objets Utilisateur
        /// </summary>
        /// <param name="filter">Filtre de recherche</param>
        /// <returns>Liste des objets Utilisateur </returns>
        [SwaggerResponse(200, "Liste des objets Utilisateur", typeof(List<UserDto>))]
        [SwaggerResponse(401, "Non autorisé")]
        [HttpGet]
        [Route("")]
        [Authorize(Policy = "ReadUsers")]
        public async Task<IActionResult> getDataForGrid([FromQuery] UsersGridFilter filter)
        {
            filter = filter ?? new UsersGridFilter();
            var users = await userService.GetDataForGrid(filter);
            return Ok(users);
        }

        /// <summary>
        /// Permet d'obtenir l'utilisateur connecté
        /// </summary>
        /// <returns>Utilisateur connecté</returns>
        [SwaggerResponse(200, "Utilisateur connecté", typeof(UserDto))]
        [SwaggerResponse(401, "Non autorisé")]
        [HttpGet]
        [Route("current")]
        public async Task<IActionResult> GetCurrent()
        {
            var currentUserId = User.GetUserId();
            if (currentUserId > 0)
            {
                var user = await userService.GetById(currentUserId);
                return Ok(user);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Permet de créer un nouvel utilisateur
        /// </summary>
        /// <param name="userDto">Objet utilisateur à créer</param>
        /// <returns>Objet crée</returns>
        [SwaggerResponse(200, "Utilisateur créé", typeof(UserDto))]
        [SwaggerResponse(401, "Non autorisé")]
        [HttpPost]
        [Route("")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> Create(UserDto userDto)
        {
            if (userDto.Id != 0)
            {
                return BadRequest();
            }

            var result = await userService.Edit(userDto);
            return Ok(result);
        }

        /// <summary>
        /// Permet de modifier un utilisateur exiant à partir de son identifiant
        /// </summary>
        /// <param name="id">Identifiant utilisateur</param>
        /// <param name="userDto">Objet modifié</param>
        /// <returns>Objet Modifié</returns>
        [SwaggerResponse(200, "Utilisateur modifié", typeof(UserDto))]
        [SwaggerResponse(401, "Non autorisé")]
        [SwaggerResponse(400, "Mauvaise requette")]
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> Edit(int id, UserDto userDto)
        {
            if (id != userDto.Id)
                return BadRequest();

            var result = await userService.Edit(userDto);
            return Ok(result);
        }

        /// <summary>
        /// Permet de modifier l'utilisateur courant
        /// </summary>
        /// <param name="userDto">Objet contenant l'objet modifié</param>
        /// <returns>Retourne le nouveau token JWT</returns>
        [SwaggerResponse(200, "Utilisateur modifié", typeof(UserDto))]
        [SwaggerResponse(400, "Mauvaise requette")]
        [HttpPut]
        [Route("current")]
        public async Task<IActionResult> EditCurrent(UserDto userDto)
        {
            var currentUserId = User.GetUserId();
            if (currentUserId != userDto.Id)
            {
                return BadRequest();
            }
            await userService.Edit(userDto);

            var newToken = await authService.GenerateToken(currentUserId);

            return Ok(newToken);
        }

        /// <summary>
        /// Permet de supprimer un utilisateur par son ID
        /// </summary>
        /// <param name="id">Identifiant de l'objet à supprimer</param>
        /// <returns></returns>
        [SwaggerResponse(200, "Utilisateur supprimé")]
        [SwaggerResponse(401, "Non autorisé")]
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await userService.Delete(id);
            return Ok(result);
        }
        
        /// <summary>
        /// Permet d'obtenir un utilisateur par son email
        /// </summary>
        /// <param name="email">Email de l'utilisateur</param>
        /// <returns>Objet Utilisateur</returns>
        [SwaggerResponse(200, "Utilisateur trouvé", typeof(UserDto))]
        [SwaggerResponse(401, "Non autorisé")]
        [SwaggerResponse(404, "Utilisateur non trouvé")]
        [HttpGet]
        [Route("email/{email}")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await userService.GetByEmail(email, false);
            if (user == null) return NotFound();
            return Ok(user);
        }
        
        /// <summary>
        /// Permet de créer un nouvel utilisateur à partir de son email
        /// </summary>
        /// <param name="email">Email de l'utilisateur</param>
        /// <returns>Objet Utilisateur</returns>
        [SwaggerResponse(200, "Utilisateur créé", typeof(UserDto))] 
        [SwaggerResponse(401, "Non autorisé")]
        [HttpPost]
        [Route("email/{email}")]
        [Authorize(Policy = "SuperAdmin")]
        public async Task<IActionResult> CreateByEmail(string email)
        {
            var user = await userService.CreateByEmail(email, true);
            return Ok(user);
        }
    }