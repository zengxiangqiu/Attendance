using Attendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Roles
{
    public class AttendanceRole
    {
        public static bool AttRole(AttendanceDetail details, int validOfAtt)
        {
            return details.Records.Count() >= validOfAtt;
        }

        public static int EarlyRole(AttendanceDetail details, int validOfAtt, string[] timesOfCheck)
        {
            //同一小时内取min
            var records = details.Records.GroupBy(x => x.AddtendanceTime.ToString("hh"))
.Select(x => x.ToList().Min()).OrderBy(x => x.AddtendanceTime).ToArray();
            //var records = details.Records.OrderBy(x => x.AddtendanceTime).ToArray();

            if (records.Count() < validOfAtt)
            {
                //异常，不计入迟到范围，人工判定
                return 0;
            }
            else
            {
                var counter = 0;
                for (int i = 0; i < records.Count(); i++)
                {
                    if (i >= validOfAtt)
                    {
                        //超出部分无法判定
                        break;
                    }
                    var AddtendanceTime = records[i].AddtendanceTime;
                    var flag = i % 2 != 0;
                    //true , even, 判定早退
                    if (flag)
                    {
                        var normalTime = new DateTime(AddtendanceTime.Year, AddtendanceTime.Month, AddtendanceTime.Day);
                        var tm = TimeSpan.Parse(timesOfCheck[i]);
                        normalTime = normalTime.Add(tm);
                        if (AddtendanceTime < normalTime)
                            counter++;
                    }
                }
                return counter;
            }
        }

        public static int LateRole(AttendanceDetail details, int validOfAtt, string[] timesOfCheck)
        {
            var records = details.Records.GroupBy(x => x.AddtendanceTime.ToString("hh"))
            .Select(x => x.ToList().Min()).OrderBy(x => x.AddtendanceTime).ToArray();
            if (records.Count() < validOfAtt)
            {
                //异常，不计入迟到范围，人工判定
                return 0;
            }
            else
            {
                var counter = 0;
                for (int i = 0; i < records.Count(); i++)
                {
                    if (i >= validOfAtt)
                    {
                        //超出部分无法判定
                        break;
                    }
                    var AddtendanceTime = records[i].AddtendanceTime;
                    var flag = i % 2 == 0;
                    //true , even, 判定迟到
                    if (flag)
                    {
                        var normalTime = new DateTime(AddtendanceTime.Year, AddtendanceTime.Month, AddtendanceTime.Day);
                        var tm = TimeSpan.Parse(timesOfCheck[i]);
                        normalTime = normalTime.Add(tm);
                        if (AddtendanceTime > normalTime)
                            counter++;
                    }
                }
                return counter;
            }
        }
    }
}
