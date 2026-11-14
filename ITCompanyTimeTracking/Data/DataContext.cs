using ITCompanyTimeTracking.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITCompanyTimeTracking.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<TimeRecord> TimeRecords { get; set; }
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ITCompanyTimeTrackingDb;Username=postgres;Password=123456");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связей "Один-ко-многим" (Fluent API)

            // У Сотрудника много Контрактов (если в твоей ER-диаграмме так, если нет — удали этот блок)
            // Судя по твоему коду, Контракт привязан к Проекту:
            modelBuilder.Entity<Contract>()
                .HasOne<Project>()
                .WithMany()
                .HasForeignKey(c => c.ProjectId);

            // Настройка связи "Многие-ко-многим" через таблицу ProjectEmployee
            modelBuilder.Entity<ProjectEmployee>()
                .HasKey(pe => pe.Id);

            // Связь ProjectEmployee -> Project
            modelBuilder.Entity<ProjectEmployee>()
                .HasOne<Project>()
                .WithMany()
                .HasForeignKey(pe => pe.ProjectId);

            // Связь ProjectEmployee -> Employee
            modelBuilder.Entity<ProjectEmployee>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(pe => pe.EmployeeId);
        }
    }
}