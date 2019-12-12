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
	public partial class NoticeSaleFinal : ContentPage
	{
		public NoticeSaleFinal ()
		{
			InitializeComponent ();
		}

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 3]);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);
            await Navigation.PopAsync();
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}