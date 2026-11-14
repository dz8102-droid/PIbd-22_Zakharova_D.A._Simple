using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Entities.Enums;
using ITCompanyTimeTracking.Repositories;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ITCompanyTimeTracking.Forms.TimeRecords
{
    public class FormTimeRecord : Form
    {
        private readonly ITimeRecordRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProjectRepository _projectRepository;

        private readonly ComboBox _employeeBox = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox _projectBox = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly DateTimePicker _date = new() { Format = DateTimePickerFormat.Short };
        private readonly NumericUpDown _hours = new() { DecimalPlaces = 2, Maximum = 24 };
        private readonly TextBox _description = new();
        private readonly CheckedListBox _bonuses = new() { CheckOnClick = true };

        public FormTimeRecord(ITimeRecordRepository repository, IEmployeeRepository employeeRepository, IProjectRepository projectRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

            Text = "Новая запись учёта времени";
            Width = 450;
            Height = 460;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;

            _employeeBox.DataSource = _employeeRepository.ReadEmployees().ToList();
            _employeeBox.DisplayMember = "FullName"; // Берем свойство из модели
            _employeeBox.ValueMember = "Id";

            _projectBox.DataSource = _projectRepository.ReadProjects().ToList();
            _projectBox.DisplayMember = "Name";
            _projectBox.ValueMember = "Id";

            foreach (BonusType value in Enum.GetValues(typeof(BonusType)))
                if (value != BonusType.None)
                    _bonuses.Items.Add(value);

            BuildLayout();
        }

        private void BuildLayout()
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                Padding = new Padding(15)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < 6; i++) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));

            void AddRow(string labelText, Control control, int row)
            {
                layout.Controls.Add(new Label { Text = labelText, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
                control.Dock = DockStyle.Fill;
                layout.Controls.Add(control, 1, row);
            }

            AddRow("Сотрудник:", _employeeBox, 0);
            AddRow("Проект:", _projectBox, 1);
            AddRow("Дата:", _date, 2);
            AddRow("Часов:", _hours, 3);
            AddRow("Описание:", _description, 4);
            AddRow("Бонусы:", _bonuses, 5);

            var btnOk = new Button { Text = "Сохранить", Width = 100, Height = 30 };
            var btnCancel = new Button { Text = "Отмена", Width = 100, Height = 30, DialogResult = DialogResult.Cancel };

            btnOk.Click += (_, _) =>
            {
                if (_employeeBox.SelectedValue == null || _projectBox.SelectedValue == null)
                {
                    MessageBox.Show("Выберите сотрудника и проект.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_hours.Value <= 0)
                {
                    MessageBox.Show("Количество часов должно быть больше нуля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                BonusType flags = BonusType.None;
                foreach (BonusType item in _bonuses.CheckedItems)
                    flags |= item;

                try
                {
                    _repository.CreateTimeRecord(TimeRecord.CreateOperation(
                        0,
                        (int)_employeeBox.SelectedValue,
                        (int)_projectBox.SelectedValue,
                        _date.Value,
                        _hours.Value,
                        _description.Text.Trim(),
                        flags));

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