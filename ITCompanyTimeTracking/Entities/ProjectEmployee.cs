using ITCompanyTimeTracking.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Entities
{
    public class ProjectEmployee
    {
        public int Id { get; private set; }

        public int ProjectId { get; private set; }

        public int EmployeeId { get; private set; }

        protected ProjectEmployee() { }

        public EmployeeRole Role { get; private set; }

        public static ProjectEmployee CreateEntity(int id, int projectId,
            int employeeId, EmployeeRole role)
        {
            return new ProjectEmployee
            {
                Id = id,
                ProjectId = projectId,
                EmployeeId = employeeId,
                Role = role
            };
        }
    }
}
