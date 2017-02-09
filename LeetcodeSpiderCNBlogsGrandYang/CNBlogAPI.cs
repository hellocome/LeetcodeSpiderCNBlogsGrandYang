using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Xml;
using System.Net;
using System.Configuration;
using LeetcodeSpiderCNBlogsGrandYang.Configuration;

namespace LeetcodeSpiderCNBlogsGrandYang
{
    public class CNBlogAPI
    {
        /*
        Url: http://wcf.open.cnblogs.com/blog/post/body/{POSTID}

        HTTP Method: GET
        Message direction Format  Body
        Request     N/A The Request body is empty.
        Response Xml     Example,Schema
        Response    Json Example

        The following is an example response Xml body:

        <string>String content</string>

        The following is an example response Json body:

        "String content"

        The following is the response Xml Schema:

        <xs:schema elementFormDefault = "qualified" xmlns:xs= "http://www.w3.org/2001/XMLSchema" >
          < xs:element name = "string" nillable= "true" type= "xs:string" />
        </ xs:schema>
        */

        private static readonly string POSTCONTENTGET = "http://wcf.open.cnblogs.com/blog/post/body/{0}";
        public static bool GetPostContent(string postID, out string pageContent)
        {
            pageContent = string.Empty;

            try
            {
                HttpWebRequest wq = WebRequest.Create(string.Format(POSTCONTENTGET, postID)) as HttpWebRequest;

                SetProxy(ref wq);

                using (HttpWebResponse wr = wq.GetResponse() as HttpWebResponse)
                {
                    if (wr.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                        {
                            XmlDocument xml = new XmlDocument();
                            xml.LoadXml(sr.ReadToEnd());
                            pageContent = xml.SelectSingleNode("string").InnerText;
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
            }

            return false;
        }
        public static bool GetPostContent(int postID, out string pageContent)
        {
            return GetPostContent(postID.ToString(), out pageContent);
        }
        public static bool GetPostContent(string postID, out XmlDocument xmlDoc)
        {
            xmlDoc = new XmlDocument();

            try
            {
                HttpWebRequest wq = WebRequest.Create(string.Format(POSTCONTENTGET, postID)) as HttpWebRequest;

                SetProxy(ref wq);

                using (HttpWebResponse wr = wq.GetResponse() as HttpWebResponse)
                {
                    if (wr.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                        {
                            xmlDoc.LoadXml(sr.ReadToEnd());

                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
            }

            return false;
        }
        public static bool GetPostContent(int postID, out XmlDocument xmlDoc)
        {
            return GetPostContent(postID.ToString(), out xmlDoc);
        }
        private static void SetProxy(ref HttpWebRequest request)
        {
            SpiderSettingSection sec = ConfigurationManager.GetSection(SpiderSettingSection.SECTION_NAME) as SpiderSettingSection;


            if (!string.IsNullOrEmpty(sec.ProxyAddress))
            {
                WebProxy proxy = new WebProxy();

                proxy.Address = new Uri(sec.ProxyAddress);

                if (!string.IsNullOrEmpty(sec.ProxyUserName) || !string.IsNullOrEmpty(sec.ProxyPassword) || !string.IsNullOrEmpty(sec.ProxyDomain))
                {
                    proxy.Credentials = new NetworkCredential(sec.ProxyUserName, sec.ProxyPassword, sec.ProxyDomain);
                }

                if(string.IsNullOrEmpty(sec.ProxyPassword))
                {
                    proxy.UseDefaultCredentials = true;
                    proxy.BypassProxyOnLocal = true;
                }

                request.Proxy = proxy;
            }
        }
    }
}
