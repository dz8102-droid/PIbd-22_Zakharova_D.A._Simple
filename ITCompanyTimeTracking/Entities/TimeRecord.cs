using ITCompanyTimeTracking.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Entities
{
    public class TimeRecord
    {
        public int Id { get; private set; }

        public int EmployeeId { get; private set; }

        public int ProjectId { get; private set; }

        public DateTime WorkDate { get; private set; }
        public decimal HoursWorked { get; private set; }
        protected TimeRecord() { }

        public string Description { get; private set; } = string.Empty;

        public BonusType BonusType { get; private set; }

        public static TimeRecord CreateOperation(int id, int employeeId, int projectId,
    DateTime workDate, decimal hoursWorked, string description, BonusType bonusType)
        {
            return new TimeRecord
            {
                Id = id,
                EmployeeId = employeeId,
                ProjectId = projectId,
                WorkDate = workDate,
                HoursWorked = hoursWorked,
                Description = description ?? string.Empty,
                BonusType = bonusType
            };
        }
    }
}
