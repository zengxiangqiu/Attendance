using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npoi.Mapper;
using Attendance.Models;

namespace Attendance.Readers
{
    public class NormalAttReader : AttendanceReader<NormalAttRecord>, IAttendanceReader
    {
        public override List<DateSplitAttRecord> SplitRecords(List<NormalAttRecord> records)
        {
            return records.Select(x => (DateSplitAttRecord)x).ToList();
        }
    }
}
