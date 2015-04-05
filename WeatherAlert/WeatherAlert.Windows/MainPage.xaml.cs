using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;
using WeatherAlert;
using NotificationsExtensions.ToastContent;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace WeatherAlert
{
    /// <summary>
    /// 可独立使用或用于导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void TestToast_Click(object sender, RoutedEventArgs e)
        {
            IToastImageAndText02 toastContent = ToastContentFactory.CreateToastImageAndText02();
            toastContent.TextHeading.Text = "明天有雨";
            toastContent.TextBodyWrap.Text = "明天您的地区有雨，注意带伞";

            DateTime dueTime = DateTime.Now.AddSeconds(5);
            var toast = new ScheduledToastNotification(toastContent.GetXml(), dueTime);
            toast.Id = "Has rain";
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            WeatherFetcher fetcher = new WeatherFetcher();
            String weather = await fetcher.getWeather();

            var locationFetcher = new LocationFetcher();
            Location location;
            try
            {
                location = await locationFetcher.getCurrentCity();
            } catch (System.UnauthorizedAccessException)
            {
                //被禁止访问位置
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
                return;
            } catch (System.TimeoutException)
            {
                //超时
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
                return;
            }

            cityTextBlock.Text = location.City;
            weatherTextBlock.Text = weather;
        }
    }
}
