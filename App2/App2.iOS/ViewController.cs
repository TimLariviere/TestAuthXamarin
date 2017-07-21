using System;

using UIKit;
using Xamarin.Auth;

namespace App2.iOS
{
	public partial class ViewController : UIViewController
	{
        public static WebAuthenticator Auth = null;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            SetupGoogle();
            SetupFacebook();
            SetupTwitter();
        }

        private void SetupTwitter()
        {
            TwitterButton.TouchUpInside += delegate {

                Auth = new OAuth1Authenticator("6ozsrJfgvSe58Pl45MLk65PfH", "tneY3Wf4hxpFM8UpuSvw9bhy6P9vF11KUS731Suk2xmkGPUZgB",
                                                new Uri("https://api.twitter.com/oauth/request_token"),
                                                new Uri("https://api.twitter.com/oauth/authorize"),
                                                new Uri("https://api.twitter.com/oauth/access_token"),
                                                new Uri("https://mobile.twitter.com"),
                                                null, false);

                Auth.Completed += OnTwitterAuthCompleted;

                var viewController = Auth.GetUI();
                PresentViewController(viewController, true, null);
            };
        }

        private async void OnTwitterAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            this.DismissViewController(true, null);

            var twitterService = new TwitterService();
            var email = await twitterService.GetEmailAsync("6ozsrJfgvSe58Pl45MLk65PfH", "tneY3Wf4hxpFM8UpuSvw9bhy6P9vF11KUS731Suk2xmkGPUZgB", e.Account.Properties["oauth_token"], e.Account.Properties["oauth_token_secret"]);
            
            TwitterButton.SetTitle(email, UIControlState.Normal);
        }

        private void SetupFacebook()
        {
            FacebookButton.TouchUpInside += delegate {

                Auth = new OAuth2Authenticator("990519181055992", "email",
                                                new Uri("https://www.facebook.com/v2.0/dialog/oauth/"),
                                                new Uri("http://www.facebook.com/connect/login_success.html"),
                                                null, false);

                Auth.Completed += OnFacebookAuthCompleted;

                var viewController = Auth.GetUI();
                PresentViewController(viewController, true, null);
            };
        }

        private async void OnFacebookAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            this.DismissViewController(true, null);

            var facebookService = new FacebookService();
            var email = await facebookService.GetEmailAsync(e.Account.Properties["access_token"]);

            FacebookButton.SetTitle(email, UIControlState.Normal);
        }

        private void SetupGoogle()
        {
            GoogleButton.TouchUpInside += delegate {

                Auth = new OAuth2Authenticator("742705120071-hkdhvr100f26g8agacuqm54a924eblf9.apps.googleusercontent.com", "", "https://www.googleapis.com/auth/userinfo.email",
                                                new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                                                new Uri("org.amffrance.finquiz:/oauth2redirect"),
                                                new Uri("https://www.googleapis.com/oauth2/v4/token"),
                                                null, true);

                Auth.Completed += OnGoogleAuthCompleted;

                var viewController = Auth.GetUI();
                PresentViewController(viewController, true, null);
            };
        }

        private async void OnGoogleAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            this.DismissViewController(true, null);

            var googleService = new GoogleService();
            var email = await googleService.GetEmailAsync(e.Account.Properties["token_type"], e.Account.Properties["access_token"]);

            GoogleButton.SetTitle(email, UIControlState.Normal);
        }

        public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

