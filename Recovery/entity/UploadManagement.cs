﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recovery.entity
{
    //上传管理者，每个管理者有一个任务使命
    [Table("bdl_uploadmanagement")]
    public class UploadManagement
    {
        //主键
        [Key]
        public int Pk_UM_Id { get; set; }
        //数据的ID
        public int UM_DataId { get; set; }
        //持有数据所在的表名
        public string UM_DataTable { get; set; }
        //操作的类型 add是0 update是1
        public int UM_Exec { get; set; }
        //ID构造器
        public UploadManagement(int keyID)
        {
            this.Pk_UM_Id = keyID;
        }
        public UploadManagement()
        {
        }

        public UploadManagement(int umDataId, string umDataTable, int umexec)
        {
            UM_DataId = umDataId;
            UM_DataTable = umDataTable;
            UM_Exec = umexec;
        }
    }
}
