using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HocCatToc.Models;
using Newtonsoft.Json;
using PagedList;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using System.Threading.Tasks;
namespace HocCatToc.Controllers
{
    public class AdminController : Controller
    {
        private hoccattocEntities db = new hoccattocEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Customer(string k, int? page)
        {
            if (Config.getCookie("is_admin") != "1") return RedirectToAction("Login", "Admin", new { message = "Bạn không được cấp quyền truy cập chức năng này" });
            if (k == null) k = "";

            var ctm = db.customers;
            var pageNumber = page ?? 1;
            var onePage = ctm.Where(o => o.phone.Contains(k) || o.email.Contains(k) || o.name.Contains(k)).OrderByDescending(f => f.id).ToPagedList(pageNumber, 20);
            ViewBag.onePage = onePage;
            ViewBag.k = k;
            return View();

        }
        [HttpPost]
        public ActionResult LogOff()
        {
            Config.removeCookie("is_admin");
            Config.removeCookie("user_id");
            Config.removeCookie("user_name");
            Config.removeCookie("user_email");
            Config.removeCookie("user_phone");            
            return View();
        }
        [HttpPost]
        public ActionResult SubmitLogin(string phone, string pass)
        {
            MD5 md5Hash = MD5.Create();
            pass = Config.GetMd5Hash(md5Hash, pass);
            if (db.customers.Any(o => o.phone == phone && o.pass == pass && o.is_admin == 1))
            {
                var us = db.customers.Where(o => o.phone == phone && o.pass == pass).FirstOrDefault();
                Config.setCookie("is_admin", "1");
                Config.setCookie("user_id", us.id.ToString());
                Config.setCookie("user_name", us.name);
                Config.setCookie("user_email", us.email);
                Config.setCookie("user_phone", us.phone);
                return RedirectToAction("Customer");
            }
            else
            {
                    ViewBag.message = "Sai số điện thoại hoặc mật khẩu";
                    return RedirectToAction("Login", new { message = ViewBag.message });
                
            }
        }
        [HttpPost]
        public string confirmAdmin(long id)
        {
            try
            {
                db.Database.ExecuteSqlCommand("update customers set is_admin=1 where id=" + id);
                return "1";
            }
            catch
            {
                return "0";
            }
        }
        public ActionResult YouTube(string k, int? page)
        {
            if (Config.getCookie("is_admin") != "1") return RedirectToAction("Login", "Admin", new { message = "Bạn không được cấp quyền truy cập chức năng này" });
            if (k == null) k = "";

            var ctm = db.videos;
            var pageNumber = page ?? 1;
            var onePage = ctm.Where(o => o.name.Contains(k) || o.des.Contains(k)).OrderByDescending(f => f.id).ToPagedList(pageNumber, 20);
            ViewBag.onePage = onePage;
            ViewBag.k = k;
            return View();

        }
        public ActionResult AddNews()
        {
            return View();
        }
        [HttpPost]
        public string addUpdateCustomer(customer cp)
        {
            MD5 md5Hash = MD5.Create();
            cp.pass = Config.GetMd5Hash(md5Hash, cp.pass);
            cp.date_time = DateTime.Now;
            return DBContext.addUpdatecustomer(cp);
        }

        [HttpPost]
        public string deleteCustomer(int cpId)
        {
            return DBContext.deletecustomer(cpId);
        }
        public string getcatvideo(string keyword)
        {
            if (keyword == null) keyword = "";

            var p = (from q in db.cats orderby q.name ascending select new { label = q.name, value = q.id }).ToList().Distinct();
            return JsonConvert.SerializeObject(p);
        }
        public class result
        {
            public string link { get; set; }
            public string name { get; set; }
            public string code { get; set; }
            public string img { get; set; }
            public string des { get; set; }
            public string tags { get; set; }
            public string date { get; set; }
            public string view { get; set; }
        }
        [HttpPost]
        public async Task<string> getLink()
        {
            try
            {
                string rs = "";
                YouTubeService yt = new YouTubeService(new BaseClientService.Initializer() { ApiKey = " AIzaSyD3Hm85CbEv-9QFpw8ARzcQuaF4ZLYOtlY" });
                    try
                    {
                        var playlistItemsListRequest = yt.PlaylistItems.List("snippet");
                        playlistItemsListRequest.PlaylistId = "PLcajz0E4AOQzVJbDgsfZJBFKIO1mj0ObC";
                        playlistItemsListRequest.MaxResults = 50;
                        //playlistItemsListRequest.PageToken = nextPageToken;

                        // Retrieve the list of videos uploaded to the authenticated user's channel.
                        var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();

                        foreach (var playlistItem in playlistItemsListResponse.Items)
                        {
                            if (!db.videos.Any(o => o.code == playlistItem.Snippet.ResourceId.VideoId)) {
                                video vd = new video();
                                vd.code = playlistItem.Snippet.ResourceId.VideoId;
                                vd.create_date = playlistItem.Snippet.PublishedAt;
                                vd.date = playlistItem.Snippet.PublishedAt.ToString();
                                vd.des = playlistItem.Snippet.Description;
                                vd.img = playlistItem.Snippet.Thumbnails.Medium.Url;
                                vd.link = "https://youtu.be/" + playlistItem.Snippet.ResourceId.VideoId;
                                vd.name = playlistItem.Snippet.Title;
                                vd.update_date = playlistItem.Snippet.PublishedAt;
                                // Print information about each video.
                                //Console.WriteLine("{0} ({1})", playlistItem.Snippet.Title, playlistItem.Snippet.ResourceId.VideoId);
                                //var videoRequest = yt.Videos.List("snippet");
                                //videoRequest.Id = playlistItem.Snippet.ResourceId.VideoId;
                                //videoRequest.MaxResults = 1;
                                //var videoItemRequestResponse = await videoRequest.ExecuteAsync();
                                //// Get the videoID of the first video in the list
                                //var video = videoItemRequestResponse.Items[0];
                                //var duration = video.ContentDetails.Duration;
                                db.videos.Add(vd);
                                db.SaveChanges();
                            }
                            else
                            {
                                db.Database.ExecuteSqlCommand("update video set name=N'" + playlistItem.Snippet.Title + "', des=N'" + playlistItem.Snippet.Description + "' where code=N'" + playlistItem.Snippet.ResourceId.VideoId + "'");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //rs += ex.ToString();
                    }
                    return "1";
            }
            catch
            {
                return "0";
            }
            //Json("", JsonRequestBehavior.AllowGet);
        }
        //public List<YouTubePlayList> GetUserPlayLists(string userEmail)
        //{
        //    List<YouTubePlayList> playLists = new List<YouTubePlayList>();

        //    try
        //    {
        //        YouTubeServiceClient service = new YouTubeServiceClient();
        //        service.GetUserPlayListsAsync(userEmail, playLists).Wait();
        //    }
        //    catch (AggregateException ex)
        //    {
        //        foreach (var e in ex.InnerExceptions)
        //        {
        //            //TODO: Add Logging
        //        }
        //    }

        //    return playLists;
        //}
        public string Search(String data, string key, int i, string stop)
        {
            string source = data;
            var x = source.IndexOf(key);
            string extract = source.Substring(source.IndexOf(key) + i);
            string result = extract.Substring(0, extract.IndexOf(stop));
            return result;
        }

        public string DeleteHtmlTags(string value)
        {
            var str1 = Regex.Replace(value, @"<[^>]+|&nbsp;", "").Trim();
            var str2 = Regex.Replace(str1, @"\s{2,}", " ");
            return str2;
        }
        [HttpPost]
        public string generateCode(long user_id)
        {
            db.Database.ExecuteSqlCommand("delete from  customer_code where customer_id=" + user_id);
            var p = (from q in db.videos select q).ToList();
            for (int i = 0; i < p.Count; i++)
            {
                customer_code cc = new customer_code();
                cc.code = (int)user_id * 100 + i * 10 + DateTime.Now.Day;
                cc.customer_id = user_id;
                cc.video_id = p[i].id;
                db.customer_code.Add(cc);
                db.SaveChanges();
            }
            return "1";
        }
        public ActionResult Login(string message)
        {
            ViewBag.message = message;
            return View();
        }
    }
}