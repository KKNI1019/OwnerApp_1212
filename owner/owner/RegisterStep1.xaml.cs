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
	public partial class RegisterStep1 : ContentPage
	{
		public RegisterStep1 ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.White;
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.Black;
            
            rental_income.FlowDirection = Device.FlowDirection;
            
        }

        private async void Step1_next_btn_Clicked(object sender, EventArgs e)
        {

            if (rental_income.Text.Equals(""))
            {
                await DisplayAlert("", "家賃収入を入力してください。", "はい");
            }
            else
            {
                Preferences.Set(Constants.RENTAL_INCOME, rental_income.Text);
                await Navigation.PushAsync(new RegisterStep2
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