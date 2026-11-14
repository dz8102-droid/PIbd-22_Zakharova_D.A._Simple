using ITCompanyTimeTracking.Entities;
using ITCompanyTimeTracking.Repositories;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ITCompanyTimeTracking.Forms.Contracts
{
    public class FormContracts : Form
    {
        private readonly IContractRepository _repository;
        private readonly DataGridView _grid;

        public FormContracts(IContractRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            Text = "Контракты";
            Width = 800;
            Height = 450;

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = true,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            Controls.Add(_grid);

            var panel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40 };
            var btnAdd = new Button { Text = "Добавить контракт", Width = 140 };
            btnAdd.Click += (_, _) => AddRecord();
            panel.Controls.Add(btnAdd);
            Controls.Add(panel);

            Load += (_, _) => Reload();
        }

        private void Reload() => _grid.DataSource = _repository.ReadContracts().ToList();

        private void AddRecord()
        {
            using var form = new FormContract(_repository);
            if (form.ShowDialog() == DialogResult.OK)
                Reload();
        }
    }
}