using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Db.Base
{
    public class DbBaseModel
    {
        public long Id { get; set; }
        public long Sort { get; set; } = 0;
        public int Flag { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;
    }
}
