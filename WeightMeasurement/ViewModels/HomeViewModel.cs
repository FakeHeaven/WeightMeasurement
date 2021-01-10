using System.Collections.Generic;
using WeightMeasurement.Models;

namespace WeightMeasurement.ViewModels
{
    public class HomeViewModel
    {
        public string UserId { get; set; }

        public List<HomeSubUserModel> SubUsers { get; set; }

    }
}
