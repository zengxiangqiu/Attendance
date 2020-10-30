using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models
{
    public class AttendanceResult
    {
        //姓名 实际出勤天数  请假 迟到/早退（次数）	加班（时数）

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 实际出勤天数
        /// </summary>
        private int _daysOfAtt;
        public int DaysOfAtt
        {
            get { return _daysOfAtt; }
            set
            {
                _daysOfAtt = value;
            }
        }

        /// <summary>
        /// 请假天数
        /// </summary>
        public int DaysOfLeave { get; set; }

        /// <summary>
        /// 迟到/早退（次数）
        /// </summary>
        private int _daysOfLateOrEarly;
        public int DaysOfLateOrEarly
        {
            get { return _daysOfLateOrEarly; }
            set
            {
                _daysOfLateOrEarly = value;
            }
        }

    }
}
