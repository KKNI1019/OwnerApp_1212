using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ZeroRegisterAlert : ContentPage
	{
        private string title;

		public ZeroRegisterAlert (string txt_title)
		{
			InitializeComponent ();

            title = txt_title;
            lbl_title.Text = title;

            var tapFinance = new TapGestureRecognizer();
            tapFinance.Tapped += (s, e) =>
            {
                 Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);
            };

            lbl_cancel.GestureRecognizers.Add(tapFinance);
            
        }

        private void OnLabelClicked()
        {
            //await Navigation.PushAsync(new ZeroRegisterPage { });
        }

        private async void Start_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ZeroRegisterPage(title));
        }

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}