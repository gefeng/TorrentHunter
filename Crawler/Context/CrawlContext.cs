using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TorrentHunter.THCrawler.Context
{
    public class CrawlContext
    {
        public CrawlContext()
        {
            CrawledCounter = 0;
        }

        /// <summary>
        /// The root of the crawl
        /// </summary>
        public Uri RootUri { get; set; }

        /// <summary>
        /// Total number of pages that have been crawled
        /// </summary>
        public int CrawledCounter { get; set; }

        /// <summary>
        /// The datetime when crawl begins
        /// </summary>
        public DateTime CrawlStartDate { get; set; }

        /// <summary>
        /// Whether a request to stop the crawl has happened. Will clear scheduled pages but allow any threads that are currently crawling to complete
        /// </summary>
        public bool IsCrawlStopRequested { get; set; }

        /// <summary>
        /// Whether a request to hard stop the crawl has happened. Will clear scheduled pages and cancel any threads that are currently crawling
        /// </summary>
        public bool IsCrawlHardStopRequested { get; set; }

        /// <summary>
        /// Cancellation token used to hard stop the crawl.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; set; }

        /// <summary>
        /// Configuration values used to determind crawl settings
        /// </summary>
        public CrawlConfiguration CrawlConfiguration { get; set; }
    }
}
