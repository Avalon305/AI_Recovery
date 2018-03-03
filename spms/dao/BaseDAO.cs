using spms.util;
using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace spms.dao
{
    class BaseDAO<T> where T : class
    {
        public List<T> ListAll()
        {
            using (var conn = DbUtil.getConn())
            {
                conn.Open();

                var result = conn.GetAll<T>().ToList();
                return result;
            }

        }

        public T Load(Object primaryKey)
        {
            using (var conn = DbUtil.getConn())
            {
                conn.Open();

                var result = conn.Get<T>(primaryKey);
                return result;
            }

        }

        public long Insert(T t)
        {
            using (var conn = DbUtil.getConn())
            {
                conn.Open();

                var result = conn.Insert<T>(t);
                return result;
            }
        }

        public long BatchInsert(List<T> list)
        {
            using (var conn = DbUtil.getConn())
            {
                conn.Open();

                var result = conn.Insert(list);
                    return result;
            }
        }

        public Boolean UpdateByPrimaryKey(T t)
        {
            using (var conn = DbUtil.getConn())
            {
                conn.Open();

                var result = conn.Update<T>(t);
                return result;
            }
        }
 

        public Boolean DeleteByPrimaryKey(T t)
        {
            using (var conn = DbUtil.getConn())
            {
                conn.Open();

                var result = conn.Delete<T>(t);
                return result;
            }
        }

        public Boolean DeleteAll()
        {
            using (var conn = DbUtil.getConn())
            {
                conn.Open();

                var result = conn.DeleteAll<T>();
                return result;
            }
        }
    }
}
