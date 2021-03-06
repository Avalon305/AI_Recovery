﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recovery.util;

namespace Recovery.entity
{
    [Table("bdl_datacode")]
        public class DataCode
    {
        //主键 自增
        [Key]
        public int Pk_Code_Id { get; set; }
        //排序号
        public int? Code_Xh { get; set; }
        //类目ID dList是大类
        public string Code_Type_Id { get; set; }
        //存储值
        public string Code_S_Value { get; set; }
        //显示值
        public string Code_D_Value{
            get
            {
                if (LanguageUtils.IsChainese())
                {
                    return this.Code_C_Value;
                }
                else
                {
                    return this.Code_E_Value;
                }
            }
            set {}
        }
        //中文值
        public string Code_C_Value{get;set;}
        //英文值
        public string Code_E_Value { get; set; }
        //启用状态
        public byte Code_State { get; set; }
       
        //构造函数，默认启用
        public DataCode() {
            this.Code_State = 1;
        }
        
    }
}
