using NuitrackScanProgress.dao;
using NuitrackScanProgress.entity;
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

namespace NuitrackScanProgress.view.Pages
{
    /// <summary>
    /// InputDiseaseName.xaml 的交互逻辑
    /// </summary>
    public partial class NfcTipTwo : Window
    {
        public int G_NfcTipTwoStatus = 0;
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

        public NfcTipTwo()
        {
            InitializeComponent();
            G_NfcTipTwoStatus = 1;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //this.Width = SystemParameters.WorkArea.Size.Width * 0.33;
            //this.Height = this.Width /2.57;
        }
        //确认按钮，关闭此窗体
        private void Button_OK(object sender, RoutedEventArgs e)
        {
            NuitrackScan nuitrackScan = new NuitrackScan
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            SkeletonLengthDAO skeletonLengthDAO = new SkeletonLengthDAO();
            NuitrackEntity nuitrackEntity = new NuitrackEntity();
            if (skeletonLengthDAO.GetPk_user_idByStatus(1) == null)
            {
                this.Hide();
                nuitrackScan.ShowDialog();
                G_NfcTipTwoStatus = 0;
                this.Close();
                return;
            }
            nuitrackEntity = skeletonLengthDAO.GetPk_user_idByStatus(1);

            nuitrackScan.Pk_User_Id = nuitrackEntity.Fk_user_id;
            skeletonLengthDAO.updateStatusByFk_user_id(nuitrackEntity);

            SkeletonLengthEntity skeletonLengthEntity = new SkeletonLengthEntity();
            skeletonLengthEntity = skeletonLengthDAO.GetByPk_User_Id(nuitrackScan.Pk_User_Id);
            if (skeletonLengthEntity != null)
            {
                if (skeletonLengthEntity.Weigth > 0)
                {
                    nuitrackScan.Weigth.Text = skeletonLengthEntity.Weigth.ToString();
                }
                nuitrackScan.Man_Height.Text = skeletonLengthEntity.Height.ToString();
                nuitrackScan.Shoulder_width.Text = skeletonLengthEntity.Shoulder_width.ToString();
                nuitrackScan.Arm_length_up.Text = skeletonLengthEntity.Arm_length_up.ToString();
                nuitrackScan.Arm_length_down.Text = skeletonLengthEntity.Arm_length_down.ToString();
                nuitrackScan.Leg_length_up.Text = skeletonLengthEntity.Leg_length_up.ToString();
                nuitrackScan.Leg_length_down.Text = skeletonLengthEntity.Leg_length_down.ToString();
                nuitrackScan.Body_length.Text = skeletonLengthEntity.Body_length.ToString();
            }

            this.Hide();
            nuitrackScan.ShowDialog();
            G_NfcTipTwoStatus = 0;
            this.Close();
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
