using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Model;

namespace TaskTracker.ViewModel
{
    public class TaskBranchViewModel : INotifyPropertyChanged
    {
        private TaskBranch _taskBranchDefault = new();
        private ObservableCollection<TaskBranch> _taskBranches = new();
        private RelayCommand _addBranchCommand;
        public TaskBranch TaskBranchDefault
        {
            get { return _taskBranchDefault; }
            set
            {
                if (_taskBranchDefault != value)
                {
                    _taskBranchDefault = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<TaskBranch> TaskBranches
        {
            get => _taskBranches;
            set
            {
                _taskBranches = value;
                OnPropertyChanged(nameof(TaskBranches));
            }
        }

        public RelayCommand AddBranchCommand
        {
            get
            {
                return _addBranchCommand ??
              (_addBranchCommand = new RelayCommand(obj =>
              {
                  TaskBranches.Insert(0, TaskBranchDefault);
                  TaskBranchDefault = new();
                  JsonTaskConvert.SaveProject(TaskBranches.ToList());
              }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
