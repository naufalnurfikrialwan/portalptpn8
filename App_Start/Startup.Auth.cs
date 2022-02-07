using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Ptpn8.UserManagement.Models;
using Owin.Security.Providers.Yahoo;

namespace Ptpn8
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            app.UseFacebookAuthentication(
               appId: "242036819488735",
               appSecret: "55a00f4414c5d1f6c9cbc5827823b51a");

            var googleOAuth2AuthOptions =
            new GoogleOAuth2AuthenticationOptions
            {
                ClientId = @"974824051238-ad1hbiet13p77cccr60d22uiir62136r.apps.googleusercontent.com",
                ClientSecret = @"64KKvKIqtIWNX8zTTC25lRCe",
            };
            googleOAuth2AuthOptions.Scope.Add(@"openid");
            googleOAuth2AuthOptions.Scope.Add(@"profile");
            googleOAuth2AuthOptions.Scope.Add(@"email");

            app.UseGoogleAuthentication(googleOAuth2AuthOptions);

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "974824051238-ad1hbiet13p77cccr60d22uiir62136r.apps.googleusercontent.com",
            //    ClientSecret = "64KKvKIqtIWNX8zTTC25lRCe"
            //});


            //var yahoo = new YahooAuthenticationOptions()
            //{
            //    //SignInAsAuthenticationType = signInAsType,
            //    AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Passive,
            //    BackchannelTimeout = TimeSpan.FromSeconds(60),
            //    Caption = "Yahoo",
            //    AuthenticationType = "Yahoo",
            //    ConsumerKey = "dj0yJmk9Z3NnR2trVHdyaGZiJmQ9WVdrOVpHeFdaR2RMTjJFbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD1kNg--",
            //    ConsumerSecret = "5e1d0f08858a31ca7d7a69c9c111f35129761c88",
            //};
            //app.UseYahooAuthentication(yahoo);
            app.UseYahooAuthentication(
                "dj0yJmk9Z3NnR2trVHdyaGZiJmQ9WVdrOVpHeFdaR2RMTjJFbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD1kNg--",
                "5e1d0f08858a31ca7d7a69c9c111f35129761c88");
        }
    }
}