using System;
using Xamarin.Forms;
using Tizen.Applications;
using Tizen.Wearable.CircularUI.Forms.Renderer.Watchface;


namespace AmbientWatch
{
    class Program : FormsWatchface
    {
        ClockViewModel ViewModel;
        private readonly IAppLaunchService _appLaunchService;

        public Program()
        {
            _appLaunchService = new AppLaunchService();
        }
        protected override async void OnCreate()
        {
            base.OnCreate();

            // Display 상태 항상 ON
            Tizen.System.Power.RequestLock(Tizen.System.PowerLock.DisplayNormal, 0);

            var watchfaceApp = new AmbientWatchApplication();
            ViewModel = new ClockViewModel(this);
            ViewModel.Time = GetCurrentTime().UtcTimestamp;
            watchfaceApp.BindingContext = ViewModel;
            LoadWatchface(watchfaceApp);

            LaunchServiceApp();
        }

        private void LaunchServiceApp()
        {
            try
            {
                string serviceAppId = "org.tizen.example.swiftPainReport";

                AppControl appControl = new AppControl
                {
                    ApplicationId = serviceAppId,
                    Operation = AppControlOperations.Default
                };

                AppControl.SendLaunchRequest(appControl, (request, reply, result) =>
                {
                    if (result == AppControlReplyResult.Succeeded)
                    {
                        Tizen.Log.Info("AmbientWatch", "Service launch succeeded");
                    }
                    else
                    {
                        Tizen.Log.Error("AmbientWatch", $"Service launch failed: {result}");
                    }
                });
            }
            catch (Exception ex)
            {
                Tizen.Log.Error("AmbientWatch", $"Service launch exception: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /// Called when the time tick event occurs
        /// <param name="time">TimeEventArgs</param>
        protected override void OnTick(TimeEventArgs time)
        {
            base.OnTick(time);
            // Set time to update UI
            if (ViewModel != null)
            {
                ViewModel.Time = time.Time.UtcTimestamp + TimeSpan.FromMilliseconds(time.Time.Millisecond);
            }
        }
        protected override void OnAmbientChanged(AmbientEventArgs mode)
        {
            base.OnAmbientChanged(mode);
            ViewModel.AmbientModeDisabled = !mode.Enabled;
        }

        protected override void OnAmbientTick(TimeEventArgs time)
        {
            base.OnAmbientTick(time);
            // Set time to update UI in ambient mode
            ViewModel.Time = time.Time.UtcTimestamp + TimeSpan.FromMilliseconds(time.Time.Millisecond);
        }

        //종료 시
        protected override void OnTerminate()
        {
            base.OnTerminate();
            Tizen.System.Power.ReleaseLock(Tizen.System.PowerLock.DisplayNormal);
        }
        static void Main(string[] args)
        {
            var app = new Program();
            Forms.Init(app);
            Tizen.Wearable.CircularUI.Forms.FormsCircularUI.Init();
            app.Run(args);
        }
    }
}