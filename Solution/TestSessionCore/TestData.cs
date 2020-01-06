using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSessionCore
{
    public static class TestData
    {
        public static readonly List<SiteLengthMedian> SiteLengthMedianData = new List<SiteLengthMedian>()
        {
            new SiteLengthMedian { SiteUrl = "www.s_1.com",  NumSessions = 3684, MedianLength = 1353.0},
            new SiteLengthMedian { SiteUrl = "www.s_2.com",  NumSessions = 3632, MedianLength = 1341.5},
            new SiteLengthMedian { SiteUrl = "www.s_3.com",  NumSessions = 3640, MedianLength = 1392.5},
            new SiteLengthMedian { SiteUrl = "www.s_4.com",  NumSessions = 3584, MedianLength = 1361.0},
            new SiteLengthMedian { SiteUrl = "www.s_5.com",  NumSessions = 3623, MedianLength = 1375.0},
            new SiteLengthMedian { SiteUrl = "www.s_6.com",  NumSessions = 3712, MedianLength = 1374.0},
            new SiteLengthMedian { SiteUrl = "www.s_7.com",  NumSessions = 3598, MedianLength = 1318.5},
            new SiteLengthMedian { SiteUrl = "www.s_8.com",  NumSessions = 3683, MedianLength = 1353.0},
            new SiteLengthMedian { SiteUrl = "www.s_9.com",  NumSessions = 3730, MedianLength = 1326.5},
            new SiteLengthMedian { SiteUrl = "www.s_10.com", NumSessions = 3674, MedianLength = 1329.0},
        };

        public static readonly List<UniqyeSiteVisitor> UniqyeSiteVisitorData = new List<UniqyeSiteVisitor>()
        {
            new UniqyeSiteVisitor { Visitor = "visitor_1", UniqeAmount = 3},
            new UniqyeSiteVisitor { Visitor = "visitor_2", UniqeAmount = 2},
            new UniqyeSiteVisitor { Visitor = "visitor_3", UniqeAmount = 2},
            new UniqyeSiteVisitor { Visitor = "visitor_4", UniqeAmount = 4},
            new UniqyeSiteVisitor { Visitor = "visitor_5", UniqeAmount = 4},
            new UniqyeSiteVisitor { Visitor = "visitor_6", UniqeAmount = 1},
            new UniqyeSiteVisitor { Visitor = "visitor_7", UniqeAmount = 3},
            new UniqyeSiteVisitor { Visitor = "visitor_8", UniqeAmount = 3},
            new UniqyeSiteVisitor { Visitor = "visitor_9", UniqeAmount = 2},
            new UniqyeSiteVisitor { Visitor = "visitor_10", UniqeAmount = 4}
        };

    }
    public class SiteLengthMedian
    {
        public string SiteUrl { get; set; }
        public int NumSessions { get; set; }
        public double MedianLength { get; set; }
    }

    public class UniqyeSiteVisitor
    {
        public string Visitor { get; set; }
        public int UniqeAmount { get; set; }
    }

}
