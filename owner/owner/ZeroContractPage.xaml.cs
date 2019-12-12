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
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        await Navigation.PushAsync(new ZeroFinalPage());
                        break;
                    case Device.Android:
                        choose_payment_type();
                        break;
                }
            }
            else
            {
                await DisplayAlert("", "管理委託契約書の内容を確認してください。", "はい");
            }
        }

        private async void choose_payment_type()
        {
            var actionsheet = await DisplayActionSheet(null, null, null, "キャリア決済", "クレジットカード決済");

            switch (actionsheet)
            {
                case "キャリア決済":
                    carrier_pay();
                    break;
                case "クレジットカード決済":
                    credit_pay();
                    break;
            }
        }
        private async void carrier_pay()
        {
            await Navigation.PushAsync(new ZeroFinalPage());
        }
        private async void credit_pay()
        {
            await Navigation.PushAsync(new ZeroFinalPage());
        }

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}