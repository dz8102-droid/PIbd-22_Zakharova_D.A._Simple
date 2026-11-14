using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Repositories;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ITCompanyTimeTracking.Forms.Positions
{
    public class FormPosition : Form
    {
        private readonly IPositionRepository _repository;
        private readonly Position? _existing;

        private readonly TextBox _nameBox = new();
        private readonly NumericUpDown _rateBox = new() { DecimalPlaces = 2, Maximum = 1_000_000 };

        public FormPosition(IPositionRepository repository, Position? existing)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _existing = existing;

            Text = existing is null ? "Новая должность" : "Редактирование должности";
            Width = 400;
            Height = 250;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;

            BuildLayout();

            if (existing is not null)
            {
                _nameBox.Text = existing.Name;
                _rateBox.Value = existing.BaseHourlyRate;
            }
        }

        private void BuildLayout()
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(15)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Фиксируем высоту строк
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));

            layout.Controls.Add(new Label { Text = "Название:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            _nameBox.Dock = DockStyle.Fill;
            layout.Controls.Add(_nameBox, 1, 0);

            layout.Controls.Add(new Label { Text = "Ставка (руб/час):", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 1);
            _rateBox.Dock = DockStyle.Fill;
            layout.Controls.Add(_rateBox, 1, 1);

            var btnOk = new Button { Text = "Сохранить", Width = 100, Height = 30 };
            var btnCancel = new Button { Text = "Отмена", Width = 100, Height = 30, DialogResult = DialogResult.Cancel };

            btnOk.Click += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(_nameBox.Text))
                {
                    MessageBox.Show("Введите название должности.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_existing is null)
                    _repository.CreatePosition(Position.CreateEntity(0, _nameBox.Text.Trim(), _rateBox.Value));
                else
                    _repository.UpdatePosition(Position.CreateEntity(_existing.Id, _nameBox.Text.Trim(), _rateBox.Value));

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