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

        public IEnumerable<Employee> ReadEmployees() => _context.Employees.ToList();

        public Employee? ReadEmployeeById(int id) => _context.Employees.FirstOrDefault(e => e.Id == id);

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

                // Достаем реальную ошибку из недр базы данных
                string realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                throw new InvalidOperationException(
                    $"Не удалось сохранить сотрудника.\n\nОШИБКА БАЗЫ ДАННЫХ:\n{realError}", ex);
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            _logger.LogInformation($"Обновление сотрудника с ID: {employee.Id}");
            try
            {
                var existing = _context.Employees.Find(employee.Id);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(employee);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении сотрудника");
                throw new InvalidOperationException("Не удалось обновить сотрудника.", ex);
            }
        }

        public void DeleteEmployee(int id)
        {
            _logger.LogInformation($"Удаление сотрудника с ID: {id}");
            try
            {
                var employee = _context.Employees.Find(id);
                if (employee != null)
                {
                    _context.Employees.Remove(employee);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении сотрудника");
                throw new InvalidOperationException("Не удалось удалить сотрудника. Возможно, у него есть записи времени.", ex);
            }
        }
    }
}