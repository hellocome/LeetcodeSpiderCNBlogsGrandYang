using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetcodeSpiderCNBlogsGrandYang
{
    class Program
    {
        static void Main(string[] args)
        {
            CNBlogsGrandYangPageBuilder builder = new CNBlogsGrandYangPageBuilder();
            builder.BuildSite(AppDomain.CurrentDomain.BaseDirectory + "\\CNBlogLeetcode");
            //Console.Read();
        }
    }
}
