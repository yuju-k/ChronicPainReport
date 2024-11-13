using System;
using System.Threading.Tasks;
using Tizen.Applications;

namespace AmbientWatch
{
    public interface IAppLaunchService
    {
        Task<bool> LaunchAppAsync(string applicationId);
    }
    class AppLaunchService : IAppLaunchService
    {
        public Task<bool> LaunchAppAsync(string applicationId)
        {
            var tcs = new TaskCompletionSource<bool>();

            try
            {
                var appControl = new AppControl
                {
                    ApplicationId = applicationId
                };

                AppControl.SendLaunchRequest(appControl, (launchRequest, reply, result) =>
                {
                    if (result == AppControlReplyResult.Succeeded)
                    {
                        tcs.SetResult(true);
                    }
                    else
                    {
                        tcs.SetResult(false);
                    }
                });
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }

            return tcs.Task;
        }
    }
}
