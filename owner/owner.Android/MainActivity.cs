using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using owner.Droid;
//using SQLite.Net.Platform.XamarinAndroid;
using Rg.Plugins.Popup.Services;
using Plugin.Media;
using Plugin.Permissions;
using Firebase;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Firebase.Iid;
using owner.WebService;
using Octane.Xamarin.Forms.VideoPlayer.Android;
using Android.Content;
using Plugin.InAppBilling;

namespace owner.Droid
{
    [Activity(Label = "owner", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public async override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                await PopupNavigation.Instance.PopAsync();
            }
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            // 16.4.0.54
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM5OTI0QDMxMzYyZTM0MmUzMGM1OEFZTGR0VjdUZUw1RWl2WEJZSzBqQmRjMldPVzRkMFM2VGtRVGR2dkk9");

            // 17.2.0.34
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM5OTIzQDMxMzcyZTMyMmUzMGhGNUZBVnh1T3RBVFlBT0xpeHlrT01rRVRNSk1RMFgxbENSZlJWc0FxbjQ9");

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            await CrossMedia.Current.Initialize();
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
            //Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.Init(this, savedInstanceState);
            //VideoViewRenderer.Init();

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.SetFlags("Shell_Experimental", "Visual_Experimental", "CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            //LoadApplication(new App());
            Stormlion.PhotoBrowser.Droid.Platform.Init(this);
            App.ScreenWidth = Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density;
            App.ScreenHeight = Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density;

            string dbPath = FileAccessHelper.GetLocalFilePath("owner_city.db");

            FormsVideoPlayer.Init();
            
            FirebaseApp.InitializeApp(this);

            try
            {
                await Task.Run(() =>
                {
                    var preferenceToken = Preferences.Get(Constants.DEVICE_TOKEN, "");
                    if (preferenceToken.Equals(""))
                    {
                        FirebaseInstanceId.Instance.DeleteInstanceId();
                    }

                });
            }
            catch
            {

            }

            

            Plugin.InputKit.Platforms.Droid.Config.Init(this, savedInstanceState);

            LoadApplication(new App(dbPath));

            RequestedOrientation = ScreenOrientation.Portrait;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);            
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
        }
    }
}
