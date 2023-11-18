using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET;
using Microsoft.AspNetCore.Mvc;
using Project3.Data;
using Project3.Dtos;

namespace Project3.Shared
{
    public class BaseController : Controller
    {
        protected readonly MyDbContext _context;
    

        public BaseController(MyDbContext context )
        {
            _context=context;
          
        }

        public  DeviceDto GetIp()
        {
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
            var userAgent = Request.Headers["User-Agent"];
            var headers = Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
            var clientHints = ClientHints.Factory(headers);  // client hints are optional

            var dd = new DeviceDetector(userAgent, clientHints);

            // OPTIONAL: Set caching method
            // By default static cache is used, which works best within one php process (memory array caching)
            // To cache across requests use caching in files or memcache
            // add using DeviceDetectorNET.Cache;
            dd.SetCache(new DictionaryCache());

            // OPTIONAL: If called, GetBot() will only return true if a bot was detected  (speeds up detection a bit)
            dd.DiscardBotInformation();

            // OPTIONAL: If called, bot detection will completely be skipped (bots will be detected as regular devices then)
            dd.SkipBotDetection();

            dd.Parse();

            if (dd.IsBot())
            {
                return null;

            }
            else
            {
                return new DeviceDto
               
                {
                    Os = dd.GetOs().Match.Name,
                    Device = dd.GetDeviceName(),
                 

                };

            }



        }


    }
}
