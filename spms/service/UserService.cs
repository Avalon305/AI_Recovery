using spms.dao;
using spms.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.service
{
    public class UserService
    {
        UserDAO userDAO = new UserDAO();
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
