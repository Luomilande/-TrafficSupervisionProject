using MyProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyProject.Controllers
{
    public class AdminController : Controller
    {
        private Models.MyProjectDBEntities db = new Models.MyProjectDBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorize(Roles = "admin")]
        public ActionResult Video()
        {
            int num = db.Video_info.Count();//总数
            int Ig = num / 5;//正数
            int Rd = num % 5;//余数
            if (Rd > 0) { Ig++; }

            ViewBag.YeShu = Ig;
            return View();
        }
        [MyAuthorize(Roles = "admin")]
        public string VideoList(int PageNumber)
        {
            var num1 = db.Video_info.OrderBy(p => p.ID).Skip((PageNumber - 1) * 5).Take(5);
            var video = db.Video_info.Select(p => new { p.AV_Number, p.UpDate, p.AuditStatus, p.Title });
            var jss = new JavaScriptSerializer();
            string json = jss.Serialize(num1).ToString();
            return json;
        }
        [MyAuthorize(Roles = "admin")]
        public ActionResult VideoHandle(Video_info video)
        {
            try
            {
                Video_info videoDb = db.Video_info.Where(p => p.ID == video.ID).FirstOrDefault();
                videoDb.VideoStatus = video.VideoStatus;
                db.SaveChanges();
            }
            catch (Exception)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet); ;
        }


        [MyAuthorize(Roles = "admin")]
        public ActionResult UserState()
        {
            int num = db.User_info.Count();//总数
            int Ig = num / 5;//正数
            int Rd = num % 5;//余数
            if (Rd > 0) { Ig++; }
            ViewBag.YeShu = Ig;
            return View();
        }


        [MyAuthorize(Roles = "admin")]
        public string UserStateList(int id)
        {
            var user_list = db.User_info.Where(p=> !p.Role.Equals("admin")).Select(t => new { t.ID, t.Name, t.Date, t.State, t.Email, t.Role }).OrderBy(p => p.ID).Skip((id - 1) * 5).Take(5);
            StringBuilder sb = new StringBuilder();
            var jss = new JavaScriptSerializer();
            string json = jss.Serialize(user_list).ToString();
            return json;
        }
        [MyAuthorize(Roles = "admin")]
        public ActionResult UserStateHandle(User_info user)
        {
            try
            {
                User_info userDb = db.User_info.Where(p => p.ID == user.ID).FirstOrDefault();
                userDb.State = user.State;
                db.SaveChanges();
            }
            catch (Exception)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public string Search(string Email)
        {
            if (Email == "")
            {
                return "";
            }
            var data = db.User_info.Where(p => p.Email.Contains(Email)&& !p.Role.Equals("admin"));
            var jss = new JavaScriptSerializer();
            string json = jss.Serialize(data).ToString();
            return json;
        }
        public string VidoSearch(string info)
        {
            if (info == "")
            {
                return "";
            }
            if(Regex.IsMatch(info, @"[\u4e00-\u9fa5]"))
            {
                var data = db.Video_info.Where(p => p.Title.Contains(info));
                var jss = new JavaScriptSerializer();
                string json = jss.Serialize(data).ToString();
                return json;
            }
            else
            {
                var data = db.Video_info.Where(p => p.AV_Number.Contains(info));
                var jss = new JavaScriptSerializer();
                string json = jss.Serialize(data).ToString();
                return json;
            }
        }
        [MyAuthorize]
        public ActionResult WebSet()
        {
            return View();
        }

        public ActionResult WebSetImg(string type)
        {
            if (type == "logo1" || type == "logo2" || type == "logo3" || type == "tip")
            {
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                string fileExtension = System.IO.Path.GetExtension(files[0].FileName);//后缀名.png/.jpg
                if (fileExtension == "")
                {
                    return Json("请选择图片文件！", JsonRequestBehavior.AllowGet);
                }
                //string filePath = files[0].FileName;
                //string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                string filename = type + fileExtension;
                //存到服务器的地址
                string serverpath = Server.MapPath("/Content/img/webset/") + filename;
                //保存图片
                files[0].SaveAs(serverpath);
                //返回保存结果
                return Json("保存成功！", JsonRequestBehavior.AllowGet);
            }
            return Json("保存失败！", JsonRequestBehavior.AllowGet);
        }
        public ActionResult WebSetTip(WebSet_Tip tip)
        {

            try
            {
                var data = db.WebSet_Tip.FirstOrDefault();
                if (data == null)
                {
                    db.WebSet_Tip.Add(tip);
                    db.SaveChanges();
                }
                else
                {
                    if (tip.Tip0 != null)
                    {
                        data.Tip0 = tip.Tip0;
                    }
                    if (tip.Tip1 != null)
                    {
                        data.Tip1 = tip.Tip1;
                    }
                    if (tip.Tip2 != null)
                    {
                        data.Tip2 = tip.Tip2;
                    }
                }
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Json("保存失败！", JsonRequestBehavior.AllowGet);
            }
            return Json("保存成功！", JsonRequestBehavior.AllowGet);
        }
        public string GetWebSetTip()
        {
            var data = db.WebSet_Tip.FirstOrDefault();
            var jss = new JavaScriptSerializer();
            string json = jss.Serialize(data).ToString();
            return json;
        }
    }
}