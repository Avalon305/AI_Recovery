
using Dapper;
using spms.dao.app;
using spms.entity;
using spms.service;
using spms.util;
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
using static spms.entity.CustomData;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// InputDiseaseName.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateDiseaseName : Window
    {
        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        public CustomData selectedDisease = new CustomData();
        public User user = new User();
        //联合更新使用
        public string OldDiseaseName;
        /// <summary>
        /// 自定义三项service
        /// </summary>
        CustomDataService customDataService = new CustomDataService();
        CustomDataDAO CustomDataDAO = new CustomDataDAO();

        public UpdateDiseaseName()
        {
            InitializeComponent();
            this.Width = SystemParameters.WorkArea.Size.Width * 0.33;
            this.Height = this.Width / 2.57;
        }
        //取消按钮，关闭此窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_SaveClick(object sender, RoutedEventArgs e)
        {
            //获取文本框的值
            string Name = DiseaseName.Text;
            CustomData CustomData = CustomDataDAO.GetListByTypeIDAndName(CustomDataEnum.Disease, Name);
            if (CustomData != null)
            {
                MessageBoxX.Info(LanguageUtils.ConvertLanguage("疾病名称已存在", "Disease name already exists"));
            }
            string value = this.DiseaseName.Text;
            selectedDisease.CD_CustomName = value;
            DynamicParameters Parameters = new DynamicParameters();
            Parameters.Add("user_illnessname", value);
            Parameters.Add("old", OldDiseaseName);
            using (var conn = DbUtil.getConn())
            {
                conn.Execute("update bdl_customdata set CD_CustomName=@CD_CustomName where Pk_CD_Id=@Pk_CD_Id", selectedDisease);
                string query = "update bdl_user SET user_illnessname = @user_illnessname where user_illnessname=@old";
                conn.Execute(query, Parameters);
            }
            this.Close();
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_SaveClick(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }
    }
}
