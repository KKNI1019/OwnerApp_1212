using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewsWebPage : ContentPage
	{
		public NewsWebPage (string news_url)
		{
			InitializeComponent ();

            lbl_url.Text = news_url;
            webview.Source = news_url;
        }

        private async void ImgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Imgmenu_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }
    }
}