using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Model
{
    public class TaskStatistic
    {
        public int TotalTasks { get; private set; }
        public int CompletedTasks { get; private set; }
        public int InProgressTasks { get; private set; }
        public int ToDoTasks { get; private set; }
        public int OverdueTasks { get; private set; }
        public double AverageCompletionTime { get; private set; }

        public void CalculateStatistics(IEnumerable<Task> tasks)
        {
            TotalTasks = tasks.Count();
            CompletedTasks = tasks.Count(t => t.Status == TaskStatus.Done);
            InProgressTasks = tasks.Count(t => t.Status == TaskStatus.InProgress);
            ToDoTasks = tasks.Count(t => t.Status == TaskStatus.ToDo);
            OverdueTasks = tasks.Count(t => t.Status != TaskStatus.Done && t.DateEndTask < DateTime.Now);
            var completedTasks = tasks.Where(t => t.Status == TaskStatus.Done);
            if (completedTasks.Any())
            {
                AverageCompletionTime = completedTasks.Average(t => (t.DateEndTask.Day - t.DateStartTask.Day));
            }
            else
            {
                AverageCompletionTime = 0;
            }
        }
    }
}
