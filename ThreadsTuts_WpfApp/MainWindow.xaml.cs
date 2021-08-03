using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThreadsTuts_WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Worker _worker;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            _worker = new Worker();
            _worker.ProcessChanged += Worker_ProcessChanged;
            _worker.WorkCompleted += Worker_WorkCompleted;

            startButton.IsEnabled = false;

            Thread thread = new Thread(_worker.Work);
            thread.Start();
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void Worker_WorkCompleted(bool cancelled)
        {
            Action action = () =>
            {
                string message = cancelled ? "Процесс отменен" : "Процесс завершен!";
                MessageBox.Show(message);
                startButton.IsEnabled = true;
            };

            this.Dispatcher.Invoke(action);
        }

        private void Worker_ProcessChanged(int progress)
        {
            Action action = () =>
            {
                mainProgressBar.Value = progress;
            };

            this.Dispatcher.Invoke(action);
        }
    }
}