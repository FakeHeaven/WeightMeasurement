using System;

namespace WeightMeasurement.Models.Api
{
    public class GetWeightModel
    {
        public int Id { get; set; }

        public decimal Weight { get; set; }

        public DateTime AddedOn { get; set; }

    }
}
