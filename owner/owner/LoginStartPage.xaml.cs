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
	public partial class LoginStartPage : ContentPage
	{
		public LoginStartPage ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
            this.SetValue(NavigationPage.BarBackgroundColorProperty, Color.White);
            this.SetValue(NavigationPage.BarTextColorProperty, Color.Black);

            var tapFinance = new TapGestureRecognizer();
            tapFinance.Tapped += (s, e) =>
            {
                Goto_login_page();
            };

            lbl_login.GestureRecognizers.Add(tapFinance);
        }

        private async void Start_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage
            {

            });
        }

        private async void Goto_login_page()
        {
            await Navigation.PushAsync(new LoginPage
            {

            });
        }
    }
}