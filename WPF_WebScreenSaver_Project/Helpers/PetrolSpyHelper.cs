using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using HtmlAgilityPack;

namespace WPF_WebScreenSaver_Project.Helpers
{
    public class PetrolSpyHelper
    {
        public static async Task<string> GetPetrolPrice()
        {
            string url = "https://petrolspy.com.au/graph/canberra_U91E10.svg";
            HtmlDocument doc = await new HtmlWeb().LoadFromWebAsync(url);
            var audNode = doc.DocumentNode.SelectSingleNode("/svg/g[2]/text");
            //Debug.WriteLine(audNode.InnerHtml);
            string text= audNode.InnerText;
            text=text.Replace("Current average:","堪培拉91号汽油今日平均价格：");
            return text;
        }
    }
}
