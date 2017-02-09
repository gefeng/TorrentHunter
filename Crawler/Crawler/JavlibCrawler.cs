using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TorrentHunter.THCrawler.Context;
using TorrentHunter.THCrawler.Util;

namespace TorrentHunter.THCrawler.Crawler
{
    public class JavlibCrawler : Crawler, IJavlibCrawler
    {
        private IList<AvInfo> _avInfoList;

        public JavlibCrawler(CrawlConfiguration config)
            :base(config)
        {
            _avInfoList = new List<AvInfo>();
        }

        protected override void CrawlSite()
        {
            base.CrawlSite();

            // Download test
           System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "\\Cover");

            foreach (AvInfo av in _avInfoList)
            {
                Logger.Write(av.Id);

                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    client.DownloadFileAsync(av.CoverImg, di + "\\" + av.Id + ".jpg");
                }
            }
        }
        
        protected override void ProcessPageContent(PageCrawled page)
        {
            if (page.Content == null)
                return;

            // Could add video info page to "to be crawled list". Would speed up?
            if (IsVideoInfoPage(page.Uri))
            {
                Logger.Write("Video Info Uri: " + page.Uri.AbsoluteUri);
                ParseVideoInfo(page);
            }
            else
            {
                ParsePageLinks(page);

                if (page.ParsedLinks != null)
                    SchedulePageLinks(page);
            }
        }

        private bool IsVideoInfoPage(Uri uri)
        {
            return uri.AbsoluteUri.Contains("/?v=") ? true : false;
        }

        private void ParseVideoInfo(PageCrawled page)
        {
            if (string.IsNullOrEmpty(page.Content.Text))
                return;

            JavLibPageParser parser = new JavLibPageParser(page.Content.Text);

            AvInfo avInfo = parser.GetVideoInfo();

            Logger.Write($"ID: [{avInfo.Id}]; Cover: [{avInfo.CoverImg.AbsoluteUri}]");

            lock (_locker)
            {
                if (avInfo != null)
                    _avInfoList.Add(avInfo);
            }
        }

        #region IJavlibCrawler

        public CrawlResult CrawlNewReleased(Uri uri)
        {
            _filter.Add("/vl_newrelease");
            _filter.Add("/?v=");
            return base.Crawl(uri, null);
        }

        public CrawlResult CrawlNewUpdated(Uri uri)
        {
            _filter.Add("/vl_update");
            _filter.Add("/?v=");
            return base.Crawl(uri, null);
        }

        public CrawlResult CrawlNewAdded(Uri uri)
        {
            _filter.Add("/vl_newentries");
            _filter.Add("/?v=");
            return base.Crawl(uri, null);
        }

        public CrawlResult CrawlMostWanted(Uri uri)
        {
            _filter.Add("/vl_mostwanted");
            _filter.Add("/?v=");
            return base.Crawl(uri, null);
        }

        public CrawlResult CrawlBestRated(Uri uri)
        {
            _filter.Add("/vl_bestrated");
            _filter.Add("/?v=");
            return base.Crawl(uri, null);
        }

        #endregion
    }
}
