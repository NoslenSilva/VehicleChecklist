using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleChecklist.Domain.Enums
{
    public enum ChecklistStatus
    {
        InProgress = 0,
        Finished   = 1,
        Approved   = 2,
        Rejected   = 3
    }
}
