using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using SQLite.Net.Interop;
using owner.DB;
using Xamarin.Essentials;
using owner.WebService;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using owner.Model;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace owner
{
    public partial class App : Application
    {
        public static string owner_ID { get; set; }
        public static string owner_name { get; set; }
        public static string owner_kana { get; set; }
        public static string owner_nickname { get; set; }
        public static string owner_email { get; set; }
        public static string owner_password { get; set; }
        public static string owner_phone1 { get; set; }
        public static string owner_phone2 { get; set; }
        public static string owner_address { get; set; }
        public static string owner_postal_address { get; set; }
        public static string owner_timezone { get; set; }
        public static string owner_memo { get; set; }
        public static string owner_estate_id { get; set; }
        public static string owner_type { get; set; }
        public static string owner_profile { get; set; }
        public static int programm_fee { get; set; }
        public static int estate_num { get; set; }
        public static string pay_info_id { get; set; }
        public static float income_rate { get; set; }
        public static string zero_id { get; set; }
        public static string owner_license { get; set; }
        public static string owner_bonus { get; set; }


        public static  int rental_income { get; set; }
        public static int admin_expense { get; set; }
        public static int repair_reserve { get; set; }
        public static int agency_fee { get; set; }
        public static string new_agency_fee { get; set; }
        public static string minimum_rental_income { get; set; }

        public static string last_news_id { get; set; }
        
        public static string newItem { get; set; }
        public static string newItem_cate { get; set; }
        public static string loan_repay_value { get; set; }
        public static string loan_reapy_type { get; set; }

        //Create two static doubles that will be used to size elements
        public static double ScreenWidth;
        public static double ScreenHeight;

        public static CityRepository CityRepo { get; private set; }
        public static NoticeData Notice_data { get; private set; }
        public static ThreadData Thread_data { get; private set; }
        public static ThreadCommentData Thread_Comment_data { get; private set; }
        public static ColumnData Column_data { get; private set; }
        public static FavoriteNewsDB Favorite_News_Data { get; private set; }
        public static NewRoportDB newReportItem { get; private set; }
       

        public App(string dbPath)
        {
            // 16.4.0.54
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM5OTI0QDMxMzYyZTM0MmUzMGM1OEFZTGR0VjdUZUw1RWl2WEJZSzBqQmRjMldPVzRkMFM2VGtRVGR2dkk9");

            //17.2.0.34
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM5OTIzQDMxMzcyZTMyMmUzMGhGNUZBVnh1T3RBVFlBT0xpeHlrT01rRVRNSk1RMFgxbENSZlJWc0FxbjQ9");

            //set database path first, then retrieve main page
            CityRepo = new CityRepository(dbPath);
            Notice_data = new NoticeData(dbPath);
            Thread_data = new ThreadData(dbPath);
            Thread_Comment_data = new ThreadCommentData(dbPath);
            Column_data = new ColumnData(dbPath);
            Favorite_News_Data = new FavoriteNewsDB(dbPath);
            newReportItem = new NewRoportDB(dbPath);

            InitializeComponent();

            MainPage = new NavigationPage(new BlankPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
