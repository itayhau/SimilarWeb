using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionCore
{
    internal class Session
    {
        public List<PageView> Pageviews { get; set; }
        public long Start { get; set; }
        public long Finish { get; set; }
        public long Min { get; set; }
        public long Max { get; set; }

        public Session()
        {
            Pageviews = new List<PageView>();
            Start = long.MaxValue;
            Finish = long.MinValue;
        }

        public static Session operator +(Session session, PageView pageview)
        {
            session.Pageviews.Add(pageview);
            if (session.Pageviews.Count == 1)
            {
                session.Start = pageview.VisitedTime - AppConfig.GAP_VALUE;
                session.Finish = pageview.VisitedTime + AppConfig.GAP_VALUE;
                session.Min = pageview.VisitedTime;
                session.Max = pageview.VisitedTime;
            }
            else
            {
                session.Start = Math.Min(session.Start, pageview.VisitedTime - AppConfig.GAP_VALUE);
                session.Finish = Math.Max(session.Finish, pageview.VisitedTime + AppConfig.GAP_VALUE);
                session.Min = Math.Min(session.Min, pageview.VisitedTime);
                session.Max = Math.Max(session.Max, pageview.VisitedTime);
            }

            return session;
        }

        public long Length
        {
            get
            {
                return Finish - Start - (AppConfig.GAP_VALUE * 2);
            }
        }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

    }
}
