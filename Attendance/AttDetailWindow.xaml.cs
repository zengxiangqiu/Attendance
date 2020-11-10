using Attendance.Models;
using Attendance.Readers;
using Attendance.Services;
using MyCalendar.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Attendance
{
    /// <summary>
    /// AttDetailWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AttDetailWindow : UserControl
    {
        public event Action<Attendance<DateSplitAttRecord>> CloseEvent;

        private Attendance<DateSplitAttRecord> _attendance;

        public AttDetailWindow()
        {
            InitializeComponent();
        }

        public void InitView(Attendance<DateSplitAttRecord> attendances, int month)
        {
            var year = DateTime.Now.Year;
            DateTime targetDate = new DateTime(year, month, 1);
            Calendar.BuildCalendar(targetDate);
            _attendance = attendances;
            getNotesFromDb(_attendance);
        }

        private void getNotesFromDb(Attendance<DateSplitAttRecord> staff)
        {
            var details = staff.Details;

            //填空白
            Calendar.Days.Where(x => x.IsTargetMonth).ToList().ForEach(x =>
            {
                x.AttendanceType = AttendanceType.Normal;
                if (!details.Any(y => y.Day == x.Date))
                {
                    AttendanceDetail detail = new AttendanceDetail
                    {
                        AttType = AttendanceType.Exception,
                        Day = x.Date,
                        Records = new List<DateSplitAttRecord>()
                    };
                    details.Add(detail);
                }
            });

            List<Days> days = details.Select(x => new Days
            {
                date = x.Day,
                AttendanceType = x.AttType,
                notes = x.Records.Aggregate("", (count, next) => count + next.AttendanceTime.ToString("HH:mm:ss") + "\r\n")
            }).ToList();

            //var month =  cboMonth.SelectedIndex + 1;
            Calendar.Days.Where(x => x.IsTargetMonth).ToList().ForEach(x => x.AttendanceType = AttendanceType.Normal);
            foreach (Days dbDay in days)
            {
                var day = Calendar.Days.Where(x => x.Date == dbDay.date).FirstOrDefault();
                if (day != null)
                {
                    day.Notes = dbDay.notes;
                    day.AttendanceType = dbDay.AttendanceType;
                }
            }
        }


        private void RefreshCalendar()
        {
            //if (cboYear.SelectedItem == null) return;
            //if (cboMonth.SelectedItem == null) return;

            //int year = (int)cboYear.SelectedItem;

            //int month = cboMonth.SelectedIndex + 1;

            //DateTime targetDate = new DateTime(year, month, 1);

            //Calendar.BuildCalendar(targetDate);

            //getNotesFromDb();
        }


        private void Calendar_DayChanged(object sender, DayChangedEventArgs e)
        {
            //DaysDBEntities ctx = new DaysDBEntities();

            //var results = (from d in ctx.Days where d.date == e.Day.Date select d);

            //if (results.Count() <= 0)
            //{
            //    Days newDay = new Days();
            //    newDay.date = e.Day.Date;
            //    newDay.notes = e.Day.Notes;

            //    ctx.Days.Add(newDay);
            //    ctx.SaveChanges();
            //}
            //else
            //{
            //    Days oldDay = results.FirstOrDefault();
            //    oldDay.notes = e.Day.Notes;
            //    ctx.SaveChanges();
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //更新状态
            foreach (Day item in Calendar.Days)
            {
                if (item.IsTargetMonth)
                {
                    try
                    {
                        var target = _attendance.Details.Where(x => x.Day == item.Date).First();
                        target.AttType = item.AttendanceType;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            CloseEvent?.Invoke(_attendance);
        }
    }
}

