


using DYLS.Model.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DYLS.Common.Utils
{
    public static class JsonResultHelper
    {
        /// <summary>
        /// 生成配置
        /// </summary>
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(), //小驼峰
            //Converters = new List<JsonConverter>() { new JavaScriptDateTimeConverter() } //javascript时间格式
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
        /// Url过期
        /// </summary>
        /// <returns></returns>
        public static JsonResult UrlExpire()
        {
            return JsonResult(503, "Url过期");
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
        /// 不存在的数据
        /// </summary>
        /// <returns></returns>
        public static JsonResult NotExist()
        {
            return JsonResult(401, "不存在的数据");
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
        /// <returns></returns>
        public static JsonResult Error(object data = null)
        {
            return JsonResult(1, "error", data);
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
        /// 无效签名
        /// </summary>
        /// <returns></returns>
        public static JsonResult InvalidSign()
        {
            return JsonResult(801, "无效签名");
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
        /// 无效密码
        /// </summary>
        /// <returns></returns>
        public static JsonResult InvalidPwd()
        {
            return JsonResult(602, "无效密码");
        }

        /// <summary>
        /// 存在同名用户
        /// </summary>
        /// <returns></returns>
        public static JsonResult ExistUser()
        {
            return JsonResult(603, "存在同名用户");
        }

        /// <summary>
        /// 存在同名用户
        /// </summary>
        /// <returns></returns>
        public static JsonResult ExistMobile()
        {
            return JsonResult(604, "手机号已经存在!");
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

        /// <summary>
        /// 失败
        /// </summary>
        /// <returns></returns>
        public static JsonResult Fail()
        {
            return JsonResult(1, "操作失败");
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <returns></returns>
        public static JsonResult Alert(string msg)
        {
            return JsonResult(101, msg);
        }

        /// <summary>
        /// BalanceLacking
        /// </summary>
        /// <returns></returns>
        public static JsonResult BalanceLacking()
        {
            return JsonResult(5100, "余额不足");
        }

        /// <summary>
        /// 更新签名key
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonResult UpdateSignKey(object data)
        {
            return JsonResult(-1, "ok", data);
        }
    }
}
