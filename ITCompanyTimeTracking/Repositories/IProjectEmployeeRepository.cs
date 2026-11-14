using ITCompanyTimeTracking.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Repositories
{
    public interface IProjectEmployeeRepository
    {
        IEnumerable<ProjectEmployee> ReadProjectEmployees();
        void CreateProjectEmployee(ProjectEmployee projectEmployee);
        void DeleteProjectEmployee(int id);
    }
}
