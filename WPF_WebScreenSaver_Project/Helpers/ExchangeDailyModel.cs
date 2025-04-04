using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class ExchangeDailyModel
    {
        /// <summary>
        /// 货币名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 现汇买入价
        /// </summary>
        public string xhmrj { get; set; }
        /// <summary>
        /// 现钞买入价
        /// </summary>
        public string xcmrj { get; set; }
        /// <summary>
        /// 现汇卖出价
        /// </summary>
        public string xhmcj { get; set; }
        /// <summary>
        /// 现钞卖出价
        /// </summary>
        public string xcmcj { get; set; }
        /// <summary>
        /// 中行折算价
        /// </summary>
        public string zhzsj { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public string publishTime { get; set; }
        
        public DateTime? publishTimeToDateTime
        {
            get
            {
                DateTime dateTime;
                IFormatProvider ifp = new CultureInfo("zh-CN", true);
                if (DateTime.TryParseExact(publishTime, "yyyy.MM.dd HH:mm:ss", ifp, DateTimeStyles.None, out dateTime))
                {
                    return dateTime;
                }
                {
                    return null;
                }
            }
        }
    }
}
