using Find2Me.Infrastructure.DbModels;
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
        return true;
      }
      catch { }
      return false;
    }

    /// <summary>
    /// Key Value Pair for Years of Birth Dropdown List.
    /// Min Age 18 years and go back to 72 years.
    /// </summary>
    /// <returns></returns>
    public static List<KeyValuePair<string, int>> GetYearsList()
    {
      int currentYear = DateTime.UtcNow.Year;
      int yearToEnd = currentYear - 18; //Min Age 18
      int yearToStart = yearToEnd - 72; //Go 72 years back

      List<KeyValuePair<string, int>> yearsDict = new List<KeyValuePair<string, int>>();
      for (int i = yearToEnd; i >= yearToStart; i--)
      {
        yearsDict.Add(new KeyValuePair<string, int>(i.ToString(), i));
      }
      return yearsDict;
    }

    /// <summary>
    /// Key Value Pair for Languages To Select
    /// </summary>
    /// <returns></returns>
    public static List<KeyValuePair<string, string>> GetLanguagesList()
    {
      List<KeyValuePair<string, string>> languageDict = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Danish", "da"),
                new KeyValuePair<string, string>("English", "en"),
            };

      return languageDict;
    }

    /// <summary>
    /// Generate Random Number
    /// </summary>
    /// <returns></returns>
    public static int GenerateRandomNumer()
    {
      int _min = 1000;
      int _max = 9999;
      Random _rdm = new Random();
      return _rdm.Next(_min, _max);
    }
  }
}
