using Npoi.Mapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models
{
    public class DateSplitAttRecord:IComparable<DateSplitAttRecord>
    {
        //工号 姓名  部门 考勤日期    考勤时间

       [Column("工号")]
        public virtual int Id { get; set; }
        [Column("部门")]
        public string Department { get; set; }
        [Column("姓名")]
        public string Name { get; set; }
        [Column("考勤日期", CustomFormat = "yyyy/MM/dd")]
        public DateTime Date { get; set; }
        [Column("考勤时间")]
        public string MultiTime { get; set; }
        public virtual DateTime AttendanceTime { get; set; }

        public int CompareTo(DateSplitAttRecord obj)
        {
            return this.AttendanceTime.CompareTo(obj.AttendanceTime);
        }
    }

    public class NormalAttRecord: DateSplitAttRecord
    {
        //部门	姓名	考勤号码	日期时间
        [Column("考勤号码")]
        public override int Id { get; set; }
        [Column("日期时间", CustomFormat = "yyyy/MM/dd hh.mm.ss")]
        public override DateTime AttendanceTime { get; set; }
    }
}
