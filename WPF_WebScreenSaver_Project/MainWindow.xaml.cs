using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

namespace WPF_WebScreenSaver_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        private string js1 = "var list=document.querySelectorAll('.setting-hide');list.forEach(x=>x.style.display='none');";
        private string js2 = "";

        public MainWindow()
        {
            InitializeComponent();
            if (!App.IsSettingMode)
            {
                this.WindowStyle = WindowStyle.None; //隐藏窗口顶部的Title bar
                Cursor = Cursors.None;  //隐藏鼠标
                SetCursorPos(4000, 2200);  //由于WebView2上隐藏鼠标不起作用，所以需要在启动时候把鼠标移到WebView2外部。
                Topmost = true; //至于所有程序的最顶层
                //目前所有的方法都不能获取鼠标的位置，因为整个画面都被WebView2占据，鼠标在上面根本不能触发任何事件，也不能获取有效的坐标，总是返回0，0
                this.PreviewMouseDown += MouseDown;
            }
            this.PreviewKeyDown += new KeyEventHandler(KeyboardDown);
            Run();

        }

        public async Task Run()
        {
            string binPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string web_Index_FilePath = "Web/Web_index.html";
            binPath = binPath.Replace("\\", "/");
            string fullPath = binPath + web_Index_FilePath;

            await WebView.EnsureCoreWebView2Async();
            //去除网页对快捷键的响应
            WebView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
            //去除网页对右键的响应
            WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            //WebView.CoreWebView2.Navigate("file:///C:/Users/jinge/Desktop/HTML&CSS/2025-Swiperjs/index.html");
            //WebView.CoreWebView2.Navigate("http://127.0.0.1:5500/index.html");
            WebView.CoreWebView2.Navigate(fullPath);
            WebView.NavigationCompleted += async (sender, e) =>
            {
                if (e.IsSuccess)
                {
                    if (!App.IsSettingMode)
                    {
                        await ((Microsoft.Web.WebView2.Wpf.WebView2)sender).ExecuteScriptAsync(js1);
                    }
                    #region removed test
                    //await ((Microsoft.Web.WebView2.Wpf.WebView2)sender).ExecuteScriptAsync("document.querySelector('.swiper-slide').innerHTML='OK'");
                    //while (true)
                    //{
                    //    await Task.Delay(1000);
                    //    await ((Microsoft.Web.WebView2.Wpf.WebView2)sender).ExecuteScriptAsync("document.querySelector('#s1').style.backgroundColor ='aquamarine'");
                    //    await Task.Delay(1000);
                    //    await ((Microsoft.Web.WebView2.Wpf.WebView2)sender).ExecuteScriptAsync("document.querySelector('#s1').style.backgroundColor ='bisque'");

                    //}
                    #endregion
                }
            };
        }

        #region 检测鼠标移动，鼠标被按下，键盘被按下
        private void KeyboardDown(object sender, KeyEventArgs e)
        {
            if (App.IsSettingMode)
            {
                if (e.Key == Key.Escape)
                {
                    Application.Current.Shutdown();
                }
            }
            else
            {
                Application.Current.Shutdown();
            }

        }

        int mouseMoveCount = 0;
        /// <summary>
        /// 程序刚刚启动后，会触发一次该方法，所以忽略第一次触发。
        /// 如果WebView2控件没有边距，全部占据Window窗口，则该方法无论如何移动鼠标都不会被触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (!App.IsSettingMode)
            {
                mouseMoveCount++;
                if (mouseMoveCount > 1)
                {
                    Application.Current.Shutdown();
                }
            }
        }

        /// <summary>
        /// 如果WebView2控件没有边距，全部占据Window窗口，则该方法在鼠标点击的时候并不会触发。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine(DateTime.Now.ToString("hh:mm:ss fff") + " MouseDown");
            Application.Current.Shutdown();
        }
        #endregion
    }
}
