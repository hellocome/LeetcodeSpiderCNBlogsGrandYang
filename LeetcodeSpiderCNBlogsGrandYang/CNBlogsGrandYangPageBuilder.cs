using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LeetcodeSpiderCNBlogsGrandYang
{
    public class CNBlogsGrandYangPageBuilder
    {
#pragma warning disable CS0414
        //<a href="http://www.cnblogs.com/grandyang/p/5931874.html">
        // The field 'CNBlogsGrandYangPageBuilder.LEETCODE_POSTID_SEARCH' is assigned but its value is never used
        // Used in Release Version.
        private static readonly string LEETCODE_POSTID_SEARCH = @"href=""http:\/\/www.cnblogs.com\/grandyang\/p\/(?<postid>\d{1,9})\.html""";

#pragma warning restore CS0414


        CNBlogGrandYangLeetcodeItemCollection blogItemList = new CNBlogGrandYangLeetcodeItemCollection();
        public static readonly int LEETCODEINDEX_POSTID = 4606334;
        private static readonly string LEETCODE_ONE_QUESION_SEARCH = @"<tr ((?!<tr)[\s\S])+?>(?<trEle>[\s\S]+?)</tr>";
        private static readonly string LEETCODE_ONE_QUESION_ID_SEARCH = @"<td .[^>]+?>\s*(?<tdID>[\d]+)\s*</td>";
        private static readonly string LEETCODE_ONE_QUESION_LINK_DES_SEARCH = @"<td\b[^>]*?><a href=""(?<tdLink>[\s\S^>]+?)""[\s\S^>]*?>(?<tdDescription>[^>]*?)</a>[^>]*?</td>";
        private static readonly string LEETCODE_ONE_QUESION_DIFFICULTY_SEARCH = @"<td\b[^>]*?>(?<tdDifficulty>\s*[Hard|Medium|Easy]+\s*)</td>";
        private static readonly string LEETCODE_ONE_QUESION_ACCEPTANCE_SEARCH = @"<td\b[^>]*?>\s*(?<tdAcceptance>[\d\.]+)%\s*</td>";

        public XmlDocument mIndexContentXML = null;
        public string IndexContent
        {
            get;
            set;
        }

        private Dictionary<string, Func<string, string, string>> ReplaceDic = new Dictionary<string, Func<string, string, string>>()
        {
            {"4606334.html", new Func<string, string, string>((a, b) => { return Regex.Replace(a, b, "Index.html", RegexOptions.IgnoreCase); }) }
            {"http://www.cnblogs.com/grandyang/p/", new Func<string, string, string>((a, b) => { return Regex.Replace(a, b, string.Empty, RegexOptions.IgnoreCase); }) },
            {"http://www.cnblogs.com/grandyang/p", new Func<string, string, string>((a, b) => { return Regex.Replace(a, b, string.Empty, RegexOptions.IgnoreCase); }) }
        };

        public bool LoadIndex()
        {
            bool bGetContent = false;

            try
            {
                blogItemList.Clear();
#if DEBUG
                mIndexContentXML = CNBlogHTMLTemplate.DebugIndexPageTemplateXML;
                IndexContent = mIndexContentXML.SelectSingleNode("string").InnerText;
                bGetContent = true;
#else

                bGetContent = CNBlogAPI.GetPostContent(LEETCODEINDEX_POSTID, out IndexContent);
#endif

                string deHtmlIndexContent = System.Web.HttpUtility.HtmlDecode(IndexContent);

                XmlNodeList trNodeList = mIndexContentXML.SelectNodes("tr");
                MatchCollection mcOneQ = Regex.Matches(deHtmlIndexContent, LEETCODE_ONE_QUESION_SEARCH);

                foreach(Match matchQ in mcOneQ)
                {
                    string trMatch = matchQ.Value;
                    
                    Match mTdID = Regex.Match(trMatch, LEETCODE_ONE_QUESION_ID_SEARCH, RegexOptions.IgnoreCase);
                    string idID = mTdID.Groups["tdID"].Value;

                    Match mLinkDes = Regex.Match(trMatch, LEETCODE_ONE_QUESION_LINK_DES_SEARCH, RegexOptions.IgnoreCase);
                    string tdLink = mLinkDes.Groups["tdLink"].Value;
                    string tdDescription = mLinkDes.Groups["tdDescription"].Value;

                    Match mDifficulty = Regex.Match(trMatch, LEETCODE_ONE_QUESION_DIFFICULTY_SEARCH, RegexOptions.IgnoreCase);
                    string tdDifficulty = mDifficulty.Groups["tdDifficulty"].Value;

                    Match mAcceptance = Regex.Match(trMatch, LEETCODE_ONE_QUESION_ACCEPTANCE_SEARCH, RegexOptions.IgnoreCase);
                    string tdAcceptance = mAcceptance.Groups["tdAcceptance"].Value;

                    CNBlogGrandYangLeetcodeItem item = new CNBlogGrandYangLeetcodeItem(idID, tdDifficulty, tdDescription, tdAcceptance, tdLink);

                    blogItemList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
                bGetContent = false;
            }

            return bGetContent;
        }

        private bool SaveIndexPage(string page)
        {
            string pageHtmlContent = CNBlogHTMLTemplate.IndexPageTemplate.Replace("[TITLE]", "Leetcode from CNBlog - GrandYang");
            pageHtmlContent = pageHtmlContent.Replace("[CONTENT]", blogItemList.BuildHTML());

            return SavePage(page, pageHtmlContent);
        }

        private void BuildPages(string root)
        {
            for (int i = 0; i < blogItemList.Count; i++)
            {
                string content = string.Empty;

                bool res = blogItemList.GetPageContent(i, out content);

                string pageHtmlContent = CNBlogHTMLTemplate.PageTemplate.Replace("[TITLE]", blogItemList[i].ToString());
                pageHtmlContent = pageHtmlContent.Replace("[CONTENT]", content);

                if (true == res)
                {
                    res = SavePage(root + System.IO.Path.DirectorySeparatorChar + blogItemList[i].PostPage, pageHtmlContent);
                }

                if (true != res)
                {
                    Console.WriteLine("Failure: " + blogItemList[i]);
                }
            }
        }

        private bool SavePage(string page, string content, bool overwrite = true)
        {
            try
            {
                if (File.Exists(page))
                {
                    if (overwrite)
                    {
                        File.Delete(page);
                    }
                }

                foreach (string strKey in ReplaceDic.Keys)
                {
                    content = ReplaceDic[strKey](content, strKey);
                }

                File.WriteAllText(page, content);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        private void CopyCssJs(string root)
        {
            File.WriteAllText(root + "\\cnblog.css", CNBlogHTMLTemplate.Css1);
            File.WriteAllText(root + "\\cnblog2.css", CNBlogHTMLTemplate.Css2);
            File.WriteAllText(root + "\\jquery-latest.js", CNBlogHTMLTemplate.JQuery);
            File.WriteAllText(root + "\\jquery.tablesorter.js", CNBlogHTMLTemplate.JQuerySort);
        }

        public bool BuildSite(string root)
        {
            bool res = false;

            try
            {
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                CopyCssJs(root);

                res = LoadIndex();

                if (true == res)
                {
                    res = SaveIndexPage(root + "\\Index.html");
                }

                if(true == res)
                {
                    BuildPages(root);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
            }

            return res;
        }


    }
}
