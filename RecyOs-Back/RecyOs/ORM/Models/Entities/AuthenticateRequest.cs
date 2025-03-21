using System.ComponentModel.DataAnnotations;

namespace RecyOs.ORM.Entities;

public class AuthenticateRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}