
namespace MobileNomad.MAUI.PushNotifications.FCM;
public static class PermissionChecker
{
    public static async Task<PermissionStatus> CheckPermissions()
    {
#if ANDROID
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>().ConfigureAwait(false);
        if (status != PermissionStatus.Granted)
        {
            return await Permissions.RequestAsync<Permissions.PostNotifications>().ConfigureAwait(false);
        }
        else
        {
            return status;
        }
#else
        var tcs = new TaskCompletionSource<PermissionStatus>();
        tcs.SetResult(PermissionStatus.Unknown);
        return await tcs.Task.ConfigureAwait(false);
#endif
    }
}
