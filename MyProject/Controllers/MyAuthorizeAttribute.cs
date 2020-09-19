using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using MyProject.Models;

namespace MyProject.Controllers
{
    public class MyAuthorizeAttribute: ActionFilterAttribute
    {
        /// <summary>
        /// 权限身份1
        /// </summary>
        public  string Roles { get; set; }
        /// <summary>
        /// 权限身份2
        /// </summary>
        public string Roles2 { get; set; }
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{}
        /// <summary>
        /// 过滤器,用于身份以及权限过滤
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            ////1.登录状态获取用户信息（自定义保存的用户）
            //var cookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            ////2.使用 FormsAuthentication 解密用户凭据
            //var ticket = FormsAuthentication.Decrypt(cookie.Value);
            //User_info loginUser = new User_info(); 

            ////3. 直接解析到用户模型里去，有没有很神奇
            //loginUser = new JavaScriptSerializer().Deserialize<User_info>(ticket.UserData);

            ////4. 将要使用的数据放到ViewData 里，方便页面使用
            //filterContext.Controller.ViewData["UserName"] = loginUser.Name;
            //filterContext.Controller.ViewData["UserRole"] = loginUser.Role;
            //filterContext.Controller.ViewData["UserEmail"] = loginUser.Email;
            //filterContext.Controller.ViewData["UserId"] = loginUser.ID;

            if (filterContext.HttpContext.Session["UserName"] != null)
            {
                if ((Roles != null|| Roles2 != null) && filterContext.HttpContext.Session["UserRole"] != null)
                {
                    if (filterContext.HttpContext.Session["UserRole"].ToString() != Roles)
                    {
                        if(filterContext.HttpContext.Session["UserRole"].ToString() != Roles2)
                        {
                            try
                            {
                                JsonResul(filterContext, "/Home/Error");
                            }
                            catch (Exception e)
                            {

                                
                            }
                        }
                        // filterContext.HttpContext.Response.Redirect("/Home/Error");
                    }
                }
            }
            else
            {
                //filterContext.HttpContext.Response.Redirect("/User/Login");
                JsonResul(filterContext,"/User/Login");
            }
        }
        /// <summary>
        /// 消息处理,处理过滤器结果
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="url">指定返回的页面</param>
        public void JsonResul(ActionExecutingContext filterContext, string url)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        ResultMess = "请求用户未登录！"
                    }
                };
            }else
            {
                filterContext.HttpContext.Response.Redirect(url);
            }
            filterContext.Result = new ContentResult();
        }


        //    if (httpContext.Session["UserName"] != null)
        //    {
        //        if (Roles != null && httpContext.Session["UserRole"] != null)
        //        {
        //            string role = httpContext.Session["UserRole"].ToString();
        //            if (role == Roles)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }else
        //        {
        //            return true;

        //        }
        //    }else
        //    {
        //        return false;
        //    }
        //}

        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    //如果是Ajax请求
        //    //if (filterContext.HttpContext.Request.IsAjaxRequest())
        //    //{
        //    //    filterContext.Result = new JsonResult
        //    //    {
        //    //        Data = new
        //    //        {
        //    //            ResultMess = "请求用户未登录！"
        //    //        }
        //    //    };
        //    //}
        //    //else
        //    //{
        //        filterContext.HttpContext.Response.Redirect("/Home/Index");
        //        //处理Url请求
        //        //验证不通过,直接跳转到相应页面，注意：如果不使用以下跳转，则会继续执行Action方法 
        //   // }


        //    //base.HandleUnauthorizedRequest(filterContext);
        //}
    }
}