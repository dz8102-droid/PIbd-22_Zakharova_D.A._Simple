using ITCompanyTimeTracking.Data;
using ITCompanyTimeTracking.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ITCompanyTimeTracking.Repositories.Implementations
{
    public class TimeRecordRepository : ITimeRecordRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<TimeRecordRepository> _logger;

        public TimeRecordRepository(DataContext context, ILogger<TimeRecordRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<TimeRecord> ReadTimeRecords() => _context.TimeRecords.ToList();

        public void CreateTimeRecord(TimeRecord timeRecord)
        {
            _logger.LogInformation($"Запись времени для сотрудника ID: {timeRecord.EmployeeId}");
            _context.TimeRecords.Add(timeRecord);
            _context.SaveChanges();
        }
    }
}