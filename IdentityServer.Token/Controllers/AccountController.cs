using IdentityModel;
using IdentityServer.Token.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Token.Controllers
{
    [Route("Account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        public AccountController(UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            , IConfiguration configuration)
        {
            this._userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("seed")]
        public async Task<IActionResult> Dataseed()
        {
            var moynul = _userManager.FindByNameAsync("moynul").Result;
            if (moynul == null)
            {
                moynul = new ApplicationUser
                {
                    UserName = "moynul",
                    Email = "moynul@email.com",
                    EmailConfirmed = true,
                };
                var result = _userManager.CreateAsync(moynul, "pass1234").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                if (!await roleManager.RoleExistsAsync(ApplicationUserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(ApplicationUserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.User));

                // Add role to user
                await _userManager.AddToRoleAsync(moynul, ApplicationUserRoles.Admin);
                await _userManager.AddToRoleAsync(moynul, ApplicationUserRoles.User);
              
            }
            else
            {
                return Ok("User : moynul already exists");
            }

            var bappy = _userManager.FindByNameAsync("bappy").Result;
            if (bappy == null)
            {
                bappy = new ApplicationUser
                {
                    UserName = "bappy",
                    Email = "bappy@email.com",
                    EmailConfirmed = true
                };
                var result = _userManager.CreateAsync(bappy, "pass1234").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                if (!await roleManager.RoleExistsAsync(ApplicationUserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(ApplicationUserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.User));

                // Add role to user
                await _userManager.AddToRoleAsync(bappy, ApplicationUserRoles.User);
            }
            else
            {
                return Ok("User : bappy  already exists");
            }

            return Ok("Default user created");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] Register registerModel)
        {

            var userExists = await _userManager.FindByNameAsync(registerModel.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerModel.Username
            };

            // Create user
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            // Checking roles in database and creating if not exists
            if (!await roleManager.RoleExistsAsync(ApplicationUserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(ApplicationUserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.User));

            // Add role to user
            if (!string.IsNullOrEmpty(registerModel.Role) && registerModel.Role == ApplicationUserRoles.Admin)
            {
                await _userManager.AddToRoleAsync(user, ApplicationUserRoles.Admin);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, ApplicationUserRoles.User);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }


    }
}
