using CommunityToolkit.Mvvm.Messaging;
using MobileNomad.MAUI.PushNotifications.FCM;
using MobileNomad.MAUI.PushNotifications.Helpers;
using MobileNomad.MAUI.PushNotifications.Interfaces;

namespace MobileNomad.MAUI.PushNotifications.Sample;

public partial class App : Application
{
    private readonly INotificationRegistrationService _notificationRegistrationService;

    public App(INotificationRegistrationService notificationRegistrationService)
	{
        _notificationRegistrationService = notificationRegistrationService;

        InitializeComponent();
        RegisterMessageListeners();

    }

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

    private void RegisterMessageListeners()
    {
        WeakReferenceMessenger.Default.Register<RegisterDeviceMessage>(this, async (r, m) =>
        {
            if ((await _notificationRegistrationService.RegisterDevice(m)).Status != ReturnStatus.Success)
            {
                Console.WriteLine("Failed to register device with notification hub.");
            }
        });
    }

    protected override async void OnStart()
    {
        base.OnStart();
		var permissionStatus = await PermissionChecker.CheckPermissions();
		if (permissionStatus == PermissionStatus.Granted)
		{
            var message = new RegisterDeviceMessage();
            WeakReferenceMessenger.Default.Send(message);
        }
    }
}