using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Entities
{
    public class Position
    {
        public int Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public decimal BaseHourlyRate { get; private set; }

        public static Position CreateEntity(int id, string name, decimal baseHourlyRate)
        {
            return new Position
            {
                Id = id,
                Name = name ?? string.Empty,
                BaseHourlyRate = baseHourlyRate
            };
        }
    }
}
