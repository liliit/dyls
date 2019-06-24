using Microsoft.AspNetCore.Mvc;
using Model.Response;
using Newtonsoft.Json;

namespace Kachannel.Tencentads.Common.Utils
{
    public static class JsonResultHelper
    {
        /// <summary>
        /// 生成配置
        /// </summary>
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(), //小驼峰
            DateFormatHandling = DateFormatHandling.IsoDateFormat
        };


        /// <summary>
        /// 返回一个结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonResult JsonResult(int code, string msg, object data = null)
        {
            var respones = new ResponseResult()
            {
                Code = code,
                Msg = msg,
                Data = data
            };
            return new JsonResult(respones, JsonSerializerSettings);
        }

        /// <summary>
        /// 直接输出json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static JsonResult JsonResult(object obj)
        {
            return new JsonResult(obj, JsonSerializerSettings);
        }

        /// <summary>
        /// 返回ck上传错误消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static JsonResult CkUploadError(string msg)
        {
            return JsonResult(new
            {
                error = new
                {
                    message = msg
                }
            });
        }

        /// <summary>
        /// 超时
        /// </summary>
        /// <returns></returns>
        public static JsonResult Expire()
        {
            return JsonResult(2100, "超时");
        }

        /// <summary>
        /// 返回无效参数
        /// </summary>
        /// <returns></returns>
        public static JsonResult InvalidParameter()
        {
            return JsonResult(901, "无效参数");
        }

        /// <summary>
        /// 无效用户
        /// </summary>
        /// <returns></returns>
        public static JsonResult InvalidUser()
        {
            return JsonResult(1101, "无效用户");
        }

        /// <summary>
        /// 尝试超过最大限制
        /// </summary>
        /// <returns></returns>
        public static JsonResult TryOverflow()
        {
            return JsonResult(1001, "尝试超过最大限制");
        }

        /// <summary>
        /// 禁止访问
        /// </summary>
        /// <returns></returns>
        public static JsonResult NoAccess()
        {
            return JsonResult(1002, "禁止访问");
        }

        /// <summary>
        /// 无效签名
        /// </summary>
        /// <returns></returns>
        public static JsonResult InvalidSignature()
        {
            return JsonResult(501, "无效签名");
        }

        /// <summary>
        /// 无效的验证码
        /// </summary>
        /// <returns></returns>
        public static JsonResult InvalidVerificationCode()
        {
            return JsonResult(701, "无效验证码");
        }

        /// <summary>
        /// 需要登陆
        /// </summary>
        /// <returns></returns>
        public static JsonResult RequiredLogin()
        {
            return JsonResult(601, "需要登陆");
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public static JsonResult Success(object data = null)
        {
            return JsonResult(0, "ok", data);
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static JsonResult Success(string msg, object data)
        {
            return JsonResult(0, msg, data);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <returns></returns>
        public static JsonResult Fail()
        {
            return JsonResult(1, "操作失败");
        }

        /// <summary>
        /// 服务器错误
        /// </summary>
        /// <returns></returns>
        public static JsonResult Error()
        {
            return JsonResult(500, "服务器错误");
        }

        /// <summary>
        /// 服务器错误
        /// </summary>
        /// <returns></returns>
        public static JsonResult Error(string msg)
        {
            return JsonResult(500, msg);
        }
    }
}
