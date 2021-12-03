using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApiIPFiltering.Filters
{
    public class IpControlAttribute : ActionFilterAttribute
    {
        IConfiguration _configuration;
        public IpControlAttribute(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Client'ın IP adresini alıyoruz.
            IPAddress remoteIp = context.HttpContext.Connection.RemoteIpAddress;
            //Whitelist'te ki tüm IP'leri çekiyoruz.
            var ips = _configuration.GetSection("WhiteList").AsEnumerable().Where(ip => !string.IsNullOrEmpty(ip.Value)).Select(ip => ip.Value).ToList();

            //Client IP, whitelist'te var mı kontrol ediyoruz.
            if (!ips.Where(ip => IPAddress.Parse(ip).Equals(remoteIp)).Any())
            {
                //Eğer yoksa 403 hatası veriyoruz.
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
