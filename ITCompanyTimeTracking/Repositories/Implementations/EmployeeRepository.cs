using ITCompanyTimeTracking.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Repositories.Implementations
{
    internal class EmployeeRepository : IEmployeeRepository
    {
        public void CreateEmployee(Employee employee) { }
        public void DeleteEmployee(int id) { }
        public Employee ReadEmployeeById(int id)
            => Employee.CreateEntity(0, string.Empty, string.Empty, 0, DateTime.Now);
        public IEnumerable<Employee> ReadEmployees() => [];
        public void UpdateEmployee(Employee employee) { }
    }
}
