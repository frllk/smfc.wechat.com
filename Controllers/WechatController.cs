using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
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
            if (!isAuthorty()) return null;
            if (Request.HttpMethod != "GET")
            {
                using (Stream input = Request.InputStream)
                {
                    input.Position = 0;
                    using (StreamReader reader = new StreamReader(input))
                    {
                        System.IO.File.AppendAllText(Server.MapPath("~/Logs/info.html"), reader.ReadToEnd());
                    }
                }
                return Content(@"<xml>
                    <ToUserName><![CDATA[CDATA[oS_iCvwyarJMkpB-a-WPASqxXz3g]]></ToUserName>
                    <FromUserName><![CDATA[gh_bdf6a2107fdc]]></FromUserName>
                    <CreateTime>1438523413</CreateTime>
                    <MsgType><![CDATA[text]]></MsgType>
                    <Content><![CDATA[你好]]></Content>
                    </xml>");
            }
            else
            {
                return Content(Request["echostr"]);
            }
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

    }
}
