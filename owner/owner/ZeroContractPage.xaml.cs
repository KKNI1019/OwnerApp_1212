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
	public partial class ZeroContractPage : ContentPage
	{
		public ZeroContractPage ()
		{
			InitializeComponent ();
		}

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Btn_unchecked_Clicked(object sender, EventArgs e)
        {
            btn_unchecked.IsVisible = false;
            btn_checked.IsVisible = true;
        }

        private void Btn_checked_Clicked(object sender, EventArgs e)
        {
            btn_checked.IsVisible = false;
            btn_unchecked.IsVisible = true;
        }

        private async void Btn_downgrade_Clicked(object sender, EventArgs e)
        {
            if (btn_checked.IsVisible == true)
            {
                await Navigation.PushAsync(new EstateSaleStep5Page("rent"));
            }
            else
            {
                await DisplayAlert("", "管理委託契約書の内容を確認してください。", "はい");
            }
        }

        
        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}