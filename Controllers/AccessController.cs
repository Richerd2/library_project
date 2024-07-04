using library77.Models;
using library77.Models.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace library77.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccessController : Controller
    {
        private readonly ILogger<AccessController> _logger;
        public AccessController(ILogger<AccessController> logger)
        {
            _logger = logger;


        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> GetToken([FromBody][Required] Models.GetTokenModel model)
        {
            var identity = GetIdentity(model.Username, model.Password);
            if (identity == null)
            {
                return Unauthorized(new { errorText = "Invalid username or password." });
            }
            
            //var identitys = GetIdentity()
            

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = "Bearer " + encodedJwt
                
            };

            return Ok(response);
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterUser([FromBody][Required] Models.RegisterUserModel model)
        {
            using (LibraryContext context = new LibraryContext())
            {
                Reader newReader = new Reader
                {
                    Fullname = model.Firstname,
                    Username = model.Username,
                    Password = model.Password,
                    LibraryCard = model.ReaderCard,
                    Lastname = model.Lastname,
                    Middlname = model.Middlename,
                    DateAdd = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    
                };
                context.Readers.Add(newReader);
                
                await context.SaveChangesAsync();
            }


            return Ok(model);
        }




        private ClaimsIdentity GetIdentity(string username, string password)
        {
            using (LibraryContext context = new LibraryContext())
            {
                var user = context.Readers
                            .Where(x => x.Username == username && x.Password == password)
                            .Select(x => x)
                            .FirstOrDefault();

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                       // new Claim(ClaimsIdentity.DefaultRoleClaimType, "role")
                    };

                    var roless = context.ReaderRoles
                            .Where(x => x.ReaderId == user.Id)
                            .Select(x => x)
                            .ToList();


                    for(int i = 0; i < roless.Count;i++)
                    {
                        if(roless[i].RolesId == (int)Enums.roles.READER)
                        {
                            claims.Add( new Claim(ClaimsIdentity.DefaultRoleClaimType, Configs.ReaderRole));

                        }

                        if (roless[i].RolesId == (int)Enums.roles.ADMIN)
                        {
                            claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, Configs.AdminRole));

                        }

                        if (roless[i].RolesId == (int)Enums.roles.TEXNSPEC)
                        {
                            claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, Configs.TechSpecRole));

                        }

                    }


                    ClaimsIdentity claimsIdentity =
                        new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                    return claimsIdentity;
                }
                // если пользователя не найдено
                return null;
            }
        }
    }
}
