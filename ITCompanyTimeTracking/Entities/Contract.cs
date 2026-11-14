using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Entities
{

    public class Contract
    {
        public int Id { get; private set; }

        public int ProjectId { get; private set; }

        public string ClientName { get; private set; } = string.Empty;

        public decimal Amount { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public static Contract CreateEntity(int id, int projectId, string clientName,
            decimal amount, DateTime startDate, DateTime endDate)
        {
            return new Contract
            {
                Id = id,
                ProjectId = projectId,
                ClientName = clientName ?? string.Empty,
                Amount = amount,
                StartDate = startDate,
                EndDate = endDate
            };
        }
    }
}
