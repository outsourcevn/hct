﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using HocCatToc.Models;
using System.Security.Cryptography;
using System.Data.Entity;
namespace HocCatToc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        private hoccattocEntities db = new hoccattocEntities();
        public string Api(string status, Dictionary<string, string> field, string message)
        {
            string data = "[{";
            for (int i = 0; i < field.Count; i++)
            {
                data += "\"" + field.ElementAt(i).Key + "\":\"" + field.ElementAt(i).Value + "\",";
            }
            if (data.EndsWith(",")) data = data.Substring(0, data.Length - 1);
            data += "}]";
            string temp = "{\"status\":\"" + status + "\",\"data\":" + data + ",\"message\":\"" + message + "\"}";
            return temp;
        }
        public string ApiArray(string status, Dictionary<string, string> field, string message)
        {
            string data = "[{";
            for (int i = 0; i < field.Count; i++)
            {
                if (field.ElementAt(i).Value != null && field.ElementAt(i).Value.Contains("{"))
                {
                    data += "\"" + field.ElementAt(i).Key + "\":" + field.ElementAt(i).Value + ",";
                }
                else
                {
                    if (field.ElementAt(i).Value != null)
                    {
                        data += "\"" + field.ElementAt(i).Key + "\":\"" + field.ElementAt(i).Value + "\",";
                    }
                    else
                    {
                        data += "\"" + field.ElementAt(i).Key + "\":\"\",";
                    }
                }
            }
            if (data.EndsWith(",")) data = data.Substring(0, data.Length - 1);
            data += "}]";
            string temp = "{\"status\":\"" + status + "\",\"data\":" + data + ",\"message\":\"" + message + "\"}";
            return temp;
        }
        public bool getKeyApi(double? key)
        {
            try
            {

                double keytime1 = (DateTime.Now.AddMinutes(-43200) - new DateTime(1970, 1, 1)).TotalMilliseconds;
                double keytime2 = (DateTime.Now.AddMinutes(43200) - new DateTime(1970, 1, 1)).TotalMilliseconds;
                keytime1 = keytime1 * 13 + 27;
                keytime2 = keytime2 * 13 + 27;
                if (keytime1 <= key && key <= keytime2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { return false; }

        }
        [HttpPost]
        public string register(string name, string email, string phone, string pass, long? user_id, double? key)
        {
            Dictionary<string, string> field = new Dictionary<string, string>();
            try
            {
                if (key == null || !getKeyApi(key))
                {
                    field.Add("TotalMilliseconds", (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds.ToString());
                    return Api("failed", field, "Bảo mật server, hàm api này không chạy được do ngày giờ ở máy bị sai số quá 10 phút hoặc chưa gửi key bảo mật!");
                }
                phone = phone.Trim();
                email = email.Trim();
                if (phone == "" || phone == null || pass == "" || pass == null)
                {
                    field.Add("user_id", "");
                    return Api("failed", field, "Số điện thoại hoặc pass phải khác rỗng!");
                }
                if ((user_id == 0 || user_id == null) && db.customers.Any(o => o.email == email || o.phone == phone))
                {
                    field.Add("user_id", "");
                    return Api("failed", field, "Đã tồn tại email hoặc số điện thoại này");
                }
                if (user_id == 0 || user_id == null)
                {
                    MD5 md5Hash = MD5.Create();
                    string hash = Config.GetMd5Hash(md5Hash, pass);
                    customer ct = new customer();
                    ct.email = email;
                    ct.name = name;
                    ct.pass = hash;
                    ct.phone = phone;
                    ct.date_time = DateTime.Now;
                    ct.points = 100;
                    db.customers.Add(ct);
                    db.SaveChanges();
                    //Cộng điểm cho người giới thiệu
                    //if (ref_phone != null && ref_phone != "")
                    //{
                    //    if (db.customers.Any(o => o.phone == ref_phone))
                    //    {
                    //        var bnu = db.customers.Where(o => o.phone == ref_phone).OrderBy(o => o.id).FirstOrDefault();
                    //        int? bnp = db.config_bonus_point.Find(1).ref_point;
                    //        db.Database.ExecuteSqlCommand("update customers set points=points+" + bnp + " where id=" + bnu.id);
                    //    }
                    //}
                    field.Add("user_id", ct.id.ToString());
                    return Api("success", field, "Đăng ký thành công!");
                }
                else
                {
                    MD5 md5Hash = MD5.Create();
                    string hash = Config.GetMd5Hash(md5Hash, pass);
                    customer ct = db.customers.Find((long)user_id);
                    db.Entry(ct).State = EntityState.Modified;
                    ct.email = email;
                    ct.name = name;
                    ct.pass = hash;
                    ct.phone = phone;
                    //ct.identify = identify;
                    //ct.address = address;
                    ct.date_time = DateTime.Now;
                    db.SaveChanges();
                    field.Add("user_id", user_id.ToString());
                    return Api("success", field, "Cập nhật thành công!");
                }
            }
            catch (Exception ex)
            {
                field.Add("user_id", "");
                return Api("error", field, "Cập nhật lỗi sql: " + ex.ToString());
            }
        }
        //Hàm này login bằng số phone và mật khẩu, gửi kèm key bảo mật
        //Trả về là user_id của user này nếu đăng nhập thành công, nếu không báo sai pass và mật khẩu kèm theo user_id là rỗng
        [HttpPost]
        public string login(string phone, string pass, double? key)
        {
            Dictionary<string, string> field = new Dictionary<string, string>();
            try
            {
                MD5 md5Hash = MD5.Create();
                string hash = Config.GetMd5Hash(md5Hash, pass);
                if (db.customers.Any(o => o.phone == phone && o.pass == hash))
                {
                    string user_id = db.customers.Where(o => o.phone == phone && o.pass == hash).FirstOrDefault().id.ToString();
                    field.Add("user_id", user_id);
                    return Api("success", field, "Đăng nhập thành công!");
                }
                else
                {
                    field.Add("user_id", "");
                    return Api("failed", field, "Đăng nhập không thành công, sai số điện thoại hoặc mật khẩu!");
                }
            }
            catch (Exception ex)
            {
                field.Add("user_id", "");
                return Api("error", field, "Lỗi sql: " + ex.ToString());
            }
        }
        //Hàm này trả về danh sách các voucher sắp xếp giảm dần theo id, mới nhất lên đầu
        //Là những video mà user có id là user_id được phép học, kèm code đi cùng
        //Giải thích các trường
        //id là id của video
        //create_date là ngày tạo
        //des là mô tả
        //image là ảnh
        //link là đường link đầy đủ
        //name là tên video
        //code là mã mà user phải nhập khi click vào để học, lần nào cũng phải nhập vì có thể thỉnh thoảng đổi code để bảo mật
        public string getListVideo(long user_id, string keyword)
        {
            Dictionary<string, string> field = new Dictionary<string, string>();
            try
            {
                if (keyword == null) keyword = "";
                DateTime dtn = DateTime.Now;
                //var p = (from q in db.videos where q.name.Contains(keyword) select q).OrderBy(o => o.id).ToList();
                var p = (from q in db.customers
                         where q.id==user_id
                         join q2 in db.customer_code on q.id equals q2.customer_id 
                         join q3 in db.videos on q2.video_id equals q3.id
                         select new
                         {
                             id=q3.id,
                             create_date = q3.create_date,
                             des=q3.des,
                             image=q3.img,
                             link=q3.link,
                             name=q3.name,
                             code=q2.code
                         }).Where(o=>o.name.Contains(keyword)).OrderByDescending(o => o.id).ToList();
                field.Add("list", JsonConvert.SerializeObject(p));
                return ApiArray("success", field, "Danh sách các video");
            }
            catch (Exception ex)
            {
                field.Add("list", "[]");
                return Api("error", field, "Lỗi sql: " + ex.ToString());
            }
        }
        //Kiểm tra video_id này của khách hàng có id là user_id có mã code là code có hợp lệ không
        //Trả về trường true=1 nếu hợp lệ, true=0 nếu không hợp lệ, rỗng nếu api có lỗi
        public string isTrueCode(int video_id,int code,long user_id)
        {
            Dictionary<string, string> field = new Dictionary<string, string>();
            try
            {

                if (db.customer_code.Any(o => o.video_id == video_id && o.code == code && o.customer_id == user_id))
                {
                    
                    field.Add("true", "1");
                    return Api("success", field, "Mã code này đúng là của khách hàng này học video này!");
                }
                else
                {
                    field.Add("true", "0");
                    return Api("failed", field, "Mã code này không thuộc về khách hàng này học video này! Xin bạn hỏi quản trị website");
                }
            }
            catch (Exception ex)
            {
                field.Add("true", "");
                return Api("error", field, "Lỗi sql: " + ex.ToString());
            }
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}