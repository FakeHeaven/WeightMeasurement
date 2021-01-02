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
using WeightMeasurement.Services;
using model = WeightMeasurement.Models.Api;

namespace WeightMeasurement.Controllers.Api
{
    [Route("api")]
    public class SubUserController : Controller 
    {
        private readonly ApplicationDbContext _data;
        private readonly IUserDataService _ud;

        public SubUserController(ApplicationDbContext data, IUserDataService ud) 
        {
            _data = data;
            _ud = ud;
        }

        [HttpGet("subusers")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<HomeSubUserModel>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAllSubUsers()
        {
            var token = new Guid(Request.Headers["Authorization"][0].Substring(7));
            var user = await _ud.GetUser(token);
            var data = new List<SubUser>();
            if (user.IsAdmin)
            {
                data = _data.SubUsers.Where(m => !m.SoftDeleted).ToList();
            }
            else
            {
                data = _data.SubUsers.Where(m => m.UserId == user.UserId && !m.SoftDeleted).ToList();
            }

            return Ok(data.Select(m => new HomeSubUserModel()
            {
                Id = m.Id,
                Name = m.Name,
                Age = m.DateOfBirth.GetAge(),
                Email = user.Email, 
            }).OrderBy(m => m.Email).ThenBy(m => m.Name).ToList());
        }

        [HttpGet("subusers/{id}")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<HomeSubUserModel>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public IActionResult GetSubUser([FromRoute]int id)
        {
            try
            {
                if (!_data.SubUsers.Any(m => m.Id == id))
                {
                    return NotFound("Not Found");
                }
                return Ok(_data.SubUsers.Single(m => m.Id == id));

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("subusers")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(401)]
        public IActionResult PostSubUser([FromBody] model.SubUser subUser)
        {
            try
            {
                var su = new SubUser() { 
                   UserId = subUser.UserId,
                   Name = subUser.Name,
                   DateOfBirth = subUser.DateOfBirth
                };
                _data.SubUsers.Add(su);
                _data.SaveChanges();

                return Ok(su.Id);

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("subusers/{id}")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(401)]
        public IActionResult PutSubUser([FromRoute] int id,[FromBody] SubUser subUser)
        {
            try
            {
                var su = _data.SubUsers.Single(m => m.Id == id);

                su.Name = subUser.Name;
                su.DateOfBirth = subUser.DateOfBirth;

                _data.SubUsers.Update(su);
                _data.SaveChanges();

                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("subusers/{id}")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(401)]
        public IActionResult DeleteSubUser([FromRoute] int id)
        {
            try
            {
                var su = _data.SubUsers.Single(m => m.Id == id);

                su.SoftDeleted = true;

                _data.SubUsers.Update(su);
                _data.SaveChanges();

                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

    }
}
