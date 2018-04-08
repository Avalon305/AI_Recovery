
using Microsoft.Win32;
using spms.entity;
using spms.service;
using spms.util;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : Window
    {
        //照片的url
        public string photoName { get; set; }
        //用户是否自己选择照片
        private bool userIfSelectPic = false;
        //去除窗体叉号
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private string oldPhotoName;
        //保存用户照片的路径
        string userPhotoPath = null;
        //service层初始化
        UserService userService = new UserService();
        CustomDataService customDataService = new CustomDataService();
        //小组的名称列表
        List<string> groupList;
        //疾病名称列表
        List<string> diseaseList ;
        //残障名称列表
        List<string> diagnosisList;
        //护理度列表
        List<string> careList = new List<string> { "没有申请", "自理", "要支援一", "要支援二", "要介护1", "要介护2", "要介护3", "要介护4", "要介护5" };


        public Register()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.MaxHeight = SystemParameters.WorkArea.Size.Height;
            this.MaxWidth = SystemParameters.WorkArea.Size.Width;

            groupList = customDataService.GetAllByType(CustomDataEnum.Group);
            diseaseList = customDataService.GetAllByType(CustomDataEnum.Disease);
            diagnosisList = customDataService.GetAllByType(CustomDataEnum.Diagiosis);
            //初始化下拉框值
            c2.ItemsSource = groupList;
            c5.ItemsSource = diseaseList;
            c6.ItemsSource = diagnosisList;
            //护理程度下拉框
            c3.ItemsSource = careList;
            c4.ItemsSource = careList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void c1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //添加疾病名称
        private void DiseaseNameAddition(object sender, RoutedEventArgs e)
        {
            InputDiseaseName inputDiseaseName = new InputDiseaseName
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            inputDiseaseName.ShowDialog();
            //flush 界面
            diseaseList = customDataService.GetAllByType(CustomDataEnum.Disease);
            c5.ItemsSource = diseaseList;
        }
        //添加残障名称
        private void DisabilityNameAddition(object sender, RoutedEventArgs e)
        {
            InputDisabilityName inputDisabilityName = new InputDisabilityName
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            inputDisabilityName.ShowDialog();
            //flush 界面
            diagnosisList = customDataService.GetAllByType(CustomDataEnum.Diagiosis);
            c6.ItemsSource = diagnosisList;
        }
        
        //输入非公开信息
        private void InputNonPublicInformationPassword(object sender, RoutedEventArgs e)
        {
            NonPublicInfomationPass nonPublicInfomationPass = new NonPublicInfomationPass
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            nonPublicInfomationPass.ShowDialog();
            //将非公开信息框显示
            if (nonPublicInfomationPass.result == "success")
            {
                this.Non_Public_Information.Visibility = System.Windows.Visibility.Visible;
                //调整该窗体宽度
                this.Width = 800;
            }
            else {
                MessageBox.Show(LanguageUtils.ConvertLanguage("密码不正确！", "Incorrect password!"));
            }
            
        }

        //取消操作，关闭窗体
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确定保存添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OK(object sender, RoutedEventArgs e)
        {
            //获取用户身份证的内容
            //string userID = t1.Text;
            //获取用户姓名的内容
            string userName = t2.Text;
            //获取用户姓名拼音的内容
            string usernamePY = t3.Text;
            //获取用户性别的内容
            string usersex = c1.Text;
            //获取用户出生年月的内容
            string brithday = t4.Text;
            //获得身份证号
            string IDCard = this.IDCard.Text;
            //获得手机号
            string phone = this.phoneNum.Text;
            //获取身份证与手机号之后马上查重
            //if (userService.GetByIdCard(IDCard) != null)
            //{
            //    //身份证重复气泡提示
            //    Error_Info_IDCard.Content = "该身份证已注册";
            //    bubble_IDCard.IsOpen = true;
            //}
            //if (userService.GetByPhone(phone) != null)
            //{
            //    //手机重复气泡提示
            //    Error_Info_Phone.Content = "该手机号已注册";
            //    bubble_phone.IsOpen = true;
            //}
            if (!String.IsNullOrEmpty(IDCard))
            {
                if (IDCard.Length < 18)
                {
                    for (int i = IDCard.Length; i < 18; i++)
                    {
                        IDCard = IDCard + '0';
                    }
                    if (userService.GetByIdCard(IDCard) != null)
                    {
                        //身份证重复气泡提示
                        Error_Info_IDCard.Content = LanguageUtils.ConvertLanguage("该身份证已注册", "This ID is registered");
                        bubble_IDCard.IsOpen = true;
                        return;

                    }
                }
                else if (!inputlimited.InputLimited.IsIDcard(IDCard))
                {
                    Error_Info_IDCard.Content = LanguageUtils.ConvertLanguage("请输入正确的身份证号码", "Please enter a valid ID number");
                    bubble_IDCard.IsOpen = true;
                    return;
                }
                else if (userService.GetByIdCard(IDCard) != null)
                {
                    //身份证重复气泡提示
                    Error_Info_IDCard.Content = LanguageUtils.ConvertLanguage("该身份证已注册", "This ID is registered");
                    bubble_IDCard.IsOpen = true;
                    return;

                }
                else
                {
                    bubble_IDCard.IsOpen = false;
                }


            }
            else
            {
                Error_Info_IDCard.Content = LanguageUtils.ConvertLanguage("请输入身份证号码", "Please enter the ID number");
                bubble_IDCard.IsOpen = true;
                return;
            }
            if (String.IsNullOrEmpty(phone))
            {
                Error_Info_Phone.Content = LanguageUtils.ConvertLanguage("请输入手机号", "Please enter phone number");
                bubble_phone.IsOpen = true;
                return;
            }
            else { 
                if (userService.GetByPhone(phone) != null)
                {
                    //手机重复气泡提示
                    Error_Info_Phone.Content = LanguageUtils.ConvertLanguage("该手机号已注册", "The phone number is registered");
                    bubble_phone.IsOpen = true;
                    return;
                }
                else if (!inputlimited.InputLimited.IsHandset(phone) && !String.IsNullOrEmpty(phone))
                {
                    Error_Info_Phone.Content = LanguageUtils.ConvertLanguage("请输入正确的手机号", "Please enter a valid phone number");
                    bubble_phone.IsOpen = true;
                    return;
                }
            }
            string IdCard = this.IDCard.Text;
            string name = t3.Text;
            //获取小组名称的内容
            string groupName = c2.Text;
            //获取初期要介护度的内容
            string initial = c3.Text;
            //获取现在要介护度的内容
            string now = c4.Text;
            //获取疾病名称的内容
            string sicknessName = c5.Text;
            //获取残障名称的内容
            string disabilityName = c6.Text;
            //获取备忘的内容
            TextRange text = new TextRange(t6.Document.ContentStart, t6.Document.ContentEnd);
            string memo = text.Text;
            //私密信息
            String noPublicInfo = this.noPublicInfoText.Text;
            //string secretMessage = this.Non_Public_Information.
            User user = new User();
            user.User_Privateinfo = noPublicInfo;
            user.User_Birth = Convert.ToDateTime(brithday);
            user.User_GroupName = groupName;
            //设置私密信息
            user.User_Privateinfo = noPublicInfo == null ? "" : noPublicInfo;
            //user.User_Privateinfo = secretMessage==null?"":secretMessage;
            if (IdCard == null || name == null || IDCard == "" || name == "")
            {
                System.Windows.MessageBox.Show(LanguageUtils.ConvertLanguage("没有填写身份证或者名字（拼音）", "No identity card or First Name"), LanguageUtils.ConvertLanguage("信息提示", "Tips"));
                return;
            }

            user.User_IDCard = IDCard;
            user.User_IllnessName = sicknessName;
            user.User_InitCare = initial;
            user.User_Memo = memo;
            user.User_Name = userName;
            user.User_Namepinyin = usernamePY;
            user.User_Nowcare = now;
            user.User_PhysicalDisabilities = disabilityName;
            user.User_Sex = (byte?)(LanguageUtils.EqualsResource(usersex, "AddOrEditView.M") ? 1 : 0);
            user.User_Phone = phone;

            // 如果用户是自己选择现成的图片，将图片保存在安装目录下
            if (IdCard != null && name != null && IDCard != "" && name != "" && userIfSelectPic != false)
            {
                // 用户选择的图片文件的 原路径
                string sourcePic = userPhotoPath;
                // 压缩
                string targetPic = CommUtil.GetUserPicTemp();
                Random random = new Random();
                int rd = random.Next(0, 100000);
                targetPic += t3.Text.ToString() + rd.ToString() + ".gif";
                
                //targetPic += ".gif";
                // 获得保存图片的文件夹
                String dirPath = CommUtil.GetUserPicTemp();

                //判断是否存在
                if (Directory.Exists(dirPath))
                {
                    //Response.Write("已存在");
                }
                else
                {
                    //Response.Write("不存在，正在创建");
                    Directory.CreateDirectory(dirPath);//创建新路径
                }

                //压缩一下并且储存图片
                long picLen = 0;
                GetPicThumbnail(sourcePic, targetPic,184,259,20);

                FileInfo di = new FileInfo(targetPic);
                picLen = di.Length;
                picLen /= 1024;
                
                // 如果图片太大就重新选择
                if (picLen > 40)
                {
                    MessageBox.Show(LanguageUtils.ConvertLanguage("图片过大，请重新选择，不能超过40KB", "The picture is too large. Please select it again. Cannot exceed 40KB"));
                    File.Delete(targetPic);
                    return;
                }

                string userSelectFinalPic = CommUtil.GetUserPic(photoName);
                File.Copy(targetPic, userSelectFinalPic);
                
            }
            else if(userIfSelectPic != false)
            {
                MessageBox.Show(LanguageUtils.ConvertLanguage("没有填写身份证或者名字（拼音）", "No identity card or First Name"), LanguageUtils.ConvertLanguage("信息提示", "Tips"));
                return;
            }

            string tempPic = CommUtil.GetUserPicTemp(photoName);

            // 如果是正常拍照得到的图片
            if (File.Exists(tempPic))
            {
                string finalPic = CommUtil.GetUserPic(photoName);                
                File.Copy(tempPic, finalPic);
            }

            //将图片的url传到数据库
            user.User_PhotoLocation = photoName;
            userService.InsertUser(user);
            //保存照片的路径
            this.Close();
        }

        // 摄像
        private void Photograph(object sender, RoutedEventArgs e)
        {
            if (t3.Text == "" || IDCard.Text == "")
            {
                MessageBox.Show(LanguageUtils.ConvertLanguage("请填写完整信息", "Please fill in the complete information"));
                return;
            }
           
            Photograph photograph = new Photograph
            {
                Owner = Window.GetWindow(this),
                ShowActivated = true,
                ShowInTaskbar = false,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            
            photograph.getName = t3.Text;
            photograph.id = IDCard.Text;
            photograph.oldPhotoName = oldPhotoName;
            photograph.ShowDialog();
            photoName = photograph.photoName;
            oldPhotoName = photoName;
            //photograph.Close();
            Console.WriteLine(photoName);

            //展示摄像的时候的图片
            if (File.Exists(CommUtil.GetUserPic() + photoName))
            {
                //MessageBox.Show("hi open!");
                BitmapImage bitmap = new BitmapImage(new Uri(CommUtil.GetUserPic() + photoName, UriKind.Absolute));//打开图片
                pic.Source = bitmap.Clone();//将控件和图片绑定
                
            }
        }

        // 用户自主选择照片
        private void Select_Picture_Show(object sender, RoutedEventArgs e)
        {

            if (t3.Text == "" || IDCard.Text == "")
            {
                MessageBox.Show(LanguageUtils.ConvertLanguage("请填写完整信息", "Please fill in the complete information"));
                return;
            }
            
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Title = "请选择要插入的图片";
                ofd.Filter = "JPG图片|*.jpg|BMP图片|*.bmp|Gif图片|*.gif";
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                ofd.Multiselect = false;

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    BitmapImage image = new BitmapImage(new Uri(ofd.FileName, UriKind.Absolute));//打开图片
                    var win2 = new PhotoCutWindow
                    {
                        Owner = Window.GetWindow(this),
                        ShowActivated = true,
                        ShowInTaskbar = false,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };
                    win2.getName = t3.Text;
                    win2.id = IDCard.Text;
                    win2.oldPhotoName = oldPhotoName; ;
                    win2.SetImage(image);

                    win2.ShowDialog();
                    photoName = win2.photoName;
                    oldPhotoName = photoName;
                    //photograph.Close();
                    Console.WriteLine(photoName);
                    //在更新页面上展示用户 刚刚更新的照片
                    string photoUpdatePhoto = CommUtil.GetUserPic() + photoName;
                    if (File.Exists(photoUpdatePhoto))
                    {
                        //MessageBox.Show("hi open!");
                        BitmapImage i = new BitmapImage(new Uri(photoUpdatePhoto, UriKind.Absolute));//打开图片
                        pic.Source = i.Clone();//将控件和图片绑定
                    }
                }
                else
                {
                    userIfSelectPic = false;
                }

            }
        }

        //设置手机号输入框只能输入数字
        private void OnlyInputNumbers(object sender, TextCompositionEventArgs e)
        {
            inputlimited.InputLimited.OnlyInputNumbers(e);
        }

        //身份证号验证和查重
        private void IsIDCard(object sender, RoutedEventArgs e)
        {
            UserService userService = new UserService();
            if (String.IsNullOrEmpty(IDCard.Text))
            {
                Error_Info_IDCard.Content = LanguageUtils.ConvertLanguage("请输入身份证号码", "Please enter the ID number");
                bubble_IDCard.IsOpen = true;
            }else if (IDCard.Text.Length>18||(IDCard.Text.Length == 18&&!inputlimited.InputLimited.IsIDcard(IDCard.Text) && !String.IsNullOrEmpty(IDCard.Text)))
            {
                Error_Info_IDCard.Content = LanguageUtils.ConvertLanguage("请输入正确的身份证号码", "Please enter a valid ID number");
                bubble_IDCard.IsOpen = true;
            }
            else if (userService.GetByIdCard(IDCard.Text) != null)
            {
                Error_Info_IDCard.Content = LanguageUtils.ConvertLanguage("该身份证已注册", "This ID is registered");
                bubble_IDCard.IsOpen = true;
            }
            else
            {
                bubble_IDCard.IsOpen = false;
            }
        }

        //手机号验证和查重
        private void IsPhone(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(phoneNum.Text))
            {
                Error_Info_Phone.Content = LanguageUtils.ConvertLanguage("请输入手机号", "Please enter phone number");
                bubble_phone.IsOpen = true;
            }else if (!inputlimited.InputLimited.IsHandset(phoneNum.Text) && !String.IsNullOrEmpty(phoneNum.Text))
            {
                Error_Info_Phone.Content = LanguageUtils.ConvertLanguage("请输入正确的手机号", "Please enter a valid phone number");
                bubble_phone.IsOpen = true;
            }
            else if (userService.GetByPhone(phoneNum.Text) != null)
            {
                Error_Info_Phone.Content = LanguageUtils.ConvertLanguage("该手机号已注册", "The phone number is registered");
                bubble_phone.IsOpen = true;
            }
            else
            {
                bubble_phone.IsOpen = false;
            }
        }
        //解决气泡不随着窗体移动问题
        private void windowmove(object sender, EventArgs e)
        {
            if(bubble_phone.IsOpen == true || bubble_IDCard.IsOpen == true || bubble_name.IsOpen == true || bubble_disease.IsOpen == true || bubble_Diagnosis.IsOpen == true) { 
            var mi = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            mi.Invoke(bubble_phone, null);
            mi.Invoke(bubble_IDCard, null);
            mi.Invoke(bubble_name, null);
            mi.Invoke(bubble_disease, null);
            mi.Invoke(bubble_Diagnosis, null);
            }
        }
        //验证用户是否存在
        private void IsName(object sender, RoutedEventArgs e)
        {
            User user = new User
            {
                User_Name = t2.Text
            };
            UserService userService = new UserService();
            userService.SelectByCondition(user);
            if(userService.SelectByCondition(user).Count != 0 && !String.IsNullOrEmpty(t2.Text))
            {
                Error_Info_Name.Content = LanguageUtils.ConvertLanguage("该用户名已注册", "The username is registered");
                bubble_name.IsOpen = true;
            }
            else
            {
                bubble_name.IsOpen = false;
            }
        }
        //疾病名称是否存在
        private void IsDisease(object sender, KeyboardFocusChangedEventArgs e)
        {

            if (!diseaseList.Contains(c5.Text)&&!String.IsNullOrEmpty(c5.Text))
            {
                Error_Info_disease.Content = LanguageUtils.ConvertLanguage("不存在该疾病名称", "There is no name for the disease");
                bubble_disease.IsOpen = true;
            }
            else
            {
                bubble_disease.IsOpen = false;
            }
        }
        //残障名称是否存在
        private void IsDiagnosis(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!diagnosisList.Contains(c6.Text) && !String.IsNullOrEmpty(c6.Text))
            {
                Error_Info_Diagnosis.Content = LanguageUtils.ConvertLanguage("不存在该残障名称", "There is no such disability name");
                bubble_Diagnosis.IsOpen = true;
            }
            else
            {
                bubble_Diagnosis.IsOpen = false;
            }
        }

        /// <param name="sFile">原图片</param>    
        /// <param name="dFile">压缩后保存位置</param>    
        /// <param name="dHeight">高度</param>    
        /// <param name="dWidth"></param>    
        /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>    
        /// <returns></returns>    
        /// (源文件，目标文件，高度，宽度，压缩比例)
        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放  
            System.Drawing.Size tem_size = new System.Drawing.Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * (int)tem_size.Height) / (int)tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = ((int)tem_size.Width * dHeight) / (int)tem_size.Height;
                }
            }
            else
            {
                sW = (int)tem_size.Width;
                sH = (int)tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(System.Drawing.Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new System.Drawing.Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            //以下代码为保存图片时，设置压缩质量    
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100    
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("BMP"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    //MessageBox.Show("保存1");
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径    
                }
                else
                {
                    //MessageBox.Show("保存2");
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
        //回车按钮
        private void key_dowm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_OK(this, null);
                //使键盘失去焦点，解决窗口反复出现
                Keyboard.ClearFocus();
            }

        }

        private void c6_KeyUp(object sender, KeyEventArgs e)
        {
            List<string> mylist = new List<string>();
            mylist = diagnosisList.FindAll(delegate (string s) { return s.Contains(c6.Text.Trim()); });
            c6.ItemsSource = mylist;
            c6.IsDropDownOpen = true;
        }

        private void c5_KeyUp(object sender, KeyEventArgs e)
        {
            List<string> mylist = new List<string>();
            mylist = diseaseList.FindAll(delegate (string s) { return s.Contains(c5.Text.Trim()); });
            c5.ItemsSource = mylist;
            c5.IsDropDownOpen = true;
        }

        

        private void limit_input(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[A-Za-z]*$"))
            {
                
                e.Handled = true;//阻止非法字符输入。
                return;
            }
           
        }
    }

}