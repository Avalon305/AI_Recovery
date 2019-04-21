using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spms.util
{
    public enum TrainInfoStatus : byte
    {
        //正常  单个用户只可以有一个未完成的数据
        Normal = 0,
        //保存   单个用户只有一个保存状态的数据
        Save = 1,
        //完成   单个用户可以有多个完成的数据
        Finish = 2,
        //废除    单个用户可以有多个废除的数据
        Abandon = 3,
        //暂存    点击写卡的时候，会先删除某用户所有的暂存状态的表，然后根据界面的选项插入一张暂存状态的数据（包含训练信息单1张+n张具体设备训练明细），等到有成功反馈的时候，再将该用户暂存状态的表转为nomal状态
        Temp = 4
    }
}
