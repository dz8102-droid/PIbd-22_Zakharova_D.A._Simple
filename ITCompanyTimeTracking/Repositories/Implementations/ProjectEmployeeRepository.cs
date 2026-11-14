using ITCompanyTimeTracking.Data;
using ITCompanyTimeTracking.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITCompanyTimeTracking.Repositories.Implementations
{
    public class ProjectEmployeeRepository : IProjectEmployeeRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ProjectEmployeeRepository> _logger;

        public ProjectEmployeeRepository(DataContext context, ILogger<ProjectEmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<ProjectEmployee> ReadProjectEmployees()
        {
            _logger.LogInformation("Чтение списка связей сотрудников и проектов");
            return _context.ProjectEmployees.ToList();
        }

        public void CreateProjectEmployee(ProjectEmployee projectEmployee)
        {
            _logger.LogInformation($"Создание связи для сотрудника ID: {projectEmployee.EmployeeId}");

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.ProjectEmployees.Add(projectEmployee);
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании связи");
                transaction.Rollback();
                throw new InvalidOperationException("Не удалось сохранить связь. Убедитесь, что такой проект и сотрудник существуют.", ex);
            }
        }

        public void DeleteProjectEmployee(int id)
        {
            _logger.LogInformation($"Удаление связи с ID: {id}");
            var entity = _context.ProjectEmployees.Find(id);
            if (entity != null)
            {
                _context.ProjectEmployees.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}