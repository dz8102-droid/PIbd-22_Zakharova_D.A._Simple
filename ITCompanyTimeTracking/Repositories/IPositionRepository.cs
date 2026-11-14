using ITCompanyTimeTracking.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Repositories
{
    public interface IPositionRepository
    {
        IEnumerable<Position> ReadPositions();
        Position ReadPositionById(int id);
        void CreatePosition(Position position);
        void UpdatePosition(Position position);
        void DeletePosition(int id);
    }
}
