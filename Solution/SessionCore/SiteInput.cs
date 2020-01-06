using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionCore
{
    internal class SiteInput
    {
        internal string Visitor_ID { get; set; }
        internal string Site_Url { get; set; }
        internal string Page_View_Url { get; set; }
        internal long Timestamp_1970 { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
