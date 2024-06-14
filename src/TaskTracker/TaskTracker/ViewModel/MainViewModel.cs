using GongSolutions.Wpf.DragDrop;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using TaskTracker.Model;
using Task = TaskTracker.Model.Task;
using TaskStatus = TaskTracker.Model.TaskStatus;

namespace TaskTracker.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged, IDropTarget
    {
        private Task _selectedTask;
        private Task _tempTask;
        private RelayCommand _addTaskCommand;
        private RelayCommand _editTaskCommand;
        private RelayCommand _confirmAddTaskCommand;
        private RelayCommand _cancelAddTaskCommand;
        private RelayCommand _confirmEditTaskCommand;
        private RelayCommand _deleteTaskCommand;
        private bool _isAddTask = false;
        private bool _isEditTask = false;
        private TaskStatistic _statistic = new();
        private ObservableCollection<Task> _tasks;
        public List<TaskStatus> StatusList { get; } = Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>().ToList();
        public ObservableCollection<Task> ToDoTasks { get; set; }
        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
                UpdateStatistics();
            }
        }
        public TaskStatistic Statistic
        {
            get => _statistic;
            set
            {
                _statistic = value;
                OnPropertyChanged(nameof(Statistic));
            }
        }
        public ObservableCollection<Task> InProgressTasks { get; set; }
        public ObservableCollection<Task> DoneTasks { get; set; }

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.TargetCollection is ObservableCollection<Task> tasks)
            {
                if (dropInfo.Data is Task task)
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Move;
                }
            }
        }
        private void UpdateStatistics()
        {
            _statistic.CalculateStatistics(_tasks);
            OnPropertyChanged(nameof(_statistic));
        }
        public void Drop(IDropInfo dropInfo)
        {
            var task = dropInfo.Data as Task;
            Task targetItem = dropInfo.TargetItem as Task;
            task.Status = targetItem.Status;
            SetTasks();
            JsonTaskConvert.SaveProject(Tasks.ToList());
        }

        public Task SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                OnPropertyChanged("selectedTask");
            }
        }

        public bool IsEditTask
        {
            get { return _isEditTask; }
            set
            {
                if (_isEditTask != value)
                {
                    _isEditTask = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsAddTask
        {
            get { return _isAddTask; }
            set
            {
                if (_isAddTask != value)
                {
                    _isAddTask = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand AddTaskCommand
        {
            get
            {
                return _addTaskCommand ??
              (_addTaskCommand = new RelayCommand(obj =>
              {
                  SelectedTask = new();
                  Enum.TryParse((string?)obj, out TaskStatus status);
                  SelectedTask.Status = status;
                  IsAddTask = true;
              }));
            }
        }

        public RelayCommand ConfirmAddTaskCommand
        {
            get
            {
                return _confirmAddTaskCommand ??
              (_confirmAddTaskCommand = new RelayCommand(obj =>
              {
                  Task task = new Task(SelectedTask);
                  Tasks.Insert(0, task);
                  IsAddTask = false;
                  SetTasks();
                  JsonTaskConvert.SaveProject(Tasks.ToList());
              }));
            }
        }

        public RelayCommand ConfirmEditTaskCommand
        {
            get
            {
                return _confirmEditTaskCommand ??
              (_confirmEditTaskCommand = new RelayCommand(obj =>
              {
                  Tasks.Remove(SelectedTask);
                  Task task = SelectedTask;
                  Tasks.Insert(0, task);
                  IsEditTask = false;
                  SetTasks();
                  JsonTaskConvert.SaveProject(Tasks.ToList());
              }));
            }
        }

        public RelayCommand CancelAddTaskCommand
        {
            get
            {
                return _cancelAddTaskCommand ??
              (_cancelAddTaskCommand = new RelayCommand(obj =>
              {
                  if (_tempTask != null)
                  {
                      SelectedTask.Name = _tempTask.Name;
                      SelectedTask.Description = _tempTask.Description;
                      SelectedTask.Status = _tempTask.Status;
                      SelectedTask.DateStartTask = _tempTask.DateStartTask;
                  }
                  IsAddTask = false;
                  IsEditTask = false;
              }));
            }
        }

        public RelayCommand EditTaskCommand
        {
            get
            {
                return _editTaskCommand ??
                    (_editTaskCommand = new RelayCommand(obj =>
                    {
                        SelectedTask = obj as Task;
                        _tempTask = new(SelectedTask);
                        IsEditTask = true;
                        OnPropertyChanged("SelectedTask");
                    }));
            }
        }

        public RelayCommand DeleteTaskCommand
        {
            get
            {
                return _deleteTaskCommand ??
             (_deleteTaskCommand = new RelayCommand(obj =>
             {
                 Task phone = obj as Task;
                 if (phone != null)
                 {
                     Tasks.Remove(phone);
                     SetTasks();
                     JsonTaskConvert.SaveProject(Tasks.ToList());
                 }
             }))
             ;
            }
        }

        public MainViewModel()
        {
            SelectedTask = new Task();
            Tasks = new ObservableCollection<Task>(JsonTaskConvert.LoadProject());
            ToDoTasks = new ObservableCollection<Task>();
            InProgressTasks = new ObservableCollection<Task>();
            DoneTasks = new ObservableCollection<Task>();
            SetTasks();
        }

        private void SetTasks()
        {
            ToDoTasks.Clear();
            InProgressTasks.Clear();
            DoneTasks.Clear();
            foreach (var task in Tasks)
            {
                switch (task.Status)
                {
                    case TaskStatus.ToDo:
                        {
                            ToDoTasks.Insert(0, task);
                            break;
                        }
                    case TaskStatus.InProgress:
                        {
                            InProgressTasks.Insert(0, task);
                            break;
                        }
                    case TaskStatus.Done:
                        {
                            DoneTasks.Insert(0, task);
                            break;
                        }
                    default:
                        {
                            ToDoTasks.Insert(0, task);
                            break;
                        }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
