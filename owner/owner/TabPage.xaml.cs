using owner.WebService;
using Plugin.Badge.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TabPage : TabbedPage
    {
		public TabPage ()
		{
			InitializeComponent ();

            Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage.SetIsSwipePagingEnabled(this, false);
            Preferences.Set(Constants.REGISTERED, true);

            showBadge();

            MessagingCenter.Subscribe<App>((App)Application.Current, "BadgeCountRefresh", (sender) => {
                showBadge();
            });
        }

        private void showBadge()
        {
            string strBadgeNum;
            int badgeNum = Preferences.Get("badgeNum", 0);

            if (badgeNum > 0)
            {
                //strBadgeNum = badgeNum.ToString();
                strBadgeNum = "1";
            }
            else
            {
                strBadgeNum = string.Empty;
            }
            TabBadge.SetBadgeText(NotiTab, strBadgeNum);
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    closeApp_check();

        //    //return base.OnBackButtonPressed();
        //    return true;
        //}

        //private async void closeApp_check()
        //{
        //    var action = await DisplayAlert("", "アプリを終了しますか？", "はい", "キャンセル");
        //    if (action)
        //    {
        //        System.Diagnostics.Process.GetCurrentProcess().Kill();
        //    }
        //}
    }
}