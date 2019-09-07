using Recovery.constant;
using Recovery.entity;
using Recovery.entity.newEntity;
using Recovery.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.view.dto
{
    class NewTrainDTO
    {
        public int ID { get; set; }
        public string DateStr { get; set; }
        public TrainInfo trainInfo { get; set; }

        public NewDevicePrescription devicePrescription { get; set; }

        public PrescriptionResultTwo prescriptionResult { get; set; }

        //移乘方式
        public string moveway { get; set; }

        public List<TrainDTO> ConvertDtoList(List<TrainInfo> trainInfos)
        {
            List<TrainDTO> trainDtos = new List<TrainDTO>();
            foreach (var trainInfo in trainInfos)
            {
                trainDtos.Add(new TrainDTO(trainInfo, null, null));
            }

            return trainDtos;
        }

        public NewTrainDTO()
        {

        }
        public NewTrainDTO(TrainInfo trainInfo, NewDevicePrescription devicePrescription, PrescriptionResultTwo prescriptionResult)
        {
            if (trainInfo != null)
            {
                this.ID = trainInfo.Pk_TI_Id;
                this.DateStr = trainInfo.Gmt_Create.ToString();
            }
            this.trainInfo = trainInfo;
            this.devicePrescription = devicePrescription;
            this.prescriptionResult = prescriptionResult;
            moveway = prescriptionResult != null ? DataCodeCache.GetInstance()
                .GetCodeDValue(DataCodeTypeEnum.MoveWay, devicePrescription.Dp_moveway.ToString()) : "";
        }
    }
}
