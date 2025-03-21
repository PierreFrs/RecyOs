
// /** IAuthenticationService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RecyOs.ORM.DTO;

namespace RecyOs.ORM.Interfaces;

public interface IAuthenticationService
{
    Task<AuthResult<LoginResponse>> Login(LoginDto loginDto);
    Task<AuthResult<Token>> ChangePassword(ChangePasswordDto changePasswordDto, int currentUserId);
    Task<AuthResult<UserDto>> SignUp(SignUpDto signUpDto);
    Task<AuthResult<string>> RequestPassword(RequestPasswordDto requestPasswordDto);
    Task<AuthResult<Token>> RestorePassword(RestorePasswordDto restorePasswordDto);
    Task<AuthResult<Token>> RefreshToken(RefreshTokenDto refreshTokenDto);
    Task<Token> GenerateToken(int userId);
    void SetTokenInsideCookie(Token token, HttpContext httpContext);
    void RemoveTokenFromCookie(HttpContext httpContext);
    bool IsRefreshTokenValid(string refreshToken);
}