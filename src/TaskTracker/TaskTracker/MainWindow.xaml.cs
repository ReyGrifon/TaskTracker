using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskTracker.ViewModel;
using Task = TaskTracker.Model.Task;
using TaskStatus = TaskTracker.Model.TaskStatus;

namespace TaskTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel viewModel = new();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("task"))
                e.Effects = DragDropEffects.Move;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void Border_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("task"))
                e.Effects = DragDropEffects.Move;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            var name = ((Grid)sender).ColumnDefinitions;
            Point dropPosition = e.GetPosition(this);
            foreach (var border in FindVisualChildren<Border>(this))
            {
                        var droppedTask = e.Data.GetData("task") as Task; // Замените "Task" на ваш тип данных задачи
                                                                          // Определите, в каком столбце находится место, куда была брошена задача
                        var dropTarget = (Border)sender;
                        if (dropTarget.Name == "ToDoTasks")
                            viewModel.MoveTask(droppedTask, TaskStatus.ToDo);
                        else if (dropTarget.Name == "InProcessTasks")
                            viewModel.MoveTask(droppedTask, TaskStatus.InProgress);
                        else if (dropTarget.Name == "DoneTasks")
                            viewModel.MoveTask(droppedTask, TaskStatus.Done);
                    }
                    e.Handled = true;
        }
        private bool isDragging = false;
        private Point startPoint;

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !isDragging)
            {
                startPoint = e.GetPosition(null);
                isDragging = true;
                DragDrop.DoDragDrop((sender as Border), (sender as Border).DataContext, DragDropEffects.Move);
                isDragging = false;
            }
        }
        private bool IsMouseOverBorder(Border border, Point dropPosition)
        {
            Point borderPosition = border.PointToScreen(new Point(0, 0));
            double borderWidth = border.ActualWidth;
            double borderHeight = border.ActualHeight;

            return dropPosition.X >= borderPosition.X && dropPosition.X <= borderPosition.X + borderWidth &&
                   dropPosition.Y >= borderPosition.Y && dropPosition.Y <= borderPosition.Y + borderHeight;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}