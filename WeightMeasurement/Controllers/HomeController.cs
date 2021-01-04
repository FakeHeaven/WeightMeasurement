using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly UserManager<ApplicationUser> _um;
        private readonly IAuthorizationService _auth;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext data, UserManager<ApplicationUser> um, IAuthorizationService auth)
        {
            _logger = logger;
            _data = data;
            _um = um;
            _auth = auth;

        }

        [Authorize]
        public IActionResult Index(string userId = null)
        {
            if(userId == null)
            {
                userId = _um.GetUserId(User);
            }
            var vm = new HomeViewModel();

            vm.SubUsers = GetSubUsers(userId);

            return View(vm);
        }

        public IActionResult GraphDateRangeSelecter(int subUserId)
        {
            var vm = new GraphDateRangeSelecterViewModel();


            vm.FirstDate = _data.SubUserWeights.OrderBy(m => m.AddedOn).First(m => m.SubUserId == subUserId && !m.SoftDeleted).AddedOn;
            vm.LastDate = _data.SubUserWeights.OrderByDescending(m => m.AddedOn).First(m => m.SubUserId == subUserId && !m.SoftDeleted).AddedOn;
            vm.SubUserId = subUserId; 

            return PartialView("_GraphDateRangeSelecter", vm); 
        }

        public IActionResult RetrieveSubUserList(string userId)
        {
            return PartialView("_SubUserList", GetSubUsers(userId));
        }

        private List<HomeSubUserModel> GetSubUsers(string userId)
        {

            var data = _data.SubUsers.Where(m => m.UserId == userId && !m.SoftDeleted).ToList();

            return data.Select(m => new HomeSubUserModel()
            {
                Id = m.Id,
                Name = m.Name,
                Age = m.DateOfBirth.GetAge(),
                Email = GetUser(m.UserId).Result.Email
            }).OrderBy(m => m.Email).ThenBy(m => m.Name).ToList();
        }

        private List<HomeWeightModel> GetWeights(int subUserId)
        {

            var data = _data.SubUserWeights.Include(m => m.SubUser)
                    .Where(m => m.SubUserId == subUserId && !m.SoftDeleted && !m.SubUser.SoftDeleted)
                    .ToList();

            
            return data.Select(m => new HomeWeightModel()
            {
                Id = m.Id,
                Name = m.SubUser.Name,
                Weight = m.Weight,
                Date = m.AddedOn.ToString("d.M.yyyy"),
                SubUserId = m.SubUserId,
                Email = GetUser(m.SubUser.UserId).Result.Email
            }).OrderBy(m => m.Email).ThenBy(m => m.Name).ToList();
        }

        private async Task<ApplicationUser> GetUser(string userId)
        {
            return await _um.FindByIdAsync(userId); 
        }



        public IActionResult RetrieveWeightList(int subUserId)
        {
           return PartialView("_WeightList", GetWeights(subUserId));
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
