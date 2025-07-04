# MobileNomad MAUI Push Notifications

A comprehensive .NET MAUI library for implementing cross-platform push notifications using Azure Notification Hubs. This solution supports multiple push notification services including APNS (Apple Push Notification Service), FCM (Firebase Cloud Messaging), and WNS (Windows Notification Service).

## Purpose

This library provides a unified, platform-specific implementation for push notifications in .NET MAUI applications, leveraging Azure Notification Hubs as the central messaging service. It abstracts the complexity of platform-specific push notification implementations while maintaining full control over notification handling.

## Projects Overview

### Core Projects

#### **MobileNomad.MAUI.PushNotifications.Core**
- **Purpose**: Provides the foundational interfaces, base classes, and shared functionality for push notifications
- **Key Components**:
  - `INotificationRegistrationService` - Core registration interface
  - `NotificationRegistrationServiceBase` - Base implementation for platform-specific services
  - `PushMessages` - Static event system for notification handling
  - `RegisterDeviceMessage` - Data structure for device registration
  - Azure Notification Hub client configuration

### Platform-Specific Projects

#### **MobileNomad.MAUI.PushNotifications.APNS**
- **Purpose**: Apple Push Notification Service implementation for iOS and Mac Catalyst
- **Platforms**: iOS 15.0+, Mac Catalyst 15.0+
- **Key Components**:
  - Platform-specific `NotificationRegistrationService` implementations
  - `UserNotificationDelegate` for handling notification events
  - `AppDelegateHelpers` for easy integration into AppDelegate

#### **MobileNomad.MAUI.PushNotifications.FCM**
- **Purpose**: Firebase Cloud Messaging implementation for Android
- **Platforms**: Android API 21+
- **Dependencies**: Xamarin.Firebase.Messaging

#### **MobileNomad.MAUI.PushNotifications.WNS**
- **Note**: This project is in progress.
- **Purpose**: Windows Notification Service implementation for Windows
- **Platforms**: Windows 10.0.17763.0+

### Sample Project

#### **MobileNomad.MAUI.PushNotifications.Sample**
- **Purpose**: Demonstrates how to integrate and use the push notification libraries
- **Platforms**: All supported platforms (Android, iOS, Mac Catalyst, Windows)

## Getting Started

### 1. Installation

Add references to the required projects in your MAUI application:
```
<PackageReference Include="MobileNomad.MAUI.PushNotifications.Core" Version="1.0.0" />
<PackageReference Include="MobileNomad.MAUI.PushNotifications.APNS" Version="1.0.1" />
<PackageReference Include="MobileNomad.MAUI.PushNotifications.FCM" Version="1.0.1" />
```

### 2. Setup in MauiProgram.cs

Configure the notification services in your `MauiProgram.cs`:
```
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .SetupNotificationsCore(azureConnectionString, hubName)
            .SetupNotificationsFCM()    // For Android
            .SetupNotificationsAPNS();  // For iOS/Mac Catalyst

        return builder.Build();
    }
}
```
### 3. Azure Notification Hub Configuration

You'll need:
- Azure Notification Hub connection string
- Hub name
- Platform-specific credentials (APNS certificates, FCM server key, etc.)

### 4. Platform-Specific Setup

#### iOS/Mac Catalyst
- Add notification setup to your `AppDelegate`:
```
using MobileNomad.MAUI.PushNotifications.APNS;

public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        AppDelegateHelpers.NotificationFinishedLaunching(this);
        return base.FinishedLaunching(application, launchOptions);
    }
}
```

- Then in the register for remote notifications event, use the received token to register for notifications
then register the device. You will need a reference to the `INotificationRegistrationService` to call the registration method:

```
    [Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
    public void RegisteredForRemoteNotificationsWithDeviceToken(UIApplication application, NSData deviceToken)
    {
        var message = AppDelegateHelpers.GetRegisterDeviceMessage(deviceToken);
        _notificationRegistrationService.RegisterDevice(message);
    }
```

- Ensure there is a entitlements.plist file with the push notifications entitlement

#### MacCatalyst
Same as iOS

#### Android
- Ensure your `google-services.json` file is included in the Android platform folder.

- Add the following to the project file to load the `google-services.json` file:

```
<ItemGroup>
    <GoogleServicesJson Include="Platforms\Android\google-services.json" />
</ItemGroup>
```

- Ensure the `AndroidManifest.xml` has the following permissions:
```
<uses-permission android:name="com.google.android.c2dm.permission.RECEIVE" />
<uses-permission android:name="android.permission.WAKE_LOCK" />
<uses-permission android:name="android.permission.GET_ACCOUNTS" />
```

- On the `App.xaml.cs` file, in the OnStart event, you can ask the user for notificaiton permissions.
The library does have a default implementation to request permissions.
You can use it, or provide your own implementation to request permissions.
Like with iOS and MacCatalyst, you will need to get a reference to the Android implementation of
`INotificationRegistrationService` to register the device.

```
#if ANDROID
var permissionStatus = await PermissionChecker.CheckPermissions();
if (permissionStatus == PermissionStatus.Granted)
{
    var message = new RegisterDeviceMessage();
    _notificationRegistrationService.RegisterDevice(message);
}
#endif
```

## Architecture

### Key Features

- **Cross-Platform**: Single API for all supported platforms
- **Azure Integration**: Built specifically for Azure Notification Hubs
- **Event-Driven**: Uses static event system for notification handling
- **Modular**: Platform-specific implementations can be included as needed
- **Type-Safe**: Strongly-typed message and response objects

### Notification Flow

1. App initializes with Azure Notification Hub credentials
2. Platform-specific services register device tokens
3. Azure Notification Hub manages device registrations
4. Incoming notifications are handled by platform delegates
5. Events are raised through the `PushMessages` static class

## Requirements

- .NET 9.0
- .NET MAUI
- Azure Notification Hub service
- Platform-specific push notification credentials

## Supported Platforms

- **Android**: API 21+ (via FCM)
- **iOS**: 15.0+ (via APNS)
- **Mac Catalyst**: 15.0+ (via APNS)
- **Windows**: 10.0.17763.0+ (via WNS - implementation still in progress)

## Dependencies

- Microsoft.Maui.Controls 9.0.50
- Microsoft.Azure.NotificationHubs 4.2.0
- Xamarin.Firebase.Messaging 124.1.0.1 (Android only)

## Sample Application

The included sample application demonstrates:
- Proper service configuration
- Device registration
- Notification handling
- Cross-platform compatibility

Run the sample to see the push notification system in action across different platforms.
