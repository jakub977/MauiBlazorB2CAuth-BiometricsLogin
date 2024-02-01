
using Android.App;
using Android.Content;
using Microsoft.Identity.Client;

namespace MsalAuthInMaui.Platforms.Android
{
	[Activity(Exported = true)]
	[IntentFilter(new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
		DataHost = "auth",
		DataScheme = "msal20ec9298-ff08-466a-a2e4-5abf8f3a0ec1")]
	public class MsalActivity : BrowserTabActivity
	{
	}
}