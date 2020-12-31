using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeightMeasurement.Models
{
    public class AdminUserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public int SubUserCount { get; set; }

        public bool IsActive { get; set; }
    }
}
