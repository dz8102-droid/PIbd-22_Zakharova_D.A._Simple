using ITCompanyTimeTracking.Data;
using ITCompanyTimeTracking.Forms;
using ITCompanyTimeTracking.Forms.Contracts;
using ITCompanyTimeTracking.Forms.Employees;
using ITCompanyTimeTracking.Forms.Positions;
using ITCompanyTimeTracking.Forms.Projects;
using ITCompanyTimeTracking.Forms.TimeRecords;
using ITCompanyTimeTracking.Repositories;
using ITCompanyTimeTracking.Repositories.Implementations;
using Microsoft.Extensions.Logging;
using Serilog;
using Unity;
using Serilog.Sinks.File;
using Unity.Lifetime;

namespace ITCompanyTimeTracking
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Настройка Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            ApplicationConfiguration.Initialize();

            try
            {
                var container = CreateContainer();
                Application.Run(container.Resolve<FormMain>());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Приложение упало с ошибкой");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();

            // Настраиваем LoggerFactory для внедрения ILogger в репозитории
            var loggerFactory = new LoggerFactory().AddSerilog();
            container.RegisterInstance<ILoggerFactory>(loggerFactory);
            container.RegisterType(typeof(ILogger<>), typeof(Logger<>));

            // Регистрируем DbContext
            container.RegisterType<DataContext>(new TransientLifetimeManager());

            // Репозитории
            container.RegisterType<IPositionRepository, PositionRepository>(new TransientLifetimeManager());
            container.RegisterType<IEmployeeRepository, EmployeeRepository>(new TransientLifetimeManager());
            container.RegisterType<IProjectRepository, ProjectRepository>(new TransientLifetimeManager());
            container.RegisterType<ITimeRecordRepository, TimeRecordRepository>(new TransientLifetimeManager());
            container.RegisterType<IContractRepository, ContractRepository>(new TransientLifetimeManager());
            container.RegisterType<IProjectEmployeeRepository, ProjectEmployeeRepository>(new TransientLifetimeManager());

            container.RegisterInstance<IUnityContainer>(container);

            // Формы
            container.RegisterType<FormMain>();
            container.RegisterType<FormPositions>();
            container.RegisterType<FormEmployees>();
            container.RegisterType<FormProjects>();
            container.RegisterType<FormTimeRecords>();
            container.RegisterType<FormContracts>();

            return container;
        }
    }
}