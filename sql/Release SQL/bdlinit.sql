/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50559
Source Host           : localhost:3306
Source Database       : bdlinit

Target Server Type    : MYSQL
Target Server Version : 50559
File Encoding         : 65001

Date: 2018-04-19 21:58:48
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for bdl_auth
-- ----------------------------
DROP TABLE IF EXISTS `bdl_auth`;
CREATE TABLE `bdl_auth` (
  `pk_auth_id` int(4) NOT NULL AUTO_INCREMENT COMMENT '权限表id',
  `auth_username` varchar(255) DEFAULT NULL COMMENT '权限用户名',
  `auth_userpass` varchar(255) DEFAULT NULL COMMENT '密码',
  `auth_level` tinyint(1) DEFAULT NULL COMMENT '角色权限级别',
  `gmt_create` timestamp NULL DEFAULT NULL,
  `gmt_modified` timestamp NULL DEFAULT NULL,
  `user_status` int(3) DEFAULT NULL COMMENT '使用状态',
  `auth_offlinetime` date DEFAULT NULL COMMENT '离线时间',
  PRIMARY KEY (`pk_auth_id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_auth
-- ----------------------------
INSERT INTO `bdl_auth` VALUES ('6', 'admin', 'admin', '0', '2018-04-07 15:44:04', '2018-04-07 15:44:08', '2', '9999-06-09');

-- ----------------------------
-- Table structure for bdl_customdata
-- ----------------------------
DROP TABLE IF EXISTS `bdl_customdata`;
CREATE TABLE `bdl_customdata` (
  `pk_cd_id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `cd_customname` varchar(255) NOT NULL COMMENT '自定义姓名',
  `cd_type` tinyint(255) NOT NULL COMMENT '类型编号，枚举控制',
  `is_deleted` tinyint(4) NOT NULL COMMENT '是否删除  默认0不删除',
  PRIMARY KEY (`pk_cd_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_customdata
-- ----------------------------
 

-- ----------------------------
-- Table structure for bdl_datacode
-- ----------------------------
DROP TABLE IF EXISTS `bdl_datacode`;
CREATE TABLE `bdl_datacode` (
  `fk_code_id` int(11) NOT NULL AUTO_INCREMENT,
  `code_xh` int(11) DEFAULT NULL COMMENT '排序号，下拉列表按这个排序',
  `code_type_id` varchar(255) DEFAULT NULL COMMENT '类型ID，dList是数据项',
  `code_s_value` varchar(255) DEFAULT NULL COMMENT '存储值',
  `code_c_value` varchar(255) DEFAULT NULL COMMENT '展示值',
  `code_state` tinyint(4) DEFAULT NULL COMMENT '是否启用 0 不启用 1启用',
  `code_e_value` varchar(255) DEFAULT NULL COMMENT '英语显示值',
  PRIMARY KEY (`fk_code_id`)
) ENGINE=InnoDB AUTO_INCREMENT=116 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_datacode
-- ----------------------------
INSERT INTO `bdl_datacode` VALUES ('1', '1', 'DList', 'Evaluate', '时机，姿势，评价', '1', 'Evaluate');
INSERT INTO `bdl_datacode` VALUES ('2', '1', 'Evaluate', '0', '没有问题', '1', 'Normal');
INSERT INTO `bdl_datacode` VALUES ('3', '2', 'Evaluate', '1', '有些许问题', '1', 'Slightly Not Normal');
INSERT INTO `bdl_datacode` VALUES ('4', '3', 'Evaluate', '2', '有问题', '1', 'Not Normal');
INSERT INTO `bdl_datacode` VALUES ('14', '1', 'DList', 'MoveWay', '移乘方式', '1', 'MoveWay');
INSERT INTO `bdl_datacode` VALUES ('15', '1', 'MoveWay', '0', '自理', '1', 'Independent');
INSERT INTO `bdl_datacode` VALUES ('16', '2', 'MoveWay', '1', '照看', '1', 'Need Observing');
INSERT INTO `bdl_datacode` VALUES ('17', '3', 'MoveWay', '2', '完全失能', '1', 'Full Supported');
INSERT INTO `bdl_datacode` VALUES ('20', '1', 'DList', 'OrganizationSort', '机构类别', '1', 'OrganizationSort');
INSERT INTO `bdl_datacode` VALUES ('21', '1', 'OrganizationSort', '0', '医院', '1', 'Hospital');
INSERT INTO `bdl_datacode` VALUES ('22', '2', 'OrganizationSort', '1', '诊所', '1', 'Clinic');
INSERT INTO `bdl_datacode` VALUES ('23', '3', 'OrganizationSort', '2', '老人保健机构', '1', 'Health care institution');
INSERT INTO `bdl_datacode` VALUES ('24', '4', 'OrganizationSort', '3', '特别护理老人院', '1', 'Special nursing home');
INSERT INTO `bdl_datacode` VALUES ('25', '5', 'OrganizationSort', '4', '日托服务', '1', 'Daycare facilities');
INSERT INTO `bdl_datacode` VALUES ('26', '6', 'OrganizationSort', '5', '市区政府', '1', 'Urban government');
INSERT INTO `bdl_datacode` VALUES ('27', '7', 'OrganizationSort', '6', '私立老人院', '1', 'Private seniors');
INSERT INTO `bdl_datacode` VALUES ('28', '8', 'OrganizationSort', '7', '其他', '1', 'Other');
INSERT INTO `bdl_datacode` VALUES ('30', '1', 'DList', 'Language', '语言', '1', 'Language');
INSERT INTO `bdl_datacode` VALUES ('31', '1', 'Language', '0', 'English', '1', 'English');
INSERT INTO `bdl_datacode` VALUES ('32', '2', 'Language', '1', '中文', '1', '中文');
INSERT INTO `bdl_datacode` VALUES ('33', '1', 'DList', 'CareLevel', '要介护度', '1', 'CareLevel');
INSERT INTO `bdl_datacode` VALUES ('34', '1', 'CareLevel', '0', '没有申请', '1', 'No application');
INSERT INTO `bdl_datacode` VALUES ('35', '2', 'CareLevel', '1', '自理', '1', 'Self-care');
INSERT INTO `bdl_datacode` VALUES ('36', '3', 'CareLevel', '2', '要支援一', '1', 'To support one');
INSERT INTO `bdl_datacode` VALUES ('37', '4', 'CareLevel', '3', '要支援二', '1', 'To support two');
INSERT INTO `bdl_datacode` VALUES ('38', '5', 'CareLevel', '4', '要护理1', '1', 'To care 1');
INSERT INTO `bdl_datacode` VALUES ('39', '6', 'CareLevel', '5', '要护理2', '1', 'To care 2');
INSERT INTO `bdl_datacode` VALUES ('40', '7', 'CareLevel', '6', '要护理3', '1', 'To care 3');
INSERT INTO `bdl_datacode` VALUES ('41', '8', 'CareLevel', '7', '要护理4', '1', 'To care 4');
INSERT INTO `bdl_datacode` VALUES ('42', '9', 'CareLevel', '8', '要护理5', '1', 'To care 5');

-- ----------------------------
-- Table structure for bdl_deviceprescription
-- ----------------------------
DROP TABLE IF EXISTS `bdl_deviceprescription`;
CREATE TABLE `bdl_deviceprescription` (
  `pk_dp_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '设备处方id',
  `gmt_create` timestamp NULL DEFAULT NULL,
  `gmt_modified` timestamp NULL DEFAULT NULL,
  `fk_ti_id` int(8) DEFAULT NULL COMMENT '外键（bdl_trianinfo : pk_ti_id）',
  `fk_ds_id` int(8) DEFAULT NULL COMMENT '外键（bdl_devicesort : pk_ds_id）',
  `dp_attrs` varchar(255) DEFAULT NULL COMMENT '设备属性，格式：设备名*属性-属性值*[属性-属性值*]',
  `dp_groupcount` int(3) DEFAULT NULL COMMENT '组数',
  `dp_groupnum` int(3) DEFAULT NULL COMMENT '每组的个数',
  `dp_relaxtime` int(3) DEFAULT NULL COMMENT '休息时间',
  `dp_moveway` int(3) DEFAULT NULL COMMENT '移乘方式',
  `dp_memo` text COMMENT '注意点、指示',
  `dp_status` tinyint(1) DEFAULT NULL COMMENT '1做了 0没做',
  `dp_weight` double(3,1) DEFAULT NULL COMMENT '砝码',
  `dp_timer` tinyint(1) DEFAULT NULL COMMENT '计时器是否有效',
  `dp_timecount` int(4) DEFAULT NULL COMMENT '计时',
  `dp_timetype` tinyint(1) DEFAULT NULL COMMENT '计时方式',
  `dp_movedistance` double(4,1) DEFAULT NULL COMMENT '移动距离',
  PRIMARY KEY (`pk_dp_id`)
) ENGINE=InnoDB AUTO_INCREMENT=219 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_deviceprescription
-- ----------------------------


-- ----------------------------
-- Table structure for bdl_deviceset
-- ----------------------------
DROP TABLE IF EXISTS `bdl_deviceset`;
CREATE TABLE `bdl_deviceset` (
  `bdl_dset_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '设备系列id',
  `gmt_create` timestamp NULL DEFAULT NULL,
  `gmt_modified` timestamp NULL DEFAULT NULL,
  `dset_name` varchar(255) DEFAULT NULL COMMENT '设备系列名',
  PRIMARY KEY (`bdl_dset_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_deviceset
-- ----------------------------
INSERT INTO `bdl_deviceset` VALUES ('1', '2018-04-19 21:47:16', '2018-04-19 21:47:18', '宝德龙系列');

-- ----------------------------
-- Table structure for bdl_devicesort
-- ----------------------------
DROP TABLE IF EXISTS `bdl_devicesort`;
CREATE TABLE `bdl_devicesort` (
  `pk_ds_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '设备分类id',
  `ds_name` varchar(255) DEFAULT NULL COMMENT '分类名称',
  `gmt_create` timestamp NULL DEFAULT NULL,
  `gmt_modified` timestamp NULL DEFAULT NULL,
  `fk_dset_id` int(8) DEFAULT NULL COMMENT '外键（bdl_deviceset : pk_dset_id）',
  `ds_status` tinyint(1) DEFAULT NULL COMMENT '复选框状态是否选中',
  PRIMARY KEY (`pk_ds_id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_devicesort
-- ----------------------------
INSERT INTO `bdl_devicesort` VALUES ('1', '胸部推举机', '2018-03-18 10:47:52', '2018-03-18 10:47:54', '1', '1');
INSERT INTO `bdl_devicesort` VALUES ('2', '坐姿划船机', '2018-03-21 23:54:19', '2018-03-21 23:54:21', '1', '1');
INSERT INTO `bdl_devicesort` VALUES ('3', '身体伸展弯曲机', '2018-04-19 21:47:26', '2018-04-19 21:47:28', '1', '1');
INSERT INTO `bdl_devicesort` VALUES ('4', '腿部伸展弯曲机', '2018-04-19 21:47:30', '2018-04-19 21:47:34', '1', '1');
INSERT INTO `bdl_devicesort` VALUES ('5', '腿部推蹬机', '2018-04-19 21:47:36', '2018-04-19 21:47:38', '1', '1');
INSERT INTO `bdl_devicesort` VALUES ('6', '腿部内外弯机', '2018-04-19 21:47:40', '2018-04-19 21:47:44', '1', '1');

-- ----------------------------
-- Table structure for bdl_onlinedevice
-- ----------------------------
DROP TABLE IF EXISTS `bdl_onlinedevice`;
CREATE TABLE `bdl_onlinedevice` (
  `pk_od_id` int(11) NOT NULL AUTO_INCREMENT,
  `od_clientid` varchar(255) DEFAULT NULL,
  `od_clientname_en` varchar(255) DEFAULT NULL,
  `od_clientname_ch` varchar(255) DEFAULT NULL,
  `od_gmt_modified` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`pk_od_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_onlinedevice
-- ----------------------------
INSERT INTO `bdl_onlinedevice` VALUES ('3', '011804120001', 'Chest Press', '胸部推举机', '2018-04-12 17:31:48');

-- ----------------------------
-- Table structure for bdl_physicalpower
-- ----------------------------
DROP TABLE IF EXISTS `bdl_physicalpower`;
CREATE TABLE `bdl_physicalpower` (
  `pk_pp_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '体力评价id',
  `gmt_create` timestamp NULL DEFAULT NULL,
  `gmt_modified` timestamp NULL DEFAULT NULL,
  `fk_user_id` int(8) DEFAULT NULL COMMENT '外键（bdl_user : pk_user_id）',
  `pp_high` varchar(255) DEFAULT NULL COMMENT '身高',
  `pp_weight` varchar(255) DEFAULT NULL COMMENT '体重',
  `pp_grip` varchar(255) DEFAULT NULL COMMENT '握力',
  `pp_eyeopenstand` varchar(255) DEFAULT NULL COMMENT '睁眼单脚站立',
  `pp_functionprotract` varchar(255) DEFAULT NULL COMMENT '功能性前伸',
  `pp_sitandreach` varchar(255) DEFAULT NULL COMMENT '坐姿体前屈',
  `pp_timeupgo` varchar(255) DEFAULT NULL COMMENT 'timeup go',
  `pp_walk5milegeneral` varchar(255) DEFAULT NULL COMMENT '5m步行',
  `pp_walk5milefast` varchar(255) DEFAULT NULL COMMENT '5m步行，最快',
  `pp_walk10mile` varchar(255) DEFAULT NULL COMMENT '10m步行，第一个字段是模式',
  `pp_walk6minute` varchar(255) DEFAULT NULL COMMENT '6分钟步行',
  `pp_step2minute` varchar(255) DEFAULT NULL COMMENT '2分钟踏步',
  `pp_legraise2minute` varchar(255) DEFAULT NULL COMMENT '2分钟抬腿',
  `pp_usermemo` text COMMENT '利用者感想',
  `pp_workermemo` text COMMENT '工作人员感想',
  PRIMARY KEY (`pk_pp_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_physicalpower
-- ----------------------------
 
-- ----------------------------
-- Table structure for bdl_prescriptionresult
-- ----------------------------
DROP TABLE IF EXISTS `bdl_prescriptionresult`;
CREATE TABLE `bdl_prescriptionresult` (
  `pk_pr_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '处方结果id',
  `gmt_create` timestamp NULL DEFAULT NULL,
  `gmt_modified` timestamp NULL DEFAULT NULL,
  `pr_sportstrength` int(2) DEFAULT NULL COMMENT '自觉运动强度',
  `fk_dp_id` int(8) DEFAULT NULL COMMENT '外键（dbl_deviceprescreiption : pk_dp_id）',
  `pr_time1` double(30,0) DEFAULT NULL COMMENT '第一个时间',
  `pr_distance` int(10) DEFAULT NULL COMMENT '距离',
  `pr_countworkquantity` double(10,2) DEFAULT NULL COMMENT '总工作量',
  `pr_cal` double(10,2) DEFAULT NULL COMMENT '热量',
  `pr_index` double(10,2) DEFAULT NULL COMMENT '指数',
  `pr_time2` double(30,0) DEFAULT NULL COMMENT '第二个时间',
  `pr_finishgroup` int(3) DEFAULT NULL COMMENT '完成组数',
  `pr_evaluate` tinyint(1) DEFAULT NULL COMMENT '时机，姿势',
  `pr_attentionpoint` text COMMENT '注意点',
  `pr_userthoughts` text COMMENT '病人感想',
  `pr_memo` text COMMENT '备忘',
  PRIMARY KEY (`pk_pr_id`)
) ENGINE=InnoDB AUTO_INCREMENT=69 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_prescriptionresult
-- ----------------------------
 
-- ----------------------------
-- Table structure for bdl_set
-- ----------------------------
DROP TABLE IF EXISTS `bdl_set`;
CREATE TABLE `bdl_set` (
  `pk_set_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '设置id',
  `set_unique_id` text COMMENT '客户机唯一id',
  `set_organizationname` varchar(255) DEFAULT NULL COMMENT '组织名称',
  `set_photolocation` varchar(255) DEFAULT NULL COMMENT '照片位置',
  `set_organizationphone` varchar(255) DEFAULT NULL COMMENT '联系电话',
  `set_language` varchar(255) DEFAULT NULL,
  `set_organizationsort` varchar(255) DEFAULT NULL,
  `set_version` varchar(255) DEFAULT NULL,
  `back_up` varchar(255) DEFAULT NULL COMMENT '备份路径',
  PRIMARY KEY (`pk_set_id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_set
-- ----------------------------
 
-- ----------------------------
-- Table structure for bdl_symptominfo
-- ----------------------------
DROP TABLE IF EXISTS `bdl_symptominfo`;
CREATE TABLE `bdl_symptominfo` (
  `pk_si_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '症状id',
  `gmt_create` timestamp NULL DEFAULT NULL COMMENT '创建日期',
  `gmt_modified` timestamp NULL DEFAULT NULL COMMENT '修改日期',
  `fk_user_id` int(8) DEFAULT NULL COMMENT '外键（dbl_user : pk_user_id）',
  `fk_ti_id` int(8) DEFAULT NULL COMMENT '外键（dbl_traininfo : pk_ti_id）',
  `si_isjoin` tinyint(1) DEFAULT NULL COMMENT '是否参加',
  `si_waterinput` varchar(255) DEFAULT NULL COMMENT '水分摄取',
  `si_careinfo` text COMMENT '看护记录',
  `si_inquiry` varchar(255) DEFAULT NULL COMMENT '问诊票',
  `si_pre_highpressure` varchar(255) DEFAULT NULL COMMENT '高血压(康复前)',
  `si_pre_lowpressure` varchar(255) DEFAULT NULL COMMENT '低血压(康复前)',
  `si_pre_heartrate` varchar(255) DEFAULT NULL COMMENT '心率(康复前)',
  `si_pre_pulse` int(3) DEFAULT NULL COMMENT '脉(康复前)',
  `si_pre_animalheat` varchar(255) DEFAULT NULL COMMENT '体温(康复前)',
  `si_suf_highpressure` varchar(255) DEFAULT NULL COMMENT '高血压(康复后)',
  `si_suf_lowpressure` varchar(255) DEFAULT NULL COMMENT '低血压(康复后)',
  `si_suf_heartrate` varchar(255) DEFAULT NULL COMMENT '心率(康复后)',
  `si_suf_pulse` int(3) DEFAULT NULL COMMENT '脉(康复后)',
  `si_suf_animalheat` varchar(255) DEFAULT NULL COMMENT '体温(康复后)',
  PRIMARY KEY (`pk_si_id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_symptominfo
-- ----------------------------
 
-- ----------------------------
-- Table structure for bdl_traininfo
-- ----------------------------
DROP TABLE IF EXISTS `bdl_traininfo`;
CREATE TABLE `bdl_traininfo` (
  `pk_ti_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '训练信息id',
  `gmt_create` timestamp NULL DEFAULT NULL COMMENT '创建时间',
  `gmt_modified` timestamp NULL DEFAULT NULL COMMENT '修改时间',
  `fk_user_id` int(8) DEFAULT NULL COMMENT '外键（dbl_user : pk_user_id）',
  `status` int(1) DEFAULT NULL COMMENT '0 未做 1 完成 2 废弃',
  PRIMARY KEY (`pk_ti_id`)
) ENGINE=InnoDB AUTO_INCREMENT=206 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_traininfo
-- ----------------------------
 

-- ----------------------------
-- Table structure for bdl_uploadmanagement
-- ----------------------------
DROP TABLE IF EXISTS `bdl_uploadmanagement`;
CREATE TABLE `bdl_uploadmanagement` (
  `pk_um_id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键 自增',
  `um_dataid` int(11) NOT NULL COMMENT '待上传的数据的id',
  `um_datatable` varchar(255) NOT NULL COMMENT '待上传的数据表的名字',
  PRIMARY KEY (`pk_um_id`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_uploadmanagement
-- ----------------------------
 
-- ----------------------------
-- Table structure for bdl_user
-- ----------------------------
DROP TABLE IF EXISTS `bdl_user`;
CREATE TABLE `bdl_user` (
  `pk_user_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '病人id',
  `gmt_create` timestamp NULL DEFAULT NULL COMMENT '创建时间',
  `gmt_modified` timestamp NULL DEFAULT NULL COMMENT '修改时间',
  `user_name` varchar(255) DEFAULT NULL COMMENT '病人姓名',
  `user_namepinyin` varchar(255) DEFAULT NULL COMMENT '姓名拼音',
  `user_sex` tinyint(1) DEFAULT NULL COMMENT '性别',
  `user_birth` date DEFAULT NULL COMMENT '出生日期',
  `user_groupname` varchar(255) DEFAULT NULL COMMENT '小组名称',
  `user_initcare` varchar(255) DEFAULT NULL COMMENT '初期要介护度',
  `user_nowcare` varchar(255) DEFAULT NULL COMMENT '现在要介护度',
  `user_illnessname` varchar(255) DEFAULT NULL COMMENT '疾病名称',
  `user_physicaldisabilities` varchar(255) DEFAULT NULL COMMENT '残障名称',
  `user_memo` text COMMENT '备忘',
  `user_photolocation` varchar(255) DEFAULT NULL COMMENT '照片位置',
  `user_idcard` varchar(255) DEFAULT NULL COMMENT 'id卡',
  `user_phone` varchar(255) DEFAULT NULL COMMENT '电话号码',
  `is_deleted` tinyint(1) DEFAULT NULL COMMENT '是否删除',
  `user_privateinfo` text COMMENT '非公开信息',
  PRIMARY KEY (`pk_user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_user
-- ----------------------------
 