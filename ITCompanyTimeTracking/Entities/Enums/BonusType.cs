using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Entities.Enums
{
    [Flags]
    public enum BonusType
    {
        None = 0,
        Overtime = 1,
        Weekend = 2,
        Holiday = 4,
        Urgent = 8,
        NightShift = 16
    }
}
