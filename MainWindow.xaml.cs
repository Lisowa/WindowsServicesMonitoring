using System.Threading;
using System.Windows;

namespace WindowsServicesMonitoring
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = (MainWindowViewModel)DataContext;

            var getServicesThread = new Thread(() =>
            {
               while (true)
               {
                   _viewModel.Refresh();
                   Thread.Sleep(1000);
               }
            });

            getServicesThread.IsBackground = true;
            getServicesThread.Start();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
