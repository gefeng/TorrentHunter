using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentHunter.THCrawler.Util
{
    public interface IThreadManager : IDisposable
    {
        /// <summary>
        /// Maximum number of threads to use
        /// </summary>
        int MaxThreads { get; set; }

        /// <summary>
        /// Whether there are running threads
        /// </summary>
        bool HasRunningThreads { get; }

        /// <summary>
        /// Perform action asynchronously on a separate thread
        /// </summary>
        void DoWork(Action action);

        /// <summary>
        /// Abort all running thread
        /// </summary>
        void AbortAll();
    }
}
