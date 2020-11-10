using Attendance;
using Attendance.Models;
using Attendance.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Attendance.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ICommand _gotoView1Command;
        private ICommand _gotoView2Command;
        private object _currentView;
        private object _view1;
        private object _view2;

        public MainWindowViewModel()
        {
            var attWindow = new AttWindow();
            attWindow.SelectedChange += AttWindow_SelectedChange;
            _view1 = attWindow;
            
            CurrentView = _view1;
        }

        private void AttWindow_SelectedChange(Attendance<DateSplitAttRecord> obj,int month)
        {
            var detailWindow= new AttDetailWindow();
            detailWindow.InitView(obj, month);
            detailWindow.CloseEvent += DetailWindow_CloseEvent;
            _view2 = detailWindow;
            CurrentView = _view2;
        }

        private void DetailWindow_CloseEvent(Attendance<DateSplitAttRecord> attendance)
        {
            CurrentView = _view1;
            var service = new AttendanceService();
            service.ResetDays(attendance);
        }

        public object GotoView1Command
        {
            get
            {
                return _gotoView1Command ?? (_gotoView1Command = new RelayCommand(
                   x =>
                   {
                       GotoView1();
                   }));
            }
        }

        public ICommand GotoView2Command
        {
            get
            {
                return _gotoView2Command ?? (_gotoView2Command = new RelayCommand(
                   x =>
                   {
                       GotoView2();
                   }));
            }
        }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        private void GotoView1()
        {
            CurrentView = _view1;
        }

        private void GotoView2()
        {
            CurrentView = _view2;
        }
    }
}
