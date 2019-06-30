using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DYLS.Model.Db.Web
{
    [Table("ls_ad")]
    public class Ad:DbBaseModel
    {
        /// <summary>
        /// 当前类型
        /// </summary>
        public AdType AdType { get; set; }

        /// <summary>
        /// Banner图
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 商家ID
        /// </summary>
        public int ShopId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }

    public enum AdType
    {
        banner,
        shop
    }
}
