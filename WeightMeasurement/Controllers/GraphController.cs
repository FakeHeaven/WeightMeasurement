using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeightMeasurement.Controllers
{
    public class GraphController : Controller
    {
        public IActionResult Index(int subUserId, string startDate, string endDate)
        {
            TempData["subUserId"] = subUserId;

            return View();
        }
    }
}
