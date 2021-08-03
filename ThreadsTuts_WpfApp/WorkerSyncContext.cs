using System;
using System.Threading;

namespace ThreadsTuts_WpfApp
{
    public class WorkerSyncContext
    {
        private bool _cancelled = false;

        public void Cancel()
        {
            _cancelled = true;
        }

        public void Work(object param)
        {
            var context = (SynchronizationContext) param;
            
            for (var i = 1; i <= 100; i++)
            {
                if (_cancelled)
                    break;

                Thread.Sleep(50);

                context.Send(OnProgressChanged, i);
            }

            context.Send(OnWorkComplete, _cancelled);
        }

        private void OnProgressChanged(object i)
        {
            ProcessChanged?.Invoke((int) i);
        }

        private void OnWorkComplete(object cancelled)
        {
            WorkCompleted?.Invoke((bool) cancelled);            
        }

        public event Action<int> ProcessChanged;

        public event Action<bool> WorkCompleted;
    }
}