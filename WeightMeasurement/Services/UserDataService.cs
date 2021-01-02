using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightMeasurement.Data;
using WeightMeasurement.Models;

namespace WeightMeasurement.Services
{
    public interface IUserDataService 
    {
        Task<SubUserModel> GetUser(Guid token);
    }
    public class UserDataService : IUserDataService
    {
        private readonly ApplicationDbContext _data;
        private readonly UserManager<ApplicationUser> _um;

        public UserDataService(ApplicationDbContext data, UserManager<ApplicationUser> um)
        {
            _data = data;
            _um = um;
        }

        public async Task<SubUserModel> GetUser(Guid token)
        {          
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

    }
}
