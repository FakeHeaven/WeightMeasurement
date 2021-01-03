using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeightMeasurement.Models.Api
{
    public class PostSubUserModel
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

    }
}
