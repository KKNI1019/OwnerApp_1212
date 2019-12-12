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
	public partial class SaleEstimateCompletePagexaml : ContentPage
	{
        

		public SaleEstimateCompletePagexaml (int rental_income, int admin_expense)
		{
			InitializeComponent ();

            estate_price_min.Text = ((rental_income - admin_expense) * 12 / 0.05).ToString();
            estate_price_max.Text = ((rental_income - admin_expense) * 12 / 0.048).ToString();

            var tapFinance = new TapGestureRecognizer();
            tapFinance.Tapped += (s, e) =>
            {
                Back_btn_click();
            };

            lbl_back.GestureRecognizers.Add(tapFinance);
        }

        private async void Back_btn_click()
        {
            await Navigation.PopAsync();
        }

        private async void Sale_request_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EstateSaleStep1Page());
        }
    }
}