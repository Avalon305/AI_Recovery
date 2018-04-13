using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using spms.util;
using spms.http.entity;
using spms.service;
using spms.entity;
using spms.dao;

namespace spms.http
{
    //大数据交互指挥官
    public class BigDataOfficer
    {
        private static string WEBSUCCESS = "success";

        //private static string WEBQUESTION = "question";

        
        SetterDAO setterDao = new SetterDAO();
        UploadManagementService uploadManagementService = new UploadManagementService();

       



        //定时器轮询方法
        public void Run()
        {
            if (setterDao.ListAll()==null)
            {
                //网路不通 或 未注册 不上传
                return;
            }

            //Console.WriteLine("大数据线程实例化run方法-执行:");
            var result = uploadManagementService.ListLimit30();
            if (result==null) {
                //Console.WriteLine("大数据线程RUN方法-result==null");
            }
            foreach (var uploadManagement in result)
            {
               // Console.WriteLine("大数据线程实例化Upload方法-table:" + uploadManagement.UM_DataTable);
                //1.查询
                ServiceResult serviceResult = uploadManagementService.GetServiceResult(uploadManagement);
                if (serviceResult == null)
                {
                    //没有查到返回
                    return;
                }

                //2.上传
                string webResult = HttpSender.POSTByJsonStr(serviceResult.URL, serviceResult.Data);
                //3.根据结果删除
                if (webResult.Contains(BigDataOfficer.WEBSUCCESS))
                {
                    uploadManagementService.deleteByPrimaryKey(uploadManagement.Pk_UM_Id);
                }
                else
                {
                    //问题数据不作处理
                }
            }
        }

        
    }
}