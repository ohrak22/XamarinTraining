using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Hello_Android_Multiscreen
{
	[Activity(Label = "@string/callHistory")]
	public class CallHistoryActivity : ListActivity
	{
		static readonly List<string> phoneNumbers = new List<string>();

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here.
			var phoneNumbers = Intent.Extras.GetStringArrayList("phone_numbers") ?? new string[0];
			this.ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, phoneNumbers);
			
			//callDialog.SetNeutralButton("Call", delegate
			//{
			//	// add dialed number to list of called numbers.
			//	phoneNumbers.Add(translatedNumber);
			//	// enable the Call History button
			//	callHistoryButton.Enabled = true;
			//	// Create intent to dial phone
			//	var callIntent = new Intent(Intent.ActionDial);
			//	callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
			//	StartActivity(callIntent);
			//});
		}
	}
}