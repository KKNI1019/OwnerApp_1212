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
	public partial class ZeroWrongPage : ContentPage
	{
		public ZeroWrongPage ()
		{
			InitializeComponent ();
		}

        private async void Btn_completion_Clicked(object sender, EventArgs e)
        {
            var masterPage = this.Parent.Parent as TabbedPage;
            if (masterPage != null)
            {
                masterPage.CurrentPage = masterPage.Children[0];
            }

            //await Navigation.PushAsync(new HomePage());
        }
    }
}