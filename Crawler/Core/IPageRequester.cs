using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TorrentHunter.THCrawler.Context;

namespace TorrentHunter.THCrawler.Core
{
    public interface IPageRequester : IDisposable
    {
        /// <summary>
        /// Make a HTTP request to url and download its content
        /// </summary>
        PageCrawled MakeRequest(Uri uri);
    }
}
