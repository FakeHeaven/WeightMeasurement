using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeightMeasurement.Data.Entities
{
    public class SubUser
    {
        [Key,Required,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

    }
}
