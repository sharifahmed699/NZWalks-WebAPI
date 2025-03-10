using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;

    public AuthController(UserManager<IdentityUser> userManager)
    {
        this.userManager = userManager;
    }
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var identityUser = new IdentityUser
        {
            UserName = dto.Username,
            Email = dto.Username
        };

      var indentityResult=  await userManager.CreateAsync(identityUser,dto.Password);

        if(indentityResult.Succeeded)
        {
            if(dto.Roles.Any() && dto.Roles !=null)
            {
                indentityResult= await userManager.AddToRolesAsync(identityUser, dto.Roles);

                if (indentityResult.Succeeded)
                {
                    return Ok("User Was Register!");
                }
            }
        }
        return BadRequest("Something went wrong");
    }

    [HttpPost]
    [Route("Login")]

    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Username);

        if(user != null)
        {
            var checkPassowrdResult = await userManager.CheckPasswordAsync(user, dto.Password);
            if (checkPassowrdResult)
            {
                return Ok();
            }
        }

        return BadRequest("UserName or Password Incorrect!");
    }

}
