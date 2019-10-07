using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recovery.util;
using Recovery.http.entity;
using Recovery.service;
using Recovery.entity;
using Recovery.dao;
using Recovery.http.dto;
using NLog;

namespace Recovery.http
{
    //大数据交互指挥官
    public class BigDataOfficer
    {

        SetterDAO setterDao = new SetterDAO();
        UploadManagementService uploadManagementService = new UploadManagementService();
        private static Logger logger = LogManager.GetCurrentClassLogger();





        /// <summary>
        /// 定时器轮询方法
        /// </summary>
        /// <param name="threadCount"></param>
        ///
        public void Run()
        {

            if (setterDao.ListAll() == null)
            {
                //网路不通 或 未注册 不上传//Console.WriteLine("大数据线程实例化run方法-执行:");
                return;
            }



            //从上传表查询30条数据
            var result = uploadManagementService.ListLimit30();

            if (result == null)
            {
                //Console.WriteLine("大数据线程RUN方法-result==null");
            }
            //遍历查询到的数据集合
            foreach (var uploadManagement in result)
            {
                //生成新的SendMsgDTO对象（上传云平台的最终对象）
                SendMsgDTO sendMsgDto = new SendMsgDTO();

                //根据表名，给字段dataType（数据所在实体类）赋值
                switch (uploadManagement.UM_DataTable)
                {
                    case "bdl_auth":
                        sendMsgDto.dataType = "Auther";
                        break;
                    case "bdl_customdata":
                        sendMsgDto.dataType = "CustomData";
                        break;
                    case "bdl_datacode":
                        sendMsgDto.dataType = "DataCode";
                        break;
                    case "bdl_deviceprescription":
                        sendMsgDto.dataType = "DevicePrescription";
                        break;
                    case "bdl_deviceset":
                        sendMsgDto.dataType = "DeviceSet";
                        break;
                    case "bdl_devicesort":
                        sendMsgDto.dataType = "DeviceSort";
                        break;
                    case "bdl_onlinedevice":
                        sendMsgDto.dataType = "OnlineDevice";
                        break;
                    case "bdl_physicalpower":
                        sendMsgDto.dataType = "PhysicalPower";
                        break;
                    case "bdl_prescriptionresult":
                        sendMsgDto.dataType = "PrescriptionResult";
                        break;
                    case "bdl_set":
                        sendMsgDto.dataType = "Setter";
                        break;
                    case "bdl_symptominfo":
                        sendMsgDto.dataType = "SymptomInfo";
                        break;
                    case "bdl_traininfo":
                        sendMsgDto.dataType = "TrainInfo";
                        break;
                    case "bdl_user":
                        sendMsgDto.dataType = "User";
                        break;
                    case "bdl_error":
                        sendMsgDto.dataType = "Error";
                        break;
                    default:
                        Console.WriteLine("没找到对应的表");
                        //Console.WriteLine(uploadManagement.UM_DataTable);
                        break;
                }

                sendMsgDto.dataExec = uploadManagement.UM_Exec;//操作方式 0是add（insert） 1是update
                sendMsgDto.dataId = uploadManagement.UM_DataId;//数据的id
                sendMsgDto.belongProduct = "Recovery";//项目名称必须是这个，与云平台相一致
                if(sendMsgDto.dataType == "Error")
                {
                    sendMsgDto.belongProduct = "RecoveryError";
                }


                // Console.WriteLine("大数据线程实例化Upload方法-table:" + uploadManagement.UM_DataTable);
                //1.根据上传表内容查询具体数据（这里的数据是已经转换成json串形式），赋值给字段content
                ServiceResult serviceResult = null;
                try
                {
                    serviceResult = uploadManagementService.GetServiceResult(uploadManagement);
                }
                catch(Exception e)
                {
                    logger.Error("uploadManagement上传云平台失败");
                }

                /*
                int i = 1;
                Console.WriteLine("这里是上传表的每一条内容" + i + "数字" + uploadManagement.Pk_UM_Id + uploadManagement.UM_DataId + uploadManagement.UM_Exec);
                i++;*/
                if (serviceResult == null)
                {
                    //没有查到返回
                    //Console.WriteLine("上传表查询失败____________________");
                    logger.Error("上传表查询失败，" + DateTime.Now.ToString());
                    return;
                }
                sendMsgDto.content = serviceResult.Data;
                //用于接受云服务器端返回的字符串
                string strWebResult = "";
                //创建一个新的用来接受服务器端返回结果的对象
                WebResult webResult = new WebResult();

                //这里是url的后缀，每一条数据都是发送到这个地方
                serviceResult.URL = "msg";

                //将发送的实体类SendMsgDTO装成json串
                serviceResult.Data = JsonTools.Obj2JSONStrNew<SendMsgDTO>(sendMsgDto);

                //2.上传和接受云服务器端返回的字符串
                strWebResult = HttpSender.POSTByJsonStr(serviceResult.URL, serviceResult.Data);
              

                //将接受到的字符串赋值给webResult对象
                webResult = JsonTools.DeserializeJsonToObject<WebResult>(strWebResult);

                //Console.WriteLine("线程"+ threadCount +"   id: "+webResult.dataId +"type: "+ webResult.dataType + "结果："+webResult.result);


                //3.根据返回结果删除
                //返回值result为0或1删除上传表所对应的的内容，2则跳出循环，不做处理，隔五分钟再次上传
                if (webResult.result == "0")
                {
                    uploadManagementService.deleteByPrimaryKey(uploadManagement.Pk_UM_Id);
                }
                else
                 if (webResult.result == "1")
                {
                    uploadManagementService.deleteByPrimaryKey(uploadManagement.Pk_UM_Id);
                }
                else
                 if (webResult.result == "2")
                {
                    break;//不做处理，五分钟后再发，目的是等待云服务器建表
                }
                else
                {
                    logger.Error("失败的内容：" + webResult.result + "，" + DateTime.Now.ToString());
                    //Console.WriteLine("失败的内容：" + webResult.result);
                }
                //Console.WriteLine("-----------------------------返回结果 dataid"+webResult.dataId +"type:"+webResult.dataType+"result:"+webResult.result);
            }
        }


    }
}