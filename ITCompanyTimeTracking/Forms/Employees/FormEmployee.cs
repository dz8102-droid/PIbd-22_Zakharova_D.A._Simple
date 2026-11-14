using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Repositories;
using System;
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
            Width = 420;
            Height = 300;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;

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

            Add("Имя:", _firstNameBox, 0);
            Add("Фамилия:", _lastNameBox, 1);
            Add("Должность (Id):", _positionIdBox, 2);
            Add("Дата приёма:", _hireDateBox, 3);

            var btnOk = new Button { Text = "Сохранить", Width = 100 };
            var btnCancel = new Button { Text = "Отмена", Width = 100, DialogResult = DialogResult.Cancel };

            btnOk.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(_firstNameBox.Text) || string.IsNullOrWhiteSpace(_lastNameBox.Text))
                {
                    MessageBox.Show("Заполните имя и фамилию.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_positionIdBox.Value <= 0)
                {
                    MessageBox.Show("Укажите должность.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = _existing?.Id ?? 0;
                var employee = Employee.CreateEntity(id, _firstNameBox.Text.Trim(),
                    _lastNameBox.Text.Trim(), (int)_positionIdBox.Value, _hireDateBox.Value);

                if (_existing is null)
                    _repository.CreateEmployee(employee);
                else
                    _repository.UpdateEmployee(employee);

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