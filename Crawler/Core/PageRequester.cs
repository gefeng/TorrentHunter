using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using TorrentHunter.THCrawler.Context;

namespace TorrentHunter.THCrawler.Core
{
    public class PageRequester : IPageRequester
    {
        CrawlConfiguration _config;
        IWebContentExtractor _extractor;

        public PageRequester(CrawlConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            _config = config;

            if (_config.HttpServicePointConnectionLimit > 0)
                ServicePointManager.DefaultConnectionLimit = _config.HttpServicePointConnectionLimit;

            _extractor = new WebContentExtractor();
        }
        
        #region IPageRequest

        public PageCrawled MakeRequest(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            PageCrawled pageCrawled = new PageCrawled(uri);

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            
            try
            {
                request = BuildHttpRequest(uri);

                pageCrawled.RequestStartedAt = DateTime.Now;

                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException we)
            {
                pageCrawled.WebException = we;

                if (we.Response != null)
                    response = (HttpWebResponse)we.Response;
            }
            catch (Exception e)
            {
                // Keep silent...
                // Add log here...
            }
            finally
            {
                pageCrawled.RequestCompleteAt = DateTime.Now;

                try
                {
                    if (response != null)
                    {
                        pageCrawled.Content = _extractor.GetContent(response);

                        response.Close();
                    }
                }
                catch(Exception e)
                {
                    // Add log here...
                }
            }

            return pageCrawled;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_extractor != null)
                _extractor.Dispose();
        }

        #endregion

        public HttpWebRequest BuildHttpRequest(Uri uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            if (_config.HttpRequestTimeoutInSeconds > 0)
                request.Timeout = _config.HttpRequestTimeoutInSeconds * 1000;

            return request;
        }
    }
}
