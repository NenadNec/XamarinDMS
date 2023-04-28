using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NecDMS.Interface;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Plugin.CurrentActivity;
using NecDMS.Droid.Services;

[assembly: Dependency(typeof(ToolbarItemBadgeService))]
namespace NecDMS.Droid.Services
{
	public class ToolbarItemBadgeService : IToolbarItemBadgeService
	{
		public void SetBadge(Page page, ToolbarItem item, string value, Color backgroundColor, Color textColor)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				var test = CrossCurrentActivity.Current.Activity;

				var toolbar = CrossCurrentActivity.Current.Activity.FindViewById(Resource.Id.toolbar) as Android.Support.V7.Widget.Toolbar;
				if (toolbar != null)
				{
					if (!string.IsNullOrEmpty(value))
					{
						var idx = page.ToolbarItems.IndexOf(item);
						if (toolbar.Menu.Size() > idx)
						{
							var menuItem = toolbar.Menu.GetItem(idx);
							BadgeDrawable.SetBadgeText(CrossCurrentActivity.Current.Activity, menuItem, value, backgroundColor.ToAndroid(), textColor.ToAndroid());
						}
					}
				}
			});
		}
	}
}