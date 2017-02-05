using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TorrentHunter.THCrawler.Context;

namespace TorrentHunter.THCrawler.Core
{
    public class Scheduler : IScheduler
    {
        private IPageToCrawlRepository _pageToCrawlRepository;
        private ICrawledUrlRepository _crawledUrlRepository;

        public Scheduler()
        {
            _pageToCrawlRepository = new PageToCrawlRepository();
            _crawledUrlRepository = new CrawledUrlRepository();
        }

        #region IScheduler

        public int Count
        {
            get
            {
                return _pageToCrawlRepository.Count();
            }
        }

        public void Add(PageToCrawl page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            if(_crawledUrlRepository.AddIfNew(page.Uri))
                _pageToCrawlRepository.Add(page);
        }

        public void Add(IEnumerable<PageToCrawl> pages)
        {
            if (pages == null)
                throw new ArgumentNullException("pages");

            foreach(PageToCrawl page in pages)
            {
                Add(page);
            }
        }

        public PageToCrawl GetNext()
        {
            return _pageToCrawlRepository.GetNext();
        }

        public void Clear()
        {
            _pageToCrawlRepository.Clear();
        }

        public void AddKnownUri(Uri uri)
        {
            _crawledUrlRepository.AddIfNew(uri);
        }

        public bool IsKnownUri(Uri uri)
        {
            return _crawledUrlRepository.Contains(uri);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_pageToCrawlRepository != null)
                _pageToCrawlRepository.Dispose();

            if (_crawledUrlRepository != null)
                _crawledUrlRepository.Dispose();
        }

        #endregion
    }
}
