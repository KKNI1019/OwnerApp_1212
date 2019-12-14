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
	public partial class QuizConfPage : ContentPage
	{
		public QuizConfPage ()
		{
			InitializeComponent ();
		}

        private async void Btn_back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Btn_gohomepage_Clicked(object sender, EventArgs e)
        {
            var masterPage = this.Parent.Parent as TabbedPage;
            if (masterPage != null)
            {
                masterPage.CurrentPage = masterPage.Children[0];
            }
        }
    }
}