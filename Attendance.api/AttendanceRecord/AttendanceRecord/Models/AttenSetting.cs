using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models
{
    public class AttenSetting
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 有效打卡数
        /// </summary>
        public int ValidOfAtt { get; set; }

        /// <summary>
        /// 是否倒班
        /// </summary>
        public string IsShiftWork { get; set; }
    }
}
