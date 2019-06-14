using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PicturePay.Models
{
    public class PictureRecognize
    {
        /// <summary>
        /// 图片转化成bmp格式
        /// </summary>
        /// <param name="PictureAddress">图片地址</param>
        /// <returns>是否转换成功</returns>
        public static bool ChangeBmp(string PictureAddress) 
        {
            return true;
        }
        /// <summary>
        /// 二维码信息识别
        /// </summary>
        /// <param name="PictureAddress">二维码图片地址</param>
        /// <returns>识别到的信息</returns>
        public static string Qr_Code(string PictureAddress) 
        {
            return "Qr_Code";
        }
    }
}