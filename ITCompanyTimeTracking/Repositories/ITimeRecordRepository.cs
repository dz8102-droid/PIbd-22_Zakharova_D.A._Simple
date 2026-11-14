using ITCompanyTimeTracking.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanyTimeTracking.Repositories
{
    public interface ITimeRecordRepository
    {
        IEnumerable<TimeRecord> ReadTimeRecords();
        void CreateTimeRecord(TimeRecord timeRecord);
    }
}
