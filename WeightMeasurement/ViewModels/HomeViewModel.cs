using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightMeasurement.Models;

namespace WeightMeasurement.ViewModels
{
    public class HomeViewModel
    {
        public List<HomeSubUserModel> SubUsers { get; set; }
        public List<HomeWeightModel> Weights { get; set; }

    }
}
