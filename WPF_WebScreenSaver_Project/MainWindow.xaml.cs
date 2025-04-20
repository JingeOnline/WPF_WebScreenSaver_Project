using SharedLibrary.Helpers;
using SharedLibrary.Models;
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
using WPF_WebScreenSaver_Project.Helpers;

namespace WPF_WebScreenSaver_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private string aud_rate;
        private string usd_rate;
        private string aud_updateTime;
        private string usd_updateTime;

        private string hideElementsInRuningMode = "var list=document.querySelectorAll('.setting-hide');list.forEach(x=>x.style.display='none');";
        private string updateBocAudRate = "document.querySelector('#div_boc_aud_rate').innerHTML=";
        private string updateBocUsdRate = "document.querySelector('#div_boc_usd_rate').innerHTML=";
        private string updateBocAudTime = "document.querySelector('#div_boc_aud_rate_time').innerHTML=";
        private string updateBocUsdTime = "document.querySelector('#div_boc_usd_rate_time').innerHTML=";
        private string updatePetrolPrice = "document.querySelector('#h2-petrol-price').innerHTML=";

        public MainWindow()
        {
            InitializeComponent();
            logger.Info("初始化主窗口完成");
            if (!App.IsSettingMode)
            {
                //this.WindowStyle = WindowStyle.None; //隐藏窗口顶部的Title bar
                Cursor = Cursors.None;  //隐藏鼠标
                SetCursorPos(4000, 2200);  //由于WebView2上隐藏鼠标不起作用，所以需要在启动时候把鼠标移到WebView2外部。
                Topmost = true; //至于所有程序的最顶层
                //目前所有的方法都不能获取鼠标的位置，因为整个画面都被WebView2占据，鼠标在上面根本不能触发任何事件，也不能获取有效的坐标，总是返回0，0
                this.PreviewMouseDown += MouseDown;
            }
            this.PreviewKeyDown += new KeyEventHandler(KeyboardDown);
            Run();
            ReHideCursor();

        }

        public async Task ReHideCursor()
        {
            while (true)
            {
                await Task.Delay(30 * 1000);
                SetCursorPos(4000, 2200);
            }
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
                        await ((Microsoft.Web.WebView2.Wpf.WebView2)sender).ExecuteScriptAsync(hideElementsInRuningMode);
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
            UpdateBocRate();
            UpdatePetrolPrice();
        }

        private async Task UpdateBocRate()
        {
            while (true)
            {
                try
                {
                    List<ExchangeDailyModel> list = await BocHelper.GetExchangeDailyModelsFromBocHomePage();
                    aud_rate = list.First(x => x.name == "AUD").xhmcj;
                    usd_rate = list.First(x => x.name == "USD").xhmcj;
                    aud_updateTime = "'" + "发布时间（中国）: " + list.First(x => x.name == "AUD").publishTime + "'";
                    usd_updateTime = "'" + "发布时间（中国）: " + list.First(x => x.name == "USD").publishTime + "'";
                    await WebView.ExecuteScriptAsync(updateBocAudRate + aud_rate);
                    await WebView.ExecuteScriptAsync(updateBocUsdRate + usd_rate);
                    await WebView.ExecuteScriptAsync(updateBocAudTime + aud_updateTime);
                    await WebView.ExecuteScriptAsync(updateBocUsdTime + usd_updateTime);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    await WebView.ExecuteScriptAsync(updateBocAudTime + ex.Message);
                    await WebView.ExecuteScriptAsync(updateBocUsdTime + ex.Message);
                }
                await Task.Delay(60 * 1000);
            }
        }

        private async Task UpdatePetrolPrice()
        {
            try
            {
                string price = await PetrolSpyHelper.GetPetrolPrice();
                await WebView.ExecuteScriptAsync(updatePetrolPrice + "'" +price + "'");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }

        #region 检测鼠标移动，鼠标被按下，键盘被按下
        private void KeyboardDown(object sender, KeyEventArgs e)
        {
            //允许使用键盘的左右键来导航Slider页面
            if (e.Key == Key.Left)
            {
                WebView.ExecuteScriptAsync("swiper.slidePrev()");
            }
            else if (e.Key == Key.Right)
            {
                WebView.ExecuteScriptAsync("swiper.slideNext()");
            }
            //其他键在运行模式下全部退出程序，在设定模式下Esc键退出
            else
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
            //if (!App.IsSettingMode)
            //{
            //    mouseMoveCount++;
            //    if (mouseMoveCount > 1)
            //    {
            //        Application.Current.Shutdown();
            //    }
            //}
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
