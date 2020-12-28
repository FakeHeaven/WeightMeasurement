using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeightMeasurement.Data;
using WeightMeasurement.Data.Entities;
using WeightMeasurement.Extensions;
using WeightMeasurement.Models;
using WeightMeasurement.ViewModels;

namespace WeightMeasurement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _data;
        private readonly UserManager<IdentityUser> _um;
        private readonly IAuthorizationService _auth;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext data, UserManager<IdentityUser> um, IAuthorizationService auth)
        {
            _logger = logger;
            _data = data;
            _um = um;
            _auth = auth;

        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var vm = new HomeViewModel();

            vm.SubUsers = await GetSubUsers();

            vm.Weights = await GetWeights();

            return View(vm);
        }

        public async Task<IActionResult> RetrieveSubUserList()
        {
            return PartialView("_SubUserList", await GetSubUsers());
        }

        private async Task<List<HomeSubUserModel>> GetSubUsers()
        {
            var isAdmin = (await _auth.AuthorizeAsync(User, "Admin")).Succeeded;
            var data = new List<SubUser>();
            if (isAdmin)
            {
                data = _data.SubUsers.Where(m => !m.SoftDeleted).ToList();
            }
            else
            {
                data = _data.SubUsers.Where(m => m.UserId == _um.GetUserId(User) && !m.SoftDeleted).ToList();
            }

            return data.Select(m => new HomeSubUserModel()
            {
                Id = m.Id,
                Name = m.Name,
                Age = m.DateOfBirth.GetAge()
            }).ToList();
        }

        private async Task<List<HomeWeightModel>> GetWeights()
        {
            var isAdmin = (await _auth.AuthorizeAsync(User, "Admin")).Succeeded;
            var data = new List<SubUserWeight>();
            if (isAdmin)
            {
                data = _data.SubUserWeights.Include(m => m.SubUser).Where(m => !m.SoftDeleted).ToList();
            }
            else
            {
                data = _data.SubUserWeights.Where(m => m.SubUser.UserId == _um.GetUserId(User) && !m.SoftDeleted && !m.SubUser.SoftDeleted).ToList();
            }

            return data.Select(m => new HomeWeightModel()
            {
                Id = m.Id,
                Name = $"{m.SubUser.Name} {((isAdmin) ? $"({_um.GetUserName(User)})" : "")}",
                Weight = m.Weight,
                Date = m.AddedOn.ToString("d.M.yyyy"),
                SubUserId = m.SubUserId
            }).ToList();
        }

        public async Task<IActionResult> RetrieveWeightList()
        {
           return PartialView("_WeightList", await GetWeights());
        }


        public IActionResult WeightManage(int id, int subuserid)
        {
            var su = new SubUserWeight();
            if (id != 0)
            {
                su = _data.SubUserWeights.First(m => m.Id == id);
            }
            var vm = new WeightManageViewModel();

            vm.Id = su.Id;
            vm.SubUserId = subuserid;
            vm.Weight = su.Weight;
            vm.AddedOn = su.AddedOn;

            vm.SubUserName = _data.SubUsers.Single(m => m.Id == subuserid)?.Name;

            return PartialView("_WeightManage", vm);
        }

        public IActionResult WeightUpdate(int id, int subUserId, decimal weight, DateTime date)
        {
            try
            {
                var su = new SubUserWeight();
                if (id != 0)
                {
                    su = _data.SubUserWeights.First(m => m.Id == id);
                }

                su.SubUserId = subUserId;
                su.Weight = weight;
                su.AddedOn = date;

                if (id == 0)
                {
                    _data.SubUserWeights.Add(su);
                }
                else
                    _data.SubUserWeights.Update(su);


                _data.SaveChanges();

            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        public IActionResult WeightDelete(int id)
        {
            try
            {
                var su = _data.SubUserWeights.First(m => m.Id == id);

                su.SoftDeleted = true;
                _data.SubUserWeights.Update(su);
                _data.SaveChanges();

            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        public IActionResult SubUserManage(int id)
        {
            var su = new SubUser();
            if(id != 0)
            {
                su = _data.SubUsers.First(m => m.Id == id);
            }
            var vm = new SubUserManageViewModel();

            vm.Id = su.Id;
            vm.Name = su.Name;
            vm.DateOfBirth = su.DateOfBirth;

            return PartialView("_SubUserManage", vm);
        }

        public IActionResult SubUserUpdate(int id, string name, DateTime dob)
        {
            try
            {
                var su = new SubUser();
                if (id != 0)
                {
                    su = _data.SubUsers.First(m => m.Id == id);
                }
                
                su.UserId = _um.GetUserId(User);
                su.Name = name;
                su.DateOfBirth = dob;

                if (id == 0)
                    _data.SubUsers.Add(su);
                else
                    _data.SubUsers.Update(su);

                _data.SaveChanges();

            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        public IActionResult SubUserDelete(int id)
        {
            try
            {
                var su = _data.SubUsers.First(m => m.Id == id);

                su.SoftDeleted = true;
                _data.SubUsers.Update(su);
                _data.SaveChanges();

            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
