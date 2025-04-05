//using AngleSharp.Dom;
//using AngleSharp;
using HtmlAgilityPack;
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
            List<ExchangeDailyModel> list = new List<ExchangeDailyModel>();
            try
            {
                HtmlDocument doc =await new HtmlWeb().LoadFromWebAsync(url);
                var audNode = doc.DocumentNode.SelectNodes("//tr/td[1]");
                foreach (var node in audNode)
                {
                    if(node.InnerHtml== "澳大利亚元")
                    {
                        var nodes = node.ParentNode.Elements("td").ToList();
                        ExchangeDailyModel audModel = new ExchangeDailyModel()
                        {
                            name = nodes[0].InnerHtml,
                            xhmrj = nodes[1].InnerHtml,
                            xcmrj = nodes[2].InnerHtml,
                            xhmcj = nodes[3].InnerHtml,
                            xcmcj = nodes[4].InnerHtml,
                            zhzsj = nodes[5].InnerHtml,
                            publishTime = nodes[6].InnerHtml,
                        };
                        if (audModel.name == "澳大利亚元")
                        {
                            audModel.name = "AUD";
                            list.Add(audModel);
                        }
                        else
                        {
                            throw new Exception("BOC网页爬虫无法找到澳元汇率");
                        }
                    }
                    if (node.InnerHtml == "美元")
                    {
                        var nodes = node.ParentNode.Elements("td").ToList();
                        ExchangeDailyModel audModel = new ExchangeDailyModel()
                        {
                            name = nodes[0].InnerHtml,
                            xhmrj = nodes[1].InnerHtml,
                            xcmrj = nodes[2].InnerHtml,
                            xhmcj = nodes[3].InnerHtml,
                            xcmcj = nodes[4].InnerHtml,
                            zhzsj = nodes[5].InnerHtml,
                            publishTime = nodes[6].InnerHtml,
                        };
                        if (audModel.name == "美元")
                        {
                            audModel.name = "USD";
                            list.Add(audModel);
                        }
                        else
                        {
                            throw new Exception("BOC网页爬虫无法找到澳元汇率");
                        }
                    }
                }
                //ExchangeDailyModel audModel = new ExchangeDailyModel()
                //{
                //    name = audNode.ChildNodes[0].InnerHtml,
                //    xhmrj = audNode.ChildNodes[1].InnerHtml,
                //    xcmrj = audNode.ChildNodes[2].InnerHtml,
                //    xhmcj = audNode.ChildNodes[3].InnerHtml,
                //    xcmcj = audNode.ChildNodes[4].InnerHtml,
                //    zhzsj = audNode.ChildNodes[5].InnerHtml,
                //    publishTime = audNode.ChildNodes[6].InnerHtml + " " + audNode.ChildNodes[7].InnerHtml,
                //};
                //if(audModel.name == "澳大利亚元")
                //{
                //    audModel.name = "AUD";
                //    list.Add(audModel);
                //}
                //else
                //{
                //    throw new Exception("BOC网页爬虫无法找到澳元汇率");
                //}
                //var usdNode = doc.DocumentNode.SelectSingleNode("/html/body/div/div[5]/div[1]/div[2]/table/tbody/tr[28]");
                //ExchangeDailyModel usdModel = new ExchangeDailyModel()
                //{
                //    name = usdNode.ChildNodes[0].InnerHtml,
                //    xhmrj = usdNode.ChildNodes[1].InnerHtml,
                //    xcmrj = usdNode.ChildNodes[2].InnerHtml,
                //    xhmcj = usdNode.ChildNodes[3].InnerHtml,
                //    xcmcj = usdNode.ChildNodes[4].InnerHtml,
                //    zhzsj = usdNode.ChildNodes[5].InnerHtml,
                //    publishTime = usdNode.ChildNodes[6].InnerHtml + " " + audNode.ChildNodes[7].InnerHtml,
                //};
                //if (audModel.name == "美元")
                //{
                //    audModel.name = "USD";
                //    list.Add(audModel);
                //}
                //else
                //{
                //    throw new Exception("BOC网页爬虫无法找到美元汇率");
                //}
                //IConfiguration config = Configuration.Default.WithDefaultLoader();
                //IBrowsingContext context = BrowsingContext.New(config);
                //IDocument document = await context.OpenAsync(url);
                //IHtmlCollection<IElement> elements = document.QuerySelectorAll("tr")
                //    .Where(x => names.Contains(x.Children[0].InnerHtml)).ToCollection();
                //foreach (IElement element in elements)
                //{
                //    ExchangeDailyModel model = new ExchangeDailyModel()
                //    {
                //        name = element.Children[0].InnerHtml,
                //        xhmrj = element.Children[1].InnerHtml,
                //        xcmrj = element.Children[2].InnerHtml,
                //        xhmcj = element.Children[3].InnerHtml,
                //        xcmcj = element.Children[4].InnerHtml,
                //        zhzsj = element.Children[5].InnerHtml,
                //        publishTime = element.Children[6].InnerHtml,
                //    };
                //    if(model.name== "澳大利亚元")
                //    {
                //        model.name = "AUD";
                //    }
                //    else
                //    {
                //        model.name = "USD";
                //    }
                //    list.Add(model);
                //}
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("BOC网页爬虫获取当前汇率时发生异常", ex);
            }
        }


    }
}
