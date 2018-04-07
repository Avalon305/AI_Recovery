using System;
using System.Collections.Generic;
using System.Windows;
using AutoUpdater.Utils;
using AutoUpdater.ViewModels;

namespace AutoUpdater
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {

        private const string AppId = "{7F280539-0814-4F9C-95BF-D2BB60023657}";

        [STAThread]
        static void Main(string[] args)
        {
           
       
            //args =new string[]{ "0.9.0.0", "1.0.0.0" , "https://github.com/WELL-E","http://localhost:8080/test/test.zip", a , "f3a92690c2834f4098f4e229d3338096" };

            if (args.Length != 6) return; 
            if (SingleInstance<App>.InitializeAsFirstInstance(AppId))
            {
                var win = new MainWindow();
                var vm = new MainWindowViewModel(args, win.Close);
                win.DataContext = vm;

                var application = new App();
                application.InitializeComponent();
                application.Run(win);
                SingleInstance<App>.Cleanup();
            }
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            if (this.MainWindow.WindowState == WindowState.Minimized)
            {
                this.MainWindow.WindowState = WindowState.Normal;
            }
            this.MainWindow.Activate();

            return true;
        }
    }
}
