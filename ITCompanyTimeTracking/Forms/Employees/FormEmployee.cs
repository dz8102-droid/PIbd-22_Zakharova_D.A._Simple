using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Repositories;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ITCompanyTimeTracking.Forms.Employees
{
    public class FormEmployee : Form
    {
        private readonly IEmployeeRepository _repository;
        private readonly Employee? _existing;

        private readonly TextBox _firstNameBox = new();
        private readonly TextBox _lastNameBox = new();
        private readonly NumericUpDown _positionIdBox = new() { Maximum = int.MaxValue };
        private readonly DateTimePicker _hireDateBox = new() { Format = DateTimePickerFormat.Short };

        public FormEmployee(IEmployeeRepository repository, Employee? existing)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _existing = existing;

            Text = existing is null ? "Новый сотрудник" : "Редактирование сотрудника";
            Width = 450;
            Height = 320;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;

            BuildLayout();

            if (existing is not null)
            {
                _firstNameBox.Text = existing.FirstName;
                _lastNameBox.Text = existing.LastName;
                _positionIdBox.Value = existing.PositionId;
                _hireDateBox.Value = existing.HireDate;
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

            AddRow("Имя:", _firstNameBox, 0);
            AddRow("Фамилия:", _lastNameBox, 1);
            AddRow("Должность (Id):", _positionIdBox, 2);
            AddRow("Дата приёма:", _hireDateBox, 3);

            var btnOk = new Button { Text = "Сохранить", Width = 100, Height = 30 };
            var btnCancel = new Button { Text = "Отмена", Width = 100, Height = 30, DialogResult = DialogResult.Cancel };

            btnOk.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(_firstNameBox.Text) || string.IsNullOrWhiteSpace(_lastNameBox.Text))
                {
                    MessageBox.Show("Заполните имя и фамилию.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_positionIdBox.Value <= 0)
                {
                    MessageBox.Show("Укажите должность.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = _existing?.Id ?? 0;
                var employee = Employee.CreateEntity(id, _firstNameBox.Text.Trim(), _lastNameBox.Text.Trim(), (int)_positionIdBox.Value, _hireDateBox.Value);

                if (_existing is null) _repository.CreateEmployee(employee);
                else _repository.UpdateEmployee(employee);

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