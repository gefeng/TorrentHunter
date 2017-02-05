using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using TorrentHunter.THCrawler.Context;

namespace TorrentHunter.THCrawler.Core
{
    public interface IWebContentExtractor : IDisposable
    {
        /// <summary>
        /// Retrievel content from a WebResponse
        /// </summary>
        PageContent GetContent(HttpWebResponse response);
    }
}
