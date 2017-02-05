using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HtmlAgilityPack;

using TorrentHunter.THCrawler.Context;

namespace TorrentHunter.THCrawler.Util
{
    public class HTMLPageParser
    {
        //private static readonly string _hyperlinkRegex = "<a\\s+(?:[^>]*\\s+)?href\\s*=\\s*\"(?<Link>[^\"]+)\"";
        private static readonly string _currentDirRegex = "^./";
        private static readonly string _parentDirRegex = "^../";

        protected HtmlDocument _doc;

        public HTMLPageParser(string htmlString)
        {
            if (string.IsNullOrEmpty(htmlString))
                throw new ArgumentNullException(htmlString);

            _doc = new HtmlDocument();
            _doc.LoadHtml(htmlString);
        }

        public virtual IEnumerable<string> ExtractHyperLinks()
        {
            if (_doc == null)
                throw new ArgumentNullException("HtmlDocument");

            IList<string> links = new List<string>();

            foreach (HtmlNode link in _doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                links.Add(link.GetAttributeValue("href", null));
            }

            return (IEnumerable<string>)links;
        }

        /*public virtual void ExtractHyperLinks(PageCrawled page)
        {
            IList<Uri> links = new List<Uri>();

            MatchCollection matches = Regex.Matches(page.Content.Text, HTMLPageParser._hyperlinkRegex, RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;

                string link = groups["Link"].Value;

                Uri uri = MakeAbsoluteUri(link, page);

                if (IsInternalLink(uri, page.Uri))
                    links.Add(uri);
            }

            page.ParsedLinks = (IEnumerable<Uri>)links;
        }*/

        #region static methods

        public static Uri MakeAbsoluteUri(string uriString, PageCrawled page)
        {
            Uri uri;
            
            if (!Uri.TryCreate(uriString, UriKind.Absolute, out uri))
            {
                uri = new Uri(page.Uri, uriString);
            }

            return uri;
        }

        public static bool IsInternalLink(Uri uri, Uri parent)
        {
            int res = Uri.Compare(uri, parent, UriComponents.Host, UriFormat.SafeUnescaped, StringComparison.CurrentCulture);

            string host1 = uri.Host;
            string host2 = parent.Host;

            return (res == 0) ? true : false;
        }

        #endregion
    }
}
