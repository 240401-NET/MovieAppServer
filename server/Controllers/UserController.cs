using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Dto;
using server.Models;
using server.Services;
using System;
using System.Threading.Tasks;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _userService = userService;
        }

        // Auth endpoints
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username, please try again");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Incorrect credentials provided, please try again");
            }

            var token = _tokenService.CreateToken(user);

            return Ok(new UserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = token
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
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

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                var token = _tokenService.CreateToken(user);

                return Ok(new UserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = token
                });
            }

            return StatusCode(500, result.Errors);
        }

        // UserMovie endpoints
        [HttpPost("addFavoriteMovie")]
        public async Task<IActionResult> AddMovieToUser([FromBody] FavoritedMovieDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.AddMovieToUser(dto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        /*GET USER'S FAVORITE MOVIES*/
        [HttpGet("{username}/favorites")]
        public async Task<IActionResult> GetUserMoviesAsync(string username)
        {
            try
            {
                var userMovies = await _userService.GetUserMoviesAsync(username);
                return Ok(userMovies);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("removeFavorite/")]
        public async Task<IActionResult> RemoveFavoriteMovie([FromBody] FavoritedMovieDto dto)
        {
          try {
            await _userService.RemoveMovieFromUser(dto);
            return Ok();
          }
          catch (Exception e)
          {
            return BadRequest(e.Message);
          }
        }

        

  }

}