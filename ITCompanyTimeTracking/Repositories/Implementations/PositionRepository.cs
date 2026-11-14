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

        public IEnumerable<Position> ReadPositions() => _context.Positions.ToList();

        public Position? ReadPositionById(int id) => _context.Positions.FirstOrDefault(p => p.Id == id);

        public void CreatePosition(Position position)
        {
            _logger.LogInformation($"Создание новой должности: {position.Name}");
            try
            {
                _context.Positions.Add(position);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании должности");
                throw new InvalidOperationException("Не удалось создать должность.", ex);
            }
        }

        public void UpdatePosition(Position position)
        {
            _logger.LogInformation($"Обновление должности с ID: {position.Id}");
            try
            {
                var existing = _context.Positions.Find(position.Id);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(position);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении должности");
                throw new InvalidOperationException("Не удалось обновить должность.", ex);
            }
        }

        public void DeletePosition(int id)
        {
            _logger.LogInformation($"Удаление должности с ID: {id}");
            try
            {
                var position = _context.Positions.Find(id);
                if (position != null)
                {
                    _context.Positions.Remove(position);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении должности");
                throw new InvalidOperationException("Не удалось удалить должность. Возможно, на нее уже назначены сотрудники.", ex);
            }
        }
    }
}