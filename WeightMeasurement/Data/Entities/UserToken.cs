using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeightMeasurement.Data.Entities
{
    public class UserToken
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserId { get; set; }

        public Guid Token { get; set; }

        public DateTime Expiration { get; set; }

    }
}
