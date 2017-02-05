using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TorrentHunter.THCrawler.Context;

namespace TorrentHunter.THCrawler.Core
{
    public interface IScheduler : IDisposable 
    {
        /// <summary>
        /// Count of the remaining items that are currently scheduled
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Schedules the param to be crawled
        /// </summary>
        void Add(PageToCrawl page);

        /// <summary>
        /// Schedules the param to be crawled
        /// </summary>
        void Add(IEnumerable<PageToCrawl> pages);

        /// <summary>
        /// Gets the next page to crawl
        /// </summary>
        PageToCrawl GetNext();

        /// <summary>
        /// Clear all current scheduled pages
        /// </summary>
        void Clear();

        /// <summary>
        /// Add the url to the list of crawled url without scheduling it to be crawled
        /// </summary>
        void AddKnownUri(Uri uri);

        /// <summary>
        /// Whether the given uri was already scheduled to be crawled
        /// </summary>
        bool IsKnownUri(Uri uri);
    }
}
