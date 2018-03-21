/*
 Navicat Premium Data Transfer

 Source Server         : mysql_local
 Source Server Type    : MySQL
 Source Server Version : 50639
 Source Host           : localhost:3306
 Source Schema         : bdl

 Target Server Type    : MySQL
 Target Server Version : 50639
 File Encoding         : 65001

 Date: 21/03/2018 21:13:57
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

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
BEGIN;
INSERT INTO `bdl_auth` VALUES (5, '123', '123', 1, NULL, NULL, 0, '2018-07-20');
INSERT INTO `bdl_auth` VALUES (6, 'admin', 'admin', 0, NULL, NULL, 1, '2018-06-09');
COMMIT;

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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_customdata
-- ----------------------------
BEGIN;
INSERT INTO `bdl_customdata` VALUES (1, '康复A组', 0, 0);
INSERT INTO `bdl_customdata` VALUES (2, '康复B组', 0, 0);
INSERT INTO `bdl_customdata` VALUES (3, '神经病', 1, 0);
INSERT INTO `bdl_customdata` VALUES (4, '智障', 2, 0);
INSERT INTO `bdl_customdata` VALUES (5, '白痴', 2, 0);
INSERT INTO `bdl_customdata` VALUES (6, '脑残晚期', 1, 0);
COMMIT;

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
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_datacode
-- ----------------------------
BEGIN;
INSERT INTO `bdl_datacode` VALUES (1, 1, 'DList', 'Evaluate', '时机，姿势，评价', 1);
INSERT INTO `bdl_datacode` VALUES (2, 1, 'Evaluate', '0', '没有问题', 1);
INSERT INTO `bdl_datacode` VALUES (3, 2, 'Evaluate', '1', '有些许问题', 1);
INSERT INTO `bdl_datacode` VALUES (4, 3, 'Evaluate', '2', '有问题', 1);
INSERT INTO `bdl_datacode` VALUES (14, 1, 'DList', 'MoveWay', '移乘方式', 1);
INSERT INTO `bdl_datacode` VALUES (15, 1, 'MoveWay', '0', '自理', 1);
INSERT INTO `bdl_datacode` VALUES (16, 2, 'MoveWay', '1', '照看', 1);
INSERT INTO `bdl_datacode` VALUES (17, 3, 'MoveWay', '2', '完全失能', 1);
COMMIT;

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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_deviceprescription
-- ----------------------------
BEGIN;
INSERT INTO `bdl_deviceprescription` VALUES (1, '2018-03-18 10:50:18', '2018-03-18 10:50:20', 7, 2, 'attr1-33*attr2-33*attr3-3*attr4-3*', 5, 10, 30, 2, '注意安全', 1, 30.0);
INSERT INTO `bdl_deviceprescription` VALUES (2, '2018-03-20 00:00:00', '2018-03-20 14:16:39', 2, 1, 'attr1-33*attr2-3*attr3-3*attr4-3*', 3, 3, 3, 0, NULL, 1, 3.0);
INSERT INTO `bdl_deviceprescription` VALUES (3, '2018-03-21 00:00:00', '2018-03-20 14:26:50', 3, 3, 'attr1-4*attr2-4*attr3-4*attr4-4*', 4, 4, 4, 1, NULL, 1, 4.0);
INSERT INTO `bdl_deviceprescription` VALUES (4, '2018-03-19 00:00:00', '2018-03-20 15:09:48', 4, 6, 'attr1-9*attr2-9*attr3-9*', 9, 9, 9, 0, NULL, 1, 9.0);
INSERT INTO `bdl_deviceprescription` VALUES (5, '2018-03-20 00:00:00', '2018-03-20 15:15:54', 5, 2, 'attr1-66*attr2-66*attr3-66*attr4-66*', 66, 66, 66, 2, NULL, 1, 66.0);
INSERT INTO `bdl_deviceprescription` VALUES (6, '2018-03-20 00:00:00', '2018-03-20 15:52:14', 6, 5, 'attr1-4*attr2-44*attr3-4*', 4, 4, 4, 0, NULL, 1, 4.0);
INSERT INTO `bdl_deviceprescription` VALUES (7, '2018-03-20 00:00:00', '2018-03-20 15:52:54', 7, 1, 'attr1-33*attr2-33*attr3-3*attr4-3*', 34, 34, 33, 0, NULL, 1, 3.0);
INSERT INTO `bdl_deviceprescription` VALUES (8, '2018-03-21 17:29:42', '2018-03-21 17:29:42', 8, 1, '水平腿部推蹬机*attr1-0*attr2-0*attr3-1*attr4-2*', 2, 3, 1, 2, '444', 0, 2.0);
COMMIT;

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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_deviceset
-- ----------------------------
BEGIN;
INSERT INTO `bdl_deviceset` VALUES (1, NULL, NULL, '水平腿部推蹬机');
INSERT INTO `bdl_deviceset` VALUES (2, NULL, NULL, '坐姿划船机');
INSERT INTO `bdl_deviceset` VALUES (3, NULL, NULL, '身体伸展弯曲机');
INSERT INTO `bdl_deviceset` VALUES (4, NULL, NULL, '腿部伸展弯曲机');
INSERT INTO `bdl_deviceset` VALUES (5, NULL, NULL, '臀部外展内收机');
INSERT INTO `bdl_deviceset` VALUES (6, NULL, NULL, '胸部推举机');
COMMIT;

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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_devicesort
-- ----------------------------
BEGIN;
INSERT INTO `bdl_devicesort` VALUES (1, '水平腿部推蹬机', '2018-03-18 10:47:52', '2018-03-18 10:47:54', NULL);
INSERT INTO `bdl_devicesort` VALUES (2, '坐姿划船机', NULL, NULL, NULL);
INSERT INTO `bdl_devicesort` VALUES (3, '身体伸展弯曲机', NULL, NULL, NULL);
INSERT INTO `bdl_devicesort` VALUES (4, '腿部伸展弯曲机', NULL, NULL, NULL);
INSERT INTO `bdl_devicesort` VALUES (5, '臀部外展内收机', NULL, NULL, NULL);
INSERT INTO `bdl_devicesort` VALUES (6, '胸部推举机', NULL, NULL, NULL);
COMMIT;

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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_prescriptionresult
-- ----------------------------
BEGIN;
INSERT INTO `bdl_prescriptionresult` VALUES (1, '2018-03-20 00:00:00', '2018-03-20 14:16:39', 3, 2, 3.00, 3, 3.00, 3.00, 3.00, 3.00, 3, 1, '3', '3', '3');
INSERT INTO `bdl_prescriptionresult` VALUES (2, '2018-03-21 00:00:00', '2018-03-20 14:26:50', 4, 3, 4.00, 4, 4.00, 4.00, 4.00, 4.00, 4, 2, '4', '4', '4');
INSERT INTO `bdl_prescriptionresult` VALUES (3, '2018-03-19 00:00:00', '2018-03-20 15:09:48', 9, 4, 9.00, 9, 9.00, 9.00, 9.00, 99.00, 9, 2, '9', '9', '9');
INSERT INTO `bdl_prescriptionresult` VALUES (4, '2018-03-20 00:00:00', '2018-03-20 15:15:54', 66, 5, 66.00, 66, 66.00, 66.00, 66.00, 66.00, 66, 2, '66', '66', '66');
INSERT INTO `bdl_prescriptionresult` VALUES (5, '2018-03-20 00:00:00', '2018-03-20 15:52:14', 4, 6, 4.00, 4, 4.00, 44.00, 4.00, 4.00, 4, 0, '4', '44', '4');
INSERT INTO `bdl_prescriptionresult` VALUES (6, '2018-03-20 00:00:00', '2018-03-20 15:52:54', 3, 7, 3.00, 3, 3.00, 3.00, 3.00, 3.00, 3, 2, '3', '3', '3');
COMMIT;

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
BEGIN;
INSERT INTO `bdl_set` VALUES (1, '00:1C:42:C3:D1:CF', NULL, NULL, NULL, NULL, NULL);
COMMIT;

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
  `si_waterinput` varchar(10) DEFAULT NULL COMMENT '水分摄取',
  `si_careinfo` text COMMENT '看护记录',
  `si_inquiry` varchar(100) DEFAULT NULL COMMENT '问诊票',
  `si_pre_highpressure` varchar(10) DEFAULT NULL COMMENT '高血压(康复前)',
  `si_pre_lowpressure` varchar(10) DEFAULT NULL COMMENT '低血压(康复前)',
  `si_pre_heartrate` varchar(3) DEFAULT NULL COMMENT '心率(康复前)',
  `si_pre_pulse` int(3) DEFAULT NULL COMMENT '脉(康复前)',
  `si_pre_animalheat` varchar(3) DEFAULT NULL COMMENT '体温(康复前)',
  `si_suf_highpressure` varchar(10) DEFAULT NULL COMMENT '高血压(康复后)',
  `si_suf_lowpressure` varchar(10) DEFAULT NULL COMMENT '低血压(康复后)',
  `si_suf_heartrate` varchar(3) DEFAULT NULL COMMENT '心率(康复后)',
  `si_suf_pulse` int(3) DEFAULT NULL COMMENT '脉(康复后)',
  `si_suf_animalheat` varchar(3) DEFAULT NULL COMMENT '体温(康复后)',
  PRIMARY KEY (`pk_si_id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_symptominfo
-- ----------------------------
BEGIN;
INSERT INTO `bdl_symptominfo` VALUES (10, '2018-03-20 13:21:26', '2018-03-20 13:21:26', 1, 0, 0, '12', '111', '1. 身体疲倦,2. 腹泻', '110', '89', '60', 1, '37', '120', '90', '66', 0, '36');
INSERT INTO `bdl_symptominfo` VALUES (11, '2018-03-21 13:46:30', '2018-03-21 13:46:30', 1, 2, 0, '3', '3', '1. 身体疲倦', '3', '3', '3', 0, '3', '3', '3', '3', 1, '3');
INSERT INTO `bdl_symptominfo` VALUES (12, '2018-03-21 13:50:44', '2018-03-21 13:50:44', 1, 2, 0, '5', '5', '1. 身体疲倦', '5', '5', '5', 1, '5', '55', '5', '55', 0, '5');
INSERT INTO `bdl_symptominfo` VALUES (13, '2018-03-21 15:41:03', '2018-03-21 15:41:03', 1, 3, 0, '2', '2', '1. 身体疲倦,9.持续便秘', '2', '2', '2', 0, '2', '2', '2', '2', 1, '2');
COMMIT;

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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_traininfo
-- ----------------------------
BEGIN;
INSERT INTO `bdl_traininfo` VALUES (1, '2018-03-18 10:49:34', '2018-03-18 10:49:36', 2, NULL);
INSERT INTO `bdl_traininfo` VALUES (2, '2018-03-20 00:00:00', '2018-03-20 14:16:39', 1, NULL);
INSERT INTO `bdl_traininfo` VALUES (3, '2018-03-21 00:00:00', '2018-03-20 14:26:50', 1, NULL);
INSERT INTO `bdl_traininfo` VALUES (4, '2018-03-19 00:00:00', '2018-03-20 15:09:48', 1, NULL);
INSERT INTO `bdl_traininfo` VALUES (5, '2018-03-20 00:00:00', '2018-03-20 15:15:54', 1, NULL);
INSERT INTO `bdl_traininfo` VALUES (6, '2018-03-20 00:00:00', '2018-03-20 15:52:14', 1, NULL);
INSERT INTO `bdl_traininfo` VALUES (7, '2018-03-20 00:00:00', '2018-03-20 15:52:54', 1, NULL);
INSERT INTO `bdl_traininfo` VALUES (8, '2018-03-21 17:29:42', '2018-03-21 17:29:42', 1, NULL);
COMMIT;

-- ----------------------------
-- Table structure for bdl_uploadmanagement
-- ----------------------------
DROP TABLE IF EXISTS `bdl_uploadmanagement`;
CREATE TABLE `bdl_uploadmanagement` (
  `pk_um_id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键 自增',
  `um_dataid` int(11) NOT NULL COMMENT '待上传的数据的id',
  `um_datatable` varchar(255) NOT NULL COMMENT '待上传的数据表的名字',
  PRIMARY KEY (`pk_um_id`)
) ENGINE=InnoDB AUTO_INCREMENT=71 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_uploadmanagement
-- ----------------------------
BEGIN;
INSERT INTO `bdl_uploadmanagement` VALUES (1, 1, 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES (2, 1, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (3, 2, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (4, 2, 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES (5, 3, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (6, 4, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (7, 3, 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES (8, 5, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (9, 6, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (10, 4, 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES (11, 7, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (12, 8, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (13, 5, 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES (14, 9, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (15, 10, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (16, 6, 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES (17, 11, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (18, 12, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (19, 7, 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES (20, 13, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (21, 14, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (22, 8, 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES (23, 15, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (24, 16, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (25, 9, 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES (26, 17, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (27, 18, 'bdl_symptominfochild');
INSERT INTO `bdl_uploadmanagement` VALUES (28, 1, 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES (29, 1, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (30, 2, 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES (31, 2, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (32, 3, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (33, 3, 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES (34, 4, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (35, 5, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (36, 6, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (37, 7, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (38, 8, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (39, 9, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (40, 4, 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES (41, 10, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (42, 11, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (43, 12, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (44, 13, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (45, 14, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (46, 15, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (47, 10, 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES (48, 2, 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES (49, 2, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (50, 1, 'bdl_prescriptionresult');
INSERT INTO `bdl_uploadmanagement` VALUES (51, 3, 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES (52, 3, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (53, 2, 'bdl_prescriptionresult');
INSERT INTO `bdl_uploadmanagement` VALUES (54, 4, 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES (55, 4, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (56, 3, 'bdl_prescriptionresult');
INSERT INTO `bdl_uploadmanagement` VALUES (57, 5, 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES (58, 5, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (59, 4, 'bdl_prescriptionresult');
INSERT INTO `bdl_uploadmanagement` VALUES (60, 6, 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES (61, 6, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (62, 5, 'bdl_prescriptionresult');
INSERT INTO `bdl_uploadmanagement` VALUES (63, 7, 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES (64, 7, 'bdl_deviceprescription');
INSERT INTO `bdl_uploadmanagement` VALUES (65, 6, 'bdl_prescriptionresult');
INSERT INTO `bdl_uploadmanagement` VALUES (66, 11, 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES (67, 12, 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES (68, 13, 'bdl_symptominfo');
INSERT INTO `bdl_uploadmanagement` VALUES (69, 8, 'bdl_traininfo');
INSERT INTO `bdl_uploadmanagement` VALUES (70, 8, 'bdl_deviceprescription');
COMMIT;

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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bdl_user
-- ----------------------------
BEGIN;
INSERT INTO `bdl_user` VALUES (1, '2018-03-15 11:22:26', '2018-03-17 11:29:27', '李泽', 'lize', 1, '2018-03-15', '康复A组', '自理', '自理', '神经病', '智障', '傻子\r\n\r\n\r\n', NULL, '1231231232', '438', 0);
INSERT INTO `bdl_user` VALUES (2, '2018-03-15 11:29:07', '2018-03-15 11:29:10', '范明勇', 'fmy', 1, '2018-03-15', 'B组', '不能自理', '不能自理', '智障', '小儿麻痹', '呆子', 'fmy007.jpg', '370111111111111115', '123', 0);
INSERT INTO `bdl_user` VALUES (3, '2018-03-17 11:30:15', '2018-03-17 11:30:15', '周生昌', 'zsc', 1, '1999-12-12', '康复A组', '没有申请', '没有申请', '神经病', '智障', '..\r\n', NULL, '123131231242342', '1231231231', 0);
COMMIT;

SET FOREIGN_KEY_CHECKS = 1;
