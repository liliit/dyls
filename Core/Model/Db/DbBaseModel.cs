

using System;

namespace DYLS.Model.Db
{
    /// <summary>
    /// db模型基类
    /// </summary> 
    [Serializable]
    public class DbBaseModel
    {
        public long Id { get; set; }
        public byte Flag { get; set; } = 0;
        public int OrderID { get; set; }
        public virtual DateTime CreateTime { get; set; } = DateTime.Now;
        public virtual DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}
