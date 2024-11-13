using Tizen.Applications;
using Tizen.Applications.Notifications;

namespace painReportAlarm
{
    class App : ServiceApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Tizen.Log.Info("App", "Service created");
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