using Newtonsoft.Json;
using owner.Model;
using owner.WebService;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NoticeSaleDetail : ContentPage
	{
        private string selected_notice_id;
        private string selected_other_id;
        public IList<SaleRequestData> SaleRequests { get; set; }
        private string imagesource;

        public NoticeSaleDetail (string notice_id, string other_id)
		{
			InitializeComponent ();

            selected_notice_id = notice_id;
            selected_other_id = other_id;

            SaleRequests = new List<SaleRequestData>();

            getSaleRequestData();
            
		}

        private async void getSaleRequestData()
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("sale_id", selected_other_id)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_GET_SALE_REQUEST_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);
                        
                        var sale_request_num = resultData.sale_request_data.Length;
                        if (sale_request_num > 0)
                        {
                            for (int i = 0; i < sale_request_num; i++)
                            {
                                if (i == 0)
                                {
                                    imagesource = "img_gold_star.png";
                                }
                                else if (i == 1)
                                {
                                    imagesource = "img_silver_star.png";
                                }
                                else if (i == 2)
                                {
                                    imagesource = "img_bronze_star.png";
                                }
                                else
                                {
                                    imagesource = null;
                                }

                                SaleRequests.Add(new SaleRequestData
                                {
                                    ranking = (i + 1).ToString(),
                                    imgsource = imagesource,
                                    request_user_name = resultData.sale_request_data[i].request_user_name,
                                    request_price = resultData.sale_request_data[i].request_price,
                                    sale_request_id = resultData.sale_request_data[i].sale_request_id
                                });
                            }
                        }

                        listview.ItemsSource = SaleRequests;
                    }
                    else
                    {
                        await DisplayAlert("", resultMsg.resp, "はい");
                    }
                }
                catch
                {
                    await DisplayAlert("", Constants.NETWORK_ERROR, "はい");
                }

            }
        }

        private void ImgMenu_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void btn_sell_tap(object sender, EventArgs e)
        {
            Image btn_agree = (Image)sender;
            var item = (TapGestureRecognizer)btn_agree.GestureRecognizers[0];
            string sale_request_id = item.CommandParameter.ToString();

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("sale_request_id", sale_request_id)
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_BASE_URL + "regist_sale_request", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        await Navigation.PushAsync(new NoticeSaleFinal());
                    }
                    else
                    {
                        await DisplayAlert("", resultMsg.resp, "はい");
                    }
                }
                catch
                {
                    await DisplayAlert("", Constants.NETWORK_ERROR, "はい");
                }

            }
        }
    }
}