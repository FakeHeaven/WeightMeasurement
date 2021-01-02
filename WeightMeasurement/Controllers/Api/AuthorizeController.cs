using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _data;

        public AuthorizeController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext data)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _data = data;
        }

        [HttpPost("token")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Token([FromBody] Credentials c)
        {
            Response.ContentType = "application/json";

            var user = await _userManager.FindByEmailAsync(c.Email);

            if (user == null || !user.IsActive)
            {
                return Unauthorized();
            }

            var result = await _signInManager.PasswordSignInAsync(c.Email, c.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var token = Guid.NewGuid();
            var expiration = DateTime.Now.AddMinutes(20);

            if(_data.UserTokens.Any(m => m.UserId == user.Id))
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

            return Ok(new Token
            {
                AccessToken = token,
                Expiration = expiration
            });

        }
    }
}