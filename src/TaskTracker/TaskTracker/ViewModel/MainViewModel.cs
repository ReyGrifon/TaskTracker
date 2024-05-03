using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskTracker.Model;
using Task = TaskTracker.Model.Task;
using TaskStatus = TaskTracker.Model.TaskStatus;

namespace TaskTracker.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public List<TaskStatus> StatusList { get; } = Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>().ToList();

        private Task _selectedTask;
        private Task _tempTask;
        public Task SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                OnPropertyChanged("selectedTask");
            }
        }
        private bool _isAddOrEditTask = false;
        public bool IsAddOrEditTask
        {
            get { return _isAddOrEditTask; }
            set
            {
                if (_isAddOrEditTask != value)
                {
                    _isAddOrEditTask = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isAddingTask = false;
        public bool IsAddingTask
        {
            get { return _isAddingTask; }
            set
            {
                if (_isAddingTask != value)
                {
                    _isAddingTask = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand _addTaskCommand;
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
                  IsAddingTask = true;
              }));
            }
        }
        private RelayCommand _confirmAddTaskCommand;
        public RelayCommand ConfirmAddTaskCommand
        {
            get
            {
                return _confirmAddTaskCommand ??
              (_confirmAddTaskCommand = new RelayCommand(obj =>
              {
                  Task task = new Task(SelectedTask.Name, SelectedTask.Description, SelectedTask.Status, SelectedTask.DateStartTask);
                  Tasks.Insert(0, task);
                  IsAddingTask = false;
                  SetTasks();
                  JsonFileService.SaveProject(Tasks.ToList());
              }));
            }
        }
        private RelayCommand _confirmEditTaskCommand;
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
                  IsAddingTask = false;
                  SetTasks();
                  JsonFileService.SaveProject(Tasks.ToList());
              }));
            }
        }
        private RelayCommand _cancelAddTaskCommand;
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
                  IsAddingTask = false;
              }));
            }
        }
        private RelayCommand _editTaskCommand;
        public RelayCommand EditTaskCommand
        {
            get
            {
                return _editTaskCommand ??
                    (_editTaskCommand = new RelayCommand(obj =>
                    {
                        SelectedTask = obj as Task;
                        _tempTask = SelectedTask.Clone();
                        IsAddingTask = true;
                        OnPropertyChanged("SelectedTask");
                    }));
            }
        }
        private RelayCommand _deleteTaskCommand;
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
                     JsonFileService.SaveProject(Tasks.ToList());
                 }
             }))
             ;
            }
        }
        public ObservableCollection<Task> ToDoTasks { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }

        private ObservableCollection<Task> _inProgressTasks;
        public ObservableCollection<Task> DoneTasks { get; set; }

        public ObservableCollection<Task> InProgressTasks
        {
            get { return _inProgressTasks; }
            set
            {
                _inProgressTasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }
        public MainViewModel()
        {
            // Инициализация коллекций задач
            SelectedTask = new Task();
            Tasks = new ObservableCollection<Task>(JsonFileService.LoadProject());
            ToDoTasks = new ObservableCollection<Task>();
            InProgressTasks = new ObservableCollection<Task>();
            DoneTasks = new ObservableCollection<Task>();
            SetTasks();
        }
        void SetTasks()
        {
            ToDoTasks.Clear();
            InProgressTasks.Clear();
            DoneTasks.Clear();
            foreach(var task in Tasks)
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
