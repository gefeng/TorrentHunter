using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentHunter.THCrawler.Context
{
    public class PageToCrawl
    {
        public PageToCrawl(Uri uri)
        {
            if(uri == null)
                throw new ArgumentNullException("uri");

            Uri = uri;
        }

        /// <summary>
        /// The uri of the page
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// The parent uri of the page
        /// </summary>
        public Uri ParentUri { get; set; }

        /// <summary>
        /// Whether the page is root page
        /// </summary>
        public bool IsRoot { get; set; }

        /// <summary>
        /// whether the page is internal to the root uri
        /// </summary>
        public bool IsInternal { get; set; }

        /// <summary>
        /// The depth from the root page of the crawl (Homepage's crawl depth is 0, page on homepage is 1...)
        /// </summary>
        public int CrawlDepth { get; set; }

        public override string ToString()
        {
            return Uri.AbsoluteUri;
        }
    }
}
