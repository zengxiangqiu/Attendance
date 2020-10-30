using Attendance.Models;
using Npoi.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using System.IO;

namespace AttendanceRecord.Readers
{
    public class AttResultReader
    {
        public List<AttendanceResult> GetRecords(string fileName)
        {
            var mapper = GetMapper(fileName);
            var records = mapper.Take<AttendanceResult>();
            var result = records.Select(x => x.Value).ToList();
            return result;  
        }

        public void InjectResult(List<AttendanceResult> attendanceResults,string fileName)
        {
            //var mapper =  GetMapper(fileName);
            ////mapper.Put(attendanceResults);
            //mapper.Save(fileName);
            XSSFWorkbook workbook;
            using (FileStream fs = new FileStream(fileName, FileMode.Open,FileAccess.Read))
            {
                workbook = new XSSFWorkbook(fs);
                var sheet = workbook.GetSheetAt(0);
                var lastRowNum = sheet.LastRowNum;
                //3 姓名
                for (int i = 0; i < lastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    var name = row.GetCell(3)?.StringCellValue;
                    if (name is null) continue;
                    var attendance = attendanceResults.Where(x => x.Name == name).FirstOrDefault();
                    if (attendance != null)
                    {
                        row.GetCell(4).SetCellValue(attendance.DaysOfAtt);
                        row.GetCell(5).SetCellValue(attendance.DaysOfLeave);
                        row.GetCell(6).SetCellValue(attendance.DaysOfLateOrEarly);
                    }
                }
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }

        private Mapper GetMapper(string fileName)
        {
            var mapper = new Mapper(fileName);
            //表头
            mapper.FirstRowIndex = 2;
            //姓名 实际出勤天数  请假 迟到/早退（次数）	加班（时数）
            mapper.Map<AttendanceResult>("姓名", "Name")
                .Map<AttendanceResult>("实际出勤天数", "DaysOfAtt")
                .Map<AttendanceResult>("请假天数", "DaysOfLeave")
                .Map<AttendanceResult>("迟到/早退（次数）", "DaysOfLateOrEarly");

            return mapper;
        }
    }
}
