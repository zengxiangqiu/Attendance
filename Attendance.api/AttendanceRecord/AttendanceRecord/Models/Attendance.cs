using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Models
{
    public class Attendance<T>: INotifyPropertyChanged where T : DateSplitAttRecord
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 有效打卡次数，当大于此次数，判定为出勤
        /// </summary>
        public int ValidOfAtt { get; set; }

        /// <summary>
        /// 8:00-12：00  13:30-17:00
        /// 判定迟到/早退 条件 egg：{"07:00:00","12:00:00","13:30:00","17:00:00"}
        /// </summary>
        public string[] TimesOfCheck { get; set; }


        public List<AttendanceDetail> Details { get; set; }

        /// <summary>
        /// 实际出勤天数
        /// </summary>
        private int _daysOfAtt;
        public int DaysOfAtt
        {
            get { return _daysOfAtt; }
            set {
                _daysOfAtt = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("DaysOfAtt"));
            }
        }

        /// <summary>
        /// 迟到次数
        /// </summary>
         private int _daysOfLate;
        public int DaysOfLate
        {
            get { return _daysOfLate; }
            set
            {
                _daysOfLate = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("DaysOfLate"));
            }
        }

        /// <summary>
        /// 早退次数
        /// </summary>
        private int _daysOfEarly;
        public int DaysOfEarly
        {
            get { return _daysOfEarly; }
            set
            {
                _daysOfEarly = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("DaysOfEarly"));
            }
        }

        /// <summary>
        /// 请假天数
        /// </summary>
        public int DaysOfLeave { get; set; }

        /// <summary>
        /// 出勤月份
        /// </summary>
        public int TargetMonth { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// 班次（正常、倒班）
        /// </summary>
        public StaffAttenType StaffAttenType { get; set; } = StaffAttenType.NormalWork;
    }

    /// <summary>
    /// 每日考勤
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AttendanceDetail
    {
        public DateTime Day { get; set; }
        public List<DateSplitAttRecord> Records { get; set; }
        public AttendanceType AttType { get; set; } = AttendanceType.Exception;
    }

    /// <summary>
    /// 出勤状态
    /// </summary>
    public enum AttendanceType
    {
        Normal,
        /// <summary>
        /// 异常，需人工判定
        /// </summary>
        Exception,

        /// <summary>
        /// 请假
        /// </summary>
        Leave

    }

    /// <summary>
    /// 员工班次类别
    /// </summary>
    public enum StaffAttenType
    {
        /// <summary>
        /// 正常
        /// </summary>
        NormalWork,

        /// <summary>
        /// 倒班
        /// </summary>
        ShiftWork
    }
}
