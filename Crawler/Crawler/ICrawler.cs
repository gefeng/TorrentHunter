using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TorrentHunter.THCrawler.Context;

namespace TorrentHunter.THCrawler.Crawler
{
    public interface ICrawler : IDisposable
    {
        /// <summary>
        /// Begins a crawl with a specified uri
        /// </summary>
        CrawlResult Crawl(Uri uri);

        /// <summary>
        /// Begins a crawl with a specified uri and can be cancelled using the CancellationToken
        /// </summary>
        CrawlResult Crawl(Uri uri, CancellationTokenSource cancellationTokenSource);
    }
}