using smfc.wechat.com.wxapi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml;

namespace smfc.wechat.com.Controllers
{
    public class WechatController : Controller
    {
        //
        // GET: /Wechat/
        public ActionResult Index()
        {
            Log();
            var MenuJson = @"{'button':[
     {	
          'type':'click',
          'name':'今日歌曲',
          'key':'V1001_TODAY_MUSIC'
      },
      {
           'type':'click',
           'name':'歌手简介',
           'key':'V1001_TODAY_SINGER'
      },
      {
           'name':'菜单',
           'sub_button':[
           {	
               'type':'view',
               'name':'搜索',
               'url':'http://www.soso.com/'
            },
            {
               'type':'view',
               'name':'视频',
               'url':'http://v.qq.com/'
            },
            {
               'type':'click',
               'name':'赞一下我们',
               'key':'V1001_GOOD'
            }]
       }]
 }".Replace("\r\n", "").Replace("\t", "").Replace(" ", "").Replace("'", "\"");
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            string setMenuUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
            setMenuUrl = string.Format(setMenuUrl, "wwwfrllkcom ");//获取token、拼凑url
            string respText = common.CommonMethod.WebRequestPostOrGet(setMenuUrl, MenuJson);
            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
            return Content(respDic["errcode"].ToString());//返回0发布成功
            #region 微信 验证  微信发送信息  被动回复信息
            //            if (Request.HttpMethod != "GET" && isAuthorty())
            //            {
            //                return Content(@"<xml><ToUserName><![CDATA[oS_iCvwyarJMkpB-a-WPASqxXz3g]]></ToUserName>
            //            <FromUserName><![CDATA[gh_bdf6a2107fdc]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[有]]></Content><MsgId>6179046501229613247</MsgId></xml>");
            //                using (Stream input = Request.InputStream)
            //                {
            //                    input.Position = 0;
            //                    using (StreamReader reader = new StreamReader(input))
            //                    {
            //                        System.IO.File.AppendAllText(Server.MapPath("~/Logs/info.html"), reader.ReadToEnd());
            //                    }
            //                }
            //                return Content(@"<xml><ToUserName><![CDATA[oS_iCvwyarJMkpB-a-WPASqxXz3g]]></ToUserName>
            //            <FromUserName><![CDATA[gh_bdf6a2107fdc]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[有]]></Content><MsgId>6179046501229613247</MsgId></xml>");
            //            }
            //            else
            //            {
            //                if (!isAuthorty())
            //                {
            //                    //return null;
            //                    return Content("error：verification failed");
            //                }
            //                return Content(Request["echostr"]);
            //            }
            #endregion
        }
        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static double ConvertDateTimeInt(System.DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
        }
        public bool isAuthorty()
        {
            string signature = Request.QueryString["signature"];
            List<string> ls = new List<string>();
            ls.Add("wwwfrllkcom");
            ls.Add(Request.QueryString["timestamp"]);
            ls.Add(Request.QueryString["nonce"]);
            ls.Sort();
            string str = string.Concat(ls);
            str = (FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1") ?? "").ToLower();
            return signature == str;
        }
        public void Log()
        {
            string file = Server.MapPath("~/Logs/access.txt");
            string url = Request.RawUrl;
            string methods = Request.HttpMethod;
            System.IO.File.AppendAllText(file, string.Format("[{0}] URL:{1}<br/>", methods, url));
        }
        public ActionResult ViewLog()
        {
            string file = Server.MapPath("~/Logs/access.txt");
            return Content(System.IO.File.ReadAllText(file));
        }
        public ActionResult ViewInfo()
        {
            string file = Server.MapPath("~/Logs/info.html");
            return Content(System.IO.File.ReadAllText(file));
        }
    }
}
