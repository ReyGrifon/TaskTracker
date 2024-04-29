using TaskTracker.Model;
namespace TaskTracker.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Task = Model.Task;

    namespace TaskTracker.ViewModels
    {
        public class MainViewModel : INotifyPropertyChanged
        {
            private ObservableCollection<Task> _toDoTasks;
            private ObservableCollection<Task> _inProgressTasks;
            private ObservableCollection<Task> _doneTasks;

            public ObservableCollection<Task> ToDoTasks
            {
                get { return _toDoTasks; }
                set { _toDoTasks = value; OnPropertyChanged(); }
            }

            public ObservableCollection<Task> InProgressTasks
            {
                get { return _inProgressTasks; }
                set { _inProgressTasks = value; OnPropertyChanged(); }
            }

            public ObservableCollection<Task> DoneTasks
            {
                get { return _doneTasks; }
                set { _doneTasks = value; OnPropertyChanged(); }
            }

            public MainViewModel()
            {
                // Инициализация коллекций задач
                ToDoTasks = new ObservableCollection<Task>() {new Task("prost","asdas", Model.TaskStatus.ToDo), new Task("prost2", "asdas", Model.TaskStatus.ToDo) };
                InProgressTasks = new ObservableCollection<Task>();
                DoneTasks = new ObservableCollection<Task>();
                // Загрузка задач из хранилища или другого источника данных
                // (например, из файла или базы данных)
                // ToDoTasks = LoadTasksFromStorage();
                // InProgressTasks = LoadTasksFromStorage();
                // DoneTasks = LoadTasksFromStorage();
            }

            // Методы для загрузки и сохранения задач
            // Можно реализовать их, используя Newtonsoft.Json, как описано ранее

            // Метод для сохранения изменений
            // private void SaveChangesToStorage()

            // Метод для загрузки задач из хранилища
            // private ObservableCollection<Task> LoadTasksFromStorage()

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
