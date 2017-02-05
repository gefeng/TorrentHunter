using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TorrentHunter.THCrawler.Context;

namespace TorrentHunter.THCrawler.Core
{
    public interface IPageToCrawlRepository : IDisposable
    {
        /// <summary>
        /// Perform add operation on PageToCrawlRepository
        /// </summary>
        void Add(PageToCrawl page);

        /// <summary>
        /// Pop up the first page in the PageToCrawlRepository
        /// </summary>
        PageToCrawl GetNext();

        /// <summary>
        /// Return the number of pages in the PageToCrawlRepository
        /// </summary>
        int Count();

        /// <summary>
        /// Clear the repository
        /// </summary>
        void Clear();
    }
}
