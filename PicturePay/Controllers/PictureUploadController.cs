using PicturePay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PicturePay.Controllers
{
    public class PictureUploadController : Controller
    {
        //
        // GET: /PictureUpload/

        public ActionResult Index(string s)
        {
            return View();
            //判断上传是否成功
            if (PictureTransport.PictureUpload()) 
            {
                
            }
            //进行图片类型转换和二维码信息读取
            if(PictureRecognize.ChangeBmp(s))
            {
               string informagton= PictureRecognize.Qr_Code(s);
            } 
            //进行图片隐写
        }

    }
}
