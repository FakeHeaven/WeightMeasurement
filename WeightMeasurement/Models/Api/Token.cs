using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeightMeasurement.Models.Api
{
    public class Token
    {
        public Guid AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
