using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightMeasurement.Data;
using WeightMeasurement.Models;

namespace WeightMeasurement.Controllers.Api
{
    public class BaseApiController : Controller
    {
        private readonly ApplicationDbContext _data;
        private readonly UserManager<ApplicationUser> _um;

        public BaseApiController(ApplicationDbContext data, UserManager<ApplicationUser> um)
        {
            _data = data;
            _um = um;
        }

        public async Task<SubUserModel> GetUser()
        {
            var token = new Guid(Request.Headers["Authorization"][0]);
            var userId = _data.UserTokens.SingleOrDefault(m => m.Token == token)?.UserId;
            var user = await _um.FindByIdAsync(userId);
            var claims = await _um.GetClaimsAsync(user);
            var isAdmin = claims.Any(m => m.Type == "AccessLevel" && m.Value == "Admin");

            return new SubUserModel()
            {
                UserId = userId,
                Email = user.Email,
                IsAdmin = isAdmin,
            };
        }

        public async Task<bool> IsAdmin(string userId)
        {
            var user = await _um.FindByIdAsync(userId);
            var claims = await _um.GetClaimsAsync(user);

            return claims.Any(m => m.Type == "AccessLevel" && m.Value == "Admin");

        }
    }
}
