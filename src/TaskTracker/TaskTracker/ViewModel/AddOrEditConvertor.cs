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
    public class AddOrEditConvertor : IMultiValueConverter
    {
        private readonly MainViewModel viewModel = new();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || !(values[0] is RelayCommand command) || !(values[1] is bool isAddingTask))
                return null;

            return isAddingTask ? viewModel.ConfirmAddTaskCommand : viewModel.ConfirmEditTaskCommand;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
