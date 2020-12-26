﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext data, UserManager<IdentityUser> um)
        {
            _logger = logger;
            _data = data;
            _um = um;

        }

        [Authorize]
        public IActionResult Index()
        {
            var vm = new HomeViewModel();

            vm.SubUsers = _data.SubUsers.Where(m => m.UserId == _um.GetUserId(User) && !m.SoftDeleted).Select(m => new HomeSubUserModel()
            {
                Id = m.Id,
                Name = m.Name,
                Age = m.DateOfBirth.GetAge()
            }).ToList(); 

            return View(vm);
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
                
                su.Id = id;
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
