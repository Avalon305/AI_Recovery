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
using spms.constant;
using System.IO;

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
        SetterDAO setterDao = new SetterDAO();
        DataCodeDAO DataCodeDAO = new DataCodeDAO();
        List<DataCode> ListDataCode = new List<DataCode>();

        int[] Selected = { 0, 0, 0 };
        int Pk_Set_Id;
        public DesignPage1()
        {
            InitializeComponent();
            //this.Height = SystemParameters.WorkArea.Size.Height;
            //this.Width = SystemParameters.WorkArea.Size.Width;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //加载图片
            if (LanguageUtils.IsChainese())
            {
                title_pic.Source = new BitmapImage(new Uri(@"\view\Images\bdl.PNG", UriKind.Relative));
            }
            else
            {
                title_pic.Source = new BitmapImage(new Uri(@"\view\Images\design.png", UriKind.Relative));
            }
            entity.Setter setter = new entity.Setter();
            //setter.Pk_Set_Id = 5;
            //setterList.Add(setterDao.Load(setter.Pk_Set_Id));
            setterList = setterDao.ListAll();
            //初始化版本号
            if (setterList != null && setterList.Count == 1)
            {
                setterList[0].Set_Version = CommUtil.GetCurrentVersion();
            }
            try
            {
                Pk_Set_Id = setterList[0].Pk_Set_Id;
            }
            catch (ArgumentOutOfRangeException ee)
            {
            }

            try { comboBox2.SelectedIndex = setterList[0].Set_Language; }
            catch (ArgumentOutOfRangeException ee)
            {
                comboBox2.SelectedIndex = 1;
            }
            try
            {
                comboBox1.SelectedIndex = int.Parse(setterList[0].Set_OrganizationSort);
            }
            catch (Exception ee)
            {
                comboBox1.SelectedIndex = 1;
            }
            ObservableCollection<entity.Setter> DataCollection = new ObservableCollection<entity.Setter>(setterList);
            textBox1.DataContext = DataCollection;//设置机构团体名称
            textBox2.DataContext = DataCollection;//设置照片保存文档
            textBox3.DataContext = DataCollection;//设置机构电话
            textBox4.DataContext = DataCollection;//设置当前版本
            textBox5.DataContext = DataCollection;//设置备份地址
            ListDataCode = DataCodeDAO.ListByTypeId("OrganizationSort");//绑定组织区分
            comboBox1.ItemsSource = ListDataCode;
            ListDataCode = DataCodeDAO.ListByTypeId("Language");//绑定语言
            comboBox2.ItemsSource = ListDataCode;
            //下方三个datagrid的实现
            groupList = customDataService.GetAllObjectByType(CustomDataEnum.Group);
            diseaseList = customDataService.GetAllObjectByType(CustomDataEnum.Disease);
            diagnosisList = customDataService.GetAllObjectByType(CustomDataEnum.Diagiosis);

            ((this.FindName("DataGrid2")) as DataGrid).ItemsSource = groupList;
            ((this.FindName("DataGrid3")) as DataGrid).ItemsSource = diseaseList;
            ((this.FindName("DataGrid4")) as DataGrid).ItemsSource = diagnosisList;
        }
        //按钮：高级设置
        private void AdvancedSettings(object sender, RoutedEventArgs e)
        {
            //Window window = (Window)this.Parent;
            //window.Content = new AdvancedSettingPassWord();
            /*AdvancedSettingPassWord advancedSettingPassWord = new AdvancedSettingPassWord
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            advancedSettingPassWord.ShowDialog();
            if (advancedSettingPassWord.IsTrue == 1)
            {
                Window window = (Window)this.Parent;
                window.Content = new AdvancedSettings();
            }*/
            AuthDAO authDAO = new AuthDAO();
            Auther auther = new Auther();
            try
            {
                auther = authDAO.Login(UserConstant.USERNAME, UserConstant.PASSWORD);
                if (auther.Auth_Level == 0x00)
                {
                    Window window = (Window)this.Parent;
                    window.Content = new AdvancedSettings();
                }
                else
                {
                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("当前用户没有权限，请与管理员取得联系", "The current user does not have permission. Please contact the administrator"));
                }
            }
            catch (Exception ee)
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("请先登陆", "Log in first, please"));
            }

        }
        List<int> selectID = new List<int>();  //保存选中要删除行的FID值  

        //返回上一页
        private void GoBack(object sender, RoutedEventArgs e)
        {
            //NavigationService.GetNavigationService(this).GoForward(); //向后转

            Window window = (Window)this.Parent;
            window.Content = new MainPage();


        }

        private void Btn_Confirm(object sender, RoutedEventArgs e)
        {//MessageBox.Show(LanguageUtils.ConvertLanguage("你确认要保存更改吗", "Are you sure you want to save your changes?"), LanguageUtils.ConvertLanguage("提示：", "Tips"), MessageBoxButton.OKCancel) == MessageBoxResult.OK
            if (MessageBoxX.Question(LanguageUtils.ConvertLanguage("你确认要保存更改吗", "Are you sure you want to save your changes?")))
            {
                string textValue1 = textBox1.Text;//机构团体名称
                string textValue2 = textBox2.Text;//照片保存文档
                string textValue3 = textBox3.Text;//机构电话
                //string textValue4 = textBox4.Text;//版本
                string textValue5 = textBox5.Text;//备份
                int comboBox1Selected = comboBox1.SelectedIndex;//机构区分被选择的index
                int comboBox2Selected = comboBox2.SelectedIndex;//语言被选择的index
                entity.Setter setter = new entity.Setter();
                setter.Pk_Set_Id = Pk_Set_Id;
                setter.Set_OrganizationName = textValue1;
                setter.Set_PhotoLocation = textValue2;
                setter.Set_OrganizationPhone = textValue3;
                //setter.Set_Version = textValue4;
                if(textValue5.Contains(" "))
                {
                    MessageBoxX.Warning(LanguageUtils.ConvertLanguage("备份路径不能含有空格", "Backup path cannot contain spaces"));

                }else{
                    setter.Back_Up = textValue5;//备份路径
                    setter.Set_Version = CommUtil.GetCurrentVersion();
                    setter.Set_Language = comboBox2Selected;
                    setter.Set_OrganizationSort = comboBox1Selected.ToString();
                    setterDao.UpdateSetter(setter);
                    //切换语言
                    LanguageUtils.SetLanguage();
                    GoBack(null, null);
                }
                
                

            }
  
            
        }


        private void Grid_Group_Click(object sender, MouseButtonEventArgs e)
        {
            Selected[0] = 1;
            group = (CustomData)DataGrid2.SelectedItem;
            DataGrid2.DataContext = group;
        }
        private void Grid_Disease_Click(object sender, MouseButtonEventArgs e)
        {
            Selected[1] = 1;
            disease = (CustomData)DataGrid3.SelectedItem;
            DataGrid3.DataContext = disease;
        }
        private void Grid_Diagnosis_Click(object sender, MouseButtonEventArgs e)
        {
            Selected[2] = 1;
            diagnosis = (CustomData)DataGrid4.SelectedItem;
            DataGrid4.DataContext = diagnosis;
        }
        private void FlushGroup()
        {
            group = null;
            groupList = customDataService.GetAllObjectByType(CustomDataEnum.Group);
            //groupCollection = new ObservableCollection<CustomData>(groupList);
            ((this.FindName("DataGrid2")) as DataGrid).ItemsSource = groupList;
        }
        private void FlushDisease()
        {
            disease = null;
            diseaseList = customDataService.GetAllObjectByType(CustomDataEnum.Disease);
            //diseaseCollection = new ObservableCollection<CustomData>(diseaseList);
            ((this.FindName("DataGrid3")) as DataGrid).ItemsSource = diseaseList;
        }
        private void FlushDiagnosis()
        {
            diagnosis = null;
            diagnosisList = customDataService.GetAllObjectByType(CustomDataEnum.Diagiosis);
            //diagnosisCollection = new ObservableCollection<CustomData>(diagnosisList);
            ((this.FindName("DataGrid4")) as DataGrid).ItemsSource = diagnosisList;
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
            FlushGroup();
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
            FlushDisease();

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
            FlushDiagnosis();

        }

        private void Group_Update(object sender, RoutedEventArgs e)
        {
            if (Selected[0] == 1 && group != null)
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
                groupUpdata.OldGroupName = group.CD_CustomName;
                groupUpdata.ShowDialog();
                FlushGroup();
                Selected[0] = 0;
            }
            else
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择更新的一行", "Please select an updated row"));
            }


        }
        private void Disease_Update(object sender, RoutedEventArgs e)
        {
            if (Selected[1] == 1 && disease != null)
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
                diseaseUpdata.OldDiseaseName = disease.CD_CustomName;
                diseaseUpdata.ShowDialog();
                FlushDisease();
                Selected[1] = 0;
            }
            else
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择更新的一行", "Please select an updated row"));
            }
        }
        private void Diagnosis_Update(object sender, RoutedEventArgs e)
        {
            if (Selected[2] == 1 && diagnosis != null)
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
                diagnosisUpdata.OldDiagnosisName = diagnosis.CD_CustomName;
                diagnosisUpdata.ShowDialog();
                FlushDiagnosis();
                Selected[2] = 0;
            }
            else
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择更新的一行", "Please select an updated row"));
            }
        }
        private void Group_Delete(object sender, RoutedEventArgs e)
        {
            if (Selected[0] == 1)
            {
                if (MessageBoxX.Question(LanguageUtils.ConvertLanguage("确认要删除所选项吗", "Are you sure you want to delete the selected item?")))
               {  customDataDAO.DeleteCustomDataByPrimaryKey(group.Pk_CD_Id);//在数据库中删除
                    FlushGroup();
                    Selected[0] = 0;
                }
            }
            else
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择删除的一行", "Please select an delete row"));
            }
        }
        private void Disease_Delete(object sender, RoutedEventArgs e)
        {
            if (Selected[1] == 1)
            {
                if (MessageBoxX.Question(LanguageUtils.ConvertLanguage("你确认要删除所选项吗", "Do you want to delete the selected item?")))
                {
                    customDataDAO.DeleteCustomDataByPrimaryKey(disease.Pk_CD_Id);//在数据库中删除
                    FlushDisease();
                    Selected[1] = 0;
                }
            }
            else
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择删除的一行", "Please select an delete row"));
            }
        }
        private void Diagnosis_Delete(object sender, RoutedEventArgs e)
        {
            if (Selected[2] == 1)
            {
                if (MessageBoxX.Question(LanguageUtils.ConvertLanguage("你确认要删除所选项吗", "Do you want to delete the selected item?")))
                {
                    customDataDAO.DeleteCustomDataByPrimaryKey(diagnosis.Pk_CD_Id);//在数据库中删除
                    FlushDiagnosis();
                    Selected[2] = 0;
                }
            }
            else
            {
                MessageBoxX.Warning(LanguageUtils.ConvertLanguage("请选择删除的一行", "Please select an delete row"));
            }
        }
        private void Output_Document(object sender, RoutedEventArgs e)
        {
            /*System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //此处做你想做的事 ...=openFileDialog1.FileName; 
                textBox2.Text = System.IO.Path.GetFullPath(openFileDialog1.FileName);
            }*/
            System.Windows.Forms.FolderBrowserDialog m_Dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = m_Dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string m_Dir = m_Dialog.SelectedPath.Trim();
            this.textBox2.Text = m_Dir + "\\";
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Btn_Confirm(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }

        private void image_load(object sender, RoutedEventArgs e)
        {
            List<spms.entity.Setter> all = new SetterDAO().ListAll();
            if (all != null && all.Count != 0)
            {
                if (all[0].Set_Language == 0)
                {
                    title_pic.Source = new BitmapImage(new Uri("/view/Images/p.jpg", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    title_pic.Source = new BitmapImage(new Uri("/view/Images/p.jpg", UriKind.RelativeOrAbsolute));
                }
            }
        }

        private void OnlineEquipmentSituation(object sender, RoutedEventArgs e)
        {
            OnlineDeviceDetails onlineDeviceDetails = new OnlineDeviceDetails
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            onlineDeviceDetails.ShowDialog();
        }

        /// <summary>
        /// 备份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackUp(object sender, RoutedEventArgs e)
        {
            try
            {

                string DbUserName = ConfigUtil.GetEncrypt("DbUserName", "");
                string DbPassword = ConfigUtil.GetEncrypt("DbPassword", "");
                string DbUrl = ConfigUtil.GetEncrypt("DbUrl", "");
                //指令
               string strAddress = string.Format("mysqldump -h{0} -u{1} -p{2} --default-character-set=utf8 --lock-tables --routines --force --quick ", DbUrl, DbUserName, DbPassword);
               //string strAddress = string.Format("mysqldump -h{0} -u{1} -p{2} --default-character-set=utf8 --lock-tables --routines --force --quick ", "127.0.0.1", "root", "53231323xjh");
                //数据库名称
                //string strDB = "bdl1";
                string strDB = ConfigUtil.GetEncrypt("DbName", "");
                //mysql的路径
                string mysqlPath = new SetterService().getPath() + @"\bin";
                //备份的路径(获取前端页面)
                //string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\BackUp\";
                string filePath = textBox5.Text;
                if (!Directory.Exists(filePath))//如果不存在就创建file文件夹　　             　　                
                    Directory.CreateDirectory(filePath);//创建该文件夹　　
                //判断是否含空格
                if (filePath.IndexOf(" ") != -1)
                {
                    Console.WriteLine("保存路径中含空格");
                    MessageBoxX.Warning(LanguageUtils.ConvertLanguage("备份路径不能含有空格", "Backup path cannot contain spaces"));
                    return;
                }
                //执行的指令
                string cmd = strAddress + strDB + " > " + filePath + "bdl.sql";
                string result = CommUtil.RunCmd(mysqlPath, cmd);
                //MessageBox.Show(result);
                //图片备份
                CommUtil.CopyDirectory(filePath);
                if (("mysqldump: [Warning] Using a password on the command line interface can be insecure.".Trim()).Contains(result.Trim())||
                    ("Warning: Using a password on the command line interface can be insecure.".Trim()).Contains(result.Trim()))
                {

                    MessageBoxX.Info(LanguageUtils.ConvertLanguage("数据备份成功", "Successful data backup"));
                }
                else
                {
                    MessageBoxX.Error(LanguageUtils.ConvertLanguage("数据备份失败", " Data backup failed"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("执行sql异常，备份异常" + ex.ToString());
            }
        }

     

        //private void upgrade(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
