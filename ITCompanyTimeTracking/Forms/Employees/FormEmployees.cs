using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Repositories;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ITCompanyTimeTracking.Forms.Employees
{
    public class FormEmployees : Form
    {
        private readonly IEmployeeRepository _repository;
        private readonly IPositionRepository _positionRepository;
        private readonly DataGridView _grid;

        public FormEmployees(IEmployeeRepository repository, IPositionRepository positionRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _positionRepository = positionRepository ?? throw new ArgumentNullException(nameof(positionRepository));

            Text = "Сотрудники";
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
            var btnAdd = new Button { Text = "Добавить", Width = 100 };
            var btnEdit = new Button { Text = "Изменить", Width = 100 };
            var btnDelete = new Button { Text = "Удалить", Width = 100 };

            btnAdd.Click += (_, _) => OpenEditor(null);
            btnEdit.Click += (_, _) => EditSelected();
            btnDelete.Click += (_, _) => DeleteSelected();

            panel.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete });
            Controls.Add(panel);

            Load += (_, _) => Reload();
        }

        private void Reload() => _grid.DataSource = _repository.ReadEmployees().ToList();

        private void EditSelected()
        {
            if (_grid.CurrentRow?.DataBoundItem is Employee employee)
                OpenEditor(employee);
        }

        private void DeleteSelected()
        {
            if (_grid.CurrentRow?.DataBoundItem is Employee employee)
            {
                try
                {
                    _repository.DeleteEmployee(employee.Id);
                    Reload();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OpenEditor(Employee? existing)
        {
            using var form = new FormEmployee(_repository, _positionRepository, existing);
            if (form.ShowDialog() == DialogResult.OK)
                Reload();
        }
    }
}