using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Models;
using Npoi.Mapper;

namespace Attendance.Readers
{
    public class AttenSettingReader
    {
        public IEnumerable<AttenSetting> LoadSetting(string filename)
        {
            if (filename is null)
                throw new Exception("请先选择人员配置表");
            var mapper = new Mapper(filename);
            mapper.Map<AttenSetting>("姓名", "Name")
                .Map<AttenSetting>("有效打卡数", "ValidOfAtt")
                .Map<AttenSetting>("是否倒班", "IsShiftWork");
            var settings = mapper.Take<AttenSetting>();
            return settings.Select(x=>x.Value);
        }
    }
}
