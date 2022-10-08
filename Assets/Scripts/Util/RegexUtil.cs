using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 正则匹配
/// </summary>
public static class RegexUtil
{
    /// <summary>
    /// 匹配手机号
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static bool MobilePhoneMatch(string number)
    {
        return Regex.IsMatch(number, @"^1[123456789]\d{9}$");
    }
    /// <summary>
    /// 是否为数字
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static bool IsNumber(string number)
    {
        return Regex.IsMatch(number, @"^[0-9]*$");
    }
    /// <summary>
    /// 匹配邮箱
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool EmailMatch(string email)
    {
        return Regex.IsMatch(email, @"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?");
    }
    /// <summary>
    /// 匹配身份证
    /// </summary>
    /// <param name="idCard"></param>
    /// <returns></returns>
    public static bool IsIdCard(string idCard)
    {
        if (string.IsNullOrEmpty(idCard))
        {
            return false;
        }

        if (idCard.Length == 15)
        {
            return Regex.IsMatch(idCard, @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$");
        }
        else if (idCard.Length == 18)
        {
            return Regex.IsMatch(idCard, @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$", RegexOptions.IgnoreCase);
        }
        else
        {
            return false;
        }
    }
}
