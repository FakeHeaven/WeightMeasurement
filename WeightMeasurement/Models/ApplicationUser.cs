using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeightMeasurement.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;

    }
}
