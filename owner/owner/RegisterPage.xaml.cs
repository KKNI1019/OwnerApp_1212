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
	public partial class RegisterPage : ContentPage
	{
		public RegisterPage ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
            //((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.White;
            ((NavigationPage)Application.Current.MainPage).SetValue(NavigationPage.BarBackgroundColorProperty, Color.White);
            //((NavigationPage)Application.Current.MainPage).BarTextColor = Color.Black;
            ((NavigationPage)Application.Current.MainPage).SetValue(NavigationPage.BarTextColorProperty, Color.Black);

            var lblTapFinance = new TapGestureRecognizer();
            lblTapFinance.Tapped += (s, e) =>
            {
                Go_to_Register_Final(s, e);
            };

            lbl_later.GestureRecognizers.Add(lblTapFinance);
        }

        private async void Start_register_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterStep1());
        }

        private async void Go_to_Register_Final(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterFinal { });
        }
    }

   
}