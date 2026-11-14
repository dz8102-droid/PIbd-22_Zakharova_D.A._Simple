using ITCompanyTimeTracking.Data;
using ITCompanyTimeTracking.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITCompanyTimeTracking.Repositories.Implementations
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ProjectRepository> _logger;

        public ProjectRepository(DataContext context, ILogger<ProjectRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Project> ReadProjects() => _context.Projects.ToList();

        public Project? ReadProjectById(int id) => _context.Projects.FirstOrDefault(p => p.Id == id);

        public void CreateProject(Project project)
        {
            _logger.LogInformation($"Создание проекта: {project.Name}");
            try
            {
                _context.Projects.Add(project);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании проекта");
                throw new InvalidOperationException("Не удалось создать проект.", ex);
            }
        }

        public void UpdateProject(Project project)
        {
            _logger.LogInformation($"Обновление проекта ID: {project.Id}");
            try
            {
                var existing = _context.Projects.Find(project.Id);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(project);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении проекта");
                throw new InvalidOperationException("Не удалось обновить проект.", ex);
            }
        }

        public void DeleteProject(int id)
        {
            _logger.LogInformation($"Удаление проекта ID: {id}");
            try
            {
                var p = _context.Projects.Find(id);
                if (p != null)
                {
                    _context.Projects.Remove(p);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении проекта");
                throw new InvalidOperationException("Не удалось удалить проект. Возможно, с ним связаны контракты или сотрудники.", ex);
            }
        }
    }
}