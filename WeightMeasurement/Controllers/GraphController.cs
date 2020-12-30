using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightMeasurement.Data;
using WeightMeasurement.ViewModels;

namespace WeightMeasurement.Controllers
{
    public class GraphController : Controller
    {
        private readonly ApplicationDbContext _data;

        public GraphController(ApplicationDbContext data)
        {
            _data = data;
        }

        public IActionResult Index(int subUserId, string startDate, string endDate)
        {
            var vm = new GraphViewModel();

            vm.Name = _data.SubUsers.Single(m => m.Id == subUserId).Name;
            vm.DateRange = $"{startDate} - {endDate}";
            vm.Data = _data.SubUserWeights
                .Where(m => m.SubUserId == subUserId && !m.SoftDeleted)
                .OrderBy(m => m.AddedOn)
                .Select(m => new 
                {
                    date = m.AddedOn.ToString("d.M.yyyy"),
                    weight = m.Weight
                }).ToArray();


            return View(vm);
        }
    }
}
