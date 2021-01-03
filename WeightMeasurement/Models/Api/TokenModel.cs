using System;

namespace WeightMeasurement.Models.Api
{
    public class TokenModel
    {
        public string UserId { get; set; }
        public Guid AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
