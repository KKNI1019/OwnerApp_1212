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
        public PaymentPage ()
		{
			InitializeComponent ();

            productId = "com.xamarin.storekit.estate";
            payload = "devId";
            Pay();
		}

        private async void Pay()
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

                btn_pay.Text = product_price + "円/月";
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
                }

                //var purchases = await billing.GetPurchasesAsync(ItemType.Subscription);
                //if (purchases?.Any(p => p.ProductId == productId) ?? false)
                //{
                //    //Purchase restored
                //    return;
                //}
                //else
                //{
                //    //no purchases found
                //    return;
                //}
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                var message = string.Empty;
                switch (purchaseEx.PurchaseError)
                {
                    case PurchaseError.AppStoreUnavailable:
                        message = "Currently the app store seems to be unavailble. Try again later.";
                        break;
                    case PurchaseError.BillingUnavailable:
                        message = "Billing seems to be unavailable, please try again later.";
                        break;
                    case PurchaseError.PaymentInvalid:
                        message = "Payment seems to be invalid, please try again.";
                        break;
                    case PurchaseError.PaymentNotAllowed:
                        message = "Payment does not seem to be enabled/allowed, please try again.";
                        break;
                }

                //Decide if it is an error we care about
                if (string.IsNullOrWhiteSpace(message))
                    return;

                //Display message to user
            }
            finally
            {
                await billing.DisconnectAsync();
            }
        }
    }
}