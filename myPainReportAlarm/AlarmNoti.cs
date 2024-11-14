using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tizen.Applications;
using Tizen.Applications.Notifications;
using Tizen.Applications.WatchfaceComplication;

namespace myPainReportAlarm
{
    internal class AlarmNoti
    {
        private AppControl appControl;
        private Notification notification;
        public AlarmNoti()
        {

            appControl = new AppControl
            {
                ApplicationId = "org.tizen.example.swiftPainReport",
                Operation = default,
            };

            // Notification 설정
            notification = new Notification
            {
                Title = "통증 기록 시간",
                Content = "현재 당신의 통증을 기록하세요.",
                Property = NotificationProperty.DisableAppLaunch,
                Action = appControl
            };

            Notification.AccessorySet accessorySet = new Notification.AccessorySet
            {
                CanVibrate = true,
            };

            notification.Accessory = accessorySet;
        }
        public void SetDailyAlarm(int hour, int min)
        {
            try
            {
                // 알람 시간 설정
                DateTime now = DateTime.Now;
                DateTime alarmTime = new DateTime(now.Year, now.Month, now.Day, hour, min, 0);

                // 시간이 이미 지났다면 다음날로 설정
                if (now > alarmTime)
                {
                    alarmTime = alarmTime.AddDays(1);
                }

                // 알람 등록
                Alarm alarm = AlarmManager.CreateAlarm(alarmTime, AlarmWeekFlag.AllDays, notification);
                Tizen.Log.Info("notiApp", $"alarm setting: {alarmTime}");
            }
            catch (Exception ex)
            {
                Tizen.Log.Error("notiApp", $"alarm error: {ex.Message}");
            }
        }
    }
}
