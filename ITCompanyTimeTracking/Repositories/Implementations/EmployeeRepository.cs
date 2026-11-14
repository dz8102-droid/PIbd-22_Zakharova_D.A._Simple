using ITCompanyTimeTracking.Data;
using ITCompanyTimeTracking.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITCompanyTimeTracking.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(DataContext context, ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Employee> ReadEmployees()
        {
            _logger.LogInformation("Чтение списка сотрудников");
            return _context.Employees.ToList();
        }

        public Employee? ReadEmployeeById(int id)
        {
            return _context.Employees.FirstOrDefault(e => e.Id == id);
        }

        public void CreateEmployee(Employee employee)
        {
            _logger.LogInformation($"Добавление сотрудника: {employee.LastName}");
            try
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании сотрудника");
                throw new InvalidOperationException("Не удалось сохранить сотрудника. Проверьте, что указанная должность (Id) существует.", ex);
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            _logger.LogInformation($"Обновление сотрудника с ID: {employee.Id}");
            try
            {
                _context.Employees.Update(employee);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении сотрудника");
                throw new InvalidOperationException("Не удалось обновить сотрудника. Проверьте, что указанная должность (Id) существует.", ex);
            }
        }

        public void DeleteEmployee(int id)
        {
            _logger.LogInformation($"Удаление сотрудника с ID: {id}");
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }
    }
}