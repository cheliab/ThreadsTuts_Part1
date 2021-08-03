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

        private WorkerSyncContext _workerSyncContext;
        private SynchronizationContext _context;
        
        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoadWindow;
        }

        #region ВариантБезИспользованияКонтекстаСинхронизации

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
            _worker.Cancel();
        }

        private void Worker_WorkCompleted(bool cancelled)
        {
            Action action = () =>
            {
                mainProgressBar.Value = 0;
                startButton.IsEnabled = true;
                
                string message = cancelled ? "Процесс отменен" : "Процесс завершен!";
                MessageBox.Show(message);
            };

            this.InvokeEx(action);
        }

        private void Worker_ProcessChanged(int progress)
        {
            Action action = () =>
            {
                mainProgressBar.Value = progress;
            };

            this.InvokeEx(action);
        }

        #endregion

        #region ВариантССинхронизациейКонтекста
        
        private void OnLoadWindow(object sender, RoutedEventArgs e)
        {
            _context = SynchronizationContext.Current;
        }

        private void SyncContext_StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            _workerSyncContext = new WorkerSyncContext();
            _workerSyncContext.ProcessChanged += SyncContext_Worker_ProcessChanged;
            _workerSyncContext.WorkCompleted += SyncContext_Worker_WorkCompleted;
            
            SyncContext_startButton.IsEnabled = false;
            
            Thread thread = new Thread(_workerSyncContext.Work);
            thread.Start(_context);
        }

        private void SyncContext_StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            _workerSyncContext.Cancel();
        }

        private void SyncContext_Worker_WorkCompleted(bool cancelled)
        {
            SyncContext_mainProgressBar.Value = 0;
            SyncContext_startButton.IsEnabled = true;
            
            string message = cancelled ? "Процесс отменен" : "Процесс завершен!";
            MessageBox.Show(message);
        }

        private void SyncContext_Worker_ProcessChanged(int progress)
        {
            SyncContext_mainProgressBar.Value = progress;
        }

        #endregion
        
    }
}