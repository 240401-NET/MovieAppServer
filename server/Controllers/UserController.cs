using server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Services;
using server.Dto;
using Microsoft.EntityFrameworkCore;

namespace server.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  private readonly SignInManager<User> _signInManager;
  private readonly UserManager<User> _userManager;
  private readonly ITokenService _tokenService;
  private readonly IUserService _userService;

  public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _tokenService = tokenService;
  }

  /*                    Auth endpoints                    */
  [HttpPost("login")]
  public async Task<IActionResult> Login(LoginDto loginDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.Equals(loginDto.Username));

    if (user == null)
    {
      return Unauthorized("Invalid username, please try again");
    }

    var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
    if (!result.Succeeded)
    {
      return Unauthorized("Incorrect credentials provided, please try again");
    }

    return Ok(
      new UserDto
      {
        Username = user.UserName,
        Email = user.Email,
        Token = _tokenService.CreateToken(user)
      }
    );
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
  {
    try
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var user = new User
      {
        UserName = registerDto.Username,
        Email = registerDto.Email
      };

      var created = await _userManager.CreateAsync(user, registerDto.Password);
      if (created.Succeeded)
      {
        return Ok(
          new UserDto
          {
            Username = user.UserName,
            Email = user.Email,
            Token = _tokenService.CreateToken(user)
          }
        );
      }
      else { return StatusCode(500, created.Errors); }
    }
    catch (Exception e)
    {
      return StatusCode(500, e);
    }
  }

  [HttpPost("favorite")]
  public async Task<IActionResult> AddMovieToUser([FromBody] FavoritedMovieDto dto)
  {
    try {
      await _userService.AddMovieToUser(dto);
      return Ok();
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }


}