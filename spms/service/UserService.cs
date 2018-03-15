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
    }
}
