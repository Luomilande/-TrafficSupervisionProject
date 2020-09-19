using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

/// <summary>
/// 用于处理视频的控制器
/// </summary>
namespace MyProject.Controllers
{
    public class VideoController : Controller
    {
        private Models.MyProjectDBEntities db = new Models.MyProjectDBEntities();
        // GET: Video
        public ActionResult Index()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult UpVideo()
        {
            return View();
        }
        /// <summary>
        /// 上传视频
        /// </summary>
        /// <param name="vide_info">上传视频信息</param>
        /// <returns></returns>
        [MyAuthorize]
        public ActionResult UploadingVideo(Models.Video_info videoinfo)
        {
            if (videoinfo.OccurrenceTime == null|| videoinfo.Synopsis == null|| videoinfo.Title == null|| videoinfo.Address == null)
            {
                return Json("上传失败,举报信息请填写完整！", JsonRequestBehavior.AllowGet);
            }
            StringBuilder str = new StringBuilder();//定义变长字符串
            string av = "";
            Random random = new Random();
            //随机生成数字，并添加到字符串
            while (true)
            {
                for (int i = 0; i < 8; i++)
                {
                    str.Append(random.Next(10));
                }
                av = "av" + str.ToString();
                if (db.Video_info.Where(p => p.AV_Number == av).Count() == 0)
                {
                    break;
                }
                else
                {
                    str.Clear();
                }
            }


            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string fileExtension = System.IO.Path.GetExtension(hfc[0].FileName);

            string video_av = av + fileExtension;
            if (hfc.Count > 0 && fileExtension != null)
            {

                if ("(.mp4)|(.avi)|(.flv)|(.rmvb)|(.wmv)".Contains(fileExtension))
                {
                    string imgPath = "~/Content/video";
                    string target = Server.MapPath("/Content/video/");

                    try
                    {
                        imgPath = target + video_av;
                        hfc[0].SaveAs(imgPath);
                        string img = GetImg(imgPath, av);
                        if (img != "")
                        {
                            videoinfo.UpDate = DateTime.Now.ToString();
                            videoinfo.AV_Number = "av" + str.ToString();
                            videoinfo.AuditStatus = false;
                            videoinfo.VideoStatus = true;
                            videoinfo.Reading = 0;
                            videoinfo.AuthorID =Convert.ToInt32(Session["UserId"].ToString());
                            db.Video_info.Add(videoinfo);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception)
                    {
                        return Json("提交失败", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("提交文件格式错误！", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("提交失败", JsonRequestBehavior.AllowGet);
            }
            return Json("提交成功", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取视频图片
        /// </summary>
        /// <param name="filePath">视频路径</param>
        /// <param name="imgName">视频av号</param>
        /// <returns></returns>
        public string GetImg(string filePath, string imgName)
        {
            string ffmpeg = Server.MapPath("~/Content/ffmpeg/ffmpeg.exe");
            string vFileName = filePath;
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            string flv_img = imgName + ".png";
            string flv_img_p = Server.MapPath("~/Content/img/video/" + flv_img);

            //string flv_img_p = System.Web.HttpContext.Current.Server.MapPath("Content/ffmpeg/" + flv_img);
            startInfo.Arguments = " -ss 00:00:07 -i " + vFileName + " -f image2 -s 160x90 -y " + flv_img_p;
            System.Diagnostics.Process.Start(startInfo);

            System.Threading.Thread.Sleep(3000);
            if (System.IO.File.Exists(flv_img_p))
            {
                return flv_img;
            }
            else
            {
                return "";
            }
        }


        public ActionResult PlayVideo(string av)
        {
            try
            {
                if (av == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                var add = db.Video_info.Where(p => p.AV_Number == av).FirstOrDefault();
                add.Reading = add.Reading + 1;
                db.SaveChanges();
                var data = (from a in db.Video_info
                            join b in db.User_info
                            on a.AuthorID equals b.ID
                            join c in db.Video_Inspect
                            on a.AV_Number equals c.AV_Number
                            where a.VideoStatus == true && a.AV_Number == av
                            select new
                            {
                                c.AuditAdminID,
                                c.AuditContents,
                                b.Name,
                                a.AV_Number,
                                a.UpDate,
                                a.Title,
                                a.Synopsis,
                                a.AuditStatus,
                                a.AuditResultStatus,
                                a.OccurrenceTime,
                                a.Address,
                                c.Score
                            }).FirstOrDefault();
                ViewBag.AuditAdmin = "";
                if (data.AuditAdminID != 0)
                {
                    var data2 = db.User_info.Where(p => p.ID == data.AuditAdminID).Select(d => new { d.Name }).FirstOrDefault();
                    ViewBag.AuditAdmin = data2.Name;//审核人名字
                }


                ViewBag.VideoTitle = data.Title;//标题

                ViewBag.Author = data.Name;//up主

                ViewBag.AV_Number = data.AV_Number;//av号

                ViewBag.VideoUrl = data.AV_Number + ".mp4";//视频路径

                ViewBag.OccurrenceTime = data.OccurrenceTime;//发生时间

                ViewBag.Synopsis = data.Synopsis;//事故简述

                ViewBag.VideoContent = data.AuditContents;//违章内容

                ViewBag.Address = data.Address;//发生 地点

                ViewBag.Score = data.Score;//扣分情况

                ViewBag.AuditResultStatus = data.AuditResultStatus;
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }

            return View();
        }
        public string GetHotVideo()
        {
            var data = (from a in db.Video_info
                        join b in db.User_info
                        on a.AuthorID equals b.ID
                        where a.VideoStatus == true 
                        select new
                        {
                            a.ID,
                            a.AV_Number,
                            a.Title,
                            b.Name
                        }).Take(3);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();
            string json = jss.Serialize(data).ToString();
            return json;
        }

        public string GetVote(string av)
        {
            var data = db.Video_Vote.Where(p => p.AV_Number == av).FirstOrDefault();

            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();
            string json = jss.Serialize(data).ToString();
            return json;
        }
        public ActionResult AddVote(bool tf,string av)
        {
            try
            {
                var vote = db.Video_Vote.Where(p => p.AV_Number == av).FirstOrDefault();
                if (tf)
                {
                    vote.Vote_T = vote.Vote_T+1;
                }
                else
                {
                    vote.Vote_F = vote.Vote_F+1;
                }
                db.SaveChanges();
            }
            catch (Exception)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize]
        public ActionResult ModifyVideo(int id)
        {
            int UserId =Convert.ToInt32(Session["UserId"].ToString());
            var data = db.Video_info
                .Where(p=>p.ID== id && p.AuthorID== UserId &&p.AuditStatus==false &&p.VideoStatus==true)
                .Select(p=>new { p.AV_Number,p.Address,p.OccurrenceTime,p.Synopsis, p.Title})
                .FirstOrDefault();
            if(data==null)
            {
                ViewBag.VideoTitle ="已审核无法修改";//标题
                return View();
            }
            ViewBag.VideoTitle = data.Title;//标题

            ViewBag.AV_Number = data.AV_Number;//av号

            ViewBag.VideoUrl = data.AV_Number + ".mp4";//视频路径

            ViewBag.OccurrenceTime = data.OccurrenceTime;//发生时间

            ViewBag.Synopsis = data.Synopsis;//事故简述


            ViewBag.Address = data.Address;//发生 地点

            return View();

        }
        [MyAuthorize]
        public ActionResult ModifyVideoHandle(Video_info info)
        {
            if (info.AV_Number == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            var data = db.Video_info.Where(p => p.AV_Number== info .AV_Number&& p.AuthorID == UserId).FirstOrDefault();
            if (data.Title != info.Title)
            {
                data.Title = info.Title;
            }
            if (data.OccurrenceTime != info.OccurrenceTime)
            {
                data.OccurrenceTime = info.OccurrenceTime;
            }
            if (data.Address != info.Address)
            {
                data.Address = info.Address;
            }
            if (data.Synopsis != info.Synopsis)
            {
                data.Synopsis = info.Synopsis;
            }
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}