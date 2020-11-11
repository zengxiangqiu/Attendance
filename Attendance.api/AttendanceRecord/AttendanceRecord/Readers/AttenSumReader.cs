using Attendance.Models;
using Npoi.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;

namespace Attendance.Readers
{
    public class AttenSumReader
    {
        public void Inject(IEnumerable<Attendance<DateSplitAttRecord>> attendances, string fileName)
        {
            IWorkbook workbook;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                try
                {
                    workbook = new HSSFWorkbook(fs);
                }
                catch (NPOI.POIFS.FileSystem.OfficeXmlFileException ex)
                {
                    using (FileStream fsx = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
                    {
                        workbook = new XSSFWorkbook(fsx);
                    }
                }
                var sheet = workbook.GetSheetAt(0);
               var lastRowNum = sheet.LastRowNum;
                foreach (Attendance<DateSplitAttRecord> staff in attendances)
                {
                    for (int i = 0; i < lastRowNum; i++)
                    {
                        try
                        {
                            var row = sheet.GetRow(i);
                            var name = row.GetCell(3)?.StringCellValue;

                            if (staff.Name.Trim() == name.Trim())
                            {
                                row.GetCell(4).SetCellValue(staff.DaysOfAtt);
                                row.GetCell(5).SetCellValue(staff.DaysOfLeave);
                                row.GetCell(6).SetCellValue(staff.DaysOfEarly + staff.DaysOfLate);
                            }
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                }
            }
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                workbook.Write(fs);
            }
        }
    }
}
