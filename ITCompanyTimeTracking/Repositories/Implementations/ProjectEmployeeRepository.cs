using ITCompanyTimeTracking.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Repositories.Implementations
{
    internal class ProjectEmployeeRepository : IProjectEmployeeRepository
    {
        public void CreateProjectEmployee(ProjectEmployee projectEmployee) { }
        public void DeleteProjectEmployee(int id) { }
        public IEnumerable<ProjectEmployee> ReadProjectEmployees() => [];
    }
}
