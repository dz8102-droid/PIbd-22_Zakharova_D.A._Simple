using System;
using System.Collections.Generic;
using System.Text;
using ITCompanyTimeTracking.Entities;

namespace ITCompanyTimeTracking.Repositories
{
    public interface IContractRepository
    {
        IEnumerable<Contract> ReadContracts();
        void CreateContract(Contract contract);
        void DeleteContract(int id);
    }
}
