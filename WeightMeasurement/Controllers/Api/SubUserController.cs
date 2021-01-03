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
using WeightMeasurement.Models.Api;
using WeightMeasurement.Services;
using model = WeightMeasurement.Models.Api;

namespace WeightMeasurement.Controllers.Api
{
    [Route("api")]
    public class SubUserController : Controller 
    {
        private readonly ApplicationDbContext _data;

        public SubUserController(ApplicationDbContext data) 
        {
            _data = data;
        }

        [HttpGet("subusers")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<GetSubUserModel>), 200)]
        [ProducesResponseType(401)]
        public IActionResult GetAllSubUsers([FromQuery] string userId)
        {
            try
            {
                var data = _data.SubUsers.Where(m => m.UserId == userId && !m.SoftDeleted).ToList();

                return Ok(data.Select(m => new GetSubUserModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    DateOfBirth = m.DateOfBirth
                }).ToList());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("subusers/{id}")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(GetSubUserModel), 200)]
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
                return Ok(_data.SubUsers.Where(m => m.Id == id).Select(m => new GetSubUserModel() {
                    Id = m.Id,
                    Name = m.Name,
                    DateOfBirth = m.DateOfBirth
                }).Single());

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
        public IActionResult PostSubUser([FromBody] PostSubUserModel subUser)
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
        public IActionResult PutSubUser([FromRoute] int id,[FromBody] PutSubUserModel model)
        {
            try
            {
                var su = _data.SubUsers.Single(m => m.Id == id);

                su.Name = model.Name;
                su.DateOfBirth = model.DateOfBirth;

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
