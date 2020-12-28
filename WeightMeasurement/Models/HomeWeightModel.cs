using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeightMeasurement.Models
{
    public class HomeWeightModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Weight { get; set; }

        public string Date { get; set; }

        public int SubUserId { get; set; }

    }
}
