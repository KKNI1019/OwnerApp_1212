using owner.Model;
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
	public partial class EstateSaleStep2Page : ContentPage
	{
        private bool update_address_checked;
		public EstateSaleStep2Page ()
		{
			InitializeComponent ();
            update_address_checked = false;

            owner_name.Text = App.owner_name;
            owner_address.Text = App.owner_address;
            owner_new_address.Text = App.owner_postal_address;
            owner_phone.Text = App.owner_phone1;
           
		}

        private async void Estate_regist_btn_Clicked(object sender, EventArgs e)
        {
            if (owner_name.Text == "")
            {
                await DisplayAlert("", "氏名を入力してください。", "はい");
            }
            else if (owner_address.Text == "")
            {
                await DisplayAlert("", "住所を入力してください。", "はい");
            }
            else if (update_address_checked && owner_new_address.Text == "")
            {
                await DisplayAlert("", "送先住所を入力してください。", "はい");
            }
            else if (owner_phone.Text == "")
            {
                await DisplayAlert("", "連絡先電話番号を入力してください。", "はい");
            }
            else if (estate_name.Text == "")
            {
                await DisplayAlert("", "物件名を入力してください。", "はい");
            }
            else if (estate_location.Text == "")
            {
                await DisplayAlert("", "物件所在地を入力してください。", "はい");
            }
            else if (estate_room_number.Text == "")
            {
                await DisplayAlert("", "号室を入力してください。", "はい");
            }
            else
            {
                Global.sale_owner_name = owner_name.Text.Trim();                
                App.owner_name = owner_name.Text.Trim();
                Global.sale_owner_address = owner_address.Text.Trim();
                App.owner_address = owner_address.Text.Trim();
                if (update_address_checked)
                {
                    Global.sale_owner_postal_address = owner_new_address.Text.Trim();
                    App.owner_postal_address = owner_new_address.Text.Trim();
                } else
                {
                    Global.sale_owner_postal_address = App.owner_postal_address;
                }
                Global.sale_owner_phone = owner_phone.Text.Trim();
                App.owner_phone1 = owner_phone.Text.Trim();
                Global.sale_estate_name = estate_name.Text.Trim();
                Global.sale_estate_location = estate_location.Text.Trim();
                Global.sale_estate_room_number = estate_room_number.Text.Trim();
                await Navigation.PushAsync(new EstateSaleStep3());
            }
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void unchecked_btn_Clicked(object sender, EventArgs e)
        {
            img_unchecked.IsVisible = false;
            img_checked.IsVisible = true;
            owner_new_address.BackgroundColor = Color.White;
            frm_new_address.BackgroundColor = Color.White;
            owner_new_address.InputTransparent = false;
        }

        private void checked_btn_Clicked(object sender, EventArgs e)
        {
            img_unchecked.IsVisible = true;
            img_checked.IsVisible = false;
            owner_new_address.BackgroundColor = Color.FromHex("#A0A0A0");
            frm_new_address.BackgroundColor = Color.FromHex("#A0A0A0");
            owner_new_address.InputTransparent = true;
        }
    }
}