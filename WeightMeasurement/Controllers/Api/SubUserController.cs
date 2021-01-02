using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using WeightMeasurement.Data;
using WeightMeasurement.Data.Entities;
using WeightMeasurement.Extensions;
using WeightMeasurement.Filters;
using WeightMeasurement.Models;

namespace WeightMeasurement.Controllers.Api
{
    [Route("api")]
    public class SubUserController : BaseApiController
    {
        private readonly ApplicationDbContext _data;

        public SubUserController(ApplicationDbContext data, UserManager<ApplicationUser> um) : base(data, um)
        {
            _data = data;   
        }

        [HttpGet("subusers")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<HomeSubUserModel>), 200)]
        [ProducesResponseType(401)]
        public async Task<List<HomeSubUserModel>> GetAllSubUsers()
        {
            var user = await GetUser();
            var data = new List<SubUser>();
            if (user.IsAdmin)
            {
                data = _data.SubUsers.Where(m => !m.SoftDeleted).ToList();
            }
            else
            {
                data = _data.SubUsers.Where(m => m.UserId == user.UserId && !m.SoftDeleted).ToList();
            }

            return data.Select(m => new HomeSubUserModel()
            {
                Id = m.Id,
                Name = m.Name,
                Age = m.DateOfBirth.GetAge(),
                Email = user.Email, 
            }).OrderBy(m => m.Email).ThenBy(m => m.Name).ToList();
        }


    }
}
