using spms.dao;
using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.service
{
    class UserService
    {
        private UserDAO userDAO = new UserDAO();

        /// <summary>
        /// 根据身份证号获取用户
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public User GetByIdCard(string idCard)
        {
            return userDAO.GetByIdCard(idCard);
        }
        /// <summary>
        /// 获得全部未删除user
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers() {
            return userDAO.GetExistUsers();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(User user) {
            user.Is_Deleted = 1;
            userDAO.UpdateByPrimaryKey(user);
        }
        /// <summary>
        ///  插入一个新用户，业务层添加
        /// </summary>
        public void InsertUser(User user) {
            user.Gmt_Create = DateTime.Now;
            user.Gmt_Modified = DateTime.Now;
            user.Is_Deleted = 0;
            userDAO.Insert(user);
        }
        /// <summary>
        /// 更新用户，根据主键
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            
            user.Gmt_Modified = DateTime.Now;
             
            userDAO.UpdateByPrimaryKey(user);
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<User> SelectByCondition(User user) {
            return userDAO.SelectByCondition(user);
        }
        /// <summary>
        /// 查重，若存在重复，返回false
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        public bool CheckExistByPhoneAndIDCard(string idCard,string phoneNum) {
            var userByIDCard = userDAO.GetByIdCard(idCard);
            var userByPhone = userDAO.GetByPhone(phoneNum);
            var checkResult = false;
            //如果有一个不为空，则说明存在重复，返回false
            if (userByIDCard!=null || userByPhone!=null) {
                checkResult = true;
            }
            return checkResult;
        }
        /// <summary>
        /// 通过手机号获取User
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public User GetByPhone(string Phone)
        {
            return userDAO.GetByPhone(Phone);
        }
    }
}
