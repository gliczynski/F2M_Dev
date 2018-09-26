using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure
{
    public class UtilityExtension
    {
        public static bool DownloadImage(string urlPath, string imagePath)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(urlPath), imagePath);
                }
            }
            catch { }
            return false;
        }
    }
}
