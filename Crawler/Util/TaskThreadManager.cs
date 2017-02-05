using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TorrentHunter.THCrawler.Util
{
    /// <summary>
    /// A thread manager implementation that use TPL tasks to handle concurrency
    /// </summary>
    public class TaskThreadManager : ThreadManager
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        
        public TaskThreadManager(int maxThreads)
            : this(maxThreads, null)
        {

        }

        public TaskThreadManager(int maxThreads, CancellationTokenSource cancellationTokenSource)
            : base(maxThreads)
        {
            _cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
        }

        public override void AbortAll()
        {
            base.AbortAll();
            _cancellationTokenSource.Cancel();
        }

        public override void Dispose()
        {
            base.Dispose();
            if (!_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();
        }

        protected override void RunActionOnDedicatedThread(Action action)
        {
            Task.Run(() => RunAction(action), _cancellationTokenSource.Token);
        }
    }
}
