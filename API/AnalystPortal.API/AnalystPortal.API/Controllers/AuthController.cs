using AnalystPortal.API.Models.Domain;
using AnalystPortal.API.Models.DTO;
using AnalystPortal.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AnalystPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly IOrganizationRepository organizationRepository;

        public AuthController(UserManager<User> userManager,
                              ITokenRepository tokenRepository,
                              IOrganizationRepository organizationRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.organizationRepository = organizationRepository;
        }

        // Post: {apiBaseUrl/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // Check Email
            var identityUser = await userManager.FindByNameAsync(request.Email);
            if (identityUser != null)
            {
                // Check Password
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);
                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    var firstName = identityUser.FirstName;
                    var lastName = identityUser.LastName;
                    var organization = await organizationRepository.GetByIdAsync(identityUser.OrganizationId);

                    // Create a Token and Response
                    var jwtToken = tokenRepository.CreateJWtToken(identityUser, roles.ToList());

                    var response = new LoginResponseDto
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = request.Email,
                        OrganizationName = organization.Name,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };

                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Email or Passoword Incorrect");

            return ValidationProblem(ModelState);
        }

        // POST: {apiBaseUrl/api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // Create IdentityUser object
            var user = new User
            {
                FirstName = request.FirstName?.Trim(),
                LastName = request.LastName?.Trim(),
                OrganizationId = request.OrganizationId,
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim(),
            };
            // Create User
            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (identityResult.Succeeded)
            {
                // Add Role to user (Reader)
                identityResult = await userManager.AddToRoleAsync(user, "Viewer");

                if (identityResult.Succeeded)
                {
                    var identityUser = await userManager.FindByNameAsync(request.Email);
                    var roles = await userManager.GetRolesAsync(identityUser);
                    var response = new RegisterResponseDto
                    {
                        FirstName = identityUser.FirstName,
                        LastName = identityUser.LastName,
                        OrganizationId = identityUser.OrganizationId,
                        Email = identityUser.Email,
                        Roles = roles.ToList()
                    };

                    return Ok(response);
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }
    }
}

