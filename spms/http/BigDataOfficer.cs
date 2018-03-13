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
        private static  string WEBSUCCESS = "success";
        private static  string WEBQUESTION = "question";
        //1.定时器
        public Timer bigDataTimer;
        //2. 构造函数中初始化定时器,新建即开始工作,参数为间隔时间
        public BigDataOfficer(int disTime) {
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
        private void Run(object state)
        {
            string pingJsonStr = JsonTools.Obj2JSONStrNew(new HttpHeartBeat("ping"));
            //测试是不是网络畅通，畅通则启动upload方法
            HttpSender httpSender = new HttpSender("communicationController/analysisJson", pingJsonStr);
            string pingResult = httpSender.sendDataToWebPlatform();
            HttpHeartBeat httpHeartBeat  = JsonTools.DeserializeJsonToObject<HttpHeartBeat>(pingResult);
            //网络畅通，查询30条数据上传
            if (httpHeartBeat!=null&& httpHeartBeat.username.Equals("pang")){
                //不属于未注册状态
                SetterDAO setterDAO = new SetterDAO();
                Setter setter = setterDAO.getSetter();
                if (setter.Set_Unique_Id!=null&& setter.Set_Unique_Id .Equals("")) {
                    Upload();
                }
              
            }
        }

      
      
         
        //3.upload方法中上传    接受返回结果后删除     测试在线，本地无记录时的不发送。
        private void Upload() {
            UploadManagementService uploadManagementService = new UploadManagementService();
            var result = uploadManagementService.ListLimit30();
            foreach(var uploadManagement in result) {
                //1.查询
                ServiceResult serviceResult = uploadManagementService.GetServiceResult(uploadManagement);
                //2.上传
                string webResult = HttpSender.POSTByJsonStr(serviceResult.URL, serviceResult.Data);
                //3.根据结果删除
                if (webResult.Equals(BigDataOfficer.WEBSUCCESS))
                {
                    uploadManagementService.deleteByPrimaryKey(uploadManagement.Pk_UM_Id);
                }
                else {
                    //问题数据不作处理
                }
            }
        }
        


        
    }
}
