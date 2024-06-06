using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Model
{
    public class TaskBranch : INotifyPropertyChanged
    {
        private string _branchName;
        private string _taskNameTemplate;
        private string _taskDescriptionTemplate;
        private int _taskDurationInDays;
        private List<Task> _tasks = new List<Task>();

        public string BranchName
        {
            get => _branchName;
            set
            {
                _branchName = value;
                OnPropertyChanged(nameof(BranchName));
            }
        }

        public string TaskNameTemplate
        {
            get => _taskNameTemplate;
            set
            {
                _taskNameTemplate = value;
                OnPropertyChanged(nameof(TaskNameTemplate));
            }
        }

        public string TaskDescriptionTemplate
        {
            get => _taskDescriptionTemplate;
            set
            {
                _taskDescriptionTemplate = value;
                OnPropertyChanged(nameof(TaskDescriptionTemplate));
            }
        }

        public int TaskDurationInDays
        {
            get => _taskDurationInDays;
            set
            {
                _taskDurationInDays = value;
                OnPropertyChanged(nameof(TaskDurationInDays));
            }
        }

        public List<Task> Tasks
        {
            get => _tasks;
            private set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
