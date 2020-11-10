using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npoi.Mapper;
using Attendance.Models;

namespace Attendance.Readers
{
    public class DateSplitAttReader : AttendanceReader<DateSplitAttRecord>, IAttendanceReader
    {
        public override List<DateSplitAttRecord> SplitRecords(List<DateSplitAttRecord> records)
        {
            var result = new List<DateSplitAttRecord>();

            foreach (var item in records)
            {
                var multiTimes = item.MultiTime.Replace("  ", " ").Split(' ');
                for (int i = 0; i < multiTimes.Length; i++)
                {
                    var record = new DateSplitAttRecord();
                    record.Id = item.Id;
                    record.Name = item.Name;
                    record.Department = item.Department;
                    record.Date = DateTime.Parse(item.Date.ToString("yyyy/MM/dd"));
                    var rDate = DateTime.Now;
                    if (DateTime.TryParse(item.Date.ToString("yyyy/MM/dd") + " " + multiTimes[i], out rDate))
                        record.AttendanceTime = rDate;
                    else
                        throw new Exception("dateformat " + item.Date + item.Date + " " + multiTimes[i]);
                    result.Add(record);
                }
            }
            return result;
        }
    }
}
