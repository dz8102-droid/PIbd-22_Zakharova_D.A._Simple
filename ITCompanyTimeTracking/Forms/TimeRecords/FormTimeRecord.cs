using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Entities.Enums;
using ITCompanyTimeTracking.Repositories;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ITCompanyTimeTracking.Forms.TimeRecords
{
    public class FormTimeRecord : Form
    {
        private readonly ITimeRecordRepository _repository;

        private readonly NumericUpDown _employeeId = new() { Maximum = int.MaxValue };
        private readonly NumericUpDown _projectId = new() { Maximum = int.MaxValue };
        private readonly DateTimePicker _date = new() { Format = DateTimePickerFormat.Short };
        private readonly NumericUpDown _hours = new() { DecimalPlaces = 2, Maximum = 24 };
        private readonly TextBox _description = new();
        private readonly CheckedListBox _bonuses = new() { CheckOnClick = true };

        public FormTimeRecord(ITimeRecordRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            Text = "Новая запись учёта времени";
            Width = 450;
            Height = 460;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;

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

            AddRow("Сотрудник (Id):", _employeeId, 0);
            AddRow("Проект (Id):", _projectId, 1);
            AddRow("Дата:", _date, 2);
            AddRow("Часов:", _hours, 3);
            AddRow("Описание:", _description, 4);
            AddRow("Бонусы:", _bonuses, 5);

            var btnOk = new Button { Text = "Сохранить", Width = 100, Height = 30 };
            var btnCancel = new Button { Text = "Отмена", Width = 100, Height = 30, DialogResult = DialogResult.Cancel };

            btnOk.Click += (_, _) =>
            {
                if (_employeeId.Value <= 0 || _projectId.Value <= 0)
                {
                    MessageBox.Show("Укажите сотрудника и проект.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        (int)_employeeId.Value,
                        (int)_projectId.Value,
                        _date.Value,
                        _hours.Value,
                        _description.Text.Trim(),
                        flags));

                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    // Ловим нашу ошибку и показываем красивое окошко
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