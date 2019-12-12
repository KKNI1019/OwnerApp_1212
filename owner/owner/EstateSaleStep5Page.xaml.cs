using owner.Model;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
	public partial class EstateSaleStep5Page : ContentPage
	{
		public EstateSaleStep5Page ()
		{
			InitializeComponent ();
		}

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Camera_btn_Clicked(object sender, EventArgs e)
        {
            if (certify_type.SelectedItem == null)
            {
                await DisplayAlert("", "身分証を選択してください。", "はい");
            }
            else
            {
                Global.sale_certify_type = certify_type.SelectedItem.ToString();

                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                StoreCameraMediaOptions options = new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "Test",
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Rear                    
                };

                //options.Location = new Location();

                MediaFile file = await CrossMedia.Current.TakePhotoAsync(options);

                if (file == null) return;

                await Navigation.PushAsync(new EstateSaleStep6Page(file));
            }
        }
    }
}