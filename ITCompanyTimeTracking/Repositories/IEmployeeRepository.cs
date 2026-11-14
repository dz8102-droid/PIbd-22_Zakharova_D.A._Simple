using ITCompanyTimeTracking.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Repositories
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> ReadEmployees();
        Employee? ReadEmployeeById(int id);
        void CreateEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int id);
    }
}
