using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SessionCore
{
    public static class SessionManager
    {

        private static string GetStartupDataPath()
        {
            Console.WriteLine(Environment.CurrentDirectory);
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return path;
        }

        private static Dictionary<string, Dictionary<string, TimeLine>> userToSiteToTimeline = new Dictionary<string, Dictionary<string, TimeLine>>();
        private static Dictionary<string, Dictionary<string, TimeLine>> siteToUserToTimeline = new Dictionary<string, Dictionary<string, TimeLine>>();

        public static int GetNumSessions(string site_url)
        {

            var userToTimeline = siteToUserToTimeline[site_url];
            int counter = 0;
            foreach (var user in userToTimeline.Keys)
            {
                counter += userToTimeline[user].Sessions.Count;
            }

            return counter;
        }
        public static int GetNumUniqueVisitedSite(string visitor_id)
        {
            var siteToTimeline = userToSiteToTimeline[visitor_id];
            int counter = siteToTimeline.Count;

            return counter;
        }

        public static double GetMedianSessionLength(string site_url)
        {
            List<long> len = new List<long>();
            var userToTimeline = siteToUserToTimeline[site_url];
            foreach (var user in userToTimeline.Keys)
            {
                foreach (var item in userToTimeline[user].Sessions)
                {
                    len.Add(item.Length);
                }
            }
            len.Sort();
            return len.Count % 2 == 1 ? len[len.Count / 2] : (len[len.Count / 2 - 1] + len[len.Count / 2]) / 2.0;
        }
    
        public static void LoadStartupData(string pathRef)
        {
            string path = GetStartupDataPath();

            foreach (string file in AppConfig.Startup_Files)
            {
                using (StreamReader sr = new StreamReader(path + pathRef + file))
                {
                    string currentLine;
                    // currentLine will be null when the StreamReader reaches the end of file
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        // Console.WriteLine(currentLine);
                        string[] values = currentLine.Split(',');
                        SiteInput s = new SiteInput
                        {
                            Visitor_ID = values[AppConfig.VISITOR_ID_FILE_INDEX],
                            Site_Url = values[AppConfig.SITE_FILE_INDEX],
                            Page_View_Url = values[AppConfig.SITE_URL_FILE_INDEX],
                            Timestamp_1970 = Convert.ToInt64(values[AppConfig.TIMESTAMP_INDEX])

                        };

                        if (!userToSiteToTimeline.TryGetValue(s.Visitor_ID, out Dictionary<String, TimeLine> siteToTimeline))
                        {
                            siteToTimeline = new Dictionary<string, TimeLine>();
                            userToSiteToTimeline.Add(s.Visitor_ID, siteToTimeline);
                        }
                        if (!siteToTimeline.TryGetValue(s.Site_Url, out TimeLine timeline))
                        {
                            timeline = new TimeLine
                            {
                                SiteUrl = s.Site_Url,
                                Visitor_ID = s.Visitor_ID
                            };
                            siteToTimeline.Add(s.Site_Url, timeline);
                        }

                        PageView pageView = new PageView
                        {
                            Page_Url = s.Page_View_Url,
                            VisitedTime = s.Timestamp_1970
                        };

                        timeline = timeline + pageView;

                        if (!siteToUserToTimeline.TryGetValue(s.Site_Url, out Dictionary<String, TimeLine> userToTimeline))
                        {
                            userToTimeline = new Dictionary<string, TimeLine>();
                            siteToUserToTimeline.Add(s.Site_Url, userToTimeline);
                        }
                        if (!userToTimeline.TryGetValue(s.Visitor_ID, out TimeLine timelineSite))
                        {
                            userToTimeline.Add(s.Visitor_ID, timeline);
                        }

                    }
                }
            }


            

            
        }


    }
    
}
