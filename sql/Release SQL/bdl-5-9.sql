/*
Navicat MySQL Data Transfer

Source Server         : 本机
Source Server Version : 50642
Source Host           : localhost:3306
Source Database       : bdl

Target Server Type    : MYSQL
Target Server Version : 50642
File Encoding         : 65001

Date: 2019-05-09 08:32:01
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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_auth
-- ----------------------------
INSERT INTO `bdl_auth` VALUES ('1', 'admin', 'admin', '0', '2018-04-07 15:44:04', '2018-04-07 15:44:08', '2', '9999-12-31');
INSERT INTO `bdl_auth` VALUES ('2', '123', '123', '1', '2018-08-07 18:21:46', '2018-08-07 18:21:46', '0', '2020-12-31');

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

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
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8;

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
) ENGINE=InnoDB AUTO_INCREMENT=179 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_deviceprescription
-- ----------------------------
INSERT INTO `bdl_deviceprescription` VALUES ('177', '2019-05-08 22:28:27', '2019-05-08 22:28:27', '136', '3', '1*1*1*1', '1', '1', '1', '0', '', '0', '0.5', '1', '0', '0', '1.0');
INSERT INTO `bdl_deviceprescription` VALUES ('178', '2019-05-08 22:30:35', '2019-05-08 22:30:35', '137', '1', '1*1', '1', '1', '1', '0', '', '0', '0.5', '1', '0', '0', '1.0');

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
INSERT INTO `bdl_devicesort` VALUES ('2', '腿部外弯机', '2018-04-19 21:47:40', '2018-04-19 21:47:44', '1', '1');
INSERT INTO `bdl_devicesort` VALUES ('3', '腿部伸展弯曲机', '2018-04-19 21:47:30', '2018-04-19 21:47:34', '1', '1');
INSERT INTO `bdl_devicesort` VALUES ('4', '身体伸展弯曲机', '2018-04-19 21:47:26', '2018-04-19 21:47:28', '1', '1');
INSERT INTO `bdl_devicesort` VALUES ('5', '坐姿划船机', '2018-03-21 23:54:19', '2018-03-21 23:54:21', '1', '1');
INSERT INTO `bdl_devicesort` VALUES ('6', '腿部推蹬机', '2018-04-19 21:47:36', '2018-04-19 21:47:38', '1', '1');

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
INSERT INTO `bdl_onlinedevice` VALUES ('1', '010000000000', 'Chest Press', '胸部推举机', '2019-03-25 15:04:30');
INSERT INTO `bdl_onlinedevice` VALUES ('2', '020000000000', 'Hip Abduction', '腿部外弯机', '2019-04-02 10:48:23');
INSERT INTO `bdl_onlinedevice` VALUES ('3', '010000000001', 'Chest Press', '胸部推举机', '2019-04-18 22:49:05');

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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_physicalpower
-- ----------------------------
INSERT INTO `bdl_physicalpower` VALUES ('1', '2019-05-08 22:24:03', '2019-05-08 22:24:03', '1', 'param1,100,param3,param4,param5', 'param1,25,param3,param4,param5', '左,52,param3,param4,param5', '左,25,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', '通常,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', '', '');
INSERT INTO `bdl_physicalpower` VALUES ('2', '2019-05-08 22:47:25', '2019-05-08 22:47:25', '2', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', '通常,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', '', '');
INSERT INTO `bdl_physicalpower` VALUES ('3', '2019-05-08 22:47:41', '2019-05-08 22:47:41', '2', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,8,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', '通常,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', '', '');
INSERT INTO `bdl_physicalpower` VALUES ('4', '2019-05-08 22:47:54', '2019-05-08 22:47:54', '2', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,678,86,坐姿,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,以坐姿进行测定,param5', 'param1,6,86,param4,param5', 'param1,param2,867,param4,param5', 'param1,param2,param3,param4,param5', 'param1,8,8678,param4,param5', '通常,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', '', '');
INSERT INTO `bdl_physicalpower` VALUES ('5', '2019-05-08 22:48:46', '2019-05-08 22:48:46', '2', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', '通常,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', '', '');
INSERT INTO `bdl_physicalpower` VALUES ('6', '2019-05-08 22:59:15', '2019-05-08 22:59:15', '2', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', '通常,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', '', '');
INSERT INTO `bdl_physicalpower` VALUES ('7', '2019-05-08 23:05:24', '2019-05-08 23:05:24', '2', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', '通常,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', 'param1,param2,param3,param4,param5', '', '');

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_set
-- ----------------------------
INSERT INTO `bdl_set` VALUES ('3', '25A103F03E0F63EB92A3FC48AE7662AD566712DEEC4FC049FF7BDFC233C06A5799C953D15887771A1FAF8C681D1D26F86A442B8CCBF18D3968E215CACB845EDB66D2578051AE1752986835C48C2996826394BDA46F234A1B036FCE3FB9B1DC93', '', 'D:\\spms_desktop\\bin\\Debug\\image\\', '', '1', '1', '1.0', 'D:\\spms_desktop\\bin\\Debug\\BackUp\\');

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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_symptominfo
-- ----------------------------
INSERT INTO `bdl_symptominfo` VALUES ('1', '2019-05-08 22:21:53', '2019-05-08 22:22:40', '1', '0', '0', '100', '', '身体倦怠,腹泻,咳嗽、有痰', '66', '51', '120', '0', '37', '98', '65', '120', '1', '37');

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
) ENGINE=InnoDB AUTO_INCREMENT=138 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_traininfo
-- ----------------------------
INSERT INTO `bdl_traininfo` VALUES ('125', '2019-04-21 19:31:01', '2019-04-21 19:31:01', '2', '1');
INSERT INTO `bdl_traininfo` VALUES ('126', '2019-04-21 19:31:09', '2019-04-21 19:31:09', '2', '3');
INSERT INTO `bdl_traininfo` VALUES ('127', '2019-04-21 19:31:13', '2019-04-21 19:31:13', '2', '3');
INSERT INTO `bdl_traininfo` VALUES ('128', '2019-04-21 19:31:17', '2019-04-21 19:31:17', '2', '3');
INSERT INTO `bdl_traininfo` VALUES ('129', '2019-04-21 19:31:21', '2019-04-21 19:31:21', '2', '3');
INSERT INTO `bdl_traininfo` VALUES ('130', '2019-04-21 19:37:57', '2019-04-21 19:37:57', '2', '0');
INSERT INTO `bdl_traininfo` VALUES ('136', '2019-05-08 22:28:27', '2019-05-08 22:28:27', '3', '1');
INSERT INTO `bdl_traininfo` VALUES ('137', '2019-05-08 22:30:35', '2019-05-08 22:30:35', '1', '1');

-- ----------------------------
-- Table structure for bdl_uploadmanagement
-- ----------------------------
DROP TABLE IF EXISTS `bdl_uploadmanagement`;
CREATE TABLE `bdl_uploadmanagement` (
  `pk_um_id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键 自增',
  `um_dataid` int(11) NOT NULL COMMENT '待上传的数据的id',
  `um_datatable` varchar(255) NOT NULL COMMENT '待上传的数据表的名字',
  `um_exec` int(11) unsigned NOT NULL,
  PRIMARY KEY (`pk_um_id`)
) ENGINE=InnoDB AUTO_INCREMENT=323 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_uploadmanagement
-- ----------------------------
INSERT INTO `bdl_uploadmanagement` VALUES ('296', '0', 'bdl_set', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('297', '5', 'bdl_user', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('298', '1', 'bdl_symptominfo', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('299', '131', 'bdl_traininfo', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('300', '166', 'bdl_deviceprescription', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('301', '132', 'bdl_traininfo', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('302', '167', 'bdl_deviceprescription', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('303', '168', 'bdl_deviceprescription', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('304', '169', 'bdl_deviceprescription', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('305', '170', 'bdl_deviceprescription', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('306', '171', 'bdl_deviceprescription', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('307', '172', 'bdl_deviceprescription', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('308', '133', 'bdl_traininfo', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('309', '173', 'bdl_deviceprescription', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('310', '134', 'bdl_traininfo', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('311', '174', 'bdl_deviceprescription', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('312', '135', 'bdl_traininfo', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('313', '175', 'bdl_deviceprescription', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('314', '176', 'bdl_deviceprescription', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('315', '136', 'bdl_traininfo', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('316', '177', 'bdl_deviceprescription', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('317', '137', 'bdl_traininfo', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('318', '178', 'bdl_deviceprescription', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('319', '5', 'bdl_user', '1');
INSERT INTO `bdl_uploadmanagement` VALUES ('320', '3', 'bdl_set', '1');
INSERT INTO `bdl_uploadmanagement` VALUES ('321', '0', 'bdl_physicalpower', '0');
INSERT INTO `bdl_uploadmanagement` VALUES ('322', '7', 'bdl_physicalpower', '0');

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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_user
-- ----------------------------
INSERT INTO `bdl_user` VALUES ('1', '2018-08-07 18:34:16', '2019-03-25 16:11:10', 'song', 'song', '1', '2018-08-07', '', '1', '1', '', '', '\r\n', null, '371422198701107419', '18953458737', '0', '');
INSERT INTO `bdl_user` VALUES ('2', '2019-01-26 10:30:12', '2019-01-26 10:30:38', '苑朝阳', 'YuanZhaoyang', '1', '1991-03-27', '', '1', '1', '', '', '\r\n', 'YuanZhaoyang371422199103270000zzz.jpg', '371422199103270000', '15066632128', '0', '');
INSERT INTO `bdl_user` VALUES ('3', '2019-05-07 20:34:46', '2019-05-07 20:34:46', '实对', 'sd', '1', '2019-05-07', '', '-1', '2', '', '', '', null, '312164547887878999', '17860756666', '0', '');
INSERT INTO `bdl_user` VALUES ('4', '2019-05-07 21:29:56', '2019-05-07 21:29:56', 'efgjdshgfj', 'sadsad', '1', '2019-05-07', '', '-1', '-1', '', '', '', null, '23564564565456465', '17854546648', '0', '');
INSERT INTO `bdl_user` VALUES ('5', '2019-05-08 22:21:07', '2019-05-08 22:47:01', 'jksjfds', 'dfdsafds', '1', '2019-05-08', '', '-1', '-1', '', '', '\r\n', null, '312154568978', '17860756667', '0', '');
