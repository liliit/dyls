using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Pager.Base
{
    [Serializable]
    public class BasePager
    {
        private long _pageSize = 10;

        public long CurrentPage { get; set; } = 1;

        [JsonIgnore]
        public long PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > 20 ? 20 : value; }
        }

        public long TotalPage
        {
            get
            {
                return TotalPage % PageSize == 0 ? TotalPage / PageSize : TotalPage / PageSize + 1;
            }
        }

        public long Id { get; set; } = -1;

        public string key { get; set; } = "";

        public long TotalRecord { get; set; } = -1;
    }
}
