using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using MyProject.Models;

namespace MyProject.Controllers
{
    /// <summary>
    /// 用于处理用户的控制器
    /// </summary>
    public class UserController : Controller
    {
        private Models.MyProjectDBEntities db = new Models.MyProjectDBEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult Account()
        {
            int id = Convert.ToInt32(Session["UserId"].ToString());
            int num = db.Video_info.Where(p => p.VideoStatus == true && p.AuthorID == id).Count();//总数
            int Ig = num / 5;//正数
            int Rd = num % 5;//余数
            if (Rd > 0) { Ig++; }

            ViewBag.YeShu = Ig;
            return View();
        }
        public string AccountList()
        {
            int id = Convert.ToInt32(Session["UserId"].ToString());
            var data = db.Video_info.Where(p => p.AuthorID == id && p.VideoStatus == true).Select(d => new
            {
                d.ID,
                d.AV_Number,
                d.UpDate,
                d.Title,
                d.VideoStatus,
                d.AuditStatus,
                d.Reading
            });
            JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder sb = new StringBuilder();
            String json = jss.Serialize(data).ToString();
            return json;
        }
        public string AccountHandle()
        {
            string email = Session["UserEmail"].ToString();
            if (email != null)
            {
                User_info user_Info = db.User_info.Where(p => p.Email == email).FirstOrDefault();

                JavaScriptSerializer jss = new JavaScriptSerializer();
                StringBuilder sb = new StringBuilder();
                String json = jss.Serialize(user_Info).ToString();
                return json;
            }
            else
            {
                return null;
            }
        }


        [MyAuthorize]
        public ActionResult Setting()
        {
            return View();
        }
        [MyAuthorize]
        public string GetSetting()
        {
            if (Session["UserEmail"] != null)
            {
                string email = Session["UserEmail"].ToString();
                if (email != null)
                {
                    User_info user_Info = db.User_info.Where(p => p.Email == email).FirstOrDefault();
                    user_Info.Password = "";
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    StringBuilder sb = new StringBuilder();
                    String json = jss.Serialize(user_Info).ToString();
                    return json;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        [MyAuthorize]
        public ActionResult SettingHandle(SettingUser getinfo)
        {
            string tip = null;
            string tip2 = null;
            bool tf = false;
            try
            {
                string email = Session["UserEmail"].ToString();
                var db_user = db.User_info.Where(p => p.Email == email).FirstOrDefault();
                if (getinfo.Address != null && getinfo.Address != "" && getinfo.Address != db_user.Address)
                {
                    db_user.Address = getinfo.Address;
                    tip = "地址、" + tip;
                    tf = true;
                }
                if (getinfo.Age != null && getinfo.Age != db_user.Age)
                {
                    db_user.Age = getinfo.Age;
                    tip = "年龄、" + tip;
                    tf = true;
                }
                if (getinfo.PhoneNumber != db_user.PhoneNumber && getinfo.PhoneNumber != null)
                {
                    db_user.PhoneNumber = getinfo.PhoneNumber;
                    tip = "号码、" + tip;
                    tf = true;
                }


                if (getinfo.Name != null && getinfo.Name != "" && getinfo.Name != db_user.Name)
                {
                    db_user.Name = getinfo.Name;
                    tip = "姓名、" + tip;
                    tf = true;
                }
                if (getinfo.Sex != null && getinfo.Sex != null && getinfo.Sex != db_user.Sex)
                {
                    db_user.Sex = getinfo.Sex;
                    tip = "性别、" + tip;
                    tf = true;
                }
                //db_user.Age = getinfo.Age;
                //db_user.Sex = getinfo.Sex;
                //db_user.PhoneNumber = getinfo.PhoneNumber;
                if (getinfo.Password != "" && getinfo.Password != null || getinfo.NewPassword != null && getinfo.NewPassword != "")
                {
                    getinfo.Password = GenerateMD5(getinfo.Password);
                    if (getinfo.Password == db_user.Password && getinfo.NewPassword != db_user.Password)
                    {
                        db_user.Password = GenerateMD5(getinfo.NewPassword);
                        tip = "密码、" + tip;
                        tf = true;
                    }
                    else
                    {

                        tip2 = "密码不匹配密码修改失败";
                        if (tf)
                        {
                            tip = tip2 + "、" + tip;
                        }

                    }
                }
                if (tf)
                {
                    db.SaveChanges();
                    tip = tip + "修改成功!";
                }
                else
                {
                    if (tip2 == null)
                    {
                        tip = "如需更改请填写完整更改信息";
                    }
                    else
                    {
                        tip = tip2;
                    }

                }
            }
            catch (Exception)
            {
                tip = "修改失败";
            }
            return Json(tip, JsonRequestBehavior.AllowGet);
        }




        [MyAuthorize(Roles = "admin", Roles2 = "inspect")]
        public ActionResult Notice()
        {
            int num = db.Notice_info.Where(p => p.State != false).Count();//总数
            int Ig = num / 5;//正数
            int Rd = num % 5;//余数
            if (Rd > 0) { Ig++; }

            ViewBag.YeShu = Ig;
            return View();
        }
        [MyAuthorize(Roles = "admin", Roles2 = "inspect")]
        public string NoticeList(int PageNumber)
        {
            var num1 = db.Notice_info.Where(p=>p.State != false).OrderByDescending(p => p.id).Skip((PageNumber - 1) * 5).Take(5);
            // var video = db.Notice_info.Select(p => new { p.id,p.Title, p.CreationDate, p.State, p.Reading });
            var jss = new JavaScriptSerializer();
            string json = jss.Serialize(num1).ToString();
            return json;
        }

        public string NoticeSearch(string info)
        {
            if (info == "")
            {
                return "";
            }
            var data = db.Notice_info.Where(p => p.Title.Contains(info) && p.State != false);
            var jss = new JavaScriptSerializer();
            string json = jss.Serialize(data).ToString();
            return json;
        }

        [MyAuthorize(Roles = "admin", Roles2 = "inspect")]
        public ActionResult NoticeEdit()
        {
            return View();
        }
        [MyAuthorize(Roles = "admin", Roles2 = "inspect")]
        public ActionResult NoticeDelete(Notice_info notice)
        {
            try
            {
                notice = db.Notice_info.Where(p => p.id == notice.id).FirstOrDefault();
                notice.State = false;
                db.SaveChanges();
            }
            catch (Exception)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [MyAuthorize(Roles = "admin", Roles2 = "inspect")]
        public ActionResult NoticeEditHandle(Notice_info notice)
        {
            ///添加公告信息
            if (notice.Title != null && notice.NoticeContent != null)
            {
                notice.CreationDate = DateTime.Now.ToString();
                notice.Author = Convert.ToInt32(Session["UserId"].ToString());
                notice.State = true;
                try
                {
                    db.Notice_info.Add(notice);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }



        [MyAuthorize(Roles = "user")]
        public ActionResult Video()
        {
            int id = Convert.ToInt32(Session["UserId"].ToString());
            int num = db.Video_info.Where(p => p.VideoStatus == true && p.AuthorID == id).Count();//总数
            int Ig = num / 5;//正数
            int Rd = num % 5;//余数
            if (Rd > 0) { Ig++; }

            ViewBag.YeShu = Ig;
            return View();
        }
        [MyAuthorize(Roles = "user")]
        public string VideoList(int PageNumber)
        {
            int id = Convert.ToInt32(Session["UserId"].ToString());
            var video = db.Video_info.Where(p => p.AuthorID == id && p.VideoStatus == true).Select(p => new { p.AV_Number, p.UpDate, p.VideoStatus, p.Title, p.Reading, p.ID,p.AuditStatus }).OrderBy(p => p.ID).Skip((PageNumber - 1) * 5).Take(5);
            //var video = db.Video_info.Select(p => new { p.AV_Number, p.UpDate, p.AuditStatus, p.Title });
            var jss = new JavaScriptSerializer();
            String json = jss.Serialize(video).ToString();
            return json;
        }
        /// <summary>
        ///  用户删除视频
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        [MyAuthorize(Roles = "user")]
        public ActionResult VideoHandle(Video_info video)
        {
            try
            {
                Video_info videoDb = db.Video_info.Where(p => p.ID == video.ID).FirstOrDefault();
                videoDb.VideoStatus = video.VideoStatus;
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet); ;
        }





        public ActionResult TestView()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LoginHandle(User_info user_Info)
        {
            string Email = user_Info.Email;
            string Password =  GenerateMD5(user_Info.Password);

            JavaScriptSerializer serial = new JavaScriptSerializer();
            if (db.User_info.Where(p => p.Email.Equals(Email) && p.Password.Equals(Password) && p.State==true).Count() == 1 ? true : false)
            {
                var userinfo = db.User_info.Where(p => p.Email.Equals(Email) && p.Password.Equals(Password) && p.State == true).Select(p => new { p.Name, p.Email, p.Role, p.ID }).FirstOrDefault();
                Session["UserName"] = userinfo.Name;
                Session["UserEmail"] = userinfo.Email;
                Session["UserRole"] = userinfo.Role;
                Session["UserId"] = userinfo.ID;

                //       FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                //1,
                //user_Info.Name,
                //DateTime.Now,
                //DateTime.Now.AddMinutes(30),
                //false,
                //serial.Serialize(userinfo)
                //);
                //       //6. 加密
                //       string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                //       //7. 响应到客户端
                //       System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                //       System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Register()
        {
            return View();
        }
        public ActionResult AddRegister(User_info user_info)
        {
            string pattern0 = "^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,16}$'";//密码验证
            string pattern1 = "/ 0 ? (13 | 14 | 15 | 18)[0 - 9]{8}/";//手机号码
            string pattern2 = "/\\w[-\\w.+] *@@([A - Za - z0 - 9][-A - Za - z0 - 9] +\\.)+[A - Za - z]{ 2,14}/";//邮箱
            bool a = Regex.IsMatch(user_info.Password, pattern0);
            if (user_info.Password == null)
            {
                return Json("密码不符合标准!", JsonRequestBehavior.AllowGet);
            }
            if (user_info.PhoneNumber == null || Regex.IsMatch(user_info.PhoneNumber, pattern1))
            {
                return Json("手机号码不符合标准!", JsonRequestBehavior.AllowGet);
            }
            if (user_info.Email == null || Regex.IsMatch(user_info.Email, pattern2))
            {
                return Json("邮箱不符合标准!", JsonRequestBehavior.AllowGet);
            }

            if (db.User_info.Where(p => p.Email == user_info.Email).Count() != 1)
            {
                user_info.Password = GenerateMD5(user_info.Password);
                user_info.Date = DateTime.Now.ToString();
                user_info.State = true;
                user_info.Role = "user";
                try
                {
                    db.User_info.Add(user_info);
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    return Json("注册失败！", JsonRequestBehavior.AllowGet);
                }

                //------------------可用来做邮箱验证使用---------------------------
                //string key = GetRandomString(16, true, true, false, true, null);
                //Response.Cookies["test"].Value = key;
                //Response.Cookies["test"].Expires = DateTime.Now.AddMinutes(30);
                //SendEmail(user_info.Email,"邮箱激活！", "/User/yanzheng?info=" + key);
                //return Json("User/yanzheng?info="+ key, JsonRequestBehavior.AllowGet);
                //------------------可用来做邮箱验证使用end---------------------------

                return Json("注册成功！", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("账号已被注册！", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LogOut()
        {
            Session["UserName"] = null;
            Session["UserEmail"] = null;
            Session["UserRole"] = null;
            Session["UserId"] = null;
            return RedirectToAction("Index", "Home");

        }


        //辅助类方法↓
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="txt">需要加密原文</param>
        /// <returns>加密密文</returns>
        public static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        public string yanzheng(string info)
        {
            if (Request.Cookies["test"] != null)
            {
                string key = Request.Cookies["test"].Value;

                return key == info ? "值不相等" : "测试成功";
            }
            else
            {
                return "测试失败";
            }
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">要发送的邮箱</param>
        /// <param name="mailSubject">邮箱主题</param>
        /// <param name="mailContent">邮箱内容</param>
        /// <returns>返回发送邮箱的结果</returns>
        public static bool SendEmail(string mailTo, string mailSubject, string mailContent)
        {
            // 设置发送方的邮件信息,例如使用网易的smtp
            string smtpServer = "smtp.163.com"; //SMTP服务器
            string mailFrom = "luomilande@163.com"; //登陆用户名
            string userPassword = "***";//登陆密码

            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = smtpServer; //指定SMTP服务器
            smtpClient.Credentials = new System.Net.NetworkCredential(mailFrom, userPassword);//用户名和密码

            // 发送邮件设置        
            MailMessage mailMessage = new MailMessage(mailFrom, mailTo); // 发送人和收件人
            mailMessage.Subject = mailSubject;//主题
            mailMessage.Body = mailContent;//内容
            mailMessage.BodyEncoding = Encoding.UTF8;//正文编码
            mailMessage.IsBodyHtml = true;//设置为HTML格式
            mailMessage.Priority = MailPriority.Low;//优先级

            try
            {
                smtpClient.Send(mailMessage); // 发送邮件
                return true;
            }
            catch (SmtpException ex)
            {
                return false;
            }
        }

    }
   



}