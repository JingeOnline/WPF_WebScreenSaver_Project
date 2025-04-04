using AngleSharp.Dom;
using AngleSharp;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharedLibrary.Helpers
{
    public class BocHelper
    {

        public static async Task<List<ExchangeDailyModel>> GetExchangeDailyModelsFromBocHomePage()
        {
            string html;
            //中国银行外汇牌价首页
            string url = "https://www.boc.cn/sourcedb/whpj/";
            //获取汇率的名称
            List<string> names = new List<string> { "澳大利亚元", "美元" };
            List<ExchangeDailyModel> list = new List<ExchangeDailyModel>();
            try
            {
                IConfiguration config = Configuration.Default.WithDefaultLoader();
                IBrowsingContext context = BrowsingContext.New(config);
                IDocument document = await context.OpenAsync(url);
                IHtmlCollection<IElement> elements = document.QuerySelectorAll("tr")
                    .Where(x => names.Contains(x.Children[0].InnerHtml)).ToCollection();
                foreach (IElement element in elements)
                {
                    ExchangeDailyModel model = new ExchangeDailyModel()
                    {
                        name = element.Children[0].InnerHtml,
                        xhmrj = element.Children[1].InnerHtml,
                        xcmrj = element.Children[2].InnerHtml,
                        xhmcj = element.Children[3].InnerHtml,
                        xcmcj = element.Children[4].InnerHtml,
                        zhzsj = element.Children[5].InnerHtml,
                        publishTime = element.Children[6].InnerHtml,
                    };
                    if(model.name== "澳大利亚元")
                    {
                        model.name = "AUD";
                    }
                    else
                    {
                        model.name = "USD";
                    }
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("BOC网页爬虫获取当前汇率时发生异常", ex);
            }
        }


    }
}
