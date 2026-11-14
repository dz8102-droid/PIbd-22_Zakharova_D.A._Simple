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
            // Автогенерация Id
            modelBuilder.Entity<Position>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Employee>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Project>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Contract>().Property(c => c.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<TimeRecord>().Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProjectEmployee>().Property(pe => pe.Id).ValueGeneratedOnAdd();

            // СВЯЗЬ: Сотрудник -> Должность (Один-ко-многим)
            modelBuilder.Entity<Employee>()
                .HasOne<Position>()
                .WithMany()
                .HasForeignKey(e => e.PositionId)
                .OnDelete(DeleteBehavior.Cascade); // Если удалим должность, удалятся и её сотрудники

            // СВЯЗЬ: Контракт -> Проект
            modelBuilder.Entity<Contract>()
                .HasOne<Project>()
                .WithMany()
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // СВЯЗЬ: ProjectEmployee -> Project
            modelBuilder.Entity<ProjectEmployee>()
                .HasOne<Project>()
                .WithMany()
                .HasForeignKey(pe => pe.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // СВЯЗЬ: ProjectEmployee -> Employee
            modelBuilder.Entity<ProjectEmployee>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(pe => pe.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // СВЯЗЬ: TimeRecord -> Employee
            modelBuilder.Entity<TimeRecord>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // СВЯЗЬ: TimeRecord -> Project
            modelBuilder.Entity<TimeRecord>()
                .HasOne<Project>()
                .WithMany()
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}