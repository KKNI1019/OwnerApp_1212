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
	public partial class PripacyPage : ContentPage
	{
		public PripacyPage ()
		{
			InitializeComponent ();

            NavigationPage.SetHasNavigationBar(this, false);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.White;
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.Black;

            
        }

        private async void Agree_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}