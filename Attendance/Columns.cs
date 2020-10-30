using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance
{
    public class Columns
    {
        public Dictionary<string, ColumnProp> Attendance { get; set; }
    }

    public class ColumnProp
    {
        public string description { get; set; }
        public int order { get; set; }
        public bool  visiable { get; set; } 
    }

}
