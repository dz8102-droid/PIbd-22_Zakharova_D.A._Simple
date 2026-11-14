using ITCompanyTimeTracking.Forms;
using ITCompanyTimeTracking.Forms.Contracts;
using ITCompanyTimeTracking.Forms.Employees;
using ITCompanyTimeTracking.Forms.Positions;
using ITCompanyTimeTracking.Forms.Projects;
using ITCompanyTimeTracking.Forms.TimeRecords;
using ITCompanyTimeTracking.Repositories;
using ITCompanyTimeTracking.Repositories.Implementations;
using Unity;
using Unity.Lifetime;

namespace ITCompanyTimeTracking
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(CreateContainer().Resolve<FormMain>());
        }

        private static IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IPositionRepository, PositionRepository>(new TransientLifetimeManager());
            container.RegisterType<IEmployeeRepository, EmployeeRepository>(new TransientLifetimeManager());
            container.RegisterType<IProjectRepository, ProjectRepository>(new TransientLifetimeManager());
            container.RegisterType<ITimeRecordRepository, TimeRecordRepository>(new TransientLifetimeManager());
            container.RegisterType<IContractRepository, ContractRepository>(new TransientLifetimeManager());
            container.RegisterType<IProjectEmployeeRepository, ProjectEmployeeRepository>(new TransientLifetimeManager());

            container.RegisterInstance<IUnityContainer>(container);

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