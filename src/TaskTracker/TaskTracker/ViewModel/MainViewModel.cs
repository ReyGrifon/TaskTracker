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
        private bool _isEditTask = false;
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
        private bool _isAddTask = false;
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
                  IsAddTask = true;
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
                  Task task = new Task(SelectedTask);
                  Tasks.Insert(0, task);
                  IsAddTask = false;
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
                  IsEditTask = false;
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
                  IsAddTask = false;
                  IsEditTask = false;
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
                        IsEditTask = true;
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
        public ObservableCollection<Task> InProgressTasks { get; set; }
        public ObservableCollection<Task> DoneTasks { get; set; }
        public MainViewModel()
        {
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
