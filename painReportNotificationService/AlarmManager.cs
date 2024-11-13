using System;
using System.Reflection.Metadata.Ecma335;
using Tizen.Applications;
using Tizen.Applications.Notifications;

namespace painReportNotificationService
{
    public class AlarmManager
    {
        private static AlarmManager instance = null;
        private const string NOTIFICATION_TAG = "Chronic Pain";
        private const string APP_ID = "org.tizen.example.painReport3Service";

        // Singleton pattern
        public static AlarmManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AlarmManager();
                }
                return instance;
            }
        }

        private AlarmManager() { }

        //초기화
        public void InitializeDailyAlarm()
        {
            try
            {
                //기존 알람이 있다면 모두 제거
                CancelAllAlarms();

                // 시간 별 알람 설정
                SetDailyAlarm(9, 0, "아침");
                SetDailyAlarm(15, 0, "오후");
                SetDailyAlarm(21, 0, "저녁");
                SetDailyAlarm(14, 10, "테스트알람");

                //설정된 알람 Log
                CheckExistingAlarms();
            }
            catch (Exception e)
            {
                Tizen.Log.Error("painReport3", $"Failed to initialize daily alarms: {e.Message}");
            }
        }

        private void SetDailyAlarm(int hour, int minute, string period)
        {
            try
            {
                DateTime now = Tizen.Applications.AlarmManager.GetCurrentTime();
                DateTime alarmTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);

                // 지정된 시간이 현재 시간보다 이전이면 다음날로 설정
                if (alarmTime < now)
                {
                    alarmTime = alarmTime.AddDays(1);
                }

                // 알람 이벤트를 위한 AppControl 생성
                AppControl appControl = new AppControl
                {
                    Operation = "http://tizen.org/appcontrol/operation/alarm",
                    ApplicationId = APP_ID
                };

                // 알람 생성 및 설정
                var alarm = Tizen.Applications.AlarmManager.CreateAlarm(
                    alarmTime,
                    AlarmWeekFlag.AllDays,
                    appControl
                    );

                Tizen.Log.Info("painReport3", $"Alarm set for {hour}:{minute} ({period})");
            }
            catch (Exception e)
            {
                Tizen.Log.Error("painReport3", $"SetDailyAlarm failed: {e.Message}");
                throw;
            }
        }

        // 모든 알람 제거
        public void CancelAllAlarms()
        {
            try
            {
                Tizen.Applications.AlarmManager.CancelAll();
                Tizen.Log.Info("painReport3", "All alarms canceled");
            }
            catch (Exception e)
            {
                Tizen.Log.Error("painReport3", $"Failed to cancel all alarms: {e.Message}");
                throw;
            }
        }

        // 현재 설정된 모든 알람을 확인한다.
        public void CheckExistingAlarms()
        {
            try
            {
                var alarms = Tizen.Applications.AlarmManager.GetAllScheduledAlarms();
                foreach (var alarm in alarms)
                {
                    Tizen.Log.Info("painReport3", $"Existing alarm: {alarm}");
                }
            }
            catch (Exception e)
            {
                Tizen.Log.Error("painReport3", $"Failed to check existing alarms: {e.Message}");
                throw;
            }
        }
    }
}
