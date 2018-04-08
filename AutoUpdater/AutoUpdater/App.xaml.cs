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
        protected override void OnStartup(StartupEventArgs e)
        {
            LanguageUtils.SetLanguage(LanguageUtils.language);
        }

        [STAThread]
        static void Main(string[] args)
        {
         
            if (args.Length != 7) return;
            LanguageUtils.language = Int16.Parse(args[6]);
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
