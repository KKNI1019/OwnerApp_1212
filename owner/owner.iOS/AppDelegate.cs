using System;
using System.Collections.Generic;
using System.Linq;
//using SQLite.Net.Platform.XamarinIOS;
using Xamarin.Forms;
using Syncfusion.ListView.XForms.iOS;
using Foundation;
using UIKit;
using Syncfusion.SfChart.XForms.iOS.Renderers;
using Plugin.Media;
using Octane.Xamarin.Forms.VideoPlayer.iOS;
using Xamarin;
using UserNotifications;
using owner.WebService;
using Xamarin.Essentials;

namespace owner.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        //{
        //    System.Diagnostics.Debug.WriteLine($"FCM Token: {fcmToken}");
        //}

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            
            global::Xamarin.Forms.Forms.SetFlags("Shell_Experimental", "Visual_Experimental", "CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            // 16.4.0.54
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM5OTI0QDMxMzYyZTM0MmUzMGM1OEFZTGR0VjdUZUw1RWl2WEJZSzBqQmRjMldPVzRkMFM2VGtRVGR2dkk9");

            //17.2.0.34
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM5OTIzQDMxMzcyZTMyMmUzMGhGNUZBVnh1T3RBVFlBT0xpeHlrT01rRVRNSk1RMFgxbENSZlJWc0FxbjQ9");

            CrossMedia.Current.Initialize();

            Rg.Plugins.Popup.Popup.Init();
            SfListViewRenderer.Init();
            new SfChartRenderer();
            Stormlion.PhotoBrowser.iOS.Platform.Init();
            //KeyboardOverlap.Forms.Plugin.iOSUnified.KeyboardOverlapRenderer.Init();
            IQKeyboardManager.SharedManager.Enable = true;
            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
            

            App.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;
            App.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;

            //LoadApplication(new App());
            Plugin.InputKit.Platforms.iOS.Config.Init();

            string dbPath = FileAccessHelper.GetLocalFilePath("owner_city.db");

            FormsVideoPlayer.Init();

            LoadApplication(new App(dbPath));
            
            //if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            //{
            //    // iOS 10
            //    var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
            //    UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
            //    {
            //        Console.WriteLine(granted);
            //    });

            //    // For iOS 10 display notification (sent via APNS)
            //    UNUserNotificationCenter.Current.Delegate = this;
            //}
            //else
            //{
            //    // iOS 9 <=
            //    var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
            //    var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
            //    UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            //}

            //UIApplication.SharedApplication.RegisterForRemoteNotifications();

            //Firebase.Core.App.Configure();

            //Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) => {
            //    var newToken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
            //    // if you want to send notification per user, use this token
            //    System.Diagnostics.Debug.WriteLine(newToken);

            //    Messaging.SharedInstance.Connect((error) =>
            //    {
            //        if (error == null)
            //        {
            //            Messaging.SharedInstance.Subscribe("/topics/all");
            //        }
            //        System.Diagnostics.Debug.WriteLine(error != null ? "error occured" : "connect success");
            //    });
            //});

            // Register your app for remote notifications.
            //if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            //{
            //    // iOS 10 or later
            //    var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
            //    UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
            //        Console.WriteLine(granted);
            //    });

            //    // For iOS 10 display notification (sent via APNS)
            //    UNUserNotificationCenter.Current.Delegate = this;

            //    // For iOS 10 data message (sent via FCM)
            //    Messaging.SharedInstance.Delegate = this;
            //}
            //else
            //{
            //    // iOS 9 or before
            //    var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
            //    var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
            //    UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            //}


            return base.FinishedLaunching(app, options);
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            return UIInterfaceOrientationMask.Portrait;
        }


        //public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        //{
        //    PushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        //}

        //public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        //{
        //    PushNotificationManager.RemoteNotificationRegistrationFailed(error);

        //}
        //// To receive notifications in foregroung on iOS 9 and below.
        //// To receive notifications in background in any iOS version
        //public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        //{
        //    PushNotificationManager.DidReceiveMessage(userInfo);
        //}

        //public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        //{
        //    NSString title = ((userInfo["aps"] as NSDictionary)["alert"] as NSDictionary)["title"] as NSString;
        //    NSString message = ((userInfo["aps"] as NSDictionary)["alert"] as NSDictionary)["body"] as NSString;

        //    // optionally you can send a Xamarin Forms message to 
        //    // inform the Xamarin Forms Application to handle the notification
        //    //MessagingCenter.Send(new MessageNotificationReceived()
        //    //{
        //    //    Title = title,
        //    //    Message = message,
        //    //}, "");
        //}
    }
}
