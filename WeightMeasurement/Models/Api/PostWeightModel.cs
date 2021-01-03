using System;

namespace WeightMeasurement.Models.Api
{
    public class PostWeightModel
    {

        public int SubUserId { get; set; }

        public decimal Weight { get; set; }

        public DateTime AddedOn { get; set; }

    }
}
