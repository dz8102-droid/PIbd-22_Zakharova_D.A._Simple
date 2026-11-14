using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Repositories;
using System;
using System.Windows.Forms;

namespace ITCompanyTimeTracking.Forms.Contracts
{
    public class FormContract : Form
    {
        private readonly IContractRepository _repository;

        private readonly NumericUpDown _projectId = new() { Maximum = int.MaxValue };
        private readonly TextBox _clientName = new();
        private readonly NumericUpDown _amount = new() { DecimalPlaces = 2, Maximum = 100_000_000 };
        private readonly DateTimePicker _startDate = new() { Format = DateTimePickerFormat.Short };
        private readonly DateTimePicker _endDate = new() { Format = DateTimePickerFormat.Short };

        public FormContract(IContractRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            Text = "Новый контракт";
            Width = 420;
            Height = 340;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;

            BuildLayout();
        }

        private void BuildLayout()
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
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

            Add("Проект (Id):", _projectId, 0);
            Add("Клиент:", _clientName, 1);
            Add("Сумма:", _amount, 2);
            Add("Дата начала:", _startDate, 3);
            Add("Дата окончания:", _endDate, 4);

            var btnOk = new Button { Text = "Сохранить", Width = 100 };
            var btnCancel = new Button { Text = "Отмена", Width = 100, DialogResult = DialogResult.Cancel };

            btnOk.Click += (_, _) =>
            {
                if (_projectId.Value <= 0)
                {
                    MessageBox.Show("Укажите проект.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(_clientName.Text))
                {
                    MessageBox.Show("Введите имя клиента.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_endDate.Value < _startDate.Value)
                {
                    MessageBox.Show("Дата окончания не может быть раньше даты начала.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _repository.CreateContract(Contract.CreateEntity(0, (int)_projectId.Value,
                    _clientName.Text.Trim(), _amount.Value, _startDate.Value, _endDate.Value));

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