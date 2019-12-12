using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using owner.Model;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaleRequestDetailPage : ContentPage
	{
        private SaleItems mSaleItem;

        public SaleRequestDetailPage (SaleItems saleItem)
		{
			InitializeComponent ();
            mSaleItem = saleItem;
            lbl_title.Text = mSaleItem.building_name;
            img_building.Source = mSaleItem.img_url;
            month_excome.Text = mSaleItem.monthly_balance;
            int month_balance = 0;
            bool is_success = int.TryParse(mSaleItem.monthly_balance, out month_balance);
            if (is_success)
            {
                year_excome.Text = (month_balance * 12).ToString();
            }
            current_excome.Text = mSaleItem.income;
            excome.Text = mSaleItem.excome;
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private void imgMenu_clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }
    }
}