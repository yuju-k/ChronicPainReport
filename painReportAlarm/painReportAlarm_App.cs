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

            AlarmNoti alarmNoti = new AlarmNoti();
            alarmNoti.SetDailyAlarm(9, 0);
            alarmNoti.SetDailyAlarm(15, 0);
            alarmNoti.SetDailyAlarm(21, 0);
        }

        protected override void OnTerminate()
        {
            Tizen.Log.Info("notiApp", "Service terminated");
            base.OnTerminate();

            AlarmManager.CancelAll();
            NotificationManager.DeleteAll();

        }

        static void Main(string[] args)
        {
            App app = new App();
            app.Run(args);
        }
    }
}