using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentHunter.THCrawler.Context
{
    public class AvInfo
    {
        /// <summary>
        /// The uri of cover image
        /// </summary>
        public Uri CoverImg { get; set; }

        /// <summary>
        /// Video unique identification
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Video title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Video release date
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Video length
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Video director name
        /// </summary>
        public string Director { get; set; }

        /// <summary>
        /// Video maker
        /// </summary>
        public string Maker { get; set; }

        /// <summary>
        /// Video publisher
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Video labels
        /// </summary>
        public string Labels { get; set; }

        /// <summary>
        /// Actress name list
        /// </summary>
        public string[] Casters { get; set; }

        /// <summary>
        /// Video review
        /// </summary>
        public float Score { get; set; }
    }
}
