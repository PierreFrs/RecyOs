//  AuthController.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 01/02/2021
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using RecyOs.ORM;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Interfaces;

namespace RecyOs.Controllers;
    /// <summary>
    /// Controleur permettant d'interagir avec le service d'authentification
    /// </summary>
    [Route("auth")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class AuthController : BaseApiController
    {
        protected readonly IAuthenticationService authService;

        public AuthController(IAuthenticationService authService)
        {
            this.authService = authService;
        }

        //--------------------------------------------------------------------------------------
        /// <summary>
        /// Permet de vous identifier sur l'application
        /// </summary>
        /// <param name="loginDto">Données d'identification champ email et password</param>
        /// <returns >Token d'identification JWT</returns>
        [SwaggerResponse(200, "Ok", typeof(LoginResponse))]
        [SwaggerResponse(400, "Mauvaise requette")]
        [SwaggerResponse(401, "Non autorisé")]
        [SwaggerResponse(500, "Erreur webService")]
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await authService.Login(loginDto);

            if (result.Succeeded)
            {
                var token = result.Data?.Tk;
                var user = result.Data?.User; // Extract user data

                if (token != null && user != null)
                {
                    // Set token securely in cookies
                    authService.SetTokenInsideCookie(token, HttpContext);

                    // Return only the user data
                    return Ok(new
                    {
                        user = user,
                        message = "Login successful",
                        isSuccess = true
                    });
                }
            }

            if (result.IsModelValid)
            {
                return Unauthorized(new
                {
                    message = "Invalid credentials",
                    isSuccess = false
                });
            }

            return BadRequest(new
            {
                message = "Login failed",
                isSuccess = false
            });
        }


        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Permet de modifier votre mot de passe
        /// </summary>
        /// <param name="changePasswordDto">Données de nouveau mot de passe</param>
        /// <returns></returns>
        [SwaggerResponse(200, "Mot de passe modifié")]
        [SwaggerResponse(400, "Mauvaise requette")]
        [HttpPost]
        [Authorize]
        [Route("reset-pass")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var currentUserId = User.GetUserId();

            var result = await authService.ChangePassword(changePasswordDto, currentUserId);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Permet de vous enregistrer sur l'application
        /// </summary>
        /// <param name="signUpDto">Formulaire d'inscription</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "SuperAdmin")]
        [Route("sign-up")]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            var result = await authService.SignUp(signUpDto);

            if (result.Succeeded)
            {
                return Ok( result.Data );
            }

            return BadRequest();
        }

        /// <summary>
        /// Permet de demander un nouveau mot de passe
        /// </summary>
        /// <param name="requestPasswordDto"></param>
        /// <returns></returns>
        [SwaggerResponse(200, "Ok")]
        [SwaggerResponse(400, "Mauvaise requette")]
        [HttpPost]
        [AllowAnonymous]
        [Route("request-pass")]
        public async Task<IActionResult> RequestPassword(RequestPasswordDto requestPasswordDto)
        {
            var result = await authService.RequestPassword(requestPasswordDto);

            if (result.Succeeded)
                return Ok(new { result.Data, Description = "Reset Token should be sent via Email. Token in response - just for testing purpose." });

            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("restore-pass")]
        public async Task<IActionResult> RestorePassword(RestorePasswordDto restorePasswordDto)
        {
            var result = await authService.RestorePassword(restorePasswordDto);

            if (result.Succeeded)
                return Ok(new { token = result.Data });

            return BadRequest();
        }

        /// <summary>
        /// Permet de renouveller votre token d'identification
        /// </summary>
        /// <param name="refreshTokenDTO">Token de rafraichissement</param>
        /// <returns>Token d'identification JWT</returns>
        [SwaggerResponse(200, "Ok", typeof(LoginResponse))]
        [SwaggerResponse(400, "Mauvaise requette")]
        [SwaggerResponse(401, "Non autorisé")]
        [SwaggerResponse(500, "Erreur webService")]
        [HttpPost]
        [AllowAnonymous]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            // Retrieve the refreshToken from the cookies
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized(new { message = "Refresh token is missing." });
            }

            var refreshTokenDto = new RefreshTokenDto
            {
                Token = new Token
                {
                    Refresh_token = refreshToken
                }
            };

            // Pass the token string to the service
            var result = await authService.RefreshToken(refreshTokenDto);

            if (result.Succeeded)
            {
                // Set new tokens in cookies
                authService.SetTokenInsideCookie(result.Data, HttpContext);

                return Ok(new
                {
                    accessToken = result.Data.Access_token,
                    refreshToken = result.Data.Refresh_token
                });
            }

            if (result == AuthResult<Token>.UnauthorizedResult)
            {
                return Unauthorized(new { message = "Refresh token expired." });
            }

            return Unauthorized(new { message = "Invalid or expired refresh token." });
        }

        /// <summary>
        /// Permet de vous déconnecter de l'application
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse(200, "Ok")]
        [SwaggerResponse(400, "Mauvaise requette")]
        [SwaggerResponse(401, "Non autorisé")]
        [SwaggerResponse(500, "Erreur webService")]
        [HttpPost]
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                authService.RemoveTokenFromCookie(HttpContext);
                return Ok(new { message = "Déconnexion réussie" });
            }
            catch (Exception ex)
            {
                // Log the exception (optional, for debugging)
                Console.WriteLine($"Error during logout: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while logging out." });
            }
        }

        /// <summary>
        /// Permet de récupérer les informations de l'utilisateur connecté
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse(200, "Ok")]
        [SwaggerResponse(400, "Mauvaise requette")]
        [SwaggerResponse(401, "Non autorisé")]
        [SwaggerResponse(500, "Erreur webService")]
        [HttpGet]
        [Authorize]
        [Route("user-id")]
        public async Task<IActionResult> GetAuthenticatedUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            return Ok(new
            {
                UserId = userId,
            });
        }

        /// <summary>
        /// Permet de savoir si un utilisateur est connecté
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse(200, "Ok")]
        [SwaggerResponse(401, "Unauthorized - Access token needs to be refreshed.")]
        [HttpGet]
        [Route("is-authenticated")]
        [AllowAnonymous]
        public async Task<IActionResult> IsAuthenticated()
        {
            // 1. Check if the user is authenticated directly
            if (User.Identity?.IsAuthenticated == true)
            {
                return Ok(new
                {
                    isAuthenticated = true
                });
            }

            // 2. Check if a refresh token exists
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
            {
                // No refresh token -> user is not authenticated
                return Ok(new
                {
                    isAuthenticated = false
                });
            }

            // 3. Validate the refresh token
            var isValidRefreshToken = authService.IsRefreshTokenValid(refreshToken);

            if (isValidRefreshToken)
            {
                // Refresh token is valid -> trigger refresh on the frontend
                return Unauthorized(new
                {
                    message = "Access token needs to be refreshed."
                });
            }

            // 4. Refresh token is invalid or expired
            return Ok(new
            {
                isAuthenticated = false
            });
        }

    }