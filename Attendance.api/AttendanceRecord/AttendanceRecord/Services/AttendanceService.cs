using Attendance.Models;
using Attendance.Readers;
using Attendance.Roles;
using NPOI.HSSF.Model;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Services
{
    public class AttendanceService
    {
        public IEnumerable<Attendance<DateSplitAttRecord>> SplitAttsByName(IEnumerable<DateSplitAttRecord> records, DateTime targetMonth, string settingPath)
        {
            var setting = LoadSetting(settingPath);
            var year = targetMonth.Year;
            var month = targetMonth.Month;
            var attendances = records
                .Where(x => x.AttendanceTime.Year == year && x.AttendanceTime.Month == month)
                .GroupBy(x => new { x.Name, x.Id }).Select(x => new Attendance<DateSplitAttRecord>
                {
                    Id = x.Key.Id,
                    Name = x.Key.Name,
                    Details = x
                        .GroupBy(y => y.AttendanceTime.ToString("yyyy/MM/dd"))
                        .Select(y => new AttendanceDetail { Day = DateTime.Parse(y.Key), Records = y.ToList() })
                        .ToList(),
                        TimesOfCheck = new string[] { "07:00:00", "12:00:00", "13:30:00", "17:00:00" },
                        ValidOfAtt = setting.Where(y => y.Name == x.Key.Name).FirstOrDefault()?.ValidOfAtt ?? 4,
                        StaffAttenType = setting.Where(y => y.Name == x.Key.Name).FirstOrDefault()?.IsShiftWork == "是" ? StaffAttenType.ShiftWork : StaffAttenType.NormalWork
                })
           .Select(x => SetDaysOfAttendance(x))
           .Select(x => SetDaysOfLate(x))
           .Select(x => SetDaysOfEarly(x))
           .OrderBy(x => x.Id);
            return attendances;
        }

        public void ResetDays(Attendance<DateSplitAttRecord> attendance)
        {
            SetDaysOfAttendance(attendance);
            SetDaysOfLate(attendance);
            SetDaysOfEarly(attendance);
        }

        private Attendance<DateSplitAttRecord> SetDaysOfAttendance(Attendance<DateSplitAttRecord> attendance)
        {
            //累加器，符合条件+1
            attendance.DaysOfAtt = attendance.Details.Aggregate(0, (count, next) =>
            {
                var result = 0;
                //非异常，计实际出勤
                if (next.AttType != AttendanceType.Exception)
                {
                    result = 1;
                }
                else
                {
                    result = AttendanceRole.AttRole(next, attendance.ValidOfAtt) == true ? 1 : 0;
                    if (result == 1)
                        next.AttType = AttendanceType.Normal;
                }
                return count + result;
            });
            return attendance;
        }

        private Attendance<DateSplitAttRecord> SetDaysOfLate(Attendance<DateSplitAttRecord> attendance)
        {
            //累加器，符合条件+1
            attendance.DaysOfLate = attendance.Details.Aggregate(0, (count, next) => count + AttendanceRole.LateRole(next, attendance.ValidOfAtt, attendance.TimesOfCheck,attendance.StaffAttenType));
            return attendance;
        }

        private Attendance<DateSplitAttRecord> SetDaysOfEarly(Attendance<DateSplitAttRecord> attendance)
        {
            //累加器，符合条件+1
            attendance.DaysOfEarly = attendance.Details.Aggregate(0, (count, next) => count + AttendanceRole.EarlyRole(next, attendance.ValidOfAtt, attendance.TimesOfCheck, attendance.StaffAttenType));
            return attendance;
        }

        /// <summary>
        /// 读取考勤记录
        /// </summary>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        public IEnumerable<DateSplitAttRecord> GetAttRecords(string[] filePaths)
        {
            IEnumerable<DateSplitAttRecord> attRecords = new List<DateSplitAttRecord>();
            for (int i = 0; i < filePaths.Length; i++)
            {
                var filePath = filePaths[i];
                var reader = GetReader(filePath);
                var records = reader.GetRecords(filePath);
                attRecords = attRecords.Concat(records);
            }
            return attRecords;
        }

        private IAttendanceReader GetReader(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                HSSFWorkbook xssWorkbook = new HSSFWorkbook(stream);
                var sheet = xssWorkbook.GetSheetAt(0);
                var row = sheet.GetRow(0);
                var lastColNum = row.LastCellNum;
                for (int i = 0; i < lastColNum; i++)
                {
                    var cell = row.GetCell(i);
                    if (cell is null)
                        continue;
                    if (cell.StringCellValue == "工号")
                        return new DateSplitAttReader();
                }
                return new NormalAttReader();
            }
        }

        /// <summary>
        /// 注入汇总表
        /// </summary>
        /// <param name="attendances"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool Inject(IEnumerable<Attendance<DateSplitAttRecord>> attendances, string filename)
        {
            var reader = new AttenSumReader();
            try
            {
                reader.Inject(attendances, filename);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private IEnumerable<AttenSetting> LoadSetting(string filename = "Config/setting.xls")
        {
            var reader = new AttenSettingReader();
            return reader.LoadSetting(filename);
        }
    }
}
