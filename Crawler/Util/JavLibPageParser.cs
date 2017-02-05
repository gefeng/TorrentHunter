using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;

using TorrentHunter.THCrawler.Context;

namespace TorrentHunter.THCrawler.Util
{
    public class JavLibPageParser : HTMLPageParser
    {
        //private static readonly string XpathContent = "//div[@id='content']/div[@id='rightcolumn']";
        private static readonly string XpathVideoCover = "//img[@id='video_jacket_img']";
        private static readonly string XpathTitle = "//div[@id='video_title']/h3/a";
        private static readonly string XpathVideoInfo = "//div[@id='video_info']";
        private static readonly string XpathVideoId = "./div[@id='video_id']";
        private static readonly string XpathVideoDate = "./div[@id='video_date']";
        private static readonly string XpathVideoLength = "./div[@id='video_length']";
        private static readonly string XpathVideoDirector = "./div[@id='video_director']";
        private static readonly string XpathVideoMaker = "./div[@id='video_maker']";
        private static readonly string XpathVideoLabel = "./div[@id='video_label']";
        private static readonly string XpathVideoReview = "./div[@id='video_review']";
        private static readonly string XpathVideoGenres = "./div[@id='video_genres']";
        private static readonly string XpathVideoCast = "./div[@id='video_cast']";

        private static readonly string XpathTdData = ".//td[@class='text']";
        private static readonly string XpathSpanData = ".//span[@class='text']";
        private static readonly string XpathSpanScore = ".//span[@class='score']";
        private static readonly string XpathAData = ".//a";

        public JavLibPageParser(string htmlString)
            : base(htmlString)
        {

        }
        
        public AvInfo GetVideoInfo()
        {
            if (_doc == null)
                throw new ArgumentNullException("HtmlDocument");

            AvInfo info = new AvInfo();

            // <div id="content">
            //  <div id="rightcolumn">
            //HtmlNode content = _doc.DocumentNode.SelectSingleNode(XpathContent);

            // Video cover image
            HtmlNode videoCover = _doc.DocumentNode.SelectSingleNode(XpathVideoCover);
            info.CoverImg = new Uri(videoCover.GetAttributeValue("src", null));

            // Title
            info.Title = _doc.DocumentNode.SelectSingleNode(XpathTitle).InnerText;

            // <div id="video_info">
            HtmlNode videoInfo = _doc.DocumentNode.SelectSingleNode(XpathVideoInfo);

            // <div id="video_id">
            HtmlNode videoId = videoInfo.SelectSingleNode(XpathVideoId);
            info.Id = videoId.SelectSingleNode(XpathTdData).InnerText;

            // <div id="video_date">
            HtmlNode videoDate = videoInfo.SelectSingleNode(XpathVideoDate);
            info.ReleaseDate = DateTime.ParseExact(videoDate.SelectSingleNode(XpathTdData).InnerText, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // <div id="video_length">
            HtmlNode videoLength = videoInfo.SelectSingleNode(XpathVideoLength);
            info.Length = int.Parse(videoLength.SelectSingleNode(XpathSpanData).InnerText);

            // <div id="video_director">
            HtmlNode videoDirector = videoInfo.SelectSingleNode(XpathVideoDirector);
            info.Director = videoDirector.SelectSingleNode(XpathTdData).InnerText;

            // <div id="video_maker">
            HtmlNode videoMaker = videoInfo.SelectSingleNode(XpathVideoMaker);
            info.Maker = videoMaker.SelectSingleNode(XpathAData).InnerText;

            // <div id="video_label">
            HtmlNode videoLabel = videoInfo.SelectSingleNode(XpathVideoLabel);
            info.Publisher = videoLabel.SelectSingleNode(XpathAData).InnerText;

            // <div id="video_review">
            HtmlNode videoReview = videoInfo.SelectSingleNode(XpathVideoReview);
            string score = videoReview.SelectSingleNode(XpathSpanScore).InnerText;
            if (string.IsNullOrEmpty(score))
            {
                info.Score = -1;
            }
            else
            {
                score = score.Replace("(", "").Replace(")", "");
                info.Score = float.Parse(score);
            }

            // <div id="video_genres">

            // <div id="video_cast">

            return info;
        }
    }
}
