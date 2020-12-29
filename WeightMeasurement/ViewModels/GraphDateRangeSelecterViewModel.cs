using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeightMeasurement.ViewModels
{
    public class GraphDateRangeSelecterViewModel
    {
        public DateTime FirstDate { get; set; }

        public DateTime LastDate { get; set; }

        public int SubUserId { get; set; }
    }
}
