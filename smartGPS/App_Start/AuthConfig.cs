using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;

namespace smartGPS
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: "n2RwV6Yn8k6MOjF7ehDjuw",
                consumerSecret: "hCP8AbV9o8prhX7RiZ52sh0PHxi5SNlOVeWf9DEOA");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "526242434133615",
                appSecret: "00c9a5e5a3e8b0f70e7da472685e9f54");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
