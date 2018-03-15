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
using spms.entity;

namespace spms.view.Pages.ChildWin
{
    /// <summary>
    /// InputTrainingResults.xaml 的交互逻辑
    /// </summary>
    public partial class InputTrainingResults : Window
    {
        List<string> list = new List<string> { "自理", "照看", "完全失能" };
        List<string> list2 = new List<string> { "时机1", "时机2", "时机3" };
        public InputTrainingResults()
        {
            InitializeComponent();
            HLPMoveway.ItemsSource = list;
            HLPEvaluate.ItemsSource = list2;
        }
        //取消操作，关闭窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            string da = dp.SelectedDate.ToString();//实施日期
            int groupCount;//组数
            int groupNum;//每组个数
            int relaxTime;//休息时间
            double weight;//砝码
            int moveWay;//移乘方式
            string sportStrength;//自觉运动强度
            double time1;//时间1
            double time2;//时间2
            int distance;//距离
            int countWorkqu;//总工
            double cal;//热量
            double index;//指数
            int finishGroup;//已完成组数
            string evaluate;//时机、姿势
            string attentionPoint;//注意点
            string userThoughts;//用户感想
            string memo;//备忘
            
            //获取数据
            groupCount = Convert.ToInt32(HLPGroupcount.Text);
            groupNum = Convert.ToInt32(HLPGroupnum.Text);
            relaxTime = Convert.ToInt32(HLPRelaxTime.Text);
            weight = Convert.ToDouble(HLPWeight.Text);
            moveWay = HLPMoveway.SelectedIndex;
            sportStrength = HLPSportstrength.Text;
            time1 = Convert.ToDouble(HLPTime1.Text);
            time2 = Convert.ToDouble(HLPTime2.Text);
            distance = Convert.ToInt32(HLPDistance.Text);
            countWorkqu = Convert.ToInt32(HLPCountworkqu.Text);
            cal = Convert.ToDouble(HLPCal.Text);
            index = Convert.ToDouble(HLPIndex.Text);
            finishGroup = Convert.ToInt32(HLPFinishgroup.Text);
            evaluate = HLPEvaluate.Text;
            attentionPoint = HLPAttentionpoint.Text;
            userThoughts = HLPUserthoughts.Text;
            memo = HLPMemo.Text;

            //构造对象
            PrescriptionResult prescriptionResult = new PrescriptionResult();
            prescriptionResult.Fk_DP_Id = 2;
            prescriptionResult.Gmt_Create = DateTime.Now;
            prescriptionResult.Gmt_Modified = DateTime.Now;
            prescriptionResult.PR_AttentionPoint = attentionPoint;
            prescriptionResult.PR_Cal = cal;
            prescriptionResult.PR_CountWorkQuantity = countWorkqu;
            prescriptionResult.PR_Distance = distance;
            prescriptionResult.PR_Evaluate = evaluate;
            prescriptionResult.PR_FinishGroup = finishGroup;
            prescriptionResult.PR_Index = index;
            prescriptionResult.PR_Memo = memo;
            prescriptionResult.PR_SportStrength = int.Parse(sportStrength);
            
            prescriptionResult.PR_UserThoughts = userThoughts;
            prescriptionResult.PR_Time1 = time1;
            prescriptionResult.PR_Time2 = time2;

            //打印
            MessageBox.Show(prescriptionResult.ToString());
        }
    }
}
