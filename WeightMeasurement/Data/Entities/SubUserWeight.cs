using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeightMeasurement.Data.Entities
{
    public class SubUserWeight
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SubUserId { get; set; }

        [ForeignKey("SubUserId")]
        public virtual SubUser SubUser { get; set; }

        public decimal Weight { get; set; }

        public DateTime AddedOn { get; set; }

        public bool SoftDeleted { get; set; }

    }
}
