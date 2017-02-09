using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetcodeSpiderCNBlogsGrandYang
{
    public class CNBlogGrandYangLeetcodeItemCollection : List<CNBlogGrandYangLeetcodeItem>
    {
        public string BuildHTML()
        {
            int count = 0;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<table class=""tablesorter"">");
            sb.AppendLine("<thead>");
            sb.AppendLine(" <tr>");
            sb.AppendLine("     <th>ID</th>");
            sb.AppendLine("     <th>Title</th>");
            sb.AppendLine("     <th>Acceptance</th>");
            sb.AppendLine("     <th>Difficulty</th>");
            sb.AppendLine(" </tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");

            foreach (CNBlogGrandYangLeetcodeItem item in this)
            {
                if (count++ % 2 == 1)
                {
                    sb.AppendLine(item.ToTRHTML());
                }
                else
                {
                    sb.AppendLine(item.ToTRHTML(true));
                }
            }
            sb.AppendLine("</tbody>");
            sb.AppendLine(@"</table>");
            return sb.ToString();
        }

        public bool GetPageContent(int index, out string pageontent)
        {
            pageontent = string.Empty;

            if (index >= 0 && index < this.Count)
            {
                CNBlogGrandYangLeetcodeItem item = this[index];

                if (CNBlogAPI.GetPostContent(item.PostID, out pageontent))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool GetPageContent(CNBlogGrandYangLeetcodeItem item, out string pageontent)
        {
            pageontent = string.Empty;
            
            if (CNBlogAPI.GetPostContent(item.PostID, out pageontent))
            {
                return true;
            }

            return false;
        }
    }
}
