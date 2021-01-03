using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using WeightMeasurement.Data;
using WeightMeasurement.Data.Entities;
using WeightMeasurement.Models;
using WeightMeasurement.Models.Api;

namespace WeightMeasurement.Controllers.Api
{
    [Route("api")]
    public class AuthorizeController : Controller
    {

        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sm;
        private readonly ApplicationDbContext _data;

        public AuthorizeController(SignInManager<ApplicationUser> sn,
            UserManager<ApplicationUser> um,
            ApplicationDbContext data)
        {
            _um = um;
            _sm = sn;
            _data = data;
        }

        [HttpPost("token")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(TokenModel), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Token([FromBody] CredentialModel c)
        {
            try
            {
                Response.ContentType = "application/json";

                var user = await _um.FindByEmailAsync(c.Email);

                if (user == null || !user.IsActive)
                {
                    return Unauthorized();
                }

                var result = await _sm.PasswordSignInAsync(c.Email, c.Password, false, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    return Unauthorized();
                }

                var token = Guid.NewGuid();
                var expiration = DateTime.UtcNow.AddMinutes(20);

                if (_data.UserTokens.Any(m => m.UserId == user.Id))
                {
                    var userToken = _data.UserTokens.Single(m => m.UserId == user.Id);
                    userToken.Expiration = expiration;
                    token = userToken.Token;
                }
                else
                {
                    _data.UserTokens.Add(new UserToken
                    {
                        UserId = user.Id,
                        Token = token,
                        Expiration = expiration
                    });
                }

                _data.SaveChanges();

                return Ok(new TokenModel
                {
                    UserId = user.Id,
                    AccessToken = token,
                    Expiration = expiration
                });
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }
    }
}