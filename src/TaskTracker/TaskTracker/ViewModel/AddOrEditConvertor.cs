using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TaskTracker.View.ViewModel;

namespace TaskTracker.ViewModel
{
    public class AddOrEditConvertor : IValueConverter
    {
        private readonly MainViewModel viewModel = new();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var IsAddOrEditTask = (bool)value;
            return IsAddOrEditTask ? viewModel.ConfirmAddTaskCommand : viewModel.ConfirmEditTaskCommand;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
