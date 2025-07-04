namespace MobileNomad.MAUI.PushNotifications;

/// <summary>
/// Provides static methods and events for handling push notification messages across platforms.
/// </summary>
public static class PushMessages
{
    /// <summary>
    /// Event triggered when a push notification message is received.
    /// </summary>
    /// <remarks>
    /// The event provides the notification type and an array of parameters. The handler should return true if the message was handled.
    /// </remarks>
    public static event Func<NotificaitonType, object[], bool>? OnMessageReceived;

    /// <summary>
    /// Raises the <see cref="OnMessageReceived"/> event with the specified notification type and parameters.
    /// </summary>
    /// <param name="notificationType">The type of notification received.</param>
    /// <param name="parameters">Additional parameters associated with the notification.</param>
    /// <returns>True if the message was handled; otherwise, false.</returns>
    public static bool RaiseMessageReceived(NotificaitonType notificationType, params object[] parameters)
    {
        bool returnValue = true;
        if (OnMessageReceived != null)
        {
            returnValue = OnMessageReceived.Invoke(notificationType, parameters);
        }
        return returnValue;
    }
}

/// <summary>
/// Enumerates the types of push notifications supported by the application.
/// </summary>
public enum NotificaitonType
{
    /// <summary>
    /// Android Firebase Cloud Messaging notification.
    /// </summary>
    AndroidFirebase,
    /// <summary>
    /// iOS APNS notification will present event.
    /// </summary>
    iOSAPNSWillPresentNotification,
    /// <summary>
    /// iOS APNS notification did receive event.
    /// </summary>
    iOSAPNSDidReceiveNotification,
    /// <summary>
    /// Mac Catalyst APNS notification will present event.
    /// </summary>
    MacOSAPNSWillPresentNotification,
    /// <summary>
    /// Mac Catalyst APNS notification did receive event.
    /// </summary>
    MacOSAPNSDidReceiveNotification,
}