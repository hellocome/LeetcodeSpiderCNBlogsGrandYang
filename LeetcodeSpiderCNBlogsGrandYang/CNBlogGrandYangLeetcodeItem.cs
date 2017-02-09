using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetcodeSpiderCNBlogsGrandYang
{
    public class CNBlogGrandYangLeetcodeItem
    {
        public string ID { get; private set; }
        public string Difficulty { get;  private set; }
        public string Acceptance { get;  private set; }
        public string Title { get;  private set; }
        public string PostURL { get;  private set; }

        public string PostID
        {
            get
            {
                int lastBackSlash = PostURL.LastIndexOf('/');
                int lastDot = PostURL.LastIndexOf('.');
                return PostURL.Substring(lastBackSlash + 1, lastDot - lastBackSlash - 1);
            }
        }

        public string PostPage
        {
            get
            {
                int lastBackSlash = PostURL.LastIndexOf('/');
                return PostURL.Substring(lastBackSlash + 1);
            }
        }

        public CNBlogGrandYangLeetcodeItem(string id, string difficulty, string title, string acceptance, string postURL)
        {
            ID = id.Trim();
            Difficulty = difficulty.Trim();
            Acceptance = acceptance.Trim();
            Title = title;
            PostURL = postURL;
        }

        public string ToTRHTML(bool even = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<tr>");
  
            sb.AppendLine(string.Format(@"<td>{0}</td>", ID));
            sb.AppendLine(string.Format(@"<td><a href=""{0}"">{1}</a></td>", PostPage, Title));
            sb.AppendLine(string.Format(@"<td>{0}%</td>", Acceptance));
            sb.AppendLine(string.Format(@"<td>{0}</td>", Difficulty));

            sb.AppendLine(@"</tr>");

            return sb.ToString();
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", ID, Title);
        }
    }
}
