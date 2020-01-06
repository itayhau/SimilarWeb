using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SessionCore;

namespace TestSessionCore
{
    [TestClass]
    public class Test
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            SessionManager.LoadStartupData(AppConfig.TestStartUpDataPath);
        }

        [TestMethod]
        public void Test_GetNumSessions()
        {
            foreach (var item in TestData.SiteLengthMedianData)
            {
                int result = SessionManager.GetNumSessions(item.SiteUrl);
                Assert.AreEqual(item.NumSessions, result);
            }
        }

        [TestMethod]
        public void Test_GetMedianSessionLength()
        {
            foreach (var item in TestData.SiteLengthMedianData)
            {
                double result = SessionManager.GetMedianSessionLength(item.SiteUrl);
                Assert.AreEqual(item.MedianLength, result);
            }
        }

        [TestMethod]
        public void Test_GetNumUniqueVisitedSite()
        {
            foreach (var item in TestData.UniqyeSiteVisitorData)
            {
                double result = SessionManager.GetNumUniqueVisitedSite(item.Visitor);
                Assert.AreEqual(item.UniqeAmount, result);
            }
        }



    }
}
