// /** AuthenticationService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class AuthenticationService<TUser> : IAuthenticationService
        where TUser : User, new()
    {
        protected readonly UserManager<TUser> userManager;
        protected readonly JwtManager jwtManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthenticationService(
            JwtManager jwtManager,
            UserManager<TUser> userManager,
            IMapper mapper,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.jwtManager = jwtManager;
            this._mapper = mapper;
            this._configuration = configuration;
        }

        public async Task<AuthResult<LoginResponse>> Login(LoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
                return AuthResult<LoginResponse>.UnvalidatedResult;

            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user != null && user.Id > 0 && await userManager.CheckPasswordAsync(user, loginDto.Password))
                {
                    var usr = _mapper.Map<UserDto>(user);

                    if (user.UserRoles.Count > 0)
                    {
                        usr.Roles = new List<RoleDto>();
                        foreach (var role in user.UserRoles)
                        {
                            usr.Roles.Add(_mapper.Map<RoleDto>(role.Role));
                        }
                    }

                    var tk = jwtManager.GenerateToken(user);

                    return AuthResult<LoginResponse>.TokenResult(new LoginResponse
                    {
                        Tk = tk,
                        User = usr
                    });
                }

            return AuthResult<LoginResponse>.UnauthorizedResult;
        }

        public async Task<AuthResult<Token>> ChangePassword(ChangePasswordDto changePasswordDto, int currentUserId)
        {
            if (changePasswordDto == null ||
                string.IsNullOrEmpty(changePasswordDto.ConfirmPassword) ||
                string.IsNullOrEmpty(changePasswordDto.Password) ||
                changePasswordDto.Password != changePasswordDto.ConfirmPassword
            )
                return AuthResult<Token>.UnvalidatedResult;

            if (currentUserId > 0)
            {
                var user = await userManager.FindByIdAsync(currentUserId.ToString());
                var result = await userManager.ChangePasswordAsync(user, null, changePasswordDto.Password);
                if (result.Succeeded)
                    return AuthResult<Token>.SucceededResult;
            }

            return AuthResult<Token>.UnauthorizedResult;
        }

        public async Task<AuthResult<UserDto>> SignUp(SignUpDto signUpDto)
        {
            if (signUpDto == null ||
                string.IsNullOrEmpty(signUpDto.Email) ||
                string.IsNullOrEmpty(signUpDto.Password) ||
                string.IsNullOrEmpty(signUpDto.ConfirmPassword) ||
                string.IsNullOrEmpty(signUpDto.UserName) ||
                string.IsNullOrEmpty(signUpDto.FirstName) ||
                string.IsNullOrEmpty(signUpDto.LastName) ||
                signUpDto.Password != signUpDto.ConfirmPassword
            )
                return AuthResult<UserDto>.UnvalidatedResult;

            var newUser = new TUser { FirstName = signUpDto.FirstName, LastName = signUpDto.LastName, UserName = signUpDto.UserName, Email = signUpDto.Email };

            var result = await userManager.CreateAsync(newUser, signUpDto.Password);

            if (result.Succeeded && newUser.Id > 0)
            {
                await userManager.AddToRoleAsync(newUser, "User");
                return AuthResult<UserDto>.TokenResult(_mapper.Map<UserDto>(newUser));
            }

            return AuthResult<UserDto>.UnauthorizedResult;
        }

        public async Task<AuthResult<string>> RequestPassword(RequestPasswordDto requestPasswordDto)
        {
            if (requestPasswordDto == null ||
                string.IsNullOrEmpty(requestPasswordDto.Email))
                return AuthResult<string>.UnvalidatedResult;

            var user = await userManager.FindByEmailAsync(requestPasswordDto.Email);

            if (user != null && user.Id > 0)
            {
                var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                return AuthResult<string>.TokenResult(passwordResetToken);
            }

            return AuthResult<string>.UnvalidatedResult;
        }

        public async Task<AuthResult<Token>> RestorePassword(RestorePasswordDto restorePasswordDto)
        {
            if (restorePasswordDto == null ||
                string.IsNullOrEmpty(restorePasswordDto.Email) ||
                string.IsNullOrEmpty(restorePasswordDto.Token) ||
                string.IsNullOrEmpty(restorePasswordDto.NewPassword) ||
                string.IsNullOrEmpty(restorePasswordDto.ConfirmPassword) ||
                string.IsNullOrEmpty(restorePasswordDto.ConfirmPassword) ||
                restorePasswordDto.ConfirmPassword != restorePasswordDto.NewPassword
            )
                return AuthResult<Token>.UnvalidatedResult;

            var user = await userManager.FindByEmailAsync(restorePasswordDto.Email);

            if (user != null && user.Id > 0)
            {
                var result = await userManager.ResetPasswordAsync(user, restorePasswordDto.Token, restorePasswordDto.NewPassword);

                if (result.Succeeded)
                {
                    var token = jwtManager.GenerateToken(user);
                    return AuthResult<Token>.TokenResult(token);
                }
            }

            return AuthResult<Token>.UnvalidatedResult;
        }

        public async Task<AuthResult<Token>> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var refreshToken = refreshTokenDto?.Token.Refresh_token;
            if (string.IsNullOrEmpty(refreshToken))
            {
                return AuthResult<Token>.UnvalidatedResult;
            }

            try
            {
                // Validate the refresh token
                var principal = jwtManager.GetPrincipal(refreshToken, isAccessToken: false);
                var userId = principal.GetUserId();
                var user = await userManager.FindByIdAsync(userId.ToString());

                if (user != null && user.Id > 0)
                {
                    var newToken = jwtManager.GenerateToken(user);
                    return AuthResult<Token>.TokenResult(newToken);
                }
            }
            catch (Exception)
            {
                return AuthResult<Token>.UnauthorizedResult;
            }

            return AuthResult<Token>.UnauthorizedResult;
        }

        public async Task<Token> GenerateToken(int userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user != null && user.Id > 0)
            {
                return jwtManager.GenerateToken(user);
            }

            return null;
        }

        public void SetTokenInsideCookie(Token token, HttpContext httpContext)
        {
            var accessExpire = _configuration.GetValue<int>("JwtOptions:AccessExpire");
            var refreshExpire = _configuration.GetValue<int>("JwtOptions:RefreshExpire");

            httpContext.Response.Cookies.Append("accessToken", token.Access_token,
                new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddSeconds(accessExpire),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            httpContext.Response.Cookies.Append("refreshToken", token.Refresh_token,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddSeconds(refreshExpire),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
        }

        public void RemoveTokenFromCookie(HttpContext httpContext)
        {
            // Delete cookies by setting them to expired
            httpContext.Response.Cookies.Append("accessToken", string.Empty, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            httpContext.Response.Cookies.Append("refreshToken", string.Empty, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }

        public bool IsRefreshTokenValid(string refreshToken)
        {
            try
            {
                // Use JwtManager to validate the refresh token
                var principal = jwtManager.GetPrincipal(refreshToken, isAccessToken: false);

                // Ensure the principal is valid and the token is authenticated
                return principal?.Identity?.IsAuthenticated == true;
            }
            catch
            {
                // Token validation failed (invalid or expired)
                return false;
            }
        }

    }