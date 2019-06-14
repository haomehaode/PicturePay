using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PicturePay.Models
{
    public class PictureOperation
    {
        /// <summary>
        /// 8位无符号整数转化为Unicode字符数组
        /// </summary>
        private static char[] ch;
        /// <summary>
        /// 加密信息数组
        /// </summary>
        private static char[] temp;
        /// <summary>
        /// 保存的图片名称
        /// </summary>
        private static string FileName;
        /// <summary>
        /// 商家支付宝号
        /// </summary>
        private string MerchatName;
        /// <summary>
        /// 图片操作类构造函数
        /// </summary>
        /// <param name="MerchatName">商家支付宝号</param>
        public PictureOperation(string MerchatName)
        {
            this.MerchatName = MerchatName;
        }
        /// <summary>
        /// 图片隐写
        /// </summary>
        /// <param name="PaymentCode">支付链接</param>
        /// <returns>是否隐写成功</returns>
        public static bool PictureWrite(string PaymentCode)
        {
            ch = new char[(PaymentCode.Length + 2) * 16];
            for (int i = 0; i < PaymentCode.Length; i++)
            {
                ch[i] = Convert.ToChar(PaymentCode[i]);//将8位无符号整数转化为Unicode字符
            }
            temp = new char[(PaymentCode.Length + 2) * 16]; //temp存放要加密的信息
            int m, n;
            for (n = 0; n < 16; n++)
                temp[n] = Convert.ToChar(0x0001 & '#' >> n);
            for (m = 1; m < PaymentCode.Length + 1; m++)
                for (n = 0; n < 16; n++)
                    temp[16 * m + n] = Convert.ToChar(0x0001 & ch[m - 1] >> n);

            for (n = 0; n < 16; n++)
                temp[16 * m + n] = Convert.ToChar(0x0001 & '#' >> n);
            //读取图片，并将加密信息写到新建图片中保存
            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);//读图片
                FileStream fs1 = new FileStream(FileName, FileMode.Create, FileAccess.Write);//写图片  有问题
                BinaryReader br = new BinaryReader(fs);
                BinaryWriter bw = new BinaryWriter(fs1);
                long size = fs.Length;
                if (size < (PaymentCode.Length + 56))
                {
                    return false;
                }
                else
                {
                    byte write;

                    for (int i = 1, j = 0; i <= size; i++)
                    {
                        write = br.ReadByte();//从当前流中读取下一个字节，并将当前指针指向位置提升一个字节

                        if (i <= 54)
                        {
                            bw.Write(write);
                        }
                        else
                        {
                            if (j < 16 * (PaymentCode.Length + 2))
                                bw.Write(Convert.ToByte((write & 0xfe) + temp[j]));//从temp中取出一个bit，放到当前字节的最后一位，然后再将这一字节的数据写到图片里。

                            else
                                bw.Write(write);
                            j++;
                        }
                    }
                    fs.Close();
                    fs1.Close();
                    br.Close();
                    bw.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 图片隐写信息读取
        /// </summary>
        /// <param name="PictureName">图片名称</param>
        /// <returns>读取到的信息</returns>
        public static string PictureRead(string PictureName) 
        {
            try
            {
                FileStream filestream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                BinaryReader binaryreader = new BinaryReader(filestream);
                filestream.Seek(54L, 0);
                byte reader;
                char ch;
                int temp = 0;
                int state = 0;
                while (Convert.ToChar(temp) != '#' || state != 2)
                {
                    temp = 0;
                    for (int k = 0; k < 16; k++)
                    {
                        reader = binaryreader.ReadByte();
                        temp += (reader & 0x01) << k;//将读取的字节最后一位存放到temp变量中（左移k位实现）
                    }

                    ch = Convert.ToChar(temp);
                    if (ch != '#' && state != 1)
                    {
                        return "false";
                    }
                    if (ch != '#' && state == 1)
                    {
                        return ch.ToString();

                    }
                    if (ch == '#')
                        state++;
                }
                return "false";
            }
            catch (Exception)
            {
                return "false";
            }
        } 
        
    }
}
                               
        