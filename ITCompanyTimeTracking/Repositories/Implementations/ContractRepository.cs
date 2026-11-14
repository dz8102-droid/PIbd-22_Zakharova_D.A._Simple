using System;
using System.Collections.Generic;
using System.Text;
using ITCompanyTimeTracking.Entities;

namespace ITCompanyTimeTracking.Repositories.Implementations
{
    internal class ContractRepository : IContractRepository
    {
        public void CreateContract(Contract contract) { }
        public void DeleteContract(int id) { }
        public IEnumerable<Contract> ReadContracts() => [];
    }
}
