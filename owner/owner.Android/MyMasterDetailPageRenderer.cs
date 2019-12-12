using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Support.V4.App;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using owner.Droid;
using Android.Content;

[assembly: ExportRenderer(typeof(MasterDetailPage), typeof(MyMasterDetailPageRenderer))]

namespace owner.Droid
{
    public class MyMasterDetailPageRenderer : MasterDetailPageRenderer
    {
        public MyMasterDetailPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(VisualElement oldElement, VisualElement newElement)
        {
            base.OnElementChanged(oldElement, newElement);

            var fieldInfo = GetType().BaseType.GetField("_masterLayout", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var _masterLayout = (ViewGroup)fieldInfo.GetValue(this);
            var lp = new DrawerLayout.LayoutParams(_masterLayout.LayoutParameters);
            lp.Gravity = (int)GravityFlags.Right;
            _masterLayout.LayoutParameters = lp;
        }
    }
}