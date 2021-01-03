using System;

namespace WeightMeasurement.ViewModels
{
    public class WeightManageViewModel
    {
        public int Id { get; set; }

        public int SubUserId { get; set; }

        public decimal Weight { get; set; }

        public string SubUserName { get; set; }

        public DateTime AddedOn { get; set; }

    }
}
