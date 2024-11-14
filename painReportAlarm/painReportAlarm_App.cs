using Tizen.Applications;
using Tizen.Applications.Notifications;

namespace painReportAlarm
{
    class App : ServiceApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Tizen.Log.Info("notiApp", "Service created");

            Notification openNoti = new Notification
            {
                Title = "Service",
                Content = "통증 알람 서비스가 실행되었습니다.",
            };

            NotificationManager.Post(openNoti);

            AlarmNoti alarmNoti = new AlarmNoti();
            alarmNoti.SetDailyAlarm(9, 0);
            alarmNoti.SetDailyAlarm(15, 0);
            alarmNoti.SetDailyAlarm(21, 0);
        }

        protected override void OnTerminate()
        {
            AlarmManager.CancelAll();
            NotificationManager.DeleteAll();

            Tizen.Log.Info("notiApp", "Service terminated");
            base.OnTerminate();
        }

        static void Main(string[] args)
        {
            App app = new App();
            app.Run(args);
        }
    }
}