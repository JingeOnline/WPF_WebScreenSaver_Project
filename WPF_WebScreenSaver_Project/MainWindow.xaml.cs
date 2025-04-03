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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_WebScreenSaver_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string js1 = "var list=document.querySelectorAll('.setting-hide');list.forEach(x=>x.style.display='block');";
        private string js2 = "";

        public MainWindow()
        {
            InitializeComponent();
            if(!App.IsSettingMode)
            {
                this.WindowStyle = WindowStyle.None;
            }
            this.PreviewKeyDown += new KeyEventHandler(EscKeyDown);
            Run();

        }

        public async void Run()
        {
            string binPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string web_Index_FilePath = "Web/Web_index.html";
            binPath = binPath.Replace("\\","/");
            string fullPath = binPath + web_Index_FilePath;

            await WebView.EnsureCoreWebView2Async();
            //WebView.CoreWebView2.Navigate("file:///C:/Users/jinge/Desktop/HTML&CSS/2025-Swiperjs/index.html");
            //WebView.CoreWebView2.Navigate("http://127.0.0.1:5500/index.html");
            WebView.CoreWebView2.Navigate(fullPath);
            WebView.NavigationCompleted += async (sender, e) =>
            {
                if (e.IsSuccess)
                {
                    if(App.IsSettingMode)
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

        private void EscKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
