using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskTracker.Model;
using TaskTracker.View;
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
        public MainWindow()
        {
            InitializeComponent();
            var taskView = new TasksView();
            taskView.BranchSelected += OnUserControlButtonClicked;
            MainContent.Content = taskView;
        }
        public void OnUserControlButtonClicked(object sender, RoutedEventArgs e)
        {

        }
        private void BurgerButton_Click(object sender, RoutedEventArgs e)
        {
            if (BurgerMenu.RenderTransform is TranslateTransform transform && transform.X == 0)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }

        private void OpenMenu()
        {
            Overlay.Visibility = Visibility.Visible;
            Overlay.IsHitTestVisible = true;
            BurgerMenu.Visibility = Visibility.Visible;
            Storyboard sb = FindResource("OpenMenu") as Storyboard;
            sb.Begin();
        }

        private void CloseMenu()
        {
            Storyboard sb = FindResource("CloseMenu") as Storyboard;
            sb.Completed += (s, e) =>
            {
                Overlay.Visibility = Visibility.Collapsed;
                Overlay.IsHitTestVisible = false;
                BurgerMenu.Visibility = Visibility.Collapsed;
            };
            sb.Begin();
        }
        private void Overlay_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CloseMenu();
        }
        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new TasksView();
            CloseMenu();
        }

        private void BranchesButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new BranchesView();
            CloseMenu();
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new StatisticsView();
            CloseMenu();
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