﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DYLS.Model.Db.Wx
{
    [Table("Wx_User")]
    public class Wx_User:DbBaseModel
    {
        public string Applet_OpenId { get; set; }
        public string NickName { get; set; }
        public int Gender { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string AvatarUrl { get; set; }
        public string UnionId { get; set; }
    }
}
