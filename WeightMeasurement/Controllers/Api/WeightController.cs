using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using WeightMeasurement.Data;
using WeightMeasurement.Data.Entities;
using WeightMeasurement.Filters;
using WeightMeasurement.Models.Api;
using WeightMeasurement.Services;

namespace WeightMeasurement.Controllers.Api
{
    [Route("api")]
    public class WeightController : Controller
    {
        private readonly ApplicationDbContext _data;

        public WeightController(ApplicationDbContext data)
        {
            _data = data;
        }

        [HttpGet("weights")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<GetWeightModel>), 200)]
        [ProducesResponseType(401)]
        public IActionResult GetAllWeights([FromQuery] int subUserId)
        {
            try
            {
                var data = _data.SubUserWeights.Where(m => m.SubUserId == subUserId && !m.SoftDeleted).ToList();

                return Ok(data.Select(m => new GetWeightModel()
                {
                    Id = m.Id,
                    Weight = m.Weight,
                    AddedOn = m.AddedOn,
                }).ToList());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("weights/{id}")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(GetWeightModel), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public IActionResult GetWeight([FromRoute] int id)
        {
            try
            {
                if (!_data.SubUserWeights.Any(m => m.Id == id))
                {
                    return NotFound("Not Found");
                }
                return Ok(_data.SubUserWeights.Where(m => m.Id == id).Select(m => new GetWeightModel() { 
                    Id = m.Id,
                    Weight = m.Weight,
                    AddedOn = m.AddedOn
                }).Single());

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("weights")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(401)]
        public IActionResult PostWeight([FromBody] PostWeightModel model)
        {
            try
            {
                var su = new SubUserWeight()
                {
                    SubUserId = model.SubUserId,
                    Weight = model.Weight,
                    AddedOn = DateTime.UtcNow
                };
                _data.SubUserWeights.Add(su);
                _data.SaveChanges();

                return Ok(su.Id);

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("weights/{id}")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(401)]
        public IActionResult PutWeight([FromRoute] int id, [FromBody] PutWeightModel model)
        {
            try
            {
                var su = _data.SubUserWeights.Single(m => m.Id == id);

                su.Weight = model.Weight;
                su.AddedOn = model.AddedOn;

                _data.SubUserWeights.Update(su);
                _data.SaveChanges();

                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("weights/{id}")]
        [ValidateToken]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(401)]
        public IActionResult DeleteWeight([FromRoute] int id)
        {
            try
            {
                var su = _data.SubUserWeights.Single(m => m.Id == id);

                su.SoftDeleted = true;

                _data.SubUserWeights.Update(su);
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
