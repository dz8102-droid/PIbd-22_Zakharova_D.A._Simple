using ITCompanyTimeTracking.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Repositories.Implementations
{
    internal class PositionRepository : IPositionRepository
    {
        public void CreatePosition(Position position) { }
        public void DeletePosition(int id) { }
        public Position ReadPositionById(int id)
            => Position.CreateEntity(0, string.Empty, 0);
        public IEnumerable<Position> ReadPositions() => [];
        public void UpdatePosition(Position position) { }
    }
}
