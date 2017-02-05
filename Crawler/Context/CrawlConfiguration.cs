using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentHunter.THCrawler.Context
{
    public class CrawlConfiguration
    {
        public CrawlConfiguration()
        {
            MaxConcurrentThreads = 10;
            MaxPageToCrawl = 1000;
            MaxCrawlDepth = 1;
            IsExternalPageCrawlingEnabled = false;
            HttpServicePointConnectionLimit = 1000;
            HttpRequestTimeoutInSeconds = 15;
            CrawlTimeoutSeconds = 15;
            MaxRetryCount = 0;
            MinRetryDelayInMilliseconds = 10;
        }

        /// <summary>
        /// Maximum concurrent threads to use for http requests
        /// </summary>
        public int MaxConcurrentThreads { get; set; }

        /// <summary>
        /// Maximum number of pages to crawl
        /// </summary>
        public int MaxPageToCrawl { get; set; }

        /// <summary>
        /// Maximum levels below the root page to crawl. 0 means only homepage will be crawled
        /// </summary>
        public int MaxCrawlDepth { get; set; }

        /// <summary>
        /// Whether page external to the root uri should have their links crawled
        /// </summary>
        public bool IsExternalPageCrawlingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of concurrent connections allowed by System.Net.ServicePoint
        /// </summary>
        public int HttpServicePointConnectionLimit { get; set; }

        /// <summary>
        /// Gets or sets the timeout value in seconds for the System.Net.HttpWebRequest.GetResponse()
        /// </summary>
        public int HttpRequestTimeoutInSeconds { get; set; }

        /// <summary>
        /// Maximum seconds before the crawl times out and stop
        /// </summary>
        public int CrawlTimeoutSeconds { get; set; }

        /// <summary>
        /// The max number of retries for a url if a web exception is encountered
        /// </summary>
        public int MaxRetryCount { get; set; }

        /// <summary>
        /// The minimum delay between a failed http request and the next retry
        /// </summary>
        public int MinRetryDelayInMilliseconds { get; set; }
    }
}
