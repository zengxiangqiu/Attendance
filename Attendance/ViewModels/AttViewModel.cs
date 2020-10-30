using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Attendance.Models;

namespace Attendance.ViewModels
{
    public class AttViewModel: BaseViewModel
    {
        public ObservableCollection<Attendance<DateSplitAttRecord>> Attendances { get; set; } = new ObservableCollection<Attendance<DateSplitAttRecord>>();

        private ObservableCollection<string> _inputFiles = new ObservableCollection<string>();

        public ObservableCollection<string> InputFiles
        {
            get { return _inputFiles; }
            set {
                _inputFiles = value;
                OnPropertyChanged("InputFiles");
            }
        }


        private string targetPath;

        public string TargetPath
        {
            get { return targetPath; }
            set {
                targetPath = value;
                OnPropertyChanged("TargetPath");

            }
        }

    }
}
