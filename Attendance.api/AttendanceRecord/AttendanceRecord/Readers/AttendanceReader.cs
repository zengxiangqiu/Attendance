using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Models;

namespace Attendance.Readers
{
    using Npoi.Mapper;
    public abstract class AttendanceReader<T> where T: DateSplitAttRecord
    {
        public List<DateSplitAttRecord> GetRecords(string fileName)
        {
            var mapper = GetMapper(fileName);
            var records = mapper.Take<T>();
            var result = SplitRecords(records.Select(x => x.Value).ToList());
            return result;
        }

        public  Mapper GetMapper(string fileName)
        {
            var mapper = new Mapper(fileName);
            return mapper;
        }
        public abstract List<DateSplitAttRecord> SplitRecords(List<T> records);
    }
}
