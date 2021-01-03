using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

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
