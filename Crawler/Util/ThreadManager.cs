using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TorrentHunter.THCrawler.Util
{
    public abstract class ThreadManager : IThreadManager
    {
        protected static readonly int MAX_NUMBER_OF_THREADS = 10000;
        protected static readonly int MIN_NUMBER_OF_THREADS = 1;
        
        protected int _numberOfRunningThreads = 0;
        protected ManualResetEvent _hasFreeThreads = new ManualResetEvent(true);
        protected object _locker = new Object();
        protected bool _abortAllCalled = false;
        protected bool _isDisposed = false;

        public ThreadManager(int maxThreads)
        {
            if (maxThreads > MAX_NUMBER_OF_THREADS || maxThreads < MIN_NUMBER_OF_THREADS)
                throw new ArgumentOutOfRangeException(string.Format("MaxThreads must be from {0} to {1}", MIN_NUMBER_OF_THREADS, MAX_NUMBER_OF_THREADS));

            MaxThreads = maxThreads;
        }

        #region IThreadManager

        public int MaxThreads { get; set; }

        public virtual bool HasRunningThreads
        {
            get 
            { 
                return (_numberOfRunningThreads > 0);
            }
        }

        public virtual void DoWork(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (_abortAllCalled)
                throw new InvalidOperationException("Cannot call DoWork() after AbortAll() or Dispose() get called");

            if(MaxThreads > 1)
            {
                // Block until there is a free thread.
                _hasFreeThreads.WaitOne();

                lock (_locker)
                {
                    _numberOfRunningThreads++;
                    Logger.Write($"Running thread count plus 1: [{_numberOfRunningThreads}] Current Thread: [{System.Threading.Thread.CurrentThread.Name}]");

                    if (_numberOfRunningThreads >= MaxThreads)
                        _hasFreeThreads.Reset();
                }

                RunActionOnDedicatedThread(action);
            }
            else
            {
                RunAction(action);
            }
        }
        
        public virtual void AbortAll()
        {
            _abortAllCalled = true;
            _numberOfRunningThreads = 0;
        }

        #endregion

        #region IDisposal

        public virtual void Dispose()
        {
            AbortAll();
            _hasFreeThreads.Dispose();
            _isDisposed = true;
        }

        #endregion

        /// <summary>
        /// Run action on current thread
        /// </summary>
        /// <param name="action"></param>
        protected virtual void RunAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch(OperationCanceledException oce)
            {
                // log
                Logger.Write($"RunAction(Action action) -> OperationCanceledException : [{oce.ToString()}]");
            }
            catch(Exception e)
            {
                // log
                Logger.Write($"RunAction(Action action) -> Exception: [{e.ToString()}]");
            }
            finally
            {
                lock(_locker)
                {
                    _numberOfRunningThreads--;
                    Logger.Write($"Running thread count minus 1: [{_numberOfRunningThreads}]");
                    _hasFreeThreads.Set();
                }
            }
        }

        /// <summary>
        /// Run action on a separate thread
        /// </summary>
        /// <param name="action"></param>
        protected abstract void RunActionOnDedicatedThread(Action action);
    }
}
