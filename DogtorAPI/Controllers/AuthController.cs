using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using DogtorAPI.ViewModel.Auth;
using DogtorAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace DogtorAPI.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly DogtorAPIContext _context;

        public AuthController(UserManager<IdentityUser> userManager, DogtorAPIContext context)
        {
            this.userManager = userManager;
            _context = context;
       
        }
        
        [HttpPost]
        [Route("Login")]
        //[FromBody] colocar antes do login request!
        public async Task<IActionResult> Login([FromBody]LoginRequest loginRequest)
        {
            
            IdentityUser identityUser;

            if (loginRequest == null || (identityUser = await ValidateUser(loginRequest)) == null)
            {
                return new BadRequestObjectResult(new { Message = "Login failed." });
            }

            var token = GenerateToken(identityUser);

            if (_context.Tutor == null)
            {
                return NotFound();
            }

            if (_context.Veterinario == null)
            {
                return NotFound();
            }

            var tutor = await _context.Tutor.FindAsync(Guid.Parse(identityUser.Id));
            if (tutor != null)
            {
                return Ok(new { Token = token, Message = "Success.", User_ID = identityUser.Id, Permission = "tutor" });
            }

            var veterinario = await _context.Veterinario.FindAsync(Guid.Parse(identityUser.Id));
         
            if (veterinario != null)
            {
                if (veterinario.Status == 0)
                {
                    return Unauthorized(new { Message = "Veterinario ainda não foi aceito." });
                }

                if(veterinario.Status == 2)
                {
                    return Unauthorized(new { Message = "Veterinario não teve seu cadastro aceito na plataforma." });
                }

                return Ok(new { Token = token, Message = "Success.", User_ID = identityUser.Id, Permission = "veterinario"});

            }
            var admin = await _context.Admin.FindAsync(Guid.Parse(identityUser.Id));
            
            if (admin != null)
            {
                return Ok(new { Token = token, Message = "Success.", User_ID = identityUser.Id, Permission = "Admin" });

            }

            return NotFound();

        }

        private async Task<IdentityUser> ValidateUser(LoginRequest credentials)
        {
            var identityUser = await userManager.FindByNameAsync(credentials.UserName);
            if (identityUser != null)
            {
                var result = userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);
                return result == PasswordVerificationResult.Failed ? null : identityUser;
            }

            return null;
        }

        private object GenerateToken(IdentityUser identityUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ThisIsSomeSampleSymmetricEncryptionKey");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, identityUser.UserName.ToString()),
                    new Claim(ClaimTypes.Email, identityUser.Email)
                }),

                Expires = DateTime.UtcNow.AddSeconds(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = "https://localhost",
                Issuer = "DogtorAPI"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
