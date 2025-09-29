using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleChecklist.Domain.Entities
{
    public class ChecklistItem
    {
        public Guid Id { get; set; }
        public Guid ChecklistId { get; set; }
        public Checklist Checklist { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool? IsOk { get; set; }               
        public string? Observation { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
