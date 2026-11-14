using ITCompanyTimeTracking.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Entities
{
    public class Project
    {
        public int Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public ProjectType ProjectType { get; private set; }

        public DateTime StartDate { get; private set; }
        protected Project() { }

        public static Project CreateEntity(int id, string name, string description,
            ProjectType projectType, DateTime startDate)
        {
            return new Project
            {
                Id = id,
                Name = name ?? string.Empty,
                Description = description ?? string.Empty,
                ProjectType = projectType,
                StartDate = startDate
            };
        }
    }
}
