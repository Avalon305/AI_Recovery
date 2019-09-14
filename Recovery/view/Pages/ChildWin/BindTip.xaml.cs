using Recovery.dao;
using Recovery.entity;
using Recovery.entity.newEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Recovery.view.Pages.ChildWin
{
    /// <summary>
    /// BindTip.xaml 的交互逻辑
    /// </summary>
    public partial class BindTip : Window
    {
        private int userId;
        private List<UserRelation> userRelations;
        public int isCancle;

        public int UserId { get => userId; set => userId = value; }

        internal List<UserRelation> UserRelations { get => userRelations; set => userRelations = value; }

        public BindTip()
        {
            InitializeComponent();
        }

        private void Button_OK(object sender, RoutedEventArgs e)
        {
            UserRelationDao userRelationDao = new UserRelationDao();
            TrainInfoDAO trainInfoDAO = new TrainInfoDAO();
            List<TrainInfo> trainInfos = new List<TrainInfo>();
            DevicePrescriptionDAO devicePrescriptionDAO = new DevicePrescriptionDAO();
            PersonalSettingDao personalSettingDao = new PersonalSettingDao();
            if (userRelations.Count == 1)
            {
                for (int i = 0; i < userRelations.Count; i++)
                {
                    if (userRelations[i].Fk_user_id != userId)
                    {
                        UserRelation userRelationTemp = new UserRelation();
                        userRelationTemp.Gmt_modified = DateTime.Now;//
                        userRelationTemp.Fk_user_id = userRelations[i].Fk_user_id;
                        userRelationDao.updateBind_idToNull(userRelationTemp);
                    }
                    trainInfoDAO.UpdateStatusIs3ByUserId(userRelations[i].Fk_user_id);
                    trainInfos = trainInfoDAO.GetTiIdUserId(userRelations[i].Fk_user_id);
                    for(int j = 0; j < trainInfos.Count; j++)
                    {
                        devicePrescriptionDAO.updateDpStatusByTiId(trainInfos[j].Pk_TI_Id);
                    }
                    personalSettingDao.DeleteSettingByUserId(userRelations[i].Fk_user_id.ToString());
                }
            }
            this.Close();
        }

        private void Button_Cancle(object sender, RoutedEventArgs e)
        {
            isCancle = 1;
            this.Close();
        }
    }
}
