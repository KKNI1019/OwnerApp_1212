using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace owner
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaymentPage : ContentPage
	{
        private string product_name;
        private string product_price;
        private string product_description;
        private string productId;
        private string payload;
        private string building_name;

        public PaymentPage (string estate_name)
		{
			InitializeComponent ();

            productId = "com.xamarin.storekit.realestate";
            Random rand = new Random();
            int int_payload = rand.Next(1000, 500000);
            payload = int_payload.ToString();
            
            building_name = estate_name;

            Pay();
                
		}

        private async void Pay()
        {
            if (App.owner_bonus == "1")
            {
                await Navigation.PushAsync(new ZeroFinalPage());
            }
            else
            {
                var billing = CrossInAppBilling.Current;

                if (!CrossInAppBilling.IsSupported)
                    return;
                try
                {
                    var connected = await billing.ConnectAsync(ItemType.Subscription);

                    if (!connected)
                    {
                        //we are offline or can't connect, don't try to purchase
                        return;
                    }

                    var items = await billing.GetProductInfoAsync(ItemType.Subscription, productId);
                    foreach (var item in items)
                    {
                        //item info here.
                        productId = item.ProductId;
                        product_name = item.Name;
                        product_description = item.Description;
                        product_price = item.LocalizedPrice;
                    }

                    lbl_amount.Text = product_price + "/月";

                    int bank_fee = Convert.ToInt32(product_price.Replace("¥", "")) - App.programm_fee;
                    int bank_rate = bank_fee * 100 / App.programm_fee;
                    lbl_amount_des.Text = App.programm_fee + "円 + " + bank_rate + "円（消費税" + bank_rate + "％）";
                }

                catch (Exception ex)
                {
                    //Something else has gone wrong, log it
                    Debug.WriteLine("Issue connecting: " + ex);
                }
                finally
                {
                    await billing.DisconnectAsync();
                }
            }
            
        }

        private async void Btn_pay_Clicked(object sender, EventArgs e)
        {
            var billing = CrossInAppBilling.Current;

            try
            {
                //check purchases
                var purchase = await billing.PurchaseAsync(productId, ItemType.Subscription, payload);

                //possibility that a null came through.
                if (purchase == null)
                {
                    //did not purchase
                }
                else
                {
                    //purchased!
                    if (string.Equals(building_name, string.Empty))
                    {
                        await Navigation.PushAsync(new MyInfoUpdateCompletePage());
                    }
                    else
                    {
                        await Navigation.PushAsync(new ZeroFinalPage());
                    }
                }
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                var message = string.Empty;
                switch (purchaseEx.PurchaseError)
                {
                    case PurchaseError.AppStoreUnavailable:
                        message = "現在、アプリストアは利用できないようです。 あとでもう一度試してみてください。";
                        break;
                    case PurchaseError.BillingUnavailable:
                        message = "請求は利用できないようです。しばらくしてからもう一度お試しください。";
                        break;
                    case PurchaseError.PaymentInvalid:
                        message = "お支払いが無効のようです。もう一度お試しください。";
                        break;
                    case PurchaseError.PaymentNotAllowed:
                        message = "支払いが有効/許可されていないようです。もう一度お試しください。";
                        break;
                    case PurchaseError.ServiceUnavailable:
                        message = "サービスを利用することができません。もう一度お試しください。";
                        break;
                }

                //Decide if it is an error we care about
                if (string.IsNullOrWhiteSpace(message))
                {
                    await DisplayAlert("支払い", "請求は利用できないようです。しばらくしてからもう一度お試しください。", "はい");
                    return;
                }
                else
                {
                    await DisplayAlert("支払い", message, "はい");
                }
                    
            }
            finally
            {
                await billing.DisconnectAsync();
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void lbl_restore_tap(object sender, EventArgs e)
        {
            var billing = CrossInAppBilling.Current;
            if (!CrossInAppBilling.IsSupported)
                return;

            try
            {
                var purchases = await billing.GetPurchasesAsync(ItemType.Subscription);
                if (purchases?.Any(p => p.ProductId == productId) ?? false)
                {
                    //Purchase restored
                    if (string.Equals(building_name, string.Empty))
                    {
                        await Navigation.PushAsync(new MyInfoUpdateCompletePage());
                    }
                    else
                    {
                        await Navigation.PushAsync(new ZeroFinalPage());
                    }
                }
                else
                {
                    //no purchases found
                    await DisplayAlert("購入復元", "購入を復元することができません。", "はい");
                }
            }
            catch {
                await DisplayAlert("購入復元", "購入を復元することができません。", "はい");
            }
        }
    }
}