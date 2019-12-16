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
	public partial class MyInfoUpdatePage : ContentPage
	{
		public MyInfoUpdatePage ()
		{
			InitializeComponent ();
            
        }

        private async void Btn_apple_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyInfoUpdateCompletePage());
        }

        private async void Btn_carrier_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyInfoUpdateCompletePage());
        }

        private async void Btn_credit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyInfoUpdateCompletePage());
        } 

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void lbl_cancel_tap(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Btn_payment_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new MyInfoUpdateCompletePage());

            await Navigation.PushAsync(new PaymentPage(string.Empty));

            //switch (Device.RuntimePlatform)
            //{
            //    case Device.iOS:
            //        await Navigation.PushAsync(new MyInfoUpdateCompletePage());
            //        break;
            //    case Device.Android:
            //        choose_payment_type();
            //        break;
            //}
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
            await Navigation.PushAsync(new MyInfoUpdateCompletePage());
        }
        private async void credit_pay()
        {
            await Navigation.PushAsync(new MyInfoUpdateCompletePage());
        }
    }
}