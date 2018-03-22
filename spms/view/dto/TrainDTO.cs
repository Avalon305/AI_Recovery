using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using spms.constant;
using spms.entity;
using spms.util;

namespace spms.view.dto
{
    class TrainDTO
    {
        public TrainInfo trainInfo { get; set; }

        public DevicePrescription devicePrescription { get; set; }

        public PrescriptionResult prescriptionResult { get; set; }

        //移乘方式
        public string moveway { get; set; }

        //时机，姿势，评价 0没问题 1 有些许问题 2 有问题
        public string evaluate { get; set; }

        public TrainDTO(TrainInfo trainInfo, DevicePrescription devicePrescription, PrescriptionResult prescriptionResult)
        {
            this.trainInfo = trainInfo;
            this.devicePrescription = devicePrescription;
            this.prescriptionResult = prescriptionResult;
            moveway = prescriptionResult != null ? DataCodeCache.GetInstance()
                .GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.dp_moveway.ToString()) : "";

            evaluate = prescriptionResult != null ? DataCodeCache.GetInstance()
                .GetCodeDValue(DataCodeTypeEnum.Evaluate, prescriptionResult.PR_Evaluate.ToString()) : "";

        }
    }
}
