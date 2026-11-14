using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Entities.Enums;
using ITCompanyTimeTracking.Repositories;
using System;
using System.Windows.Forms;

namespace ITCompanyTimeTracking.Forms.Projects
{
    public class FormProject : Form
    {
        private readonly IProjectRepository _repository;
        private readonly Project? _existing;

        private readonly TextBox _nameBox = new();
        private readonly TextBox _descriptionBox = new();
        private readonly ComboBox _typeBox = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly DateTimePicker _startDateBox = new() { Format = DateTimePickerFormat.Short };

        public FormProject(IProjectRepository repository, Project? existing)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _existing = existing;

            Text = existing is null ? "Новый проект" : "Редактирование проекта";
            Width = 420;
            Height = 320;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;

            _typeBox.DataSource = Enum.GetValues(typeof(ProjectType));
            BuildLayout();

            if (existing is not null)
            {
                _nameBox.Text = existing.Name;
                _descriptionBox.Text = existing.Description;
                _typeBox.SelectedItem = existing.ProjectType;
                _startDateBox.Value = existing.StartDate;
            }
        }

        private void BuildLayout()
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(10)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            void Add(string label, Control c, int row)
            {
                layout.Controls.Add(new Label { Text = label, Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft }, 0, row);
                c.Dock = DockStyle.Fill;
                layout.Controls.Add(c, 1, row);
            }

            Add("Название:", _nameBox, 0);
            Add("Описание:", _descriptionBox, 1);
            Add("Тип:", _typeBox, 2);
            Add("Дата старта:", _startDateBox, 3);

            var btnOk = new Button { Text = "Сохранить", Width = 100 };
            var btnCancel = new Button { Text = "Отмена", Width = 100, DialogResult = DialogResult.Cancel };

            btnOk.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(_nameBox.Text))
                {
                    MessageBox.Show("Введите название проекта.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_typeBox.SelectedItem is not ProjectType projectType)
                {
                    MessageBox.Show("Выберите тип проекта.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = _existing?.Id ?? 0;
                var project = Project.CreateEntity(id, _nameBox.Text.Trim(),
                    _descriptionBox.Text.Trim(), projectType, _startDateBox.Value);

                if (_existing is null)
                    _repository.CreateProject(project);
                else
                    _repository.UpdateProject(project);

                DialogResult = DialogResult.OK;
                Close();
            };

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 40, FlowDirection = FlowDirection.RightToLeft };
            buttons.Controls.Add(btnCancel);
            buttons.Controls.Add(btnOk);

            Controls.Add(layout);
            Controls.Add(buttons);
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}