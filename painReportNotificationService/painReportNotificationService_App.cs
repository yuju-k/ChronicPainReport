using System;
using Tizen.Applications;
using Tizen.Applications.Notifications;

namespace painReportNotificationService
{
    class App : ServiceApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            AlarmManager.Instance.InitializeDailyAlarm();
            Tizen.Log.Info("painReport3", "Service started and alarms initialized");
        }

        protected override void OnAppControlReceived(AppControlReceivedEventArgs e)
        {
            base.OnAppControlReceived(e);

            // 알람 이벤트 처리
            if (e.ReceivedAppControl.Operation == "http://tizen.org/appcontrol/operation/alarm")
            {
                ShowPainReportNotification();
            }
        }
        private void ShowPainReportNotification()
        {
            try
            {
                var appControl = new AppControl
                {
                    Operation = AppControlOperations.Default,
                    ApplicationId = "org.tizen.example.painReport3UI"
                };

                var notification = new Notification
                {
                    Title = "통증 기록",
                    Content = "현재 통증 상태를 기록해주세요.",
                    Tag = "PainReport",
                    
                    DetailInfo = new NotificationDetailInfo
                    {
                        Action = appControl
                    },
                    Accessory = new Notification.AccessorySet
                    {
                        CanVibrate = true,
                        SoundOption = AccessoryOption.On,
                    }
                };

                NotificationManager.Post(notification);
                Tizen.Log.Info("painReport3", "Pain report notification posted");

                // 디버깅을 위한 추가 로그
                Tizen.Log.Info("painReport3", $"Notification created with AppId: {appControl.ApplicationId}");
            }
            catch (Exception ex)
            {
                Tizen.Log.Error("painReport3", $"Failed to show notification: {ex.Message}");
            }
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
        }

        static void Main(string[] args)
        {
            App app = new App();
            app.Run(args);
        }
    }
}
