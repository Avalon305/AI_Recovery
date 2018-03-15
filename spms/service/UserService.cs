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

        public User GetByIdCard(string idCard)
        {
            return userDAO.GetByIdCard(idCard);
        }
    }
}
