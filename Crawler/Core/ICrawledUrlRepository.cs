using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentHunter.THCrawler.Core
{
    public interface ICrawledUrlRepository : IDisposable
    {
        /// <summary>
        /// Whether the uri exists in the repository
        /// </summary>
        bool Contains(Uri uri);

        /// <summary>
        /// Add uri to respository if it hasn't been added
        /// </summary>
        bool AddIfNew(Uri uri);
    }
}
