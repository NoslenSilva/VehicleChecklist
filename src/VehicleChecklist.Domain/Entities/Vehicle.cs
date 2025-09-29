using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleChecklist.Domain.Entities
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string Plate { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string? Notes { get; set; }
    }
}
