using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Repositories;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ITCompanyTimeTracking.Forms.Projects
{
    public class FormProjects : Form
    {
        private readonly IProjectRepository _repository;
        private readonly DataGridView _grid;

        public FormProjects(IProjectRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            Text = "Проекты";
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

        private void Reload() => _grid.DataSource = _repository.ReadProjects().ToList();

        private void EditSelected()
        {
            if (_grid.CurrentRow?.DataBoundItem is Project project)
                OpenEditor(project);
        }

        private void DeleteSelected()
        {
            if (_grid.CurrentRow?.DataBoundItem is Project project)
            {
                _repository.DeleteProject(project.Id);
                Reload();
            }
        }

        private void OpenEditor(Project? existing)
        {
            using var form = new FormProject(_repository, existing);
            if (form.ShowDialog() == DialogResult.OK)
                Reload();
        }
    }
}