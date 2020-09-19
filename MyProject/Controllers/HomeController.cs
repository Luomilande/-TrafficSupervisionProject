using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MyProject.Models;

namespace MyProject.Controllers
{

    public class HomeController : Controller
    {
        private Models.MyProjectDBEntities db = new Models.MyProjectDBEntities();
        public ActionResult Index()
        {
            return View();
        }


        public string GetNewVideo()
        {
            var data = (from a in db.Video_info
                         join b in db.User_info
                         on a.AuthorID equals b.ID
                         where a.VideoStatus==true && a.AuditStatus == true
                        orderby a.ID descending
                         select new
                         {
                             a.ID,
                             a.AV_Number,
                             a.Title,
                             b.Name,
                             a.Reading
                         }).Take(8);
            //var data = db.Video_info.Select(d => new
            //{
            //    d.ID,
            //    d.AV_Number,
            //    d.Title,
            //    d.AuthorID
            //}).OrderByDescending(p=>p.ID).Take(8);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();
            string json = jss.Serialize(data).ToString();
            return json;
        }
        public string GetHotVideo()
        {
            var data = (from a in db.Video_info
                        join b in db.User_info
                        on a.AuthorID equals b.ID
                        where a.VideoStatus == true && a.AuditStatus == true
                        select new
                        {
                            a.ID,
                            a.AV_Number,
                            a.Title,
                            b.Name,
                            a.Reading
                        }).OrderByDescending(p=>p.Reading).Take(4);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();
            string json = jss.Serialize(data).ToString();
            return json;
        }


        public ActionResult VideoList()
        {
            int num = db.Video_info.Where(p=>p.AuditStatus==true && p.VideoStatus==true).Count();//总数
            int Ig = num / 20;//正数
            int Rd = num % 20;//余数
            if (Rd > 0) { Ig++; }
            ViewBag.YeShu = Ig;
            return View();
        }
        public string GetVideoList(int PageNumber)
        {
            var data = (from a in db.Video_info
                        join b in db.User_info
                        on a.AuthorID equals b.ID
                        where a.VideoStatus == true && a.AuditStatus == true 
                        select new
                        {
                            a.ID,
                            a.AV_Number,
                            a.Title,
                            b.Name,
                            a.Reading
                        }).OrderBy(p => p.ID).Skip((PageNumber - 1) * 20).Take(20);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();
            string json = jss.Serialize(data).ToString();
            return json;
        }
        public string GetHotVideoList()
        {
            var data = (from a in db.Video_info
                        join b in db.User_info
                        on a.AuthorID equals b.ID
                        where a.VideoStatus == true && a.AuditStatus == true
                        select new
                        {
                            a.ID,
                            a.AV_Number,
                            a.Title,
                            b.Name
                        }).Take(8);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();
            string json = jss.Serialize(data).ToString();
            return json;
        }

        public string GetNotice()
        {
            var data = db.Notice_info.Where(p=>p.State==true).Select(d => new
            {
                d.id,
                d.Title,
            }).OrderByDescending(p => p.id).Take(12);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();
            string json = jss.Serialize(data).ToString();
            return json;
        }
        public ActionResult NoticeInfo()
        {
            return View();
        }
        public string GetNoticeInfo(int id)
        {
            Notice_info notice_Info = db.Notice_info.Where(p => p.id == id).FirstOrDefault();
            notice_Info.Reading++;
            db.SaveChanges();

            var data = from a in db.Notice_info
                       join b in db.User_info
                       on a.Author equals b.ID
                       where a.id == id && a.State == true
                       select new
                       {
                           a.id,
                           a.Title,
                           a.NoticeContent,
                           a.CreationDate,
                           b.Name
                       };

            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();
            string json = jss.Serialize(data).ToString();
            return json;
        }


        public ActionResult NoticeList()
        {
            int num = db.Notice_info.Where(p => p.State == true).Count();//总数
            int Ig = num / 10;//正数
            int Rd = num % 10;//余数
            if (Rd > 0) { Ig++; }
            ViewBag.YeShu = Ig;
            return View();
        }
        public string GetNoticeList(int PageNumber)
        {
            int num = db.Notice_info.Where(p => p.State == true).Count();//总数
            int Ig = num / 10;//正数
            int Rd = num % 10;//余数
            if (Rd > 0) { Ig++; }
            if (PageNumber > Ig)
            {
                return "";
            }
            var data = (from a in db.Notice_info
                        join b in db.User_info
                        on a.Author equals b.ID
                        where a.State == true
                        select new
                        {
                            a.id,
                            a.Title,
                            a.CreationDate,
                            b.Name,
                            a.Reading
                        }).OrderBy(p => p.id).Skip((PageNumber - 1) * 10).Take(10);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();
            string json = jss.Serialize(data).ToString();
            return json;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Search(string keyword)
        {
            if (keyword == "")
            {
                return RedirectToAction("Index", "Home");
            }
            int num = db.Video_info.Where(p => p.VideoStatus == true && p.Title.Contains(keyword)).Count();
            int Ig = num / 10;//正数
            int Rd = num % 10;//余数
            if (Rd > 0) { Ig++; }
            ViewBag.YeShu = Ig;
            return View();
        }
        public string SearchInfo(string keyword)
        {
            if(keyword=="")
            {
                return "404";
            }
            var data = (from a in db.Video_info
                        join b in db.User_info
                        on a.AuthorID equals b.ID
                        where a.VideoStatus == true 
                        && a.AuditStatus == true
                        && a.Title.Contains(keyword)
                        orderby a.ID descending
                        select new
                        {
                            a.ID,
                            a.AV_Number,
                            a.Title,
                            b.Name,
                            a.Reading
                        }).Take(8);
            ;

            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();
            string json = jss.Serialize(data).ToString();
            return json;
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}