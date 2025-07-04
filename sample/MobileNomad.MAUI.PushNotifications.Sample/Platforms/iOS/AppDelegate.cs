using CommunityToolkit.Mvvm.Messaging;
using Foundation;
using MobileNomad.MAUI.PushNotifications.APNS;
using UIKit;

namespace MobileNomad.MAUI.PushNotifications.Sample;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        var launchResult = base.FinishedLaunching(application, launchOptions);

        AppDelegateHelpers.NotificationFinishedLaunching(this);

        return launchResult;
    }

    [Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
    public void RegisteredForRemoteNotificationsWithDeviceToken(UIApplication application, NSData deviceToken)
    {
        var message = AppDelegateHelpers.GetRegisterDeviceMessage(deviceToken);
        WeakReferenceMessenger.Default.Send(message);
    }
}
