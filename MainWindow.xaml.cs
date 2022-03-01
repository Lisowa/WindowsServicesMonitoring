using System;
using System.Threading;
using System.Windows;

namespace WindowsServicesMonitoring
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = (MainWindowViewModel)DataContext;

            var getServicesThread = new Thread(() =>
            {
                while (true)
                {
                    viewModel.Refresh();
                    Thread.Sleep(1000);
                }
            });

            getServicesThread.IsBackground = true;
            getServicesThread.Start();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

            var startServiceThread = new Thread(() =>
            {
                try
                {
                    viewModel.IsBusy = true;
                    viewModel.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't start the service!\n" + ex.Message);
                }
                finally
                {
                    viewModel.IsBusy = false;
                }
            });

            startServiceThread.IsBackground = true;
            startServiceThread.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            var stopServiceThread = new Thread(() =>
            {
                try
                {
                    viewModel.IsBusy = true;
                    viewModel.Stop();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Can't stop the service!\n" + ex.Message);
                }
                finally
                {
                    viewModel.IsBusy = false;
                }
            });
            stopServiceThread.IsBackground = true;
            stopServiceThread.Start();
        }


    }
}
