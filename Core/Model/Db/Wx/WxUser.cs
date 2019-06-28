using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DYLS.Model.Db.Wx
{
    [Table("Wx_User")]
    public class WxUser:DbBaseModel
    {
        public string OpenID { get; set; }
        public string NickName { get; set; }
        public int Sex { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string HeadImgUrl { get; set; }
        public string UnionID { get; set; }
    }
}
