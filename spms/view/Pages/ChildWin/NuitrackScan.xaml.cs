﻿using nuitrack;
using nuitrack.issues;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using spms.dao;
using spms.entity.newEntity;
using spms.util;

namespace spms.view.Pages.ChildWin
{
    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }

        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void SetPixel(int x, int y, Color colour)
        {
            int index = x + (y * Width);
            int col = colour.ToArgb();

            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            int col = Bits[index];
            Color result = Color.FromArgb(col);

            return result;
        }

        public void Dispose()
        {
            if (Disposed)
                return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }

    /// <summary>
    /// NuitrackScan.xaml 的交互逻辑
    /// </summary>
    public partial class NuitrackScan : Window
    {
        /// <summary>
        /// Nuitrack变量定义
        /// </summary>
        private DirectBitmap _bitmap;
        private bool _visualizeColorImage = true;
        private bool _colorStreamEnabled = false;
        ///传递过来的User
        public entity.User SelectUser { get; set; }

        private DepthSensor _depthSensor;
        private ColorSensor _colorSensor;
        private UserTracker _userTracker;
        private SkeletonTracker _skeletonTracker;
        private GestureRecognizer _gestureRecognizer;
        private HandTracker _handTracker;

        private DepthFrame _depthFrame;
        private SkeletonData _skeletonData;
        private HandTrackerData _handTrackerData;
        private IssuesData _issuesData = null;
        SkeletonLengthEntity skeletonLength = new SkeletonLengthEntity();
        bool flag = false;
        int clicknum = 0;
        SkeletonLengthDAO skeletonLengthDAO = new SkeletonLengthDAO();

        public NuitrackScan()
        {
            InitializeComponent();
            //调用3D扫描的构造函数 cqz
            NuitrackCreate();
        }

        /// <summary>
        ///3D扫描的构造函数 -byCQZ 2019.6.16
        ///
        /// </summary>
        private void NuitrackCreate()
        {
            try
            {
                Nuitrack.Init("");
                Console.WriteLine("Initialize succneed.");
            }
            catch (nuitrack.Exception exception)
            {
                Console.WriteLine("Cannot initialize Nuitrack.");
                //throw exception;
                MessageBoxX.Warning("3D摄像头初始化失败，请检查SDK配置和是否进行密钥认证。");

            }

            try
            {
                // Create and setup all required modules
                _depthSensor = DepthSensor.Create();
                _colorSensor = ColorSensor.Create();
                _userTracker = UserTracker.Create();
                _skeletonTracker = SkeletonTracker.Create();
                _handTracker = HandTracker.Create();
                _gestureRecognizer = GestureRecognizer.Create();
            }
            catch (nuitrack.Exception exception)
            {
                Console.WriteLine("Cannot create Nuitrack module.");
                //throw exception;
                MessageBoxX.Warning("3D摄像头初始化失败，请检查SDK配置和是否进行密钥认证。");


            }

            _depthSensor.SetMirror(false);
            // Add event handlers for all modules
            _depthSensor.OnUpdateEvent += onDepthSensorUpdate;
            _colorSensor.OnUpdateEvent += onColorSensorUpdate;
            _userTracker.OnUpdateEvent += onUserTrackerUpdate;
            _skeletonTracker.OnSkeletonUpdateEvent += onSkeletonUpdate;
            _handTracker.OnUpdateEvent += onHandTrackerUpdate;
            _gestureRecognizer.OnNewGesturesEvent += onNewGestures;
            // Add an event handler for the IssueUpdate event 
            Nuitrack.onIssueUpdateEvent += onIssueDataUpdate;

            // Create and configure the Bitmap object according to the depth sensor output mode
            OutputMode mode = _depthSensor.GetOutputMode();
            OutputMode colorMode = _colorSensor.GetOutputMode();

            if (mode.XRes < colorMode.XRes)
                mode.XRes = colorMode.XRes;
            if (mode.YRes < colorMode.YRes)
                mode.YRes = colorMode.YRes;
            Console.WriteLine(mode.XRes + "=====================" + mode.YRes);
            _bitmap = new DirectBitmap(mode.XRes, mode.YRes);
            for (int y = 0; y < mode.YRes; ++y)
            {
                for (int x = 0; x < mode.XRes; ++x)
                    _bitmap.SetPixel(x, y, Color.FromKnownColor(KnownColor.Aqua));
            }
            try
            {
                Nuitrack.Run();
                Console.WriteLine("Start Nuitrack.");
            }
            catch (nuitrack.Exception exception)
            {
                Console.WriteLine("Cannot start Nuitrack.");
                //throw exception;
                MessageBoxX.Warning("3D摄像头启动失败，请检查SDK配置和是否进行密钥认证。");

            }
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SkeletonLengthEntity skeletonLengthEntity = new SkeletonLengthEntity();
            skeletonLengthEntity = skeletonLengthDAO.GetByPk_User_Id(SelectUser.Pk_User_Id);
            if(skeletonLengthEntity != null)
            {
                if(skeletonLengthEntity.Weigth > 0)
                {
                    Weigth.Text = skeletonLengthEntity.Weigth.ToString();
                }
                Man_Height.Text = skeletonLengthEntity.Height.ToString();
                Shoulder_width.Text = skeletonLengthEntity.Shoulder_width.ToString();
                Arm_length_up.Text = skeletonLengthEntity.Arm_length_up.ToString();
                Arm_length_down.Text = skeletonLengthEntity.Arm_length_down.ToString();
                Leg_length_up.Text = skeletonLengthEntity.Leg_length_up.ToString();
                Leg_length_down.Text = skeletonLengthEntity.Leg_length_down.ToString();
                Body_length.Text = skeletonLengthEntity.Body_length.ToString();
            }
        }

        private void onIssueDataUpdate(IssuesData issuesData)
        {
            _issuesData = issuesData;
        }

        // Event handler for the ColorSensorUpdate event
        private void onColorSensorUpdate(ColorFrame colorFrame)
        {
            Console.WriteLine("onColorSensorUpdate");
            if (!_visualizeColorImage)
            {
                Console.WriteLine("！！！！_visualizeColorImage为false");
                return;
            }
            Console.WriteLine("！！！！_visualizeColorImage为true开启真实色彩");
            _colorStreamEnabled = true;

            float wStep = (float)_bitmap.Width / colorFrame.Cols;
            float hStep = (float)_bitmap.Height / colorFrame.Rows;

            float nextVerticalBorder = hStep;

            Byte[] data = colorFrame.Data;
            int colorPtr = 0;
            int bitmapPtr = 0;
            const int elemSizeInBytes = 3;

            for (int i = 0; i < _bitmap.Height; ++i)
            {
                if (i == (int)nextVerticalBorder)
                {
                    colorPtr += colorFrame.Cols * elemSizeInBytes;
                    nextVerticalBorder += hStep;
                }

                int offset = 0;
                int argb = data[colorPtr]
                    | (data[colorPtr + 1] << 8)
                    | (data[colorPtr + 2] << 16)
                    | (0xFF << 24);
                float nextHorizontalBorder = wStep;
                for (int j = 0; j < _bitmap.Width; ++j)
                {
                    if (j == (int)nextHorizontalBorder)
                    {
                        offset += elemSizeInBytes;
                        argb = data[colorPtr + offset]
                            | (data[colorPtr + offset + 1] << 8)
                            | (data[colorPtr + offset + 2] << 16)
                            | (0xFF << 24);
                        nextHorizontalBorder += wStep;
                    }

                    _bitmap.Bits[bitmapPtr++] = argb;
                }
            }
        }
        // Event handler for the UserTrackerUpdate event
        private void onUserTrackerUpdate(UserFrame userFrame)
        {
            Console.WriteLine("onUserTrackerUpdate");
            if (_visualizeColorImage && _colorStreamEnabled)
                return;
            if (_depthFrame == null)
                return;

            const int MAX_LABELS = 7;
            bool[] labelIssueState = new bool[MAX_LABELS];
            for (UInt16 label = 0; label < MAX_LABELS; ++label)
            {
                labelIssueState[label] = false;
                if (_issuesData != null)
                {
                    FrameBorderIssue frameBorderIssue = _issuesData.GetUserIssue<FrameBorderIssue>(label);
                    labelIssueState[label] = (frameBorderIssue != null);
                }
            }

            float wStep = (float)_bitmap.Width / _depthFrame.Cols;
            float hStep = (float)_bitmap.Height / _depthFrame.Rows;

            float nextVerticalBorder = hStep;

            Byte[] dataDepth = _depthFrame.Data;
            Byte[] dataUser = userFrame.Data;
            int dataPtr = 0;
            int bitmapPtr = 0;
            const int elemSizeInBytes = 2;
            for (int i = 0; i < _bitmap.Height; ++i)
            {
                if (i == (int)nextVerticalBorder)
                {
                    dataPtr += _depthFrame.Cols * elemSizeInBytes;
                    nextVerticalBorder += hStep;
                }

                int offset = 0;
                int argb = 0;
                int label = dataUser[dataPtr] | dataUser[dataPtr + 1] << 8;
                int depth = Math.Min(255, (dataDepth[dataPtr] | dataDepth[dataPtr + 1] << 8) / 32);
                float nextHorizontalBorder = wStep;
                for (int j = 0; j < _bitmap.Width; ++j)
                {
                    if (j == (int)nextHorizontalBorder)
                    {
                        offset += elemSizeInBytes;
                        label = dataUser[dataPtr + offset] | dataUser[dataPtr + offset + 1] << 8;
                        if (label == 0)
                            depth = Math.Min(255, (dataDepth[dataPtr + offset] | dataDepth[dataPtr + offset + 1] << 8) / 32);
                        nextHorizontalBorder += wStep;
                    }

                    if (label > 0)
                    {
                        int user = label * 40;
                        if (!labelIssueState[label])
                            user += 40;
                        argb = 0 | (user << 8) | (0 << 16) | (0xFF << 24);
                    }
                    else
                    {
                        argb = depth | (depth << 8) | (depth << 16) | (0xFF << 24);
                    }

                    _bitmap.Bits[bitmapPtr++] = argb;
                }
            }
        }
        // Event handler for the DepthSensorUpdate event
        private void onDepthSensorUpdate(DepthFrame depthFrame)
        {
            _depthFrame = depthFrame;
        }
        // Event handler for the SkeletonUpdate event
        private void onSkeletonUpdate(SkeletonData skeletonData)
        {
            Console.WriteLine("SkeletonData被更新");
            _skeletonData = skeletonData;
            const int jointSize = 10;

        }

        // Event handler for the HandTrackerUpdate event
        private void onHandTrackerUpdate(HandTrackerData handTrackerData)
        {
            _handTrackerData = handTrackerData;
        }

        // Event handler for the gesture detection event
        private void onNewGestures(GestureData gestureData)
        {
            // Display the information about detected gestures in the console
            foreach (var gesture in gestureData.Gestures)
                Console.WriteLine("Recognized {0} from user {1}", gesture.Type.ToString(), gesture.UserID);
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if (clicknum == 0)
            {
                DrawingVisual drawingVisual = new DrawingVisual();
                DrawingContext drawingContext = drawingVisual.RenderOpen();
                System.Windows.Media.Brush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
                for (var i = 0; i < 100; i = i + 20)
                {
                    drawingContext.DrawEllipse(brush, new System.Windows.Media.Pen(), new System.Windows.Point(311, i), 5, 5);
                }
                drawingContext.Close();
                RenderTargetBitmap bmp = new RenderTargetBitmap(400, 400, 100, 100, PixelFormats.Pbgra32);
                bmp.Render(drawingVisual);
                statusImage.Source = bmp;
                clicknum++;
            }
            else
            {
                DrawingVisual drawingVisual = new DrawingVisual();
                DrawingContext drawingContext = drawingVisual.RenderOpen();
                System.Windows.Media.Brush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
                for (var i = 0; i < 100; i = i + 20)
                {
                    drawingContext.DrawEllipse(brush, new System.Windows.Media.Pen(), new System.Windows.Point(111, i), 5, 5);
                }
                drawingContext.Close();
                RenderTargetBitmap bmp = new RenderTargetBitmap(400, 400, 100, 100, PixelFormats.Pbgra32);
                bmp.Render(drawingVisual);
                statusImage.Source = bmp;
            }

        }

        private void DealWith()
        {
            try
            {
                Console.WriteLine("Update");
                Nuitrack.Update(_skeletonTracker);

            }
            catch (LicenseNotAcquiredException exception)
            {
                Console.WriteLine("LicenseNotAcquired exception. Exception: ", exception);
                MessageBoxX.Error("3D摄像头认证异常，请重新进入页面重试。");
                //throw exception;
            }
            catch (nuitrack.Exception exception)
            {
                Console.WriteLine("Nuitrack update failed. Exception: ", exception);
                MessageBoxX.Error("3D摄像头更新异常，请重新进入页面重试。");

            }

            // Draw skeleton joints
            if (_skeletonData != null)
            {


                Console.WriteLine("_skeletonData不为空");
                while (_skeletonData.Skeletons.Length == 0)
                {
                    Nuitrack.Update(_skeletonTracker);
                    Console.WriteLine("Skeletons为空进行Update");
                    Thread.Sleep(100);
                }
                Joint Head = new Joint();   //衣领位置
                Joint Collar = new Joint();   //衣领位置
                Joint LeftShoulder = new Joint();  //左肩关节
                Joint LeftElbow = new Joint();  //左胳膊肘
                Joint LeftWrist = new Joint();  //左手手腕关节
                Joint LeftHip = new Joint();    //左大腿关节
                Joint LeftKnee = new Joint();   //左膝盖关节
                Joint LeftAnkle = new Joint();   //左脚踝
                Joint Waist = new Joint();    //腰部
                Console.WriteLine("Joints长度为" + _skeletonData.Skeletons[0].Joints.Length);

                try
                {
                    if (_skeletonData.Skeletons.Length > 0 && _skeletonData.Skeletons[0].Joints.Length > 0)
                    {
                        for (int i = 0; i < _skeletonData.Skeletons[0].Joints.Length; i++)
                        {
                            Skeleton skeleton = _skeletonData.Skeletons[0];

                            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                            {
                                Console.WriteLine("图像渲染前" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                                DrawingVisual drawingVisual = new DrawingVisual();
                                DrawingContext drawingContext = drawingVisual.RenderOpen();
                                System.Windows.Media.Brush brush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
                                foreach (var joint in skeleton.Joints)
                                {
                                    drawingContext.DrawEllipse(brush, new System.Windows.Media.Pen(), new System.Windows.Point((joint.Proj.X * _bitmap.Width - 10 / 2) - 53, (joint.Proj.Y * _bitmap.Height - 10 / 2) - 70), 5, 5);
                                }
                                drawingContext.Close();
                                RenderTargetBitmap bmp = new RenderTargetBitmap(640, 480, 120, 120, PixelFormats.Pbgra32);
                                bmp.Render(drawingVisual);
                                statusImage1.Source = bmp;
                                statusImage.Source = Bitmap2BitmapImage(_bitmap.Bitmap);
                                Console.WriteLine("图像渲染后" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));


                            });
                            Console.WriteLine("类型" + i + " " + skeleton.Joints[i].Type.ToString());
                            if (skeleton.Joints[i].Type.ToString() == "Head")
                            {
                                Head = skeleton.Joints[i];
                                // Console.WriteLine("头部位置坐标" + i + "||" + Head.Real.X + "||" + Head.Real.Y + "||" + Head.Real.Z);
                            }
                            if (skeleton.Joints[i].Type.ToString() == "LeftCollar")
                            {
                                Collar = skeleton.Joints[i];
                                // Console.WriteLine("衣领位置坐标" + i + "||" + Collar.Real.X + "||" + Collar.Real.Y + "||" + Collar.Real.Z);
                            }
                            if (skeleton.Joints[i].Type.ToString() == "LeftShoulder")
                            {
                                LeftShoulder = skeleton.Joints[i];
                                // Console.WriteLine("左肩关节坐标" + i + "||" + LeftShoulder.Real.X + "||" + LeftShoulder.Real.Y + "||" + LeftShoulder.Real.Z);
                            }
                            if (skeleton.Joints[i].Type.ToString() == "LeftWrist")
                            {
                                LeftWrist = skeleton.Joints[i];
                                //Console.WriteLine("左手手腕关节坐标" + i + "||" + LeftWrist.Real.X + "||" + LeftWrist.Real.Y + "||" + LeftWrist.Real.Z);
                            }
                            if (skeleton.Joints[i].Type.ToString() == "LeftHip")
                            {
                                LeftHip = skeleton.Joints[i];
                                // Console.WriteLine("左大腿关节坐标" + i + "||" + LeftHip.Real.X + "||" + LeftHip.Real.Y + "||" + LeftHip.Real.Z);
                            }
                            if (skeleton.Joints[i].Type.ToString() == "LeftAnkle")
                            {
                                LeftAnkle = skeleton.Joints[i];
                                // Console.WriteLine("左脚踝坐标" + i + "||" + LeftAnkle.Real.X + "||" + LeftAnkle.Real.Y + "||" + LeftAnkle.Real.Z);
                            }
                            if (skeleton.Joints[i].Type.ToString() == "LeftElbow")
                            {
                                LeftElbow = skeleton.Joints[i];
                                // Console.WriteLine("左胳膊肘坐标" + i + "||" + LeftElbow.Real.X + "||" + LeftElbow.Real.Y + "||" + LeftElbow.Real.Z);
                            }
                            if (skeleton.Joints[i].Type.ToString() == "LeftKnee")
                            {
                                LeftKnee = skeleton.Joints[i];
                                //Console.WriteLine("左膝盖坐标" + i + "||" + LeftKnee.Real.X + "||" + LeftKnee.Real.Y + "||" + LeftKnee.Real.Z);
                            }
                            if (skeleton.Joints[i].Type.ToString() == "Waist")
                            {
                                Waist = skeleton.Joints[i];
                                //Console.WriteLine("腰部坐标" + i + "||" + Waist.Real.X + "||" + Waist.Real.Y + "||" + Waist.Real.Z);
                            }
                            double NeckLength = ComputeDistanceBetween2Joints(Head, Collar);
                            double ShoulderWidth = ComputeDistanceBetween2Joints(LeftShoulder, Collar);
                            double ArmLengthUp = ComputeDistanceBetween2Joints(LeftShoulder, LeftElbow);
                            double ArmLengthDown = ComputeDistanceBetween2Joints(LeftElbow, LeftWrist);
                            double LegLengthUp = ComputeDistanceBetween2Joints(LeftHip, LeftKnee);
                            double LegLengthDown = ComputeDistanceBetween2Joints(LeftKnee, LeftAnkle);
                            double BodyLength = ComputeDistanceBetween2Joints(Collar, Waist);
                            double Height = LegLengthUp + LegLengthDown + BodyLength + NeckLength + 250.0;
                            // Console.WriteLine("距离差为" + (LeftHip.Real.Z - LeftKnee.Real.Z) + "是否举手" + (LeftWrist.Real.Y - LeftShoulder.Real.Y));

                            if ((LeftHip.Real.Z - LeftKnee.Real.Z > 150 && LeftHip.Real.Z - LeftKnee.Real.Z < 300) && (LeftWrist.Real.Y > LeftShoulder.Real.Y && LeftAnkle.Real.Y != 0))
                            {
                                Console.WriteLine("弯膝状态");
                                Console.WriteLine(LeftHip.Real.Z - LeftKnee.Real.Z);
                                if (ShoulderWidth != 0 && ArmLengthUp != 0 && ArmLengthDown != 0 && LegLengthUp != 0 && LegLengthDown != 0)
                                {
                                    Console.WriteLine("都不为0，开始计算");
                                    skeletonLength.Shoulder_width = ShoulderWidth;
                                    skeletonLength.Arm_length_up = ArmLengthUp;
                                    skeletonLength.Arm_length_down = ArmLengthDown;
                                    skeletonLength.Leg_length_up = LegLengthUp;
                                    skeletonLength.Leg_length_down = LegLengthDown;
                                    Nuitrack.Release();
                                    //在输入框中渲染数据
                                    this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                                    {
                                        Shoulder_width.Text = (ShoulderWidth / 10).ToString("f2");
                                        Arm_length_up.Text = (ArmLengthUp / 10).ToString("f2");
                                        Arm_length_down.Text = (ArmLengthDown / 10).ToString("f2");
                                        Leg_length_up.Text = (LegLengthUp / 10).ToString("f2");
                                        Leg_length_down.Text = (LegLengthDown / 10).ToString("f2");
                                        Body_length.Text = (BodyLength / 10).ToString("f2");
                                        Man_Height.Text = (Height / 10).ToString("f2");
                                    });
                                    break;
                                }
                                else
                                {
                                    Nuitrack.Update(_skeletonTracker);
                                    Console.WriteLine("长度某个为0进行Update,i为" + i);
                                }
                            }

                            //if ((LeftHip.Real.Z - LeftKnee.Real.Z != 0 && LeftHip.Real.Z - LeftKnee.Real.Z < 150) || (LeftWrist.Real.Y != 0 && LeftShoulder.Real.Y != 0 && LeftWrist.Real.Y < LeftShoulder.Real.Y))
                            if (i >= 22 && (LeftHip.Real.Z - LeftKnee.Real.Z < 150 || LeftWrist.Real.Y < LeftShoulder.Real.Y))
                            {
                                Nuitrack.Update(_skeletonTracker);
                                Console.WriteLine("处于直立状态,差值为" + (LeftHip.Real.Z - LeftKnee.Real.Z) + "进行Update把i=" + i + "归为0");
                                //此处将数据进行清零
                                i = 0;
                                Collar.Real = new Vector3(0, 0, 0);
                                LeftShoulder.Real = new Vector3(0, 0, 0);
                                LeftWrist.Real = new Vector3(0, 0, 0);
                                LeftHip.Real = new Vector3(0, 0, 0);
                                LeftAnkle.Real = new Vector3(0, 0, 0);
                                LeftElbow.Real = new Vector3(0, 0, 0);
                                LeftKnee.Real = new Vector3(0, 0, 0);
                                ShoulderWidth = 0;
                                ArmLengthUp = 0;
                                ArmLengthDown = 0;
                                LegLengthUp = 0;
                                LegLengthDown = 0;
                                Console.WriteLine("检查清零" + (LeftHip.Real.Z - LeftKnee.Real.Z));
                                //Thread.Sleep(100);

                            }
                        }
                    }


                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("3D扫描数组越界。");
                    MessageBoxX.Error("扫描失败，请重置后再点击开始重新扫描。");
                }



                Console.WriteLine("肩宽mm" + skeletonLength.Shoulder_width);
                Console.WriteLine("臂长(上)mm" + skeletonLength.Arm_length_up);
                Console.WriteLine("臂长(下)mm" + skeletonLength.Arm_length_down);
                Console.WriteLine("腿长(上)mm" + skeletonLength.Leg_length_up);
                Console.WriteLine("腿长(下)mm" + skeletonLength.Leg_length_down);
            }

        }

        /// <summary>
        /// 开始按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            const int jointSize = 10;
            Thread thread = new Thread(DealWith);
            thread.Start();

            Console.WriteLine("按钮被点击了");

        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {

            SkeletonLengthEntity skeletonLengthEntity = new SkeletonLengthEntity();
            skeletonLengthEntity.Fk_member_id = SelectUser.Pk_User_Id;
            if(Weigth.Text != null && Weigth.Text != "")
            {
                skeletonLengthEntity.Weigth = System.Convert.ToDouble(Weigth.Text);
            }
            skeletonLengthEntity.Height = System.Convert.ToDouble(Man_Height.Text);
            skeletonLengthEntity.Shoulder_width = System.Convert.ToDouble(Shoulder_width.Text);
            skeletonLengthEntity.Arm_length_up = System.Convert.ToDouble(Arm_length_up.Text);
            skeletonLengthEntity.Arm_length_down = System.Convert.ToDouble(Arm_length_down.Text);
            skeletonLengthEntity.Leg_length_up = System.Convert.ToDouble(Leg_length_up.Text);
            skeletonLengthEntity.Leg_length_down = System.Convert.ToDouble(Leg_length_down.Text);
            skeletonLengthEntity.Body_length = System.Convert.ToDouble(Body_length.Text);

            if (skeletonLengthDAO.getSkeletonLengthRecord(SelectUser.Pk_User_Id) == null)
            {
                skeletonLengthDAO.insertSkeletonLengthRecord(skeletonLengthEntity);
            }
            else
            {
                if (Weigth.Text != null && Weigth.Text != "")
                {
                    skeletonLengthDAO.updateSkeletonLengthAndWeightRecord(skeletonLengthEntity);
                }
                else
                {
                    skeletonLengthDAO.updateSkeletonLengthRecord(skeletonLengthEntity);
                }
            }
            MessageBoxX.Info("保存成功");
        }

        /// <summary>
        /// 重置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            //重新调用扫描的构造函数 cqz
            NuitrackCreate();

            Weigth.Text = null;
            Man_Height.Text = null;
            Shoulder_width.Text = null;
            Arm_length_up.Text = null;
            Arm_length_down.Text = null;
            Leg_length_up.Text = null;
            Leg_length_down.Text = null;
            Body_length.Text = null;
            MessageBoxX.Info("重置成功，请站在目标位置，再点击开始重新扫描。");
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Console.WriteLine("图像处理前" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                bitmap.Save(stream, ImageFormat.Png); // 坑点：格式选Bmp时，不带透明度

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                Console.WriteLine("图像处理后" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                return result;

            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private BitmapSource Bitmap2BitmapImage(Bitmap bitmap)
        {
            Console.WriteLine("图像处理前" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSource retval;

            try
            {
                retval = Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }
            Console.WriteLine("图像处理后" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            return retval;
        }

        private double ComputeDistanceBetween2Joints(Joint Joint1, Joint Joint2)
        {
            return Math.Sqrt(Math.Pow(Joint1.Real.X - Joint2.Real.X, 2) + Math.Pow(Joint1.Real.Y - Joint2.Real.Y, 2) + Math.Pow(Joint1.Real.Z - Joint2.Real.Z, 2));
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}