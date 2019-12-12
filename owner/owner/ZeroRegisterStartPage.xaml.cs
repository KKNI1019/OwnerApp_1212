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
	public partial class ZeroRegisterStartPage : ContentPage
	{
		public ZeroRegisterStartPage ()
		{
			InitializeComponent ();
            
        }

        private async void Back_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Start_btn_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new ZeroRegisterAlert { });
        }
    }
    
}