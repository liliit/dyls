using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DYLS.WebController
{
    public class HomeController:ControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
