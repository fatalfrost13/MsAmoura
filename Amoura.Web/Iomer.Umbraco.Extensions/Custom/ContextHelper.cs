﻿using System;
using System.Collections.Generic;
using Umbraco.Web;
using System.Web.Hosting;
using Umbraco.Core;
using System.Web;
using Umbraco.Web.Security;
using System.IO;

using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web.Routing;

public static class ContextHelper
{
    public static void EnsureUmbracoContext()
    {
        try
        {
            if (UmbracoContext.Current == null)
            {
                var nodeUrl = "/";
                var dummyHttpContext = new HttpContextWrapper(new HttpContext(new SimpleWorkerRequest(nodeUrl, "", new StringWriter())));
                var webSecurity = new WebSecurity(dummyHttpContext, ApplicationContext.Current);
                var umbracoSettings = UmbracoConfig.For.UmbracoSettings();
                UmbracoContext.EnsureContext(
                    dummyHttpContext,
                    ApplicationContext.Current,
                    webSecurity);
            }

        }
        catch (Exception ex)
        {
            LogHelper.Error<IPublishedContent>($"Iomer caught error on ContextHelper.EnsureUmbracoContext().", ex);
            throw;
        }
    }

    public static bool IsCrawler()
    {
        bool isCrawler;
        try
        {
            var crawlerList = new List<string>()
            {
                "bot","crawler","spider","80legs","baidu","yahoo! slurp","ia_archiver","mediapartners-google",
                "lwp-trivial","nederland.zoek","ahoy","anthill","appie","arale","araneo","ariadne",
                "atn_worldwide","atomz","bjaaland","ukonline","calif","combine","cosmos","cusco",
                "cyberspyder","digger","grabber","downloadexpress","ecollector","ebiness","esculapio",
                "esther","felix ide","hamahakki","kit-fireball","fouineur","freecrawl","desertrealm",
                "gcreep","golem","griffon","gromit","gulliver","gulper","whowhere","havindex","hotwired",
                "htdig","ingrid","informant","inspectorwww","iron33","teoma","ask jeeves","jeeves",
                "image.kapsi.net","kdd-explorer","label-grabber","larbin","linkidator","linkwalker",
                "lockon","marvin","mattie","mediafox","merzscope","nec-meshexplorer","udmsearch","moget",
                "motor","muncher","muninn","muscatferret","mwdsearch","sharp-info-agent","webmechanic",
                "netscoop","newscan-online","objectssearch","orbsearch","packrat","pageboy","parasite",
                "patric","pegasus","phpdig","piltdownman","pimptrain","plumtreewebaccessor","getterrobo-plus",
                "raven","roadrunner","robbie","robocrawl","robofox","webbandit","scooter","search-au",
                "searchprocess","senrigan","shagseeker","site valet","skymob","slurp","snooper","speedy",
                "curl_image_client","suke","www.sygol.com","tach_bw","templeton","titin","topiclink","udmsearch",
                "urlck","valkyrie libwww-perl","verticrawl","victoria","webscout","voyager","crawlpaper",
                "webcatcher","t-h-u-n-d-e-r-s-t-o-n-e","webmoose","pagesinventory","webquest","webreaper",
                "webwalker","winona","occam","robi","fdse","jobo","rhcs","gazz","dwcp","yeti","fido","wlm",
                "wolp","wwwc","xget","legs","curl","webs","wget","sift","cmc"
            };
            var ua = HttpContext.Current.Request.UserAgent.ToLower();
            isCrawler = crawlerList.Exists(x => ua.Contains(x));
        }
        catch (Exception ex)
        {
            LogHelper.Error<bool>($"Iomer caught error on ContextHelper.IsCrawler().", ex);
            throw;
        }
        return isCrawler;
    }
}