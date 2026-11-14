using ITCompanyTimeTracking.Data;
using ITCompanyTimeTracking.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ITCompanyTimeTracking.Repositories.Implementations
{
    public class ContractRepository : IContractRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ContractRepository> _logger;

        public ContractRepository(DataContext context, ILogger<ContractRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Contract> ReadContracts() => _context.Contracts.ToList();

        public void CreateContract(Contract contract)
        {
            _logger.LogInformation($"Создание контракта для клиента: {contract.ClientName}");
            _context.Contracts.Add(contract);
            _context.SaveChanges();
        }

        public void DeleteContract(int id)
        {
            var c = _context.Contracts.Find(id);
            if (c != null)
            {
                _context.Contracts.Remove(c);
                _context.SaveChanges();
            }
        }
    }
}