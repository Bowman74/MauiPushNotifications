using Foundation;
using MobileNomad.MAUI.PushNotifications.Helpers;
using UIKit;
using UserNotifications;

namespace MobileNomad.MAUI.PushNotifications.APNS;

/// <summary>
/// Provides helper methods for configuring push notification registration and handling in the iOS AppDelegate.
/// </summary>
public static class AppDelegateHelpers
{
    /// <summary>
    /// Configures push notification registration with a default user notification delegate.
    /// This should be placed in the FinishedLaunching event
    /// </summary>
    /// <param name="appDelegate">The MAUI application delegate.</param>
    public static void NotificationFinishedLaunching(MauiUIApplicationDelegate appDelegate)
    {
        var userNotificationDelegate = new UserNotificationDelegate();
        NotificationFinishedLaunching(appDelegate, userNotificationDelegate);
    }

    /// <summary>
    /// Configures push notification registration with a custom user notification center delegate.
    /// This should be placed in the FinishedLaunching event
    /// </summary>
    /// <param name="appDelegate">The MAUI application delegate.</param>
    /// <param name="userNotificationCenterDelegate">The user notification center delegate to handle notification events.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="appDelegate"/> or <paramref name="userNotificationCenterDelegate"/> is null.</exception>
    public static void NotificationFinishedLaunching(MauiUIApplicationDelegate appDelegate, IUNUserNotificationCenterDelegate userNotificationCenterDelegate)
    {
        if (appDelegate == null)
        {
            throw new ArgumentNullException(nameof(appDelegate));
        }
        if (userNotificationCenterDelegate == null)
        {
            throw new ArgumentNullException(nameof(userNotificationCenterDelegate));
        }
        
        var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
        UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
        {
            if (granted && error == null)
            {
                appDelegate.InvokeOnMainThread(() =>
                {
                    UNUserNotificationCenter.Current.Delegate = userNotificationCenterDelegate;
                    UIApplication.SharedApplication.RegisterForRemoteNotifications();
                });
            }
        });
    }

    /// <summary>
    /// Converts an iOS device token to a <see cref="RegisterDeviceMessage"/> for push notification registration.
    /// This item should be placed in application:didRegisterForRemoteNotificationsWithDeviceToken:    
    /// </summary>
    /// <param name="deviceToken">The device token received from APNS.</param>
    /// <returns>A <see cref="RegisterDeviceMessage"/> containing the formatted device token.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="deviceToken"/> is null or empty.</exception>
    public static RegisterDeviceMessage GetRegisterDeviceMessage(NSData deviceToken)
    {
        if (deviceToken == null)
        {
            throw new ArgumentNullException(nameof(deviceToken), "Device token is null");
        }

        string _token = string.Empty;
        if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
        {
            var data = deviceToken.ToArray();
            _token = BitConverter
                .ToString(data)
                .Replace("-", "")
                .Replace("\"", "");
        }
        else if (!string.IsNullOrEmpty(deviceToken.Description))
        {
            _token = deviceToken.Description.Trim('<', '>');
        }

        if (string.IsNullOrEmpty(_token))
        {
            throw new ArgumentNullException(nameof(deviceToken), "Device token is empty");
        }
        var message = new RegisterDeviceMessage
        {
            DeviceToken = _token
        };

        return message;
    }
}