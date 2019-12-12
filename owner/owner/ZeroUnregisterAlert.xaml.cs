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
	public partial class ZeroUnregisterAlert : ContentPage
	{
        private string title;

		public ZeroUnregisterAlert (string txt_title)
		{
			InitializeComponent ();

            title = txt_title;
            lbl_title.Text = title;

        }

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Btn_cancel_step_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ZeroContractPage());
            
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}