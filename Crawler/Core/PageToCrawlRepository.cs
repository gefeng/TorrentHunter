using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using TorrentHunter.THCrawler.Context;

namespace TorrentHunter.THCrawler.Core
{
    public class PageToCrawlRepository : IPageToCrawlRepository
    {
        private ConcurrentQueue<PageToCrawl> _urlQueue;

        public PageToCrawlRepository()
        {
            _urlQueue = new ConcurrentQueue<PageToCrawl>();                   
        }

        #region IPageToCrawlRepository

        public void Add(PageToCrawl page)
        {
            _urlQueue.Enqueue(page);
        }

        public PageToCrawl GetNext()
        {
            PageToCrawl pageToCrawl;

            _urlQueue.TryDequeue(out pageToCrawl);

            return pageToCrawl;
        }

        public int Count()
        {
            return _urlQueue.Count;
        }

        public void Clear()
        {
            _urlQueue = new ConcurrentQueue<PageToCrawl>();
        }

        #endregion

        #region IDisposal

        public void Dispose()
        {
            _urlQueue = null;
        }

        #endregion
    }
}
