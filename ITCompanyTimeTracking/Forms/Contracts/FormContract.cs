using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Repositories;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ITCompanyTimeTracking.Forms.Contracts
{
    public class FormContract : Form
    {
        private readonly IContractRepository _repository;
        private readonly IProjectRepository _projectRepository; // <-- ДОБАВИЛИ

        private readonly ComboBox _projectBox = new() { DropDownStyle = ComboBoxStyle.DropDownList }; // Заменили на ComboBox
        private readonly TextBox _clientName = new();
        private readonly NumericUpDown _amount = new() { DecimalPlaces = 2, Maximum = 100_000_000 };
        private readonly DateTimePicker _startDate = new() { Format = DateTimePickerFormat.Short };
        private readonly DateTimePicker _endDate = new() { Format = DateTimePickerFormat.Short };

        // <-- ДОБАВИЛИ projectRepository в конструктор
        public FormContract(IContractRepository repository, IProjectRepository projectRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

            Text = "Новый контракт";
            Width = 450;
            Height = 350;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;

            // Загружаем проекты
            var projects = _projectRepository.ReadProjects().ToList();
            _projectBox.DataSource = projects;
            _projectBox.DisplayMember = "Name";
            _projectBox.ValueMember = "Id";

            BuildLayout();
        }

        private void BuildLayout()
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(15)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < 5; i++) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));

            void AddRow(string labelText, Control control, int row)
            {
                layout.Controls.Add(new Label { Text = labelText, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
                control.Dock = DockStyle.Fill;
                layout.Controls.Add(control, 1, row);
            }

            AddRow("Проект:", _projectBox, 0);
            AddRow("Клиент:", _clientName, 1);
            AddRow("Сумма:", _amount, 2);
            AddRow("Дата начала:", _startDate, 3);
            AddRow("Дата окончания:", _endDate, 4);

            var btnOk = new Button { Text = "Сохранить", Width = 100, Height = 30 };
            var btnCancel = new Button { Text = "Отмена", Width = 100, Height = 30, DialogResult = DialogResult.Cancel };

            btnOk.Click += (_, _) =>
            {
                if (_projectBox.SelectedValue == null)
                {
                    MessageBox.Show("Выберите проект из списка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(_clientName.Text))
                {
                    MessageBox.Show("Введите имя клиента.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_endDate.Value < _startDate.Value)
                {
                    MessageBox.Show("Дата окончания не может быть раньше даты начала.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    _repository.CreateContract(Contract.CreateEntity(0, (int)_projectBox.SelectedValue, _clientName.Text.Trim(), _amount.Value, _startDate.Value, _endDate.Value));
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