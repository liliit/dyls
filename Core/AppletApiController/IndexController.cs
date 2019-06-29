using DYLS.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DYLS.AppletApiController
{
    public class IndexController:ControllerBase
    {
        [HttpPost]
        public ActionResult Index()
        {
            return JsonResultHelper.Success("123");
        }
    }
}
