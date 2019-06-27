
using System.Text.RegularExpressions;

namespace DYLS.Common.Utils
{
    public static class RegularHelper
    {
        /// <summary>
        /// 验证电话号码
        /// </summary>
        /// <param name="strTelephone"></param>
        /// <returns></returns>
        public static bool IsTelephone(string strTelephone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strTelephone, @"^(\d{3,4}-)?\d{6,8}$");
        }

        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobile(string mobile)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(mobile, @"^(0|86|17951)?1[23456789]\d{9}$");
            //1[345789]\d{9}$
        }

        /// <summary>
        /// 验证身份证号
        /// </summary>
        /// <param name="strIdcard"></param>
        /// <returns></returns>
        public static bool IsIDcard(string strIdcard)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strIdcard, @"(^\d{18}$)|(^\d{15}$)");
        }

        /// <summary>
        /// 验证是否是数字, 可以有负号, 小数点
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsDecimal(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return false;
            }
            Regex regex = new Regex(@"^(-)?\d+(\.\d+)?$");
            if (regex.IsMatch(number))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证是否是数字,只能是纯数字 不能有负号, 小数点
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumber(string strNumber)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strNumber, @"^[0-9]*$");
        }
        /// <summary>
        /// 验证邮编
        /// </summary>
        /// <param name="strPostalcode"></param>
        /// <returns></returns>
        public static bool IsPostalcode(string strPostalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strPostalcode, @"^\d{6}$");
        }
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns></returns>
        public static bool IsEmail(string strEmail)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strEmail, @"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?");
        }
        /// <summary>
        /// 验证Url
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static bool IsUrl(string strUrl)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strUrl, @"^[A-Za-z]+://[A-Za-z0-9-_]+\.[A-Za-z0-9-_%&?/.=]+$");
        }

        /// <summary>
        /// 校验字符串, 过了返回true
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool ChkString(string title, int min = -1, int max = -1)
        {
            if (string.IsNullOrEmpty(title))
            {
                return false;
            }
            if (min > -1 && title.Length < min)
            {
                return false;
            }
            if (max > -1 && title.Length > max)
            {
                return false;
            }
            return true;
        }

        public static bool IsTime(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^[0-9]{2}:[0-9]{2}$");
        }
    }
}
