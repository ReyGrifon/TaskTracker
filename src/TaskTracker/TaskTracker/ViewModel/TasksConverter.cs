using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Task = TaskTracker.Model.Task;
using TaskStatus = TaskTracker.Model.TaskStatus;

namespace TaskTracker.ViewModel
{
    public class TasksConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tasks = (ObservableCollection<Task>)value;
            Enum.TryParse((string?)parameter, out TaskStatus status);
            return tasks.Where(task => task.Status == status).ToList();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
