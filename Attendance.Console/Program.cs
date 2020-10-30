using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Models;
using Attendance.Readers;
using Attendance.Roles;

namespace Attendance.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IAttendanceReader reader = new NormalAttReader();
            var records = reader.GetRecords("test1.xls").OrderBy(x => x.Date).ToList();

            reader = new DateSplitAttReader();
            var multiRecords = reader.GetRecords("test.xls").OrderBy(x => x.Date).ToList();

            var allRecords = records.Concat(multiRecords).ToList();


            var staff = ShowAttendance(allRecords).Where(x => x.Name == "边文森").First();
            var test = staff.DaysOfEarly;
            var details = staff.Details;

        }

        static IEnumerable<Attendance<DateSplitAttRecord>> ShowAttendance(List<DateSplitAttRecord> records)
        {
            var Attendances = records.GroupBy(x => new { x.Name, x.Id }).Select(x => new Attendance<DateSplitAttRecord>
            {
                Id = x.Key.Id,
                Name = x.Key.Name,
                Details = x
                    .GroupBy(y => y.AddtendanceTime.ToString("yyyy/MM/dd"))
                    .Select(y => new AttendanceDetail { Day = DateTime.Parse(y.Key), Records = y.ToList() })
                    .ToList(),
                TimesOfCheck = new string[] { "07:00:00", "12:00:00", "13:30:00", "17:00:00" },
                ValidOfAtt = 4
            });
            return Attendances;
        }









    }
}
