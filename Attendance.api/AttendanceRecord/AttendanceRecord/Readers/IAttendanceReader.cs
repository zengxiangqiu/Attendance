using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Models;

namespace Attendance.Readers
{
    public interface IAttendanceReader
    {
        List<DateSplitAttRecord> GetRecords(string fileName);
    }
}
