using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using Tizen.Wearable.CircularUI.Forms.Renderer.Watchface;

namespace AmbientWatch
{
    public class ClockViewModel : INotifyPropertyChanged
    {
        private readonly IAppLaunchService _appLaunchService;

        public ICommand PainRecordCommand { get; private set; }
        public ClockViewModel(FormsWatchface p)
        {
            _appLaunchService = new AppLaunchService();
            AmbientModeDisabled = true;
            InitializeCommands();
        }

        private void InitializeCommands ()
        {
            PainRecordCommand = new Command(async () => await ExecutePainRecord());
        }

        private async Task ExecutePainRecord()
        {
            try
            {
                const string PainReportAppId = "org.tizen.example.swiftPainReport";
                bool success = await _appLaunchService.LaunchAppAsync(PainReportAppId);

                if (!success)
                {
                    // TODO: 앱 실행 실패 시 처리 로직 추가
                    // 예: 사용자에게 알림 표시
                }
            }
            catch (Exception ex)
            {
                // TODO: 예외 발생 시 처리 로직 추가
                // 예: 로그 기록, 사용자에게 알림
            }
        }


        DateTime _Time;
        double _Hours, _Minutes, _Seconds;
        bool _AmbientModeDisabled;

        /// <summary>
        /// Hours for an hour hand
        /// </summary>
        public double Hours
        {
            get
            {
                return _Hours;
            }

            set
            {
                SetProperty(ref _Hours, value, "Hours");
            }
        }

        /// <summary>
        /// Minutes for a minute hand
        /// </summary>
        public double Minutes
        {
            get
            {
                return _Minutes;
            }

            set
            {
                SetProperty(ref _Minutes, value, "Minutes");
            }
        }

        /// <summary>
        /// Seconds for a second hand
        /// </summary>
        public double Seconds
        {
            get
            {
                return _Seconds;
            }

            set
            {
                SetProperty(ref _Seconds, value, "Seconds");
            }
        }

        /// <summary>
        /// Check whether a device is in ambient mode or not
        /// According to it, second hand will be shown or not
        /// </summary>
        public bool AmbientModeDisabled
        {
            get
            {
                return _AmbientModeDisabled;
            }

            set
            {
                SetProperty(ref _AmbientModeDisabled, value, "AmbientModeDisabled");
            }
        }
        
        /// <summary>
        /// Time
        /// </summary>
        public DateTime Time
        {
            get => _Time;
            set
            {
                if (_Time == value)
                {
                    return;
                }

                Hours = 30 * (_Time.Hour % 12) + 0.5f * _Time.Minute;
                Minutes = 6 * _Time.Minute + 0.1f * _Time.Second;
                Seconds = 6 * _Time.Second + (0.006f * _Time.Millisecond);
                SetProperty(ref _Time, value, "Time");
            }
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName]string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
