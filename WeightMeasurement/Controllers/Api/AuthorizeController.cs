using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightMeasurement.Models;

namespace WeightMeasurement.Controllers.Api
{
    [Route("api")]
    public class AuthorizeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthorizeController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// This is the weight measurement app login 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !user.IsActive)
            {
                return Unauthorized();
            }

            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized();         
            }

            return Ok(new
            {
                token = "token"
            });
            
        }
    }
}
