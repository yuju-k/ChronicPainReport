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