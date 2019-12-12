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
	public partial class EstateSaleStep7Page : ContentPage
	{
		public EstateSaleStep7Page ()
		{
			InitializeComponent ();
		}

        private void Cancel_btn_Clicked(object sender, EventArgs e)
        {
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 7]);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 6]);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 5]);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 4]);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 3]);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);
        }

        private async void Confirm_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EstateSaleStep8());
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}