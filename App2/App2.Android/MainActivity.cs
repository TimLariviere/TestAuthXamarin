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
        public static OAuth2Authenticator Auth2;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            SetupGoogle();
            SetupFacebook();
		}

        private void SetupFacebook()
        {
            var facebookButton = FindViewById<Button>(Resource.Id.facebookButton);

            facebookButton.Click += delegate {

                Auth2 = new OAuth2Authenticator("990519181055992", "email",
                                                new Uri("https://www.facebook.com/v2.0/dialog/oauth/"),
                                                new Uri("http://www.facebook.com/connect/login_success.html"),
                                                null, false);

                Auth2.Completed += OnFacebookAuthCompleted;

                var intent = Auth2.GetUI(this);
                StartActivity(intent);

            };
        }

        private async void OnFacebookAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var facebookService = new FacebookService();
            var email = await facebookService.GetEmailAsync(e.Account.Properties["access_token"]);

            var facebookButton = FindViewById<Button>(Resource.Id.facebookButton);
            facebookButton.Text = email;
        }

        private void SetupGoogle()
        {
            var googleButton = FindViewById<Button>(Resource.Id.googleButton);

            googleButton.Click += delegate {

                Auth2 = new OAuth2Authenticator("742705120071-bep44ot1uchpo1mlhhcr242kh84ehgb1.apps.googleusercontent.com", "", "https://www.googleapis.com/auth/userinfo.email",
                                                new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                                                new Uri("org.amffrance.finquiz:/oauth2redirect"),
                                                new Uri("https://www.googleapis.com/oauth2/v4/token"),
                                                null, true);

                Auth2.Completed += OnGoogleAuthCompleted;

                var intent = Auth2.GetUI(this);
                StartActivity(intent);

            };
        }

        private async void OnGoogleAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var googleService = new GoogleService();
            var email = await googleService.GetEmailAsync(e.Account.Properties["token_type"], e.Account.Properties["access_token"]);

            var googleButton = FindViewById<Button>(Resource.Id.googleButton);
            googleButton.Text = email;
        }
    }
}


