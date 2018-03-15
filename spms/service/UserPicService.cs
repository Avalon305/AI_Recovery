using spms.constant;
using spms.entity;
using spms.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.service
{
    class UserPicService
    {

        public byte[] GetPictureData(string idCard, int packNum)
        {
            UserService userService = new UserService();
            User u = userService.GetByIdCard(idCard);
            string path = CommUtil.GetUserPic(u.User_PhotoLocation);
            return GetPictureData(path, packNum, ProtocolConstant.PIC_PACK_SIZE);
        }

        public Int16 GetPicturePackCount(string idCard)
        {
            UserService userService = new UserService();
            User u = userService.GetByIdCard(idCard);
            string path = CommUtil.GetUserPic(u.User_PhotoLocation);

            return (Int16)GetPicturePackCount(path, ProtocolConstant.PIC_PACK_SIZE);
        }

        /// <summary>
        /// 获取图片的总包数
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="packSize">每包数据的大小，字节数</param>
        /// <returns></returns>
        private long GetPicturePackCount(string imagePath, int packSize)
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Open))
            {
                long len = fs.Length;
                long total = len / packSize;
                if (len % packSize != 0)
                {
                    total++;
                }
                return total;
            }
        }

        /// <summary>
        /// 获取照片数据
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="packNum">第几包，从零开始</param>
        /// <param name="packSize">每包数据的大小，字节数</param>
        /// <returns></returns>
        private byte[] GetPictureData(string imagePath, int packNum, int packSize)
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Open))
            {
                byte[] byteData = new byte[fs.Length];
                fs.Read(byteData, 0, byteData.Length);
                long len = byteData.Length;
                long readSize = len - packNum * packSize < packSize ? len - packNum * packSize : packSize;
                byte[] result = new byte[readSize];
                Array.Copy(byteData, packNum * packSize, result, 0, readSize);
                return result;
            }
        }
    }
}
