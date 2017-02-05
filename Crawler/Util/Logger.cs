using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentHunter.THCrawler.Util
{
    public class Logger
    {
        private static string filePath;

        private static object _locker = new object();

        static Logger()
        {
            filePath = Environment.CurrentDirectory + "\\log-" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".txt";
        }

        public static void Write(string data)
        {
            lock (_locker)
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("[" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + "]" + "    " + data + "\r\n");

                    sw.Write(sb.ToString());

                    sw.Close();
                }
            }
        }
    }
}
