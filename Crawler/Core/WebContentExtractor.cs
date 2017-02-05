using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using TorrentHunter.THCrawler.Context;

namespace TorrentHunter.THCrawler.Core
{
    public class WebContentExtractor : IWebContentExtractor
    {
        public WebContentExtractor()
        {

        }
        
        #region IWebContentExtractor

        public PageContent GetContent(HttpWebResponse response)
        {
            using (MemoryStream rawData = GetRawContent(response))
            {
                string charset = GetCharsetFromHeaders(response);

                Encoding encoding = GetEncodingFromCharset(charset);

                string contentInString = string.Empty;

                // Reset position of the stream to begin after a copy
                rawData.Seek(0, SeekOrigin.Begin);

                using (StreamReader reader = new StreamReader(rawData, encoding))
                {
                    contentInString = reader.ReadToEnd();
                }

                PageContent pageContent = new PageContent();
                pageContent.Charset = charset;
                pageContent.Encoding = encoding;
                pageContent.Byte = rawData.ToArray();
                pageContent.Text = contentInString;

                return pageContent;
            }        
        }

        #endregion

        #region Dispose

        public void Dispose()
        {

        }

        #endregion

        protected MemoryStream GetRawContent(HttpWebResponse response)
        {
            MemoryStream rawData = new MemoryStream();

            try
            {
                using (Stream stream = response.GetResponseStream())
                {
                    stream.CopyTo(rawData);
                }
            }    
            catch
            {
                // Add log
            }

            return rawData;
        }

        protected string GetCharsetFromHeaders(HttpWebResponse response)
        {
            return response.CharacterSet;
        }

        protected Encoding GetEncodingFromCharset(string charset)
        {
            if (string.IsNullOrEmpty(charset))
                return Encoding.UTF8;
                
            return Encoding.GetEncoding(charset);
        }
    }
}
