using Catel;
using ChallengeBackendDisney.Entities;
using ChallengeBackendDisney.Models.Authentication;
using Jose;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeBackendDisney.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtOptions _options;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<User> userManager,
                SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
               IConfiguration configuration,
            IOptions<JwtOptions> options)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _options = options.Value;
            this._configuration = configuration;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
                if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return BuildToken(model);
                }
                else
                {
                    return BadRequest("Username or password invalid");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }





        }

        private IActionResult BuildToken(RegisterModel model)
        {
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.UniqueName,model.Email),
                    new Claim("miValor","unValor"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Secret_Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "http://localhost:44395.com",
               audience: "http://localhost:44395.com",
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expiration
            });




        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return BuildToken(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        //[HttpPost]
        //[Route("register-admin")]
        //public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model) { }



        //        [HttpPost]
        //        [Route("login")]
        //        public async Task<IActionResult> Login([FromBody] LoginModel model) {

        //            if (!ModelState.IsValid)
        //            {
        //                return BadRequest(ErrorHelper.GetModelStateErrors(ModelState));
        //            }

        //            User Users = await ctx.Usuarios.Where(x => x.Usuario == Login.Usuario).FirstOrDefaultAsync();
        //            if (Users == null)
        //            {
        //                return NotFound(ErrorHelper.Response(404, "Usuario no encontrado."));
        //}

        //            if (HashHelper.CheckHash(Login.Clave, Users.Clave, Users.Sal))
        //            {
        //                var secretKey = config.GetValue<string>("SecretKey");
        //                var key = Encoding.ASCII.GetBytes(secretKey);

        //                var claims = new ClaimsIdentity();
        //                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, Login.Usuario));

        //                var tokenDescriptor = new SecurityTokenDescriptor
        //                {
        //                    Subject = claims,
        //                    Expires = DateTime.UtcNow.AddHours(4),
        //                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //                };

        //                var tokenHandler = new JwtSecurityTokenHandler();
        //                var createdToken = tokenHandler.CreateToken(tokenDescriptor);

        //                string bearer_token = tokenHandler.WriteToken(createdToken);
        //                return Ok(bearer_token);
        //            }
        //            else
        //            {
        //                return Forbid();
        //            }
        //        }

    }


    }
