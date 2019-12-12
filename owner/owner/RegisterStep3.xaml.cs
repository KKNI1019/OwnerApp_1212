using owner.WebService;
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
	public partial class RegisterStep3 : ContentPage
	{
		public RegisterStep3 ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.White;
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.Black;
            
        }

        private async void Step3_next_btn_Clicked(object sender, EventArgs e)
        {
            if (agency_fee.Text.Equals(""))
            {
                await DisplayAlert("", "代行手数料を入力してください。", "はい");
            }
            else
            {
                Preferences.Set(Constants.AGENCY_FEE, agency_fee.Text);
                await Navigation.PushAsync(new RegisterFinal
                {

                });
            }
        }

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}