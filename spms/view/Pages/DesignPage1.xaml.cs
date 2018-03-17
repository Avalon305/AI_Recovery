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
using System.Windows.Navigation;
using System.Windows.Shapes;
using spms.entity;
using spms.dao;
using System.Collections.ObjectModel;
using spms.util;
using Dapper;
using spms.service;
using static spms.entity.CustomData;
using spms.dao.app;

namespace spms.view.Pages
{
    /// <summary>
    /// DesignPage1.xaml 的交互逻辑
    /// </summary>
    public partial class DesignPage1 : Page
    {
        List<CustomData> groupList = new List<CustomData>();
        List<CustomData> diseaseList = new List<CustomData>();
        List<CustomData> diagnosisList = new List<CustomData>();
        int judgeGroup = 0;
        int judgeDisease = 0;
        int judgeDiagnosis = 0;
        CustomDataService customDataService = new CustomDataService();
        CustomData group = new CustomData();
        CustomData disease = new CustomData();
        CustomData diagnosis = new CustomData();
        CustomDataDAO customDataDAO = new CustomDataDAO();
        List<entity.Setter> setterList = new List<entity.Setter>();
        List<entity.Setter> UniqueIdList = new List<entity.Setter>();
        List<entity.Setter> LanguageList = new List<entity.Setter>();
        SetterDAO setterDao = new SetterDAO();
        ObservableCollection<CustomData> groupCollection;
        ObservableCollection<CustomData> dieaseCollection;
        ObservableCollection<CustomData> diagnosisCollection;
        public DesignPage1()
        {
            InitializeComponent();
            entity.Setter setter = new entity.Setter();
            setter.Pk_Set_Id = 1;
            setterList.Add(setterDao.Load(setter.Pk_Set_Id));
            UniqueIdList = setterDao.ListAll();
            LanguageList = setterDao.ListAll();
            ObservableCollection<entity.Setter> DataCollection = new ObservableCollection<entity.Setter>(setterList);
            ObservableCollection<entity.Setter> UniqueIdCollection = new ObservableCollection<entity.Setter>(UniqueIdList);
            ObservableCollection<entity.Setter> LanguageCollection = new ObservableCollection<entity.Setter>(LanguageList);
            textBox1.DataContext = DataCollection;//设置机构团体名称
            textBox2.DataContext = DataCollection;//设置照片保存文档
            comboBox1.ItemsSource = UniqueIdCollection;//绑定到combobox
            //comboBox1.DisplayMemberPath = "Set_Unique_Id"; //显示机构区分
            comboBox2.ItemsSource = LanguageCollection;
            //comboBox2.DisplayMemberPath = "Set_Language";//显示语言
            //-------------------------------------------------------------------
            groupList = customDataService.GetAllObjectByType(CustomDataEnum.Group);
            groupCollection = new ObservableCollection<CustomData>(groupList);
            diseaseList = customDataService.GetAllObjectByType(CustomDataEnum.Disease);
            dieaseCollection = new ObservableCollection<CustomData>(diseaseList);
            diagnosisList = customDataService.GetAllObjectByType(CustomDataEnum.Diagiosis);
            diagnosisCollection = new ObservableCollection<CustomData>(diagnosisList);
            ((this.FindName("DataGrid2")) as DataGrid).ItemsSource = groupCollection;
            ((this.FindName("DataGrid3")) as DataGrid).ItemsSource = dieaseCollection;
            ((this.FindName("DataGrid4")) as DataGrid).ItemsSource = diagnosisCollection;


        }
        //按钮：高级设置
        private void AdvancedSettings(object sender, RoutedEventArgs e)
        {
            Window window = (Window)this.Parent;
            window.Content = new AdvancedSettings();

        }
        List<int> selectID = new List<int>();  //保存选中要删除行的FID值  

        //返回上一页
        private void GoBack(object sender, RoutedEventArgs e)
        {
            //NavigationService.GetNavigationService(this).GoForward(); //向后转

            if (this.NavigationService.CanGoForward)
            {
                this.NavigationService.GoForward();
            }


        }

        private void Btn_Confirm(object sender, RoutedEventArgs e)
        {
            string textValue1 = textBox1.Text;//机构团体名称
            string textValue2 = textBox2.Text;//照片保存文档
            int comboBox1Selected = comboBox1.SelectedIndex;//机构区分被选择的index
            int comboBox2Selected = comboBox2.SelectedIndex;//语言被选择的index
            string comboBox1Value = comboBox1.Text;//机构区分被选择的index
            string scomboBox2Value = comboBox2.Text;//语言被选择的index
            int comboBox2Value = Convert.ToInt32(scomboBox2Value);//当前值
            //comboBox1.SelectedIndex = comboBox1Selected;//选中的索引
            //comboBox2.SelectedIndex = comboBox2Selected;//选中的索引
            entity.Setter setter = new entity.Setter();
            setter.Pk_Set_Id = 1;
            setter.Set_OrganizationName = textValue1;
            setter.Set_PhotoLocation = textValue2;
            using (var conn = DbUtil.getConn())//更新机构团体和照片保存文档
            {
                conn.Execute("update bdl_set set Set_OrganizationName=@Set_OrganizationName,Set_PhotoLocation=@Set_PhotoLocation where Pk_Set_Id=@Pk_Set_Id", setter);
            }
            entity.Setter setterCombo1 = new entity.Setter();
            setterCombo1.Set_Language = comboBox2Value;
            setterCombo1.Pk_Set_Id = (int)comboBox2.SelectedValue;
            using (var conn = DbUtil.getConn())//更新语言
            {
                conn.Execute("update bdl_set set Set_Language=@Set_Language where Pk_Set_Id=@Pk_Set_Id", setterCombo1);
            }

            DataGrid3.CanUserAddRows = false;
            DataGrid4.CanUserAddRows = false;

            if (judgeGroup == 1)
            {
                DataGrid2.CanUserAddRows = false;
                group = DataGrid2.SelectedItem as CustomData; //获取该行的记录  
                customDataDAO.Insert(group);
            }
            if (judgeDisease == 1)
            {
                DataGrid3.CanUserAddRows = false;
                disease = DataGrid3.SelectedItem as CustomData;//获取该行的记录  
                customDataDAO.Insert(disease);
            }
            if (judgeDiagnosis == 1)
            {
                DataGrid4.CanUserAddRows = false;
                diagnosis = DataGrid3.SelectedItem as CustomData;//获取该行的记录  
                customDataDAO.Insert(diagnosis);
            }
        }

        private void Add_Group(object sender, RoutedEventArgs e)
        {
            judgeGroup = 1;  //现在为添加状态       
            DataGrid2.CanUserAddRows = true;

        }

        private void Add_Disease(object sender, RoutedEventArgs e)
        {
            judgeDisease = 1;  //现在为添加状态       
            DataGrid3.CanUserAddRows = true;

        }

        private void Add_Diagnosis(object sender, RoutedEventArgs e)
        {
            judgeDiagnosis = 1;  //现在为添加状态       
            DataGrid3.CanUserAddRows = true;
        }

        private void Group_Update(object sender, RoutedEventArgs e)
        {
            using (var conn = DbUtil.getConn())
            {
                conn.Execute("update bdl_customdata set CD_CustomName=@CD_CustomName where Pk_CD_Id=@Pk_CD_Id", groupList);
            }
        }
        private void Disease_Update(object sender, RoutedEventArgs e)
        {
            using (var conn = DbUtil.getConn())
            {
                conn.Execute("update bdl_customdata set CD_CustomName=@CD_CustomName where Pk_CD_Id=@Pk_CD_Id", diseaseList);
            }
        }
        private void Diagnosis_Update(object sender, RoutedEventArgs e)
        {
            using (var conn = DbUtil.getConn())
            {
                conn.Execute("update bdl_customdata set CD_CustomName=@CD_CustomName where Pk_CD_Id=@Pk_CD_Id", diagnosisList);
            }
        }
        private void CheckBox1_Click(object sender, RoutedEventArgs e)//单击CheckBox触发事件
        {
            CheckBox checkBox = sender as CheckBox;
            int ID = int.Parse(checkBox.Tag.ToString());   //获取该行的FID  
            var IsChecked = checkBox.IsChecked;
            if (IsChecked == true)
            {
                selectID.Add(ID);         //如果选中就保存FID  
            }
            else
            {
                selectID.Remove(ID);  //如果选中取消就删除里面的FID  
            }
        }
        private void CheckBox2_Click(object sender, RoutedEventArgs e)//单击CheckBox触发事件
        {
            CheckBox checkBox = sender as CheckBox;
            int ID = int.Parse(checkBox.Tag.ToString());   //获取该行的FID  
            var IsChecked = checkBox.IsChecked;
            if (IsChecked == true)
            {
                selectID.Add(ID);         //如果选中就保存FID  
            }
            else
            {
                selectID.Remove(ID);  //如果选中取消就删除里面的FID  
            }
        }
        private void CheckBox3_Click(object sender, RoutedEventArgs e)//单击CheckBox触发事件
        {
            CheckBox checkBox = sender as CheckBox;
            int ID = int.Parse(checkBox.Tag.ToString());   //获取该行的FID  
            var IsChecked = checkBox.IsChecked;
            if (IsChecked == true)
            {
                selectID.Add(ID);         //如果选中就保存FID  
            }
            else
            {
                selectID.Remove(ID);  //如果选中取消就删除里面的FID  
            }
        }
        private void Group_Delete(object sender, RoutedEventArgs e)
        {
            foreach (int ID in selectID)
            {
                group.Pk_CD_Id = ID;
                customDataDAO.DeleteByPrimaryKey(group);//在数据库中删除
                for (int i = 0; i < groupCollection.Count; i++)
                {
                    if (groupCollection[i].Pk_CD_Id == ID) groupCollection.RemoveAt(i);//在collection中删除
                }

            }
        }
        private void Disease_Delete(object sender, RoutedEventArgs e)
        {
            foreach (int ID in selectID)
            {
                disease.Pk_CD_Id = ID;
                customDataDAO.DeleteByPrimaryKey(disease);//在数据库中删除
                for (int i = 0; i < dieaseCollection.Count; i++)
                {
                    if (dieaseCollection[i].Pk_CD_Id == ID) dieaseCollection.RemoveAt(i);//在collection中删除
                }

            }
        }
        private void Diagnosis_Delete(object sender, RoutedEventArgs e)
        {
            foreach (int ID in selectID)
            {
                diagnosis.Pk_CD_Id = ID;
                customDataDAO.DeleteByPrimaryKey(diagnosis);//在数据库中删除
                for (int i = 0; i < diagnosisCollection.Count; i++)
                {
                    if (diagnosisCollection[i].Pk_CD_Id == ID) diagnosisCollection.RemoveAt(i);//在collection中删除
                }

            }
        }

    }
}
