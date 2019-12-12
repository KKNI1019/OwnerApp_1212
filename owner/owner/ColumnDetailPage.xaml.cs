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
	public partial class ColumnDetailPage : ContentPage
	{
        public ColumnDetailPage()
        {
            InitializeComponent();
        }

        private void btnMenu_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new ColumnPopup(imgWritter.Source, lbl_writterName.Text));
        }

        private void HighlightTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (lay_highlight.IsVisible)
            {
                lay_highlight.IsVisible = false;
            }
            else
            {
                lay_highlight.IsVisible = true;
            }
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            removeNew();

            await Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            removeNew();

            return base.OnBackButtonPressed();
        }

        private async void removeNew()
        {
            Columns column = this.BindingContext as Columns;
            column.IsVisible = false;
            await App.Column_data.SaveColumnAsync(column);
        }
    }
}