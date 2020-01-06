using SessionCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimilarWebApi.Controllers
{
    public class StatisticsController : ApiController
    {

        [HttpGet]
        [Route("api/Statistics/numsessions/{site_url}")]
        public IHttpActionResult GetNumSessions(string site_url)
        {
            site_url = site_url.Replace('!', '.');
            long result = SessionManager.GetNumSessions(site_url);
            return Ok(new { Result = result });
        }

        [HttpGet]
        [Route("api/Statistics/numunique/{visitor_id}")]
        public IHttpActionResult GetNumUnique(string visitor_id)
        {
            long result = SessionManager.GetNumUniqueVisitedSite(visitor_id);
            return Ok(new { Result = result });
        }

        [HttpGet]
        [Route("api/Statistics/getmedian/{site_url}")]
        public IHttpActionResult GetMedian(string site_url)
        {
            site_url = site_url.Replace('!', '.');
            double result = SessionManager.GetMedianSessionLength(site_url);
            return Ok(new { Result = result });
        }
    }
}
