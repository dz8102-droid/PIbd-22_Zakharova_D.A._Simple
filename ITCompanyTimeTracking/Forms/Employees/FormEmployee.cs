using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Repositories;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ITCompanyTimeTracking.Forms.Employees
{
    public class FormEmployee : Form
    {
        private readonly IEmployeeRepository _repository;
        private readonly IPositionRepository _positionRepository;
        private readonly Employee? _existing;

        private readonly TextBox _firstNameBox = new();
        private readonly TextBox _lastNameBox = new();
        private readonly ComboBox _positionBox = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly DateTimePicker _hireDateBox = new() { Format = DateTimePickerFormat.Short };

        public FormEmployee(IEmployeeRepository repository, IPositionRepository positionRepository, Employee? existing)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _positionRepository = positionRepository ?? throw new ArgumentNullException(nameof(positionRepository));
            _existing = existing;

            Text = existing is null ? "Новый сотрудник" : "Редактирование сотрудника";
            Width = 450;
            Height = 320;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;

            var positions = _positionRepository.ReadPositions().ToList();

            if (positions.Count == 0)
            {
                MessageBox.Show("В базе данных нет ни одной должности!\nСначала создайте должность (Справочники -> Должности).",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            _positionBox.DataSource = positions;
            _positionBox.DisplayMember = "Name";
            _positionBox.ValueMember = "Id";

            BuildLayout();

            if (existing is not null)
            {
                _firstNameBox.Text = existing.FirstName;
                _lastNameBox.Text = existing.LastName;

                // Ищем нужную должность в списке и выбираем её
                var currentPosition = positions.FirstOrDefault(p => p.Id == existing.PositionId);
                if (currentPosition != null) _positionBox.SelectedItem = currentPosition;

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
            AddRow("Должность:", _positionBox, 2);
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

                // --- ГЛАВНОЕ ИСПРАВЛЕНИЕ ЗДЕСЬ ---
                // Достаем саму должность из выбранного элемента, а не через Value
                if (_positionBox.SelectedItem is not Position selectedPosition)
                {
                    MessageBox.Show("Выберите должность из списка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int positionId = selectedPosition.Id;

                if (positionId <= 0)
                {
                    MessageBox.Show("Ошибка: выбранная должность имеет некорректный ID.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // ---------------------------------

                int id = _existing?.Id ?? 0;
                var employee = Employee.CreateEntity(
                    id,
                    _firstNameBox.Text.Trim(),
                    _lastNameBox.Text.Trim(),
                    positionId,
                    DateTime.SpecifyKind(_hireDateBox.Value, DateTimeKind.Utc) // <-- Явно указываем UTC
                );

                try
                {
                    if (_existing is null) _repository.CreateEmployee(employee);
                    else _repository.UpdateEmployee(employee);

                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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