using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionCore
{
    public static class AppConfig
    {
        public const int GAP_VALUE = 1800;
        public const int VISITOR_ID_FILE_INDEX = 0;
        public const int SITE_FILE_INDEX = 1;
        public const int SITE_URL_FILE_INDEX = 2;
        public const int TIMESTAMP_INDEX = 3;
        public static readonly List<string> Startup_Files = new List<string>()
            {
                "input_1.csv",
                "input_2.csv",
                "input_3.csv"
            };
        public static string StartUpDataPath = @"..\..\..\..\SessionCore\StartupData\";
        public static string TestStartUpDataPath = @"..\..\..\..\..\SessionCore\StartupData\";
    }
}
