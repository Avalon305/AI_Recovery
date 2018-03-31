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

        private static string WEBQUESTION = "question";

        //1.定时器
        public Timer bigDataTimer;
        SetterDAO setterDao = new SetterDAO();
        UploadManagementService uploadManagementService = new UploadManagementService();

        //2. 构造函数中初始化定时器,新建即开始工作,参数为间隔时间
        public BigDataOfficer(int disTime)
        {
            //!HttpSender.Ping() ||
            if ( string.IsNullOrEmpty(setterDao.getSetter().Set_Unique_Id))
            {
                //网路不通 或 未注册 不上传
                return;
            }
            //实例化Timer类
            bigDataTimer = new Timer(Run, null, 0, disTime);
        }

        //指挥官停止工作
        public void Stop()
        {
            // 停止定时器并执行清理工作
            bigDataTimer.Dispose();
        }

        //定时器轮询方法
        public void Run(object state)
        {
            Upload();
        }

        //3.upload方法中上传    接受返回结果后删除     测试在线，本地无记录时的不发送。
        private void Upload()
        {
            var result = uploadManagementService.ListLimit30();
            foreach (var uploadManagement in result)
            {
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