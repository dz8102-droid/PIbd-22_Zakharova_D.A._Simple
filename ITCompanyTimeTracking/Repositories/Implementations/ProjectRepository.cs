using ITCompanyTimeTracking.Data;
using ITCompanyTimeTracking.Entities;
using Microsoft.Extensions.Logging;
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
            _context.Projects.Add(project);
            _context.SaveChanges();
        }

        public void UpdateProject(Project project)
        {
            _logger.LogInformation($"Обновление проекта ID: {project.Id}");
            _context.Projects.Update(project);
            _context.SaveChanges();
        }

        public void DeleteProject(int id)
        {
            var p = _context.Projects.Find(id);
            if (p != null)
            {
                _context.Projects.Remove(p);
                _context.SaveChanges();
            }
        }
    }
}