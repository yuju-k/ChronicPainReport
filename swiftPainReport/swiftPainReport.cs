using System;
using Xamarin.Forms;
using Tizen.Wearable.CircularUI.Forms;
using Tizen.Security;
using static Tizen.Security.PrivacyPrivilegeManager;
using Tizen.Uix.VoiceControlManager;
using System.ComponentModel;

namespace swiftPainReport
{
    class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
    {
        private const string EXTERNAL_STORAGE_PRIVILEGE = "http://tizen.org/privilege/externalstorage";
        protected override void OnCreate()
        {
            base.OnCreate();

            //Storage ���� üũ ��û
            CheckStoragePermission();
            LoadApplication(new App());
		}

        private void CheckStoragePermission()
        {
            try
            {
                // �ܺ������ ���� üũ
                var result_external = PrivacyPrivilegeManager.CheckPermission(EXTERNAL_STORAGE_PRIVILEGE);

                switch (result_external)
                {
                    case CheckResult.Allow:
                        Tizen.Log.Info("storage_permission", "Storage permission already granted");
                        break;
                    case CheckResult.Deny:
                        Tizen.Log.Error("storage_permission", "Storage permission denied");
                        break;
                    case CheckResult.Ask:
                        //���� ��û
                        PrivacyPrivilegeManager.RequestPermission(EXTERNAL_STORAGE_PRIVILEGE);
                        var response = PrivacyPrivilegeManager.GetResponseContext(EXTERNAL_STORAGE_PRIVILEGE);

                        if (response.TryGetTarget(out ResponseContext context))
                        {
                            context.ResponseFetched += (s, e) =>
                            {
                                if (e.result == RequestResult.AllowForever)
                                {
                                    Tizen.Log.Info("storage_permission", "Storage permission granted");
                                }
                                else
                                {
                                    Tizen.Log.Error("storage_permission", "Storage permission denied by user");
                                }
                            };
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Tizen.Log.Error("storage_permission", $"Storage permission denied: {ex.Message}");
            }
        }

		static void Main(string[] args)
        {
            var app = new Program();
            Forms.Init(app);
            FormsCircularUI.Init();
            app.Run(args);
        }
    }
}
