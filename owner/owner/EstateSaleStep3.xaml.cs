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
	public partial class EstateSaleStep3 : ContentPage
	{
		public EstateSaleStep3 ()
		{
			InitializeComponent ();

            lbl_owner_name.Text = Global.sale_owner_name;
            lbl_owner_address.Text = Global.sale_owner_address;
            if (string.IsNullOrEmpty(Global.sale_owner_postal_address))
            {
                lbl_owner_postal_address.Text = Global.sale_owner_address;
            }
            else
            {
                lbl_owner_postal_address.Text = Global.sale_owner_postal_address;
            }
            
            lbl_owner_phone.Text = Global.sale_owner_phone;
            lbl_estate_name.Text = Global.sale_estate_name;
            lbl_estate_address.Text = Global.sale_estate_location;
            lbl_estate_room_number.Text = Global.sale_estate_room_number;            
		}

        private async void Regist_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EstateSaleStep4Page());
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Change_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}