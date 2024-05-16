using System.ComponentModel.DataAnnotations;

namespace server.Dto;

public class LoginDto
{
  [Required]
  public string? Username {get;set;}
  [Required]
  public string? Password {get;set;}
}