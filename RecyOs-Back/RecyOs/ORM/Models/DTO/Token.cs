
// /** Token.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

namespace RecyOs.ORM.DTO;

public class Token
{
    public double Expires_in { get; set; }

    public string Access_token { get; set; }

    public string Refresh_token { get; set; }
}

public class LoginResponse
{
    public Token Tk { get; set; }
    
    public UserDto User { get; set; }
    
    public string Message { get; set; }

    public bool IsSuccess { get; set; }
}