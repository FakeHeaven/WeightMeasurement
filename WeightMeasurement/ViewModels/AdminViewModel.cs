using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightMeasurement.Models;

namespace WeightMeasurement.ViewModels
{
    public class AdminViewModel
    {
        public List<AdminUserModel> Users { get; set; }
    }
}
