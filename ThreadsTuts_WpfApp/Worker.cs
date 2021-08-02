using System;
using System.Threading;
using System.Windows;

namespace ThreadsTuts_WpfApp
{
    public class Worker
    {
        private bool _cancelled = false;

        public void Cancel()
        {
            _cancelled = true;
        }

        public void Work()
        {
            for (int i = 1; i <= 100; i++)
            {
                if (_cancelled)
                    break;
                
                Thread.Sleep(50);

                ProcessChanged(i);
            }

            WorkCompleted(_cancelled);
        }

        public event Action<int> ProcessChanged;

        public event Action<bool> WorkCompleted;
    }
}