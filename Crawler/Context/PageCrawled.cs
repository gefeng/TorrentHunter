using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TorrentHunter.THCrawler.Context
{
    public class PageCrawled : PageToCrawl
    {
        public PageCrawled(Uri uri)
            : base(uri)
        {
            
        }

        /// <summary>
        /// The content of page request
        /// </summary>
        public PageContent Content { get; set; }

        /// <summary>
        /// Links parsed from page
        /// </summary>
        public IEnumerable<Uri> ParsedLinks { get; set; }

        /// <summary>
        /// A datetime of when http request starts
        /// </summary>
        public DateTime RequestStartedAt { get; set; }

        /// <summary>
        /// A datetime of when http request completes
        /// </summary>
        public DateTime RequestCompleteAt { get; set; }

        /// <summary>
        /// Collect any exception which is thrown during making http request 
        /// </summary>
        public WebException WebException { get; set; }
    }
}
