using DYLS.Common.Filter.Web.Applet;
using Microsoft.AspNetCore.Mvc;

namespace DYLS.AppletApiController
{
    /// <summary>
    /// 基类
    /// </summary>
    [Route("appletapi/[controller]/[action]")]
    [AuthorAttributeFilter]
    public class ControllerBase : Controller
    {

    }
}