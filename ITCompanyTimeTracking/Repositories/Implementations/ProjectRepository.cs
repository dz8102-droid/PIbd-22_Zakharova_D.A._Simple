using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Repositories.Implementations
{
    internal class ProjectRepository : IProjectRepository
    {
        public void CreateProject(Project project) { }
        public void DeleteProject(int id) { }
        public Project ReadProjectById(int id)
            => Project.CreateEntity(0, string.Empty, string.Empty, ProjectType.None, DateTime.Now);
        public IEnumerable<Project> ReadProjects() => [];
        public void UpdateProject(Project project) { }
    }
}
