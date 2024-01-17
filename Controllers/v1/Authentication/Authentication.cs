using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SlippageBackend.Controllers.v1.Authentication
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class Authentication : ControllerBase
    {
        
        [HttpGet]
        //[Authorize (AuthenticationSchemes = BearerTokenDefaults.AuthenticationScheme)]
        public async Task<IResult> GenerateToken()
        {
            // check if user has master claim
            //var claimExists = User.Claims.Any(claim => claim is { Type: "master", Value: "true" });
            //if (!claimExists)
            //{
            //    return Results.Unauthorized();
            //}
            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.Name , "xtreamly"),
                        new Claim("master", "true")
                    ],
                    BearerTokenDefaults.AuthenticationScheme  
                )
            );
            return Results.SignIn(claimsPrincipal);
        }
    }
}
