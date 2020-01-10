using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace HiSpaceWeb.Utilities
{
    public class SessionUtility
    {
        private readonly IHttpContextAccessor HttpContextAccessor;

        public SessionUtility(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public void SetSession(string key, string value)
        {
            HttpContextAccessor.HttpContext.Session.SetString(key, value);
        }

        public string GetSession(string key)
        {
            return HttpContextAccessor.HttpContext.Session.GetString(key);
        }
    }
}
