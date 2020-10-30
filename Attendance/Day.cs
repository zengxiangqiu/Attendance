using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Models;
using Attendance.ViewModels;

namespace Attendance
{
    public  class Days 
    {
        public System.DateTime date { get; set; }
        public string notes { get; set; }
        public AttendanceType AttendanceType { get; set; }
    }
}
