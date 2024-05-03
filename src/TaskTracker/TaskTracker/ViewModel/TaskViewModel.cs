using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Task = TaskTracker.Model.Task;
using TaskStatus = TaskTracker.Model.TaskStatus;

namespace TaskTracker.ViewModel
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private Task _task;
        public TaskViewModel(Task task)
        {
            _task = task;
        }
        public string Name
        {
            get => _task.Name;
            set
            {
                _task.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Description
        {
            get => _task.Description;
            set
            {
                _task.Description = value;
                OnPropertyChanged("Description");
            }
        }
        public TaskStatus Status
        {
            get => _task.Status;
            set
            {
                _task.Status = value;
                OnPropertyChanged("Status");
            }
        }
        public DateTime DateStartTask
        {
            get => _task.DateStartTask;
            set
            {
                _task.DateStartTask = value;
                OnPropertyChanged("DateStartTask");
            }
        }
        public DateTime DateEndTask
        {
            get => _task.DateEndTask;
            set
            {
                _task.DateEndTask = value;
                OnPropertyChanged("DateEndTask");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
