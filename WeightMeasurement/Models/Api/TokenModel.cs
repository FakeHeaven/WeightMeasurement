using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeightMeasurement.Models.Api
{
    public class TokenModel
    {
        public string UserId { get; set; }
        public Guid AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
