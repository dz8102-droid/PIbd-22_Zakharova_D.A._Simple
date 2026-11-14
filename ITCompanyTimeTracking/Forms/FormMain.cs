using ITCompanyTimeTracking.Forms.Contracts;
using ITCompanyTimeTracking.Forms.Employees;
using ITCompanyTimeTracking.Forms.Positions;
using ITCompanyTimeTracking.Forms.Projects;
using ITCompanyTimeTracking.Forms.TimeRecords;
using Unity;
using System.Windows.Forms;

namespace ITCompanyTimeTracking.Forms
{
    public partial class FormMain : Form
    {
        private readonly IUnityContainer _container;

        public FormMain(IUnityContainer container)
        {
            _container = container;
            InitializeComponent();
            Text = "IT Company Time Tracking";

            должностиToolStripMenuItem.Click += (_, _) => _container.Resolve<FormPositions>().ShowDialog();
            сотрудникиToolStripMenuItem.Click += (_, _) => _container.Resolve<FormEmployees>().ShowDialog();
            проектыToolStripMenuItem.Click += (_, _) => _container.Resolve<FormProjects>().ShowDialog();
            учётВремениToolStripMenuItem.Click += (_, _) => _container.Resolve<FormTimeRecords>().ShowDialog();
            контрактыToolStripMenuItem.Click += (_, _) => _container.Resolve<FormContracts>().ShowDialog();
        }
    }
}