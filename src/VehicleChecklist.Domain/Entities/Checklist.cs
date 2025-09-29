using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleChecklist.Domain.Enums;

namespace VehicleChecklist.Domain.Entities
{
    public class Checklist
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public Guid StartedById { get; set; }          //Quem Iniciou o Processo
        public User StartedBy { get; set; } = null!;
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public ChecklistStatus Status { get; set; } = ChecklistStatus.InProgress;
        public bool Locked { get; set; } = true;      //Indica que o processo está bloqueado
        public List<ChecklistItem> Items { get; set; } = new();
        public Guid? ReviewedById { get; set; }       //Usuário que aprovou/rejeitou
        public User? ReviewedBy { get; set; }
    }
}
