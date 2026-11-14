using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Repositories;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ITCompanyTimeTracking.Forms.TimeRecords
{
    public class FormTimeRecords : Form
    {
        private readonly ITimeRecordRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly DataGridView _grid;

        public FormTimeRecords(ITimeRecordRepository repository, IEmployeeRepository employeeRepository, IProjectRepository projectRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

            Text = "Учёт времени";
            Width = 800;
            Height = 450;

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = true,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            Controls.Add(_grid);

            var panel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40 };
            var btnAdd = new Button { Text = "Добавить запись", Width = 140 };
            btnAdd.Click += (_, _) => AddRecord();
            panel.Controls.Add(btnAdd);
            Controls.Add(panel);

            Load += (_, _) => Reload();
        }

        private void Reload() => _grid.DataSource = _repository.ReadTimeRecords().ToList();

        private void AddRecord()
        {
            using var form = new FormTimeRecord(_repository, _employeeRepository, _projectRepository);
            if (form.ShowDialog() == DialogResult.OK)
                Reload();
        }
    }
}