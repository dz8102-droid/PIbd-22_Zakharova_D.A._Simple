using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Entities.Enums;
using ITCompanyTimeTracking.Repositories;
using System;
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
            Width = 420;
            Height = 460;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;

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

            Add("Сотрудник (Id):", _employeeId, 0);
            Add("Проект (Id):", _projectId, 1);
            Add("Дата:", _date, 2);
            Add("Часов:", _hours, 3);
            Add("Описание:", _description, 4);
            Add("Бонусы:", _bonuses, 5);

            var btnOk = new Button { Text = "Сохранить", Width = 100 };
            var btnCancel = new Button { Text = "Отмена", Width = 100, DialogResult = DialogResult.Cancel };

            btnOk.Click += (_, _) =>
            {
                if (_employeeId.Value <= 0 || _projectId.Value <= 0)
                {
                    MessageBox.Show("Укажите сотрудника и проект.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_hours.Value <= 0)
                {
                    MessageBox.Show("Количество часов должно быть больше нуля.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                BonusType flags = BonusType.None;
                foreach (BonusType item in _bonuses.CheckedItems)
                    flags |= item;

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