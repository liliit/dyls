

using Newtonsoft.Json;
using System;

namespace DYLS.Model.Pager
{
    [Serializable]
    public class BasePager
    {

        private long _pageSize = 20;

        /// <summary>
        /// 当前码 默认 1
        /// </summary>
        public long CurrentPage { get; set; } = 1;

        /// <summary>
        ///  每页大小
        /// </summary>
        [JsonIgnore]
        public long PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > 20 ? 20 : value; }
        }

        /// <summary>
        /// 总页码 此参数不必传入, 由系统自行计算
        /// </summary>
        public long TotalPage
        {
            get
            {
                return TotalRecord % PageSize == 0 ? TotalRecord / PageSize : TotalRecord / PageSize + 1;
            }
        }

   
        /// <summary>
        ///  表示一个id
        /// </summary>
        public long Id { get; set; } = -1;

        /// <summary>
        ///  表示一个key
        /// </summary>
        public string Key { get; set; } = "";

        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalRecord { get; set; } = -1;
    }
}
