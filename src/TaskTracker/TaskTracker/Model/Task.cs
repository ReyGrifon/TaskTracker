using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskTracker.Model
{
    public class Task : INotifyPropertyChanged
    {
        private string _name;
        private TaskStatus _status;
        private string _description;
        private DateTime _dateStartTask = DateTime.Now;
        private DateTime _dateEndTask = DateTime.Now;
        public TaskStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                if (value == null || value == "")
                {
                    _name = "Untitled";
                    return;
                }
                _name = value;
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
            }
        }
        public DateTime DateStartTask
        {
            get => _dateStartTask;
            set
            {
                if (value == null)
                {
                    _dateStartTask = DateTime.Now;
                    OnPropertyChanged(nameof(DateStartTask));
                    return;
                }
                _dateStartTask = value;
                OnPropertyChanged(nameof(DateStartTask));
            }
        }
        public DateTime DateEndTask
        {
            get => _dateEndTask;
            set
            {
                _dateEndTask = value;
                OnPropertyChanged("DateEndTask");
            }
        }
        public Task(string name, string description, TaskStatus status, DateTime? startDate)
        {
            Name = name;
            Description = description;
            Status = status;
            DateStartTask = (DateTime)startDate;
        }
        public Task(string name, string description, TaskStatus status, DateTime startDate, DateTime endTime)
        {
            Name = name;
            Description = description;
            Status = status;
            DateStartTask = startDate;
            DateEndTask = endTime;
        }
        public Task(string name, string description, TaskStatus status)
        {
            Name = name;
            Description = description;
            DateStartTask = DateTime.Now;
            Status = status;
        }
        public Task(string name, TaskStatus status) 
        {
            Name = name;
            Status = status;
            DateStartTask = DateTime.Now;
        }
        public Task(TaskStatus status)
        {
            Status = status;
        }
        public Task(Task value)
        {
            Name = value.Name;
            Description = value.Description;
            Status = value.Status;
            DateStartTask = value.DateStartTask;
            DateEndTask= value.DateEndTask;
        }
        public Task()
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public Task Clone()
        {
            return new Task
            {
                Name = this.Name,
                Description = this.Description,
                Status = this.Status,
                DateStartTask = this.DateStartTask
            };
        }
    }
}
