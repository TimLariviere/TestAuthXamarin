using Android.App;
using Android.OS;
using Android.Widget;
using System;
using Xamarin.Auth;

namespace App2.Droid
{
    [Activity (Label = "App2.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

        public static OAuth2Authenticator Auth2;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {


                Auth2 = new OAuth2Authenticator("742705120071-bep44ot1uchpo1mlhhcr242kh84ehgb1.apps.googleusercontent.com", "", "https://www.googleapis.com/auth/userinfo.email",
                                                new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                                                new Uri("org.amffrance.finquiz:/oauth2redirect"),
                                                new Uri("https://www.googleapis.com/oauth2/v4/token"),
                                                null, true);

                Auth2.Completed += Auth2_Completed;

                var intent = Auth2.GetUI(this);
                StartActivity(intent);

			};
		}

        private async void Auth2_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            var googleService = new GoogleService();
            var email = await googleService.GetEmailAsync(e.Account.Properties["token_type"], e.Account.Properties["access_token"]);

            Button button = FindViewById<Button>(Resource.Id.myButton);
            button.Text = email;
        }
    }
}


