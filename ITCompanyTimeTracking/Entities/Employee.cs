using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Entities
{
    public class Employee
    {
        public int Id { get; private set; }

        public string FirstName { get; private set; } = string.Empty;

        public string LastName { get; private set; } = string.Empty;

        public int PositionId { get; private set; }

        public DateTime HireDate { get; private set; }

        public string FullName => $"{LastName} {FirstName}";

        public static Employee CreateEntity(int id, string firstName, string lastName,
            int positionId, DateTime hireDate)
        {
            return new Employee
            {
                Id = id,
                FirstName = firstName ?? string.Empty,
                LastName = lastName ?? string.Empty,
                PositionId = positionId,
                HireDate = hireDate
            };
        }
    }
}
