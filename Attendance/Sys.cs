using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance
{
    public class Sys
    {
        public readonly static string configPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/Config/";
        private static Columns _columns;
        public static Columns Columns
        {
            get
            {
                if (_columns == null)
                {
                    _columns = Newtonsoft.Json.JsonConvert.DeserializeObject<Columns>(File.ReadAllText(configPath + "Columns.json"));
                }
                return _columns;
            }
        }
    }
}
