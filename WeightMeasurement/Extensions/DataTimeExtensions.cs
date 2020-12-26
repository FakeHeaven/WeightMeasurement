using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeightMeasurement.Extensions
{
    public static class DataTimeExtensions
    {
        public static int GetAge(this DateTime dob) {
            var now = DateTime.Today;
            int years = now.Year - dob.Year;

            if (dob > now.AddYears(-years))
                return years--;

            return years;
        }

    }
}
