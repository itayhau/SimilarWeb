using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionCore
{
    internal class TimeLine
    {
        internal List<Session> Sessions { get; set; }
        internal int LastLeft { get; set; }
        internal string SiteUrl { get; set; }
        internal string Visitor_ID { get; set; }

        public TimeLine()
        {
            Sessions = new List<Session>();
            InitInsert();
        }

        public void InitInsert()
        {
            LastLeft = 0;
        }

        public Session this[int index]
        {
            get
            {
                return Sessions[index];
            }
            set
            {
                Sessions[index] = value;
            }
        }

        internal Session this[ListLocations location]
        {
            get
            {
                if (location == ListLocations.START)
                    return Sessions[0];
                else if (location == ListLocations.END)
                    return Sessions[Sessions.Count - 1];
                else
                    return null;
            }
            set
            {
                if (location == ListLocations.START)
                    Sessions[0] = value;
                else if (location == ListLocations.END)
                    Sessions[Sessions.Count - 1] = value;
            }
        }
        public enum Position
        {
            Between = 0,
            On = 1
        }
        private static Position BinarySearchIndexLeft(TimeLine timeline, int left, long search_me, out int node)
        {

            // 22
            // 8 10 15 30 34
            // 0 1  2  3  4
            // 8 10 15 30
            // 0 1 2 3
            int right = timeline.Sessions.Count - 1;
            int middle;
            while (true)
            {
                middle = left + (right - left) / 2;
                if (search_me < timeline.Sessions[left].Start)
                {
                    node = left - 1;
                    return Position.Between;
                }
                if (search_me >= timeline.Sessions[left].Start && search_me <= timeline.Sessions[left].Finish)
                {
                    node = left;
                    return Position.On;
                }
                if (search_me < timeline.Sessions[left + 1].Start)
                {
                    node = left;
                    return Position.Between;
                }
                if (search_me > timeline.Sessions[right].Finish)
                {
                    node = right;
                    return Position.Between;
                }
                if (search_me >= timeline.Sessions[right].Start && search_me <= timeline.Sessions[right].Finish)
                {
                    node = right;
                    return Position.On;
                }
                if (search_me > timeline.Sessions[right - 1].Finish)
                {
                    node = right - 1;
                    return Position.Between;
                }

                if (search_me >= timeline.Sessions[middle].Start && search_me <= timeline.Sessions[middle].Finish)
                {
                    node = middle;
                    return Position.On;
                }
                if (search_me > timeline.Sessions[middle - 1].Finish && search_me < timeline.Sessions[middle].Start)
                {
                    node = middle - 1;
                    return Position.Between;
                }

                if (search_me > timeline.Sessions[middle].Finish && search_me < timeline.Sessions[middle + 1].Start)
                {
                    node = middle;
                    return Position.Between;
                }


                if (search_me - AppConfig.GAP_VALUE > timeline.Sessions[middle].Start)
                    left = middle;
                else
                    right = middle;
            }

        }
        public static TimeLine operator +(TimeLine timeline, PageView pageview)
        {
            // New timeline or Append at end
            if (timeline.Sessions.Count == 0 || pageview.VisitedTime > timeline[ListLocations.END].Finish)
            {
                Session newSession = new Session();
                newSession = newSession + pageview;
                timeline.Sessions.Add(newSession);
                timeline.LastLeft = 0;
            }
            // Append at beggining
            else if (pageview.VisitedTime < timeline[ListLocations.START].Start)
            {
                Session newSession = new Session();
                newSession = newSession + pageview; // also handles MAX MIN
                timeline.Sessions.Insert(0, newSession);
                timeline.LastLeft = 0;
            }
            else if (timeline.Sessions.Count == 1)
            {
                // Merge 
                timeline[ListLocations.START] = timeline[ListLocations.START] + pageview; // also handles MAX MIN
                timeline.LastLeft = 0;
            }
            else if (pageview.VisitedTime > timeline[ListLocations.END].Start)
            {
                timeline[ListLocations.END] = timeline[ListLocations.END] + pageview;
                if (timeline[ListLocations.END].Min <= timeline.Sessions[timeline.Sessions.Count - 2].Finish)
                {
                    timeline.Sessions[timeline.Sessions.Count - 2].Pageviews.AddRange(timeline[ListLocations.END].Pageviews);
                    timeline.Sessions[timeline.Sessions.Count - 2].Finish = timeline[ListLocations.END].Finish;
                    timeline.Sessions[timeline.Sessions.Count - 2].Max = timeline[ListLocations.END].Max;
                    timeline.Sessions.RemoveAt(timeline.Sessions.Count - 1);
                }
                timeline.LastLeft = 0;

            }
            else if (pageview.VisitedTime < timeline[ListLocations.START].Finish)
            {
                timeline[ListLocations.START] = timeline[ListLocations.START] + pageview;
                if (timeline[1].Min <= timeline[0].Finish)
                {
                    timeline.Sessions[0].Pageviews.AddRange(timeline.Sessions[1].Pageviews);
                    timeline.Sessions[0].Finish = timeline.Sessions[1].Finish;
                    timeline.Sessions[0].Max = timeline.Sessions[1].Max;
                    timeline.Sessions.RemoveAt(1);
                }
                timeline.LastLeft = 0;
            }
            // where ?
            else
            {
                Position pos = BinarySearchIndexLeft(timeline, timeline.LastLeft, pageview.VisitedTime, out int newIndex);
                if (newIndex == -1)
                {
                    Console.WriteLine("Error....");
                }
                if (pos == Position.On)
                {
                    timeline[newIndex] = timeline[newIndex] + pageview;
                }
                else if (pos == Position.Between)
                {
                    Session newSession = new Session();
                    newSession = newSession + pageview;
                    timeline.Sessions.Insert(newIndex + 1, newSession);
                    newIndex++;
                }
                if (newIndex > 0)
                {
                    if (timeline[newIndex].Min <= timeline[newIndex - 1].Finish)
                    {
                        timeline.Sessions[newIndex - 1].Pageviews.AddRange(timeline.Sessions[newIndex].Pageviews);
                        timeline.Sessions[newIndex - 1].Finish = timeline.Sessions[newIndex].Finish;
                        timeline.Sessions[newIndex - 1].Max = timeline.Sessions[newIndex].Max;
                        timeline.Sessions.RemoveAt(newIndex);
                    }
                }
                if (newIndex < timeline.Sessions.Count - 1)
                {
                    if (timeline[newIndex].Max >= timeline[newIndex + 1].Start)
                    {
                        timeline.Sessions[newIndex].Pageviews.AddRange(timeline.Sessions[newIndex + 1].Pageviews);
                        timeline.Sessions[newIndex].Finish = timeline.Sessions[newIndex + 1].Finish;
                        timeline.Sessions[newIndex].Max = timeline.Sessions[newIndex + 1].Max;
                        timeline.Sessions.RemoveAt(newIndex + 1);
                    }
                }
                if (newIndex > 3)
                {
                    Console.WriteLine();
                }

                timeline.LastLeft = newIndex;
            }
            return timeline;
        }
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
