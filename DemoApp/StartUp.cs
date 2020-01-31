using SIS.HTTP;
using SIS.HTTP.Response;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DemoApp
{
    public class StartUp : IMvcApplication
    {

        //var routeTable = new List<Route>();

        //var httpServer = new HttpServer(80, routeTable);
        //await httpServer.StartAsync();
        public void Configure(IList<Route> routeTable)
        {
            routeTable.Add(new Route(HttpMethodType.Get, "/", Index));
            routeTable.Add(new Route(HttpMethodType.Post, "/Tweets/Create", CreateTweet));
            routeTable.Add(new Route(HttpMethodType.Get, "/favicon.ico", FavIcon));

        }

        public void ConfigureServices()
        {
            var db = new ApplicationDbContext();
            db.Database.EnsureCreated();
        }

        public static HttpResponse Index(HttpRequest request)
        {
            var username = request.SessionData.ContainsKey("Username") ? request.SessionData["Username"] : "Anonymous";

            var db = new ApplicationDbContext();

            var tweets = db.Tweets.Select(x => new

            {
                x.CreatedOn,
                x.Creator,
                x.Content,
            }).ToList();

            var html = new StringBuilder();

            html.Append($"<table><tr><th>Date</th><th>Creator</th><th>Content</th></tr>");

            foreach (var tweet in tweets)
            {
                html.Append($"<tr><td>{tweet.CreatedOn}</td><td>{tweet.Creator}</td><td>{tweet.Content}</td></tr>");
            }

            html.Append("</table>");

            html.Append($"<form action='/Tweets/Create' method='post'><input name='creator' /><br /><textarea type='text' name='tweetName'></textarea><br /> <input type='submit' /> </form>");

            return new HtmlResponse(html.ToString());
        }

        public static HttpResponse CreateTweet(HttpRequest request)
        {
            var db = new ApplicationDbContext();
            db.Tweets.Add(new Tweet
            {
                CreatedOn = DateTime.UtcNow,
                Creator = request.FormData["creator"],
                Content = request.FormData["tweetName"]
            });

            db.SaveChanges();

            return new RedirectResponse("/");
            //return new RedirectResponse($"<a>Creator: {request.FormData["creator"]}</a> <br /> <a>Content: {request.FormData["tweetName"]}</a>");
        }

        private static HttpResponse FavIcon(HttpRequest request)
        {
            var byteContent = File.ReadAllBytes("wwwroot/favicon.ico");
            return new FileResponse(byteContent, "image/x-icon");
        }
    }
}
