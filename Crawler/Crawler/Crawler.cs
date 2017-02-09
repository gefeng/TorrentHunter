using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Diagnostics;

using Timer = System.Timers.Timer;

using TorrentHunter.THCrawler.Context;
using TorrentHunter.THCrawler.Util;
using TorrentHunter.THCrawler.Core;

namespace TorrentHunter.THCrawler.Crawler
{
    public abstract class Crawler : ICrawler
    {
        protected CrawlResult _crawlResult;
        protected CrawlContext _crawlContext;
        protected IThreadManager _threadManager;
        protected IPageRequester _pageRequester;
        protected IScheduler _scheduler;
        protected IList<string> _filter;
        //protected IHTMLPageParser _htmlPageParser;
        protected bool _crawlComplete;
        protected Timer _timeoutTimer;

        protected Object _locker = new Object();

        public Crawler(CrawlConfiguration crawlConfiguration)
        {
            _crawlContext = new CrawlContext();
            _crawlContext.CrawlConfiguration = crawlConfiguration;

            _threadManager = new TaskThreadManager(crawlConfiguration.MaxConcurrentThreads);

            _scheduler = new Scheduler();

            _pageRequester = new PageRequester(crawlConfiguration);

            _filter = new List<string>();

            //_htmlPageParser = new HTMLPageParser();
        }

        #region ICrawl

        public virtual CrawlResult Crawl(Uri uri)
        {
            return this.Crawl(uri, null);
        }

        public virtual CrawlResult Crawl(Uri uri, CancellationTokenSource cancellationTokenSource)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");
            
            _crawlContext.RootUri = uri;

            if (cancellationTokenSource != null)
                _crawlContext.CancellationTokenSource = cancellationTokenSource;

            _crawlResult = new CrawlResult();
            _crawlResult.RootUri = _crawlContext.RootUri;
            _crawlResult.CrawlContext = _crawlContext;
            _crawlComplete = false;

            _crawlContext.CrawlStartDate = DateTime.Now;
            Stopwatch timer = Stopwatch.StartNew();

            if(_crawlContext.CrawlConfiguration.CrawlTimeoutSeconds > 0)
            {
                _timeoutTimer = new Timer(_crawlContext.CrawlConfiguration.CrawlTimeoutSeconds * 1000);
                _timeoutTimer.Elapsed += HandleCrawlTimeout;
                _timeoutTimer.Start();
            }

            try
            {
                PageToCrawl rootPage = new PageToCrawl(uri) { ParentUri = uri, IsInternal = true, IsRoot = true, CrawlDepth = 0};
                _scheduler.Add(rootPage);
                    
                CrawlSite();
            }
            catch (Exception e)
            {
                _crawlResult.ErrorException = e;
            }
            finally
            {
                if (_threadManager != null)
                    _threadManager.Dispose();
            }

            /*if (_timeoutTimer != null)
                _timeoutTimer.Stop();*/

            timer.Stop();
            _crawlResult.Elapsed = timer.Elapsed;

            return _crawlResult;
        }

        #endregion

        #region IDisposable

        public virtual void Dispose()
        {
            if (_threadManager != null)
                _threadManager.Dispose();

            if (_scheduler != null)
                _scheduler.Dispose();

            if (_pageRequester != null)
                _pageRequester.Dispose();
        }

        #endregion

        public virtual void HandleCrawlTimeout(object sender, ElapsedEventArgs args)
        {
            Timer elapsedTimer = sender as Timer;
            if (elapsedTimer != null)
                elapsedTimer.Stop();

            _crawlContext.IsCrawlHardStopRequested = true;
        }

        protected virtual void CrawlSite()
        {
            Logger.Write("Start to crawl the site");

            // main loop
            while (!_crawlComplete)
            {
                if (_scheduler.Count > 0)
                {
                    _threadManager.DoWork(() => ProcessPage(_scheduler.GetNext()));
                }
                else if(!_threadManager.HasRunningThreads)
                {
                    _crawlComplete = true;
                    Logger.Write("Crawl Complete");
                }
                else
                {
                    // Waiting for links to be crawled...
                    Logger.Write("Waiting for links to be crawled");
                    Thread.Sleep(3000);
                }
            }
        }

        /// <summary>
        /// Async thread function to crawl the page
        /// </summary>
        protected virtual void ProcessPage(PageToCrawl page)
        {
            if (page == null)
                return;

            lock (_locker)
            {
                _crawlContext.CrawledCounter++;
            }

            try
            {
                Logger.Write(string.Format("Start to crawl: [{0}]", page.Uri));

                PageCrawled pageCrawled = CrawlThePage(page);

                Logger.Write(string.Format("Finish to crawl: [{0}], Depth: [{1}]", page.Uri, page.CrawlDepth));

                ProcessPageContent(pageCrawled);
            }
            catch (Exception e)
            {
                _crawlResult.ErrorException = e;
                _crawlContext.IsCrawlHardStopRequested = true;
            }
        }

        protected virtual PageCrawled CrawlThePage(PageToCrawl page)
        {
            PageCrawled pageCrawled = _pageRequester.MakeRequest(page.Uri);

            // Add log here...
            Map(page, pageCrawled);

            return pageCrawled;
        }

        protected virtual void ProcessPageContent(PageCrawled page)
        {
            if (page.Content != null)
                ParsePageLinks(page);

            if (page.ParsedLinks != null)
                SchedulePageLinks(page);
        }

        protected void Map(PageToCrawl src, PageCrawled dest)
        {
            dest.Uri = src.Uri;
            dest.ParentUri = src.Uri;
            dest.IsRoot = src.IsRoot;
            dest.IsInternal = src.IsInternal;
            dest.CrawlDepth = src.CrawlDepth;
        }

        protected virtual void ParsePageLinks(PageCrawled page)
        {
            if (page.Content.Text == null)
                return;

            HTMLPageParser parser = new HTMLPageParser(page.Content.Text);

            IEnumerable<string> links = parser.ExtractHyperLinks();

            FilterUri(links, page);
        }

        protected virtual void FilterUri(IEnumerable<string> links, PageCrawled page)
        {
            IList<Uri> filteredLinks = new List<Uri>();

            foreach (string link in links)
            {
                Uri uri = HTMLPageParser.MakeAbsoluteUri(link, page);

                if (HTMLPageParser.IsInternalLink(uri, page.Uri))
                {
                    // Only schedule links which contain one of the key words in filter list
                    if (_filter != null && _filter.Count > 0)
                    {
                        foreach(string keyWord in _filter)
                        {
                            if (uri.AbsoluteUri.Contains(keyWord))
                            {
                                filteredLinks.Add(uri);
                            }
                        }
                    }
                    else
                    {
                        filteredLinks.Add(uri);
                    }                
                }
            }

            page.ParsedLinks = (IEnumerable<Uri>)filteredLinks;
        }

        protected virtual void SchedulePageLinks(PageCrawled page)
        {
            lock (_locker)
            {
                foreach (Uri uri in page.ParsedLinks)
                {
                    PageToCrawl pageToCrawl = new PageToCrawl(uri);

                    pageToCrawl.IsInternal = true;
                    pageToCrawl.CrawlDepth = page.CrawlDepth + 1;
                    pageToCrawl.ParentUri = page.Uri;
                    pageToCrawl.IsRoot = Uri.Compare(uri, page.Uri, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0 ? true : false;

                    if(pageToCrawl.CrawlDepth <= _crawlContext.CrawlConfiguration.MaxCrawlDepth)
                        _scheduler.Add(pageToCrawl);
                }
            }
        }
    }
}
