/*
Navicat MySQL Data Transfer

Source Server         : local_mysql
Source Server Version : 50616
Source Host           : localhost:3306
Source Database       : bdl

Target Server Type    : MYSQL
Target Server Version : 50616
File Encoding         : 65001

Date: 2018-03-16 18:28:02
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for bdl_auth
-- ----------------------------
DROP TABLE IF EXISTS `bdl_auth`;
CREATE TABLE `bdl_auth` (
  `pk_auth_id` int(4) NOT NULL AUTO_INCREMENT COMMENT '权限表id',
  `auth_username` varchar(10) DEFAULT NULL COMMENT '权限用户名',
  `auth_userpass` varchar(20) DEFAULT NULL COMMENT '密码',
  `auth_level` tinyint(1) DEFAULT NULL COMMENT '角色权限级别',
  `gmt_create` timestamp NULL DEFAULT NULL,
  `gmt_modified` timestamp NULL DEFAULT NULL,
  `user_status` tinyint(1) DEFAULT NULL COMMENT '使用状态',
  `auth_offlinetime` date DEFAULT NULL COMMENT '离线时间',
  PRIMARY KEY (`pk_auth_id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_auth
-- ----------------------------
INSERT INTO `bdl_auth` VALUES ('5', '123', '123', '1', null, null, '0', '2018-07-20');
INSERT INTO `bdl_auth` VALUES ('6', 'admin', 'admin', '0', null, null, '1', '2018-06-09');

-- ----------------------------
-- Table structure for bdl_datacode
-- ----------------------------
DROP TABLE IF EXISTS `bdl_datacode`;
CREATE TABLE `bdl_datacode` (
  `fk_code_id` int(11) NOT NULL AUTO_INCREMENT,
  `code_xh` int(11) DEFAULT NULL COMMENT '排序号，下拉列表按这个排序',
  `code_type_id` varchar(255) DEFAULT NULL COMMENT '类型ID，dList是数据项',
  `code_s_value` varchar(255) DEFAULT NULL COMMENT '存储值',
  `code_d_value` varchar(255) DEFAULT NULL COMMENT '展示值',
  `code_state` tinyint(4) DEFAULT NULL COMMENT '是否启用 0 不启用 1启用',
  PRIMARY KEY (`fk_code_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_datacode
-- ----------------------------
INSERT INTO `bdl_datacode` VALUES ('1', '1', 'DList', 'Evaluate', '时机，姿势，评价', '1');
INSERT INTO `bdl_datacode` VALUES ('2', '1', 'Evaluate', '0', '没有问题', '1');
INSERT INTO `bdl_datacode` VALUES ('3', '2', 'Evaluate', '1', '有些许问题', '1');
INSERT INTO `bdl_datacode` VALUES ('4', '3', 'Evaluate', '2', '有问题', '1');
INSERT INTO `bdl_datacode` VALUES ('5', '2', 'DList', 'Disease', '疾病', '1');
INSERT INTO `bdl_datacode` VALUES ('6', '1', 'Disease', '0', '小儿麻痹症', '1');
INSERT INTO `bdl_datacode` VALUES ('7', '2', 'Disease', '1', '神经病', '1');
INSERT INTO `bdl_datacode` VALUES ('8', '3', 'DList', 'Group', '组', '1');
INSERT INTO `bdl_datacode` VALUES ('9', '1', 'Group', '0', '康复组', '1');
INSERT INTO `bdl_datacode` VALUES ('10', '2', 'Group', '1', '诊断组', '1');

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
  PRIMARY KEY (`pk_dp_id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8;

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
  `dset_name` varchar(30) DEFAULT NULL COMMENT '设备系列名',
  PRIMARY KEY (`bdl_dset_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_deviceset
-- ----------------------------

-- ----------------------------
-- Table structure for bdl_devicesort
-- ----------------------------
DROP TABLE IF EXISTS `bdl_devicesort`;
CREATE TABLE `bdl_devicesort` (
  `pk_ds_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '设备分类id',
  `ds_name` varchar(20) DEFAULT NULL COMMENT '分类名称',
  `gmt_create` timestamp NULL DEFAULT NULL,
  `gmt_modified` timestamp NULL DEFAULT NULL,
  `fk_dset_id` int(8) DEFAULT NULL COMMENT '外键（bdl_deviceset : pk_dset_id）',
  PRIMARY KEY (`pk_ds_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_devicesort
-- ----------------------------

-- ----------------------------
-- Table structure for bdl_diagnosis
-- ----------------------------
DROP TABLE IF EXISTS `bdl_diagnosis`;
CREATE TABLE `bdl_diagnosis` (
  `pk_dn_id` int(3) NOT NULL AUTO_INCREMENT COMMENT '残疾名称id',
  `gmt_create` timestamp NULL DEFAULT NULL COMMENT '创建时间',
  `gmt_modified` timestamp NULL DEFAULT NULL COMMENT '修改时间',
  `dn_name` varchar(30) DEFAULT NULL COMMENT '残疾名',
  PRIMARY KEY (`pk_dn_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_diagnosis
-- ----------------------------
INSERT INTO `bdl_diagnosis` VALUES ('1', '2018-03-16 15:17:15', '2018-03-16 15:17:15', '555');

-- ----------------------------
-- Table structure for bdl_disease
-- ----------------------------
DROP TABLE IF EXISTS `bdl_disease`;
CREATE TABLE `bdl_disease` (
  `pk_ds_id` int(3) NOT NULL AUTO_INCREMENT COMMENT '疾病id',
  `gmt_create` timestamp NULL DEFAULT NULL COMMENT '创建时间',
  `gmt_modified` timestamp NULL DEFAULT NULL COMMENT '修改时间',
  `ds_name` varchar(30) DEFAULT NULL COMMENT '疾病名称',
  PRIMARY KEY (`pk_ds_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_disease
-- ----------------------------
INSERT INTO `bdl_disease` VALUES ('1', '2018-03-16 15:17:02', '2018-03-16 15:17:02', '555');

-- ----------------------------
-- Table structure for bdl_group
-- ----------------------------
DROP TABLE IF EXISTS `bdl_group`;
CREATE TABLE `bdl_group` (
  `pk_gr_id` int(2) NOT NULL AUTO_INCREMENT,
  `gmt_create` timestamp NULL DEFAULT NULL,
  `gmt_modified` timestamp NULL DEFAULT NULL,
  `gr_name` varchar(15) DEFAULT NULL COMMENT '分组名称',
  PRIMARY KEY (`pk_gr_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_group
-- ----------------------------

-- ----------------------------
-- Table structure for bdl_physicalpower
-- ----------------------------
DROP TABLE IF EXISTS `bdl_physicalpower`;
CREATE TABLE `bdl_physicalpower` (
  `pk_pp_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '体力评价id',
  `gmt_create` timestamp NULL DEFAULT NULL,
  `gmt_modified` timestamp NULL DEFAULT NULL,
  `fk_user_id` int(8) DEFAULT NULL COMMENT '外键（bdl_user : pk_user_id）',
  `pp_high` varchar(100) DEFAULT NULL COMMENT '身高',
  `pp_weight` varchar(100) DEFAULT NULL COMMENT '体重',
  `pp_grip` varchar(100) DEFAULT NULL COMMENT '握力',
  `pp_eyeopenstand` varchar(100) DEFAULT NULL COMMENT '睁眼单脚站立',
  `pp_functionprotract` varchar(100) DEFAULT NULL COMMENT '功能性前伸',
  `pp_sitandreach` varchar(100) DEFAULT NULL COMMENT '坐姿体前屈',
  `pp_timeupgo` varchar(100) DEFAULT NULL COMMENT 'timeup go',
  `pp_walk5milegeneral` varchar(100) DEFAULT NULL COMMENT '5m步行',
  `pp_walk5milefast` varchar(100) DEFAULT NULL COMMENT '5m步行，最快',
  `pp_walk10mile` varchar(100) DEFAULT NULL COMMENT '10m步行，第一个字段是模式',
  `pp_walk6minute` varchar(100) DEFAULT NULL COMMENT '6分钟步行',
  `pp_step2minute` varchar(100) DEFAULT NULL COMMENT '2分钟踏步',
  `pp_legraise2minute` varchar(100) DEFAULT NULL COMMENT '2分钟抬腿',
  `pp_usermemo` text COMMENT '利用者感想',
  `pp_workermemo` text COMMENT '工作人员感想',
  PRIMARY KEY (`pk_pp_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

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
  `pr_time1` double(5,2) DEFAULT NULL COMMENT '第一个时间',
  `pr_distance` int(10) DEFAULT NULL COMMENT '距离',
  `pr_countworkquantity` double(10,2) DEFAULT NULL COMMENT '总工作量',
  `pr_cal` double(10,2) DEFAULT NULL COMMENT '热量',
  `pr_index` double(10,2) DEFAULT NULL COMMENT '指数',
  `pr_time2` double(5,2) DEFAULT NULL COMMENT '第二个时间',
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
  `set_unique_id` varchar(255) DEFAULT NULL COMMENT '客户机唯一id',
  `set_language` tinyint(1) DEFAULT NULL COMMENT '语言',
  `set_organizationname` varchar(30) DEFAULT NULL COMMENT '组织名称',
  `set_organizationsort` varchar(30) DEFAULT NULL COMMENT '机构分区',
  `set_photolocation` varchar(100) DEFAULT NULL COMMENT '照片位置',
  `set_organizationphone` varchar(20) DEFAULT NULL COMMENT '联系电话',
  PRIMARY KEY (`pk_set_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_set
-- ----------------------------
INSERT INTO `bdl_set` VALUES ('1', '68:F7:28:F7:3B:D8', null, null, null, null, null);

-- ----------------------------
-- Table structure for bdl_symptominfo
-- ----------------------------
DROP TABLE IF EXISTS `bdl_symptominfo`;
CREATE TABLE `bdl_symptominfo` (
  `pk_si_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '症状id',
  `gmt_create` timestamp NULL DEFAULT NULL COMMENT '创建日期',
  `gmt_modified` timestamp NULL DEFAULT NULL COMMENT '修改日期',
  `fk_user_id` int(8) DEFAULT NULL COMMENT '外键（dbl_user : pk_user_id）',
  `si_isjoin` tinyint(1) DEFAULT NULL COMMENT '是否参加',
  `si_waterinput` varchar(10) DEFAULT NULL COMMENT '水分摄取',
  `si_careinfo` text COMMENT '看护记录',
  `si_inquiry` varchar(100) DEFAULT NULL COMMENT '问诊票',
  PRIMARY KEY (`pk_si_id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_symptominfo
-- ----------------------------
INSERT INTO `bdl_symptominfo` VALUES ('1', '2018-03-15 15:09:18', '2018-03-15 15:09:18', '2', '0', '3', '3', '1. ????');
INSERT INTO `bdl_symptominfo` VALUES ('2', '2018-03-15 15:20:47', '2018-03-15 15:20:47', '2', '0', '3', '3', '1. 身体疲倦');
INSERT INTO `bdl_symptominfo` VALUES ('3', '2018-03-15 15:39:27', '2018-03-15 15:39:27', '2', '0', '3', '3', '1. 身体疲倦');
INSERT INTO `bdl_symptominfo` VALUES ('4', '2018-03-15 15:41:02', '2018-03-15 15:41:02', '1', '0', '4', '4', '1. 身体疲倦');
INSERT INTO `bdl_symptominfo` VALUES ('5', '2018-03-15 15:41:25', '2018-03-15 15:41:25', '2', '0', '6', '6', '1. 身体疲倦');
INSERT INTO `bdl_symptominfo` VALUES ('6', '2018-03-15 15:49:46', '2018-03-15 15:49:46', '1', '0', '4', '4', '2. 腹泻');
INSERT INTO `bdl_symptominfo` VALUES ('7', '2018-03-15 15:55:36', '2018-03-15 15:55:36', '1', '0', '9', '9', '2. 腹泻');
INSERT INTO `bdl_symptominfo` VALUES ('8', '2018-03-15 16:03:41', '2018-03-15 16:03:41', '1', '1', '3', '3', '1. 身体疲倦');
INSERT INTO `bdl_symptominfo` VALUES ('9', '2018-03-15 16:13:15', '2018-03-15 16:13:15', '1', '0', '2', '2', '1. 身体疲倦');

-- ----------------------------
-- Table structure for bdl_symptominfochild
-- ----------------------------
DROP TABLE IF EXISTS `bdl_symptominfochild`;
CREATE TABLE `bdl_symptominfochild` (
  `pk_sic_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '症状子表id',
  `gmt_create` timestamp NULL DEFAULT NULL COMMENT '创建时间',
  `gmt_modified` timestamp NULL DEFAULT NULL COMMENT '修改时间',
  `fk_si_id` int(8) DEFAULT NULL COMMENT '外键（bdl_symptominfo : pk_si_id）',
  `status` tinyint(1) DEFAULT NULL COMMENT '1-康复前、2-康复后',
  `sic_highpressure` varchar(10) DEFAULT NULL COMMENT '高血压',
  `sic_lowpressure` varchar(10) DEFAULT NULL COMMENT '低血压',
  `sic_heartrate` varchar(3) DEFAULT NULL COMMENT '心率',
  `sic_pulse` int(3) DEFAULT NULL COMMENT '脉',
  `sic_animalheat` varchar(3) DEFAULT NULL COMMENT '体温',
  PRIMARY KEY (`pk_sic_id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_symptominfochild
-- ----------------------------
INSERT INTO `bdl_symptominfochild` VALUES ('1', '2018-03-15 15:09:18', '2018-03-15 15:09:18', '1', '1', '22', '22', '2', '0', '2');
INSERT INTO `bdl_symptominfochild` VALUES ('2', '2018-03-15 15:09:18', '2018-03-15 15:09:18', '1', '2', '3', '33', '3', '1', '3');
INSERT INTO `bdl_symptominfochild` VALUES ('3', '2018-03-15 15:20:47', '2018-03-15 15:20:47', '2', '1', '22', '22', '2', '0', '2');
INSERT INTO `bdl_symptominfochild` VALUES ('4', '2018-03-15 15:20:47', '2018-03-15 15:20:47', '2', '2', '3', '33', '3', '1', '3');
INSERT INTO `bdl_symptominfochild` VALUES ('5', '2018-03-15 15:39:27', '2018-03-15 15:39:27', '3', '1', '22', '22', '2', '0', '2');
INSERT INTO `bdl_symptominfochild` VALUES ('6', '2018-03-15 15:39:27', '2018-03-15 15:39:27', '3', '2', '3', '2', '3', '1', '3');
INSERT INTO `bdl_symptominfochild` VALUES ('7', '2018-03-15 15:41:02', '2018-03-15 15:41:02', '4', '1', '3', '3', '3', '0', '3');
INSERT INTO `bdl_symptominfochild` VALUES ('8', '2018-03-15 15:41:02', '2018-03-15 15:41:02', '4', '2', '4', '4', '4', '1', '4');
INSERT INTO `bdl_symptominfochild` VALUES ('9', '2018-03-15 15:41:25', '2018-03-15 15:41:25', '5', '1', '5', '5', '5', '0', '5');
INSERT INTO `bdl_symptominfochild` VALUES ('10', '2018-03-15 15:41:25', '2018-03-15 15:41:25', '5', '2', '6', '6', '6', '1', '6');
INSERT INTO `bdl_symptominfochild` VALUES ('11', '2018-03-15 15:49:46', '2018-03-15 15:49:46', '6', '1', '3', '3', '3', '0', '3');
INSERT INTO `bdl_symptominfochild` VALUES ('12', '2018-03-15 15:49:46', '2018-03-15 15:49:46', '6', '2', '4', '4', '4', '1', '4');
INSERT INTO `bdl_symptominfochild` VALUES ('13', '2018-03-15 15:55:36', '2018-03-15 15:55:36', '7', '1', '9', '9', '9', '0', '9');
INSERT INTO `bdl_symptominfochild` VALUES ('14', '2018-03-15 15:55:36', '2018-03-15 15:55:36', '7', '2', '9', '9', '9', '1', '9');
INSERT INTO `bdl_symptominfochild` VALUES ('15', '2018-03-15 16:03:41', '2018-03-15 16:03:41', '8', '1', '3', '3', '3', '0', '3');
INSERT INTO `bdl_symptominfochild` VALUES ('16', '2018-03-15 16:03:41', '2018-03-15 16:03:41', '8', '2', '3', '3', '33', '1', '3');
INSERT INTO `bdl_symptominfochild` VALUES ('17', '2018-03-15 16:13:15', '2018-03-15 16:13:15', '9', '1', '1', '1', '1', '0', '1');
INSERT INTO `bdl_symptominfochild` VALUES ('18', '2018-03-15 16:13:15', '2018-03-15 16:13:15', '9', '2', '2', '2', '2', '1', '2');

-- ----------------------------
-- Table structure for bdl_traininfo
-- ----------------------------
DROP TABLE IF EXISTS `bdl_traininfo`;
CREATE TABLE `bdl_traininfo` (
  `pk_ti_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '训练信息id',
  `gmt_create` timestamp NULL DEFAULT NULL COMMENT '创建时间',
  `gmt_modified` timestamp NULL DEFAULT NULL COMMENT '修改时间',
  `fk_user_id` int(8) DEFAULT NULL COMMENT '外键（dbl_user : pk_user_id）',
  PRIMARY KEY (`pk_ti_id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_traininfo
-- ----------------------------
INSERT INTO `bdl_traininfo` VALUES ('5', '2018-03-16 15:14:54', '2018-03-16 15:14:54', '0');
INSERT INTO `bdl_traininfo` VALUES ('6', '2018-03-16 15:15:00', '2018-03-16 15:15:00', '0');
INSERT INTO `bdl_traininfo` VALUES ('7', '2018-03-16 15:15:02', '2018-03-16 15:15:02', '0');
INSERT INTO `bdl_traininfo` VALUES ('8', '2018-03-16 15:15:03', '2018-03-16 15:15:03', '0');
INSERT INTO `bdl_traininfo` VALUES ('9', '2018-03-16 15:15:03', '2018-03-16 15:15:03', '0');
INSERT INTO `bdl_traininfo` VALUES ('10', '2018-03-16 15:15:03', '2018-03-16 15:15:03', '0');
INSERT INTO `bdl_traininfo` VALUES ('11', '2018-03-16 15:15:04', '2018-03-16 15:15:04', '0');

-- ----------------------------
-- Table structure for bdl_uploadmanagement
-- ----------------------------
DROP TABLE IF EXISTS `bdl_uploadmanagement`;
CREATE TABLE `bdl_uploadmanagement` (
  `pk_um_id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键 自增',
  `um_dataid` int(11) NOT NULL COMMENT '待上传的数据的id',
  `um_datatable` varchar(255) NOT NULL COMMENT '待上传的数据表的名字',
  PRIMARY KEY (`pk_um_id`)
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_uploadmanagement
-- ----------------------------
INSERT INTO `bdl_uploadmanagement` VALUES ('1', '1', 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('2', '1', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('3', '2', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('4', '2', 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('5', '3', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('6', '4', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('7', '3', 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('8', '5', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('9', '6', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('10', '4', 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('11', '7', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('12', '8', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('13', '5', 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('14', '9', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('15', '10', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('16', '6', 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('17', '11', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('18', '12', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('19', '7', 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('20', '13', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('21', '14', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('22', '8', 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('23', '15', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('24', '16', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('25', '9', 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('26', '17', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('27', '18', 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES ('28', '1', 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('29', '1', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('30', '2', 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('31', '2', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('32', '3', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('33', '3', 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('34', '4', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('35', '5', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('36', '6', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('37', '7', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('38', '8', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('39', '9', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('40', '4', 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('41', '10', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('42', '11', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('43', '12', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('44', '13', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('45', '14', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('46', '15', 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES ('47', '5', 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('48', '6', 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('49', '7', 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('50', '8', 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('51', '9', 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('52', '10', 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES ('53', '11', 'bdl_traininfo');

-- ----------------------------
-- Table structure for bdl_user
-- ----------------------------
DROP TABLE IF EXISTS `bdl_user`;
CREATE TABLE `bdl_user` (
  `pk_user_id` int(8) NOT NULL AUTO_INCREMENT COMMENT '病人id',
  `gmt_create` timestamp NULL DEFAULT NULL COMMENT '创建时间',
  `gmt_modified` timestamp NULL DEFAULT NULL COMMENT '修改时间',
  `user_name` varchar(20) DEFAULT NULL COMMENT '病人姓名',
  `user_namepinyin` varchar(10) DEFAULT NULL COMMENT '姓名拼音',
  `user_sex` tinyint(1) DEFAULT NULL COMMENT '性别',
  `user_birth` date DEFAULT NULL COMMENT '出生日期',
  `user_groupname` varchar(30) DEFAULT NULL COMMENT '小组名称',
  `user_initcare` varchar(20) DEFAULT NULL COMMENT '初期要介护度',
  `user_nowcare` varchar(20) DEFAULT NULL COMMENT '现在要介护度',
  `user_illnessname` varchar(20) DEFAULT NULL COMMENT '疾病名称',
  `user_physicaldisabilities` varchar(20) DEFAULT NULL COMMENT '残障名称',
  `user_memo` text COMMENT '备忘',
  `user_photolocation` varchar(40) DEFAULT NULL COMMENT '照片位置',
  `user_idcard` varchar(30) DEFAULT NULL COMMENT 'id卡',
  `user_phone` varchar(20) DEFAULT NULL COMMENT '电话号码',
  `is_deleted` tinyint(1) DEFAULT NULL COMMENT '是否删除',
  PRIMARY KEY (`pk_user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_user
-- ----------------------------
INSERT INTO `bdl_user` VALUES ('1', '2018-03-15 11:22:26', '2018-03-15 11:22:29', '李泽', 'lize', '1', '2018-03-15', 'A组', '自理', '自理', '神经病', '不健全', '傻子', null, '438', '123', '0');
INSERT INTO `bdl_user` VALUES ('2', '2018-03-15 11:29:07', '2018-03-15 11:29:10', '范明勇', 'fmy', '1', '2018-03-15', 'B组', '不能自理', '不能自理', '智障', '小儿麻痹', '呆子', null, '110', '123', '0');
