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
        private string _searchText;
        private bool _isAddTask = false;
        private bool _isEditTask = false;
        private TaskStatistic _statistic = new();
        private ObservableCollection<Task> _tasks;
        private ObservableCollection<TaskBranch> _taskBranches;
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
        public ObservableCollection<TaskBranch> TaskBranches
        {
            get => _taskBranches;
            set
            {
                _taskBranches = value;
                OnPropertyChanged(nameof(TaskBranches));
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

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    SetTasks();
                }
            }
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

        /// <summary>
        /// Метод, перемящийщий задачи в соответстсвующие по статусу листы
        /// Если строка не пуста, то сначала фильтрует задачи по соответвию
        /// введённого текста и названия задач.
        /// </summary>
        private void SetTasks()
        {
            ToDoTasks.Clear();
            InProgressTasks.Clear();
            DoneTasks.Clear();
            var filteredTasks = string.IsNullOrEmpty(SearchText) ?
                Tasks :
                Tasks.Where(t => t.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            foreach (var task in filteredTasks)
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

        /// <summary>
        /// метод для перемещения задачи.
        /// </summary>
        /// <param name="dropInfo"></param>
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

        /// <summary>
        /// Метод, обновляющий статистику задач.
        /// </summary>
        private void UpdateStatistics()
        {
            _statistic.CalculateStatistics(_tasks);
            OnPropertyChanged(nameof(_statistic));
        }

        /// <summary>
        /// Метод, работающий после окончания метода DragOver.
        /// </summary>
        /// <param name="dropInfo"></param>
        public void Drop(IDropInfo dropInfo)
        {
            var task = dropInfo.Data as Task;
            Task targetItem = dropInfo.TargetItem as Task;
            task.Status = targetItem.Status;
            if (task.Status == TaskStatus.Done)
            {
                task.ActualDateEndTask = DateTime.Now;
            }
            SetTasks();
            JsonTaskConvert.SaveProject(Tasks.ToList());
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
            Tasks = new ObservableCollection<Task>((List<Task>)JsonTaskConvert.LoadProject(typeof(Task)));
            TaskBranches = new ObservableCollection<TaskBranch>((List<TaskBranch>)JsonTaskConvert.LoadProject(typeof(TaskBranch)));
            ToDoTasks = new ObservableCollection<Task>();
            InProgressTasks = new ObservableCollection<Task>();
            DoneTasks = new ObservableCollection<Task>();
            SetTasks();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
