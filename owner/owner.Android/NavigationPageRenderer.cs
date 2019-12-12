using Android.App;
using Android.Content;
using NavBarBackImage.Droid.CustomRenderers;
using owner;
using owner.Droid;
using System.Threading.Tasks;
using Xamarin.Forms;

using AppCompToolbar = Android.Support.V7.Widget.Toolbar;
// ...
[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(NavigationPageRenderer))]

namespace NavBarBackImage.Droid.CustomRenderers
{
    public class NavigationPageRenderer : Xamarin.Forms.Platform.Android.AppCompat.NavigationPageRenderer
    {
        public AppCompToolbar toolbar;
        public Activity context;

        public NavigationPageRenderer(Context context) : base(context)
        {
        }

        protected override Task<bool> OnPushAsync(Page view, bool animated)
        {
            var retVal = base.OnPushAsync(view, animated);

            context = (Activity)Forms.Context;
            toolbar = context.FindViewById<AppCompToolbar>(Resource.Id.toolbar);

            if (toolbar != null)
            {
                if (toolbar.NavigationIcon != null)
                {
                    toolbar.NavigationIcon = Android.Support.V7.Content.Res.AppCompatResources.GetDrawable(context, Resource.Drawable.back1);
                    toolbar.Title = "";
                    
                }
            }

            return retVal;
        }
    }
}
