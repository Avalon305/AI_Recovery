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
using spms.view.Pages.ChildWin;

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
        ObservableCollection<CustomData> diseaseCollection;
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
            comboBox2.ItemsSource = LanguageCollection;
            //-------------------------------------------------------------------
            groupList = customDataService.GetAllObjectByType(CustomDataEnum.Group);
            groupCollection = new ObservableCollection<CustomData>(groupList);
            diseaseList = customDataService.GetAllObjectByType(CustomDataEnum.Disease);
            diseaseCollection = new ObservableCollection<CustomData>(diseaseList);
            diagnosisList = customDataService.GetAllObjectByType(CustomDataEnum.Diagiosis);
            diagnosisCollection = new ObservableCollection<CustomData>(diagnosisList);
            ((this.FindName("DataGrid2")) as DataGrid).ItemsSource = groupCollection;
            ((this.FindName("DataGrid3")) as DataGrid).ItemsSource = diseaseCollection;
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


        }
        private void Grid_Group_Click(object sender, MouseButtonEventArgs e)
        {
            group = (CustomData)DataGrid2.SelectedItem;
            DataGrid2.DataContext = group;
        }
        private void Add_Group(object sender, RoutedEventArgs e)
        {
            InputGroupName inputGroupName = new InputGroupName
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            inputGroupName.ShowDialog();
            //添加之后，flush界面
            //致空
            group = null;
            //刷新界面
            groupList = customDataService.GetAllObjectByType(CustomDataEnum.Group);
            groupCollection = new ObservableCollection<CustomData>(groupList);
            ((this.FindName("DataGrid2")) as DataGrid).ItemsSource = groupCollection;

        }
        private void Grid_Disease_Click(object sender, MouseButtonEventArgs e)
        {
            disease = (CustomData)DataGrid3.SelectedItem;
            DataGrid3.DataContext = disease;
        }
        private void Add_Disease(object sender, RoutedEventArgs e)
        {
            InputDiseaseName inputDiseaseName = new InputDiseaseName
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            inputDiseaseName.ShowDialog();
            //添加之后，flush界面
            //致空
            disease = null;
            //刷新界面
            diseaseList = customDataService.GetAllObjectByType(CustomDataEnum.Disease);
            diseaseCollection = new ObservableCollection<CustomData>(diseaseList);
            ((this.FindName("DataGrid3")) as DataGrid).ItemsSource = diseaseCollection;

        }
        private void Grid_Diagnosis_Click(object sender, MouseButtonEventArgs e)
        {
            diagnosis = (CustomData)DataGrid4.SelectedItem;
            DataGrid4.DataContext = diagnosis;
        }
        private void Add_Diagnosis(object sender, RoutedEventArgs e)
        {
            InputDisabilityName inputDiagnosisName = new InputDisabilityName
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            inputDiagnosisName.ShowDialog();
            //添加之后，flush界面
            //致空
            diagnosis = null;
            //刷新界面
            diagnosisList = customDataService.GetAllObjectByType(CustomDataEnum.Diagiosis);
            diagnosisCollection = new ObservableCollection<CustomData>(diagnosisList);
            ((this.FindName("DataGrid4")) as DataGrid).ItemsSource = diagnosisCollection;
        }

        private void Group_Update(object sender, RoutedEventArgs e)
        {
            UpdateGroupName groupUpdata = new UpdateGroupName
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            //类中使用
            CustomData group = (CustomData)DataGrid2.SelectedItem;
            groupUpdata.selectedGroup = group;
            //UI中使用
            groupUpdata.GroupName.Text = group.CD_CustomName;
            groupUpdata.ShowDialog();
            //致空
            group = null;
            //刷新界面
            groupList = customDataService.GetAllObjectByType(CustomDataEnum.Group);
            groupCollection = new ObservableCollection<CustomData>(groupList);
            ((this.FindName("DataGrid2")) as DataGrid).ItemsSource = groupCollection;


        }
        private void Disease_Update(object sender, RoutedEventArgs e)
        {
            UpdateDiseaseName diseaseUpdata = new UpdateDiseaseName
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            //类中使用
            CustomData disease = (CustomData)DataGrid3.SelectedItem;
            diseaseUpdata.selectedDisease = disease;
            //UI中使用
            diseaseUpdata.DiseaseName.Text = disease.CD_CustomName;
            diseaseUpdata.ShowDialog();
            //致空
            disease = null;
            //刷新界面
            diseaseList = customDataService.GetAllObjectByType(CustomDataEnum.Disease);
            diseaseCollection = new ObservableCollection<CustomData>(diseaseList);
            ((this.FindName("DataGrid3")) as DataGrid).ItemsSource = diseaseCollection;
        }
        private void Diagnosis_Update(object sender, RoutedEventArgs e)
        {
            UpdateDiagnosisName diagnosisUpdata = new UpdateDiagnosisName
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            //类中使用
            CustomData diagnosis = (CustomData)DataGrid4.SelectedItem;
            diagnosisUpdata.selectedDiagnosis = diagnosis;
            //UI中使用
            diagnosisUpdata.DiagnosisName.Text = diagnosis.CD_CustomName;
            diagnosisUpdata.ShowDialog();
            //致空
            diagnosis = null;
            //刷新界面
            diagnosisList = customDataService.GetAllObjectByType(CustomDataEnum.Diagiosis);
            diagnosisCollection = new ObservableCollection<CustomData>(diagnosisList);
            ((this.FindName("DataGrid4")) as DataGrid).ItemsSource = diagnosisCollection;
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
                CustomData group = (CustomData)DataGrid2.SelectedItem;
                group.Pk_CD_Id = ID;
                group.Is_Deleted = 1;
                //customDataDAO.DeleteByPrimaryKey(group);//在数据库中删除
                customDataDAO.UpdateByPrimaryKey(group);
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
                CustomData disease = (CustomData)DataGrid3.SelectedItem;
                disease.Pk_CD_Id = ID;
                disease.Is_Deleted = 1;
                customDataDAO.UpdateByPrimaryKey(disease);//isDeleted变为1
                for (int i = 0; i < diseaseCollection.Count; i++)
                {
                    if (diseaseCollection[i].Pk_CD_Id == ID) diseaseCollection.RemoveAt(i);//在collection中删除
                }

            }
        }
        private void Diagnosis_Delete(object sender, RoutedEventArgs e)
        {
            foreach (int ID in selectID)
            {
                CustomData diagnosis = (CustomData)DataGrid4.SelectedItem;
                diagnosis.Pk_CD_Id = ID;
                diagnosis.Is_Deleted = 1;
                customDataDAO.UpdateByPrimaryKey(diagnosis);//isDeleted变为1
                for (int i = 0; i < diagnosisCollection.Count; i++)
                {
                    if (diagnosisCollection[i].Pk_CD_Id == ID) diagnosisCollection.RemoveAt(i);//在collection中删除
                }

            }
        }

    }
}
