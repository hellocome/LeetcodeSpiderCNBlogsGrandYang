using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
namespace LeetcodeSpiderCNBlogsGrandYang
{
    public static class CNBlogHTMLTemplate
    {
        public static string IndexPageTemplate = string.Empty;
        public static string PageTemplate = string.Empty;
        public static string DebugIndexPageTemplate = string.Empty;
        public static XmlDocument DebugIndexPageTemplateXML = null;

        public static string Css1 = string.Empty;
        public static string Css2 = string.Empty;
        public static string JQuery = string.Empty;
        public static string JQuerySort = string.Empty;

        static CNBlogHTMLTemplate()
        {
            IndexPageTemplate = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Templates\\IndexPageTemplate.tmp");
            PageTemplate = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Templates\\PageTemplate.tmp");
            DebugIndexPageTemplate = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Templates\\DebugIndexPageTemplate.tmp");

            Css1 = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Templates\\cnblog.css");
            Css2 = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Templates\\cnblog2.css");
            JQuery = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Templates\\jquery-latest.js");
            JQuerySort = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Templates\\jquery.tablesorter.js");

            DebugIndexPageTemplateXML = new XmlDocument();
            DebugIndexPageTemplateXML.LoadXml(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Templates\\DebugIndexPageTemplateXml.tmp"));
        }
    }
}
