using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentHunter.THCrawler.Context
{
    public class PageContent
    {
        public PageContent()
        {

        }

        /// <summary>
        /// The raw data bytes taken from the web response
        /// </summary>
        public byte[] Byte { get; set; }

        /// <summary>
        /// String representation of the charset/encoding
        /// </summary>
        public string Charset { get; set; }

        /// <summary>
        /// The encoding of the web response
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// The raw text taken from the web response
        /// </summary>
        public string Text { get; set; }
    }
}
