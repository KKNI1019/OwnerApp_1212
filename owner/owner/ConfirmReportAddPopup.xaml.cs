using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using owner.Model;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfirmReportAddPopup : PopupPage
    {
        private int newitem_index;
        private int current_month;

		public ConfirmReportAddPopup (int month)
		{
			InitializeComponent ();

            Global.month = month;
		}

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ent_itemname.Text))
            {
                await DisplayAlert("", "項目名を入力してください。", "はい");
            }
            else if (radio_category.SelectedItem == null)
            {
                await DisplayAlert("", "項目のカテゴリを選択してください。", "はい");
            }
            else
            {
                App.newItem = ent_itemname.Text;

                newitem_index = Preferences.Get("newitem_id", 0);
                newitem_index++;

                NewReportItem newItem = new NewReportItem();
                newItem.item_index = newitem_index.ToString();
                newItem.item_name = ent_itemname.Text;
                newItem.item_type = radio_category.SelectedItem.ToString();

                await App.newReportItem.SaveReportAsync(newItem);
                                
                Global.reportItems.Add(new ReportItems
                {
                    dynamic_index = newitem_index,
                    fee_name = ent_itemname.Text,
                    calculate_type = newItem.item_type,
                    fee_value = new string[12]
                });

                MessagingCenter.Send<App>((App)Application.Current, "AddNewItem");

                await PopupNavigation.Instance.PopAsync();
            }
            
        }

        private async void imgDel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();

        }
    }
}