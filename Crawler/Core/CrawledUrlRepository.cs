using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace TorrentHunter.THCrawler.Core
{
    public class CrawledUrlRepository : ICrawledUrlRepository
    {
        private ConcurrentDictionary<string, byte> _crawledUrlRepository;

        public CrawledUrlRepository()
        {
            _crawledUrlRepository = new ConcurrentDictionary<string, byte>();
        }
        
        #region ICrawledUrlRepository

        public bool Contains(Uri uri)
        {
            return _crawledUrlRepository.ContainsKey(uri.AbsoluteUri);
        }

        public bool AddIfNew(Uri uri)
        {
            return _crawledUrlRepository.TryAdd(uri.AbsoluteUri, 0);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _crawledUrlRepository = null;
        }

        #endregion
    }
}
