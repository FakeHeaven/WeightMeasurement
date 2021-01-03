using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeightMeasurement.Models.Api
{
    public class GetWeightModel
    {
        public int Id { get; set; }

        public decimal Weight { get; set; }

        public DateTime AddedOn { get; set; }

    }
}
