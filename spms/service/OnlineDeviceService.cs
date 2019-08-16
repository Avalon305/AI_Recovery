using spms.bean;
using spms.constant;
using spms.dao;
using spms.dao.app;
using spms.entity;
using spms.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.service
{
    class OnlineDeviceService
    {
        private OnlineDeviceDAO onlineDeviceDAO = new OnlineDeviceDAO();
        /// <summary>
        /// 保存或更新在线信息，没有该ID则保存，有则更新时间
        /// </summary>
        /// <param name="frameBean"></param>
        public void SavaOrUpdateOnlineInfo(TcpFrameBean frameBean)
        {
            var onlineDevice = onlineDeviceDAO.GetByClientId(frameBean.TerminalId);
            if (onlineDevice == null)//插入
            {
                onlineDevice = new OnlineDevice();

                string name_zh = "";
                string name_en = "";

                //设置设备中英文设备名称
                switch (frameBean.DeviceType)
                {
                    case DeviceType.P00:
                        name_en = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.EN, "NewDev.Rowing");
                        name_zh = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.ZH, "NewDev.Rowing");
                        break;
                    case DeviceType.P01:
                        name_en = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.EN, "NewDev.ChestPress");
                        name_zh = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.ZH, "NewDev.ChestPress");
                        break;
                    case DeviceType.P02:
                        name_en = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.EN, "NewDev.HorizontalLegPress");
                        name_zh = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.ZH, "NewDev.HorizontalLegPress");
                        break;
                    case DeviceType.P03:
                        name_en = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.EN, "NewDev.AbdominalMuscleTraining");
                        name_zh = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.ZH, "NewDev.AbdominalMuscleTraining");
                        break;
                    case DeviceType.P04:
                        name_en = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.EN, "NewDev.TricepsTraining");
                        name_zh = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.ZH, "NewDev.TricepsTraining");
                        break;
                    case DeviceType.P05:
                        name_en = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.EN, "NewDev.LegAbduction");
                        name_zh = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.ZH, "NewDev.LegAbduction");
                        break;
                    case DeviceType.P06:
                        name_en = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.EN, "NewDev.LegInturn");
                        name_zh = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.ZH, "NewDev.LegInturn");
                        break;
                    case DeviceType.P07:
                        name_en = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.EN, "NewDev.ButterflyMachine");
                        name_zh = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.ZH, "NewDev.ButterflyMachine");
                        break;
                    case DeviceType.P08:
                        name_en = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.EN, "NewDev.ReverseButterflyMachine");
                        name_zh = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.ZH, "NewDev.ReverseButterflyMachine");
                        break;
                    case DeviceType.P09:
                        name_en = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.EN, "NewDev.SittingBackExtender");
                        name_zh = LanguageUtils.GetLanuageStrByLanguageAndKey(LanguageUtils.ZH, "NewDev.SittingBackExtender");
                        break;
                    default:
                        break;
                }

                onlineDevice.od_gmt_modified = DateTime.Now;
                onlineDevice.od_clientid = frameBean.TerminalId;
                onlineDevice.od_clientname_en = name_en;
                onlineDevice.od_clientname_ch = name_zh;
                //插入记录
                onlineDeviceDAO.Insert(onlineDevice);
                //插入至上传表
                UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
                uploadManagementDao.Insert(new UploadManagement(onlineDevice.pk_od_id, "bdl_onlinedevice", 0));
            }
            else//更新
            {
                //更新心跳时间
                onlineDeviceDAO.UpdateOnlineTime(onlineDevice.pk_od_id,DateTime.Now);
                //插入至上传表
                UploadManagementDAO uploadManagementDao = new UploadManagementDAO();
                uploadManagementDao.Insert(new UploadManagement(onlineDevice.pk_od_id, "bdl_onlinedevice", 1));
            }
        }
    }
}
