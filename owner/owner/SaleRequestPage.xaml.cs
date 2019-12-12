using owner.Model;
using Rg.Plugins.Popup.Services;
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
	public partial class SaleRequestPage : ContentPage
	{

        public IList<SaleItems> SaleItems { get; set; }
        private SaleItems _selectedSaleItem;
        private string mTitle;

        public SaleItems SelectedSaleItem
        {
            get { return _selectedSaleItem; }
            set
            {
                _selectedSaleItem = value;
                OnPropertyChanged();
            }
        }

        public SaleRequestPage (string title)
		{
			InitializeComponent ();

            mTitle = title;
            lbl_title.Text = mTitle;

            SaleItems = new List<SaleItems>();
            BuildingInfo Buildings = new BuildingInfo();

            if (App.estate_num != 0)
            {
                for (int i = 0; i < App.estate_num; i++)
                {
                    int int_income = Global.Buildings[i].rental_income;
                    int int_excome = Global.Buildings[i].admin_expense + Global.Buildings[i].agency_fee + Global.Buildings[i].repair_reserve;
                   
                    SaleItems.Add(new SaleItems
                    {
                        img_url = Global.Buildings[i].estate_image_url,
                        income = int_income.ToString(),
                        excome = int_excome.ToString(),
                        monthly_balance = (int_income - int_excome).ToString(),
                        building_name = Global.Buildings[i].building_name
                    });
                }
            }

            BindingContext = this;
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void imgAdd_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ZeroRegisterAlert("管理物件追加"));
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new SaleRequestDetailPage(SelectedSaleItem));

            ((ListView)sender).SelectedItem = null;
        }
    }
}