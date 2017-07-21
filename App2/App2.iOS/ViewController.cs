using System;

using UIKit;
using Xamarin.Auth;

namespace App2.iOS
{
	public partial class ViewController : UIViewController
	{
        public static OAuth2Authenticator Auth2 = null;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Button.TouchUpInside += delegate {
                
                Auth2 = new OAuth2Authenticator("742705120071-hkdhvr100f26g8agacuqm54a924eblf9.apps.googleusercontent.com", "", "https://www.googleapis.com/auth/userinfo.email",
                                                new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                                                new Uri("org.amffrance.finquiz:/oauth2redirect"),
                                                new Uri("https://www.googleapis.com/oauth2/v4/token"),
                                                null, true);

                Auth2.Completed += Auth2_Completed;

                var viewController = Auth2.GetUI();
                PresentViewController(viewController, true, null);
            };
        }

        private async void Auth2_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            this.DismissViewController(true, null);

            var googleService = new GoogleService();
            var email = await googleService.GetEmailAsync(e.Account.Properties["token_type"], e.Account.Properties["access_token"]);

            Button.SetTitle(email, UIControlState.Normal);
        }

        public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

