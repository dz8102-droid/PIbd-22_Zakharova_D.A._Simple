using ITCompanyTimeTracking.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Repositories.Implementations
{
    internal class TimeRecordRepository : ITimeRecordRepository
    {
        public void CreateTimeRecord(TimeRecord timeRecord) { }
        public IEnumerable<TimeRecord> ReadTimeRecords() => [];
    }
}
