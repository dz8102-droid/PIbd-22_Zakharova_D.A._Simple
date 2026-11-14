using ITCompanyTimeTracking.Data;
using ITCompanyTimeTracking.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITCompanyTimeTracking.Repositories.Implementations
{
    public class PositionRepository : IPositionRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<PositionRepository> _logger;

        public PositionRepository(DataContext context, ILogger<PositionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Position> ReadPositions()
        {
            _logger.LogInformation("Чтение списка должностей");
            return _context.Positions.ToList();
        }

        public Position ReadPositionById(int id)
        {
            _logger.LogInformation($"Поиск должности с ID: {id}");
            return _context.Positions.FirstOrDefault(p => p.Id == id);
        }

        public void CreatePosition(Position position)
        {
            _logger.LogInformation($"Создание новой должности: {position.Name}");
            _context.Positions.Add(position);
            _context.SaveChanges();
        }

        public void UpdatePosition(Position position)
        {
            _logger.LogInformation($"Обновление должности с ID: {position.Id}");
            _context.Positions.Update(position);
            _context.SaveChanges();
        }

        public void DeletePosition(int id)
        {
            _logger.LogInformation($"Удаление должности с ID: {id}");
            var position = _context.Positions.Find(id);
            if (position != null)
            {
                _context.Positions.Remove(position);
                _context.SaveChanges();
            }
        }
    }
}