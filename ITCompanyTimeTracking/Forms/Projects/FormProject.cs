using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Entities.Enums;
using ITCompanyTimeTracking.Repositories;
using System;
using System.Drawing;
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
            Width = 450;
            Height = 320;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;

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
                Padding = new Padding(15)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < 4; i++) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));

            void AddRow(string labelText, Control control, int row)
            {
                layout.Controls.Add(new Label { Text = labelText, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
                control.Dock = DockStyle.Fill;
                layout.Controls.Add(control, 1, row);
            }

            AddRow("Название:", _nameBox, 0);
            AddRow("Описание:", _descriptionBox, 1);
            AddRow("Тип:", _typeBox, 2);
            AddRow("Дата старта:", _startDateBox, 3);

            var btnOk = new Button { Text = "Сохранить", Width = 100, Height = 30 };
            var btnCancel = new Button { Text = "Отмена", Width = 100, Height = 30, DialogResult = DialogResult.Cancel };

            btnOk.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(_nameBox.Text))
                {
                    MessageBox.Show("Введите название проекта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = _existing?.Id ?? 0;
                var project = Project.CreateEntity(
                   id,
                   _nameBox.Text.Trim(),
                   _descriptionBox.Text.Trim(),
                   (ProjectType)_typeBox.SelectedItem,
                   DateTime.SpecifyKind(_startDateBox.Value, DateTimeKind.Utc)
                );
                if (_existing is null) _repository.CreateProject(project);
                else _repository.UpdateProject(project);

                DialogResult = DialogResult.OK;
                Close();
            };

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 50, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(10) };
            buttons.Controls.Add(btnCancel);
            buttons.Controls.Add(btnOk);

            Controls.Add(layout);
            Controls.Add(buttons);
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}