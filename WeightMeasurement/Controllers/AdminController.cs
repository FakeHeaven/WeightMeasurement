using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeightMeasurement.Data;
using WeightMeasurement.Models;
using WeightMeasurement.ViewModels;

namespace WeightMeasurement.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly ApplicationDbContext _data;
        public AdminController(UserManager<ApplicationUser> um, ApplicationDbContext data)
        {
            _um = um;
            _data = data;
        }       
        public IActionResult Index()
        {
            var vm = new AdminViewModel();

            vm.Users = _um.Users.Where(m => m.Id != _um.GetUserId(User)).Select(m => new AdminUserModel()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                SubUserCount = _data.SubUsers.Count(x => x.UserId == m.Id),
                IsActive = m.IsActive
            }).ToList();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UserStatusToggle(string userId, bool isChecked)
        {
            try
            {
                var user = await _um.FindByIdAsync(userId);
                user.IsActive = isChecked;
                await _um.UpdateAsync(user);

            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

    }
}
