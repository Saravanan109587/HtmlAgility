using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HtmlAgility
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> linksToVisit = ParseLinks("https://www.facebook.com");
        }

        public static List<string> ParseLinks(string urlToCrawl)
        {

            WebClient webClient = new WebClient();

            byte[] data = webClient.DownloadData(urlToCrawl);
            string download = Encoding.ASCII.GetString(data);

            HashSet<string> list = new HashSet<string>();

            var doc = new HtmlDocument();
            
            doc.LoadHtml(download);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a[@href]");

            foreach (var n in nodes)
            {
                string href = n.Attributes["href"].Value;
                list.Add(GetAbsoluteUrlString(urlToCrawl, href));
            }
            return list.ToList();
        }

        static string GetAbsoluteUrlString(string baseUrl, string url)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri)
                uri = new Uri(new Uri(baseUrl), uri);
            return uri.ToString();
        }
    }
}
