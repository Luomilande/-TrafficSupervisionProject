using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyProject.Controllers
{
    public class InspectController : Controller
    {
        private Models.MyProjectDBEntities db = new Models.MyProjectDBEntities();
        // GET: Inspect
        public ActionResult Index()
        {
            return View();
        }
        [MyAuthorize(Roles = "inspect")]
        public ActionResult Video()
        {
            int num = db.Video_info.Where(p => p.AuditStatus == false && p.VideoStatus==true).Count();//总数
            int Ig = num / 5;//正数
            int Rd = num % 5;//余数
            if (Rd > 0) { Ig++; }

            ViewBag.YeShu = Ig;
            return View();
        }
        [MyAuthorize(Roles = "inspect")]
        public string VideoList(int PageNumber)
        {
            var num1 = db.Video_info.Where(p => p.AuditStatus == false && p.VideoStatus == true).OrderBy(p => p.ID).Skip((PageNumber - 1) * 5).Take(5);
            //var video = db.Video_info.Select(p => new { p.AV_Number, p.UpDate, p.AuditStatus, p.Title });
            var jss = new JavaScriptSerializer();
            String json = jss.Serialize(num1).ToString();
            return json;
        }
        [MyAuthorize(Roles = "inspect")]
        public string VidoSearch(string info)
        {
            if (info == "")
            {
                return "";
            }
            if (Regex.IsMatch(info, @"[\u4e00-\u9fa5]"))
            {
                var data = db.Video_info.Where(p => p.Title.Contains(info) && p.AuditStatus == false && p.VideoStatus == true);
                var jss = new JavaScriptSerializer();
                string json = jss.Serialize(data).ToString();
                return json;
            }
            else
            {
                var data = db.Video_info.Where(p => p.AV_Number.Contains(info) && p.AuditStatus == false && p.VideoStatus == true);
                var jss = new JavaScriptSerializer();
                string json = jss.Serialize(data).ToString();
                return json;
            }
        }

        [MyAuthorize(Roles = "inspect")]
        public ActionResult VideoInspect(string AV_Number)
        {
            Video_info videinfo = db.Video_info.Where(p => p.AV_Number == AV_Number).FirstOrDefault();
            ViewBag.VideoTitle = videinfo.Title;
            ViewBag.AV_Number = videinfo.AV_Number;
            ViewBag.VideoUrl = ViewBag.AV_Number + ".mp4";
            ViewBag.OccurrenceTime = videinfo.OccurrenceTime;
            ViewBag.Synopsis = videinfo.Synopsis;
            ViewBag.Address = videinfo.Address;
            return View();
        }
        [MyAuthorize(Roles = "inspect")]
        public string VideoInspectList()
        {
            ///???
            Video_info videinfo = db.Video_info.Where(p => p.ID == 6).FirstOrDefault();
            var jss = new JavaScriptSerializer();
            String json = jss.Serialize(videinfo).ToString();
            return json;
        }
        [MyAuthorize(Roles = "inspect")]
        public ActionResult VideoInspectHandle(Video_Inspect video)
        {
            if (video.AuditContents == null || video.AV_Number == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            try
            {
                Video_info videoDb = db.Video_info.Where(p => p.AV_Number == video.AV_Number).FirstOrDefault();
                try
                {

                    videoDb.VideoContent = video.AuditContents;
                    videoDb.AuditResultStatus = video.AuditStatus;
                    videoDb.AuditStatus = true;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    return Json(false, JsonRequestBehavior.AllowGet);
                }

                Video_Vote vote = new Video_Vote();
                vote.AV_Number= video.AV_Number;
                vote.Vote_F = 0;
                vote.Vote_T = 0;

                Video_Inspect inspect = new Video_Inspect();
                inspect.AuditAdminID =Convert.ToInt32(Session["UserId"].ToString());
                inspect.AuditContents = video.AuditContents;
                inspect.AuditDate= DateTime.Now.ToString();
                inspect.AuditStatus = video.AuditStatus;
                inspect.AV_Number = video.AV_Number;
                inspect.Score = video.Score;

                db.Video_Inspect.Add(inspect);
                db.Video_Vote.Add(vote);
                db.SaveChanges();



            }
            catch (Exception ex)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}