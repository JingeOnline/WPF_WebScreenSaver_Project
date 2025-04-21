using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_WebScreenSaver_Project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //程序启动参数，当屏幕保护程序被设置的时候，启动设置页面；运行程序保护程序的时候，启动运行页面。
        private StartupEventArgs _startupEventArgs;
        public static bool IsSettingMode = false;
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public App()
        {
            logger.Info("Startup");
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            this.Exit += OnExit;
        }



        protected override void OnStartup(StartupEventArgs e)
        {
            string[] args = e.Args;
            if (args.Length > 0)
            {
                logger.Info("Startup - arguments: " + string.Join("", args));
                string firstArgument = args[0].ToLower().Trim();
                string secondArgument = null;

                // Handle cases where arguments are separated by colon.
                // Examples: /c:1234567 or /P:1234567
                if (firstArgument.Length > 2)
                {
                    //secondArgument = firstArgument.Substring(3).Trim();
                    firstArgument = firstArgument.Substring(0, 2);
                }
                switch (firstArgument)
                {
                    case "/c":  // Configuration mode
                        IsSettingMode = true;
                        this.StartupUri=new Uri("MainWindow.xaml",UriKind.Relative);
                        break;
                    case "/p":  // Preview mode
                        //throw new NotImplementedException();
                        Shutdown();
                        return;
                        break;
                    case "/s":  // Full-screen mode
                        this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
                        break;
                    default:
                        this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
                        break;
                }
            }
            else
            {
                logger.Info("Startup - no arguments." );
                this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            }
            base.OnStartup(e);
        }
        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            logger.Error(e.Exception);
            logger.Error("程序异常退出");
        }
        private void OnExit(object sender, ExitEventArgs e)
        {
            logger.Info("程序正常退出");
        }
    }
}
