using Tizen.Applications;
using Tizen.Applications.Notifications;

namespace myPainReportAlarm
{
    class App : ServiceApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Tizen.Log.Info("notiApp", "Service created");

            AlarmNoti alarmNoti = new AlarmNoti();

            // 이전 알람삭제
            AlarmManager.CancelAll();
            NotificationManager.DeleteAll();

            Notification openNoti = new Notification
            {
                Title = "Service",
                Content = "통증 알람 서비스가 실행되었습니다.",
            };

            NotificationManager.Post(openNoti);

            alarmNoti.SetDailyAlarm(9, 0);
            alarmNoti.SetDailyAlarm(15, 0);
            alarmNoti.SetDailyAlarm(21, 0);
        }

        protected override void OnAppControlReceived(AppControlReceivedEventArgs e)
        {
            base.OnAppControlReceived(e);
        }

        protected override void OnDeviceOrientationChanged(DeviceOrientationEventArgs e)
        {
            base.OnDeviceOrientationChanged(e);
        }

        protected override void OnLocaleChanged(LocaleChangedEventArgs e)
        {
            base.OnLocaleChanged(e);
        }

        protected override void OnLowBattery(LowBatteryEventArgs e)
        {
            base.OnLowBattery(e);
        }

        protected override void OnLowMemory(LowMemoryEventArgs e)
        {
            base.OnLowMemory(e);
        }

        protected override void OnRegionFormatChanged(RegionFormatChangedEventArgs e)
        {
            base.OnRegionFormatChanged(e);
        }

        protected override void OnTerminate()
        {
            base.OnTerminate();
            Tizen.Log.Info("myPainReportAlarm", "Terminate myPainReportAlarm");
        }

        static void Main(string[] args)
        {
            App app = new App();
            app.Run(args);
        }
    }
}
