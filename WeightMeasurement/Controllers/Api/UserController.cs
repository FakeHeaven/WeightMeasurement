using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using WeightMeasurement.Data;
using WeightMeasurement.Filters;
using WeightMeasurement.Models;
using WeightMeasurement.Models.Api;
using WeightMeasurement.Services;

namespace WeightMeasurement.Controllers.Api
{
    [Route("api")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _data;
        private readonly IUserDataService _ud;
        private readonly UserManager<ApplicationUser> _um;

        public UserController(ApplicationDbContext data, IUserDataService ud, UserManager<ApplicationUser> um)
        {
            _data = data;
            _ud = ud;
            _um = um;
        }

        [HttpGet("users")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<GetUserModel>), 200)]
        [ProducesResponseType(401)]
        public IActionResult GetAllUsers()
        {
            try
            {
                var token = new Guid(Request.Headers["Authorization"][0].Substring(7));

                if (!_ud.GetUser(token).Result.IsAdmin)
                {
                    return Unauthorized();
                }

                var data = _um.Users.ToList();

                return Ok(data.Select(m => new GetUserModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    IsActive = m.IsActive
                }).ToList());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("users/{userId}")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<GetUserModel>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> ToggleUserStatus([FromRoute] string userId)
        {
            try
            {
                var token = new Guid(Request.Headers["Authorization"][0].Substring(7));

                if (!_ud.GetUser(token).Result.IsAdmin)
                {
                    return Unauthorized();
                }

                var data = await _um.FindByIdAsync(userId);

                data.IsActive = !data.IsActive;
                await _um.UpdateAsync(data);
                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("users/{userId}")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<GetUserModel>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetUser([FromRoute] string userId)
        {
            try
            {
                var data = await _um.FindByIdAsync(userId);

                return Ok(new GetUserModel()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Email = data.Email,
                    IsActive = data.IsActive
                });

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

    }
}
