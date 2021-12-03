using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApiIPFiltering.Middlewares
{
 
    public class IPControlMiddleware
    {
        readonly RequestDelegate _next;
        IConfiguration _configuration;
        public IPControlMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _configuration = configuration;
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
           
           

            //Client'ın IP adresini alıyoruz.
            IPAddress remoteIp = context.Connection.RemoteIpAddress;
            //Whitelist'te ki tüm IP'leri çekiyoruz.
            var ips = _configuration.GetSection("WhiteList").AsEnumerable().Where(ip => !string.IsNullOrEmpty(ip.Value)).Select(ip => ip.Value).ToList();

            //Client IP, whitelist'te var mı kontrol ediyoruz.
            if (!ips.Where(ip => IPAddress.Parse(ip).Equals(remoteIp)).Any())
            {
                //Eğer yoksa 403 hatası veriyoruz.
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Bu IP'nin erişim yetkisi yoktur.");
                return;
            }

            await _next.Invoke(context);
        }
    }
}
