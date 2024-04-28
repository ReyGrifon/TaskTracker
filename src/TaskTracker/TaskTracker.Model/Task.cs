namespace TaskTracker.Model
{
    public class Task
    {
        private string _name;
        public string Description { get; set; }
        public DateTime DateStartTask { get; set; }
        private DateTime _dateEndTask;
        public TaskStatus Status { get; set; }
        public string Name
        {
            get => _name;
            set 
            {
                if (value == null)
                {
                    _name = "Intitled";
                    return;
                }
                _name = value;
            }
        }
        public DateTime DateEndTask
        {
            get => _dateEndTask;
            set
            {
                if (value < DateStartTask)
                {
                    throw new Exception("your task end date can't be earlier than the start date");
                }
                _dateEndTask = value;
            }
        }
        public Task(string name, string description, TaskStatus status)
        {
            Name = name;
            Description = description;
            DateStartTask = DateTime.Now;
            Status = status;
        }

        public Task() 
        {
            DateStartTask = DateTime.Now;
        }
    }
}
