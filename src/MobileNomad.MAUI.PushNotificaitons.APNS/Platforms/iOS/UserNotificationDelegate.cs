using Foundation;
using UIKit;
using UserNotifications;

namespace MobileNomad.MAUI.PushNotifications.APNS;

/// <summary>
/// Handles iOS user notification events and integrates with the application's push notification message system.
/// </summary>
public class UserNotificationDelegate : NSObject, IUNUserNotificationCenterDelegate
{
    /// <summary>
    /// Called when a notification is delivered to a foreground app.
    /// Raises the <see cref="PushMessages.OnMessageReceived"/> event for <see cref="NotificaitonType.iOSAPNSWillPresentNotification"/>.
    /// </summary>
    /// <param name="center">The notification center object that received the notification.</param>
    /// <param name="notification">The notification that is about to be presented.</param>
    /// <param name="completionHandler">The completion handler to execute with the presentation options.</param>
    [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
    public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
    {
        bool performDefaultNotification;

        performDefaultNotification = PushMessages.RaiseMessageReceived(NotificaitonType.iOSAPNSWillPresentNotification, center, completionHandler, completionHandler);
        if (performDefaultNotification)
        {
            completionHandler(UNNotificationPresentationOptions.Banner);
        }
    }

    /// <summary>
    /// Called when the user responds to a delivered notification.
    /// Raises the <see cref="PushMessages.OnMessageReceived"/> event for <see cref="NotificaitonType.iOSAPNSDidReceiveNotification"/>.
    /// </summary>
    /// <param name="center">The notification center object that received the notification response.</param>
    /// <param name="response">The user's response to the notification.</param>
    /// <param name="completionHandler">The completion handler to execute when the response handling is complete.</param>
    [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
    public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
    {
        bool performDefaultNotification;

        performDefaultNotification = PushMessages.RaiseMessageReceived(NotificaitonType.iOSAPNSDidReceiveNotification, center, completionHandler, completionHandler);
        if (performDefaultNotification)
        {
            completionHandler();
        }
    }

    /// <summary>
    /// Called when the app successfully registers with APNS and receives a device token.
    /// Converts the device token to a <see cref="RegisterDeviceMessage"/> for further processing.
    /// </summary>
    /// <param name="application">The application instance.</param>
    /// <param name="deviceToken">The device token received from APNS.</param>
    [Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
    public void RegisteredForRemoteNotificationsWithDeviceToken(UIApplication application, NSData deviceToken)
    {
        var message = AppDelegateHelpers.GetRegisterDeviceMessage(deviceToken);
        //WeakReferenceMessenger.Default.Send(message);
    }
}