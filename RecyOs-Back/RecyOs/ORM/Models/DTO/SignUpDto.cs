// /** SignUpDTO.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

namespace RecyOs.ORM.DTO;

public class SignUpDto : LoginDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string ConfirmPassword { get; set; }
}