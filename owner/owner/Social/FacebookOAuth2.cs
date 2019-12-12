using Newtonsoft.Json;
using owner.WebService;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace owner.Social
{
    public class FacebookOAuth2 : OAuth2Base
    {
        private static readonly Lazy<FacebookOAuth2> lazy = new Lazy<FacebookOAuth2>(() => new FacebookOAuth2());

        public static FacebookOAuth2 Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private FacebookOAuth2()
        {
            Initialize();
        }

        void Initialize()
        {
            ProviderName = "Facebook";
            Description = "Facebook Login Provider";
            Provider = SNSProvider.Facebook;
            ClientId = Constants.FB_CLIENT_ID;
            ClientSecret = Constants.FB_CLIENT_SECRET;
            Scope = "email";
            AuthorizationUri = new Uri(Constants.FB_AUTHORIZATION_URI);
            RequestTokenUri = new Uri(Constants.FB_REQUEST_TOKEN_URI);
            RedirectUri = new Uri(Constants.FB_REDIRECT_URI);
            UserInfoUri = new Uri(Constants.FB_USERINFO_URI);
        }

        public override async Task<User> GetUserInfoAsync(Account account)
        {
            User user = null;
            string token = account.Properties["access_token"];
            int expriesIn;
            int.TryParse(account.Properties["expires_in"], out expriesIn);


            Dictionary<string, string> dictionary = new Dictionary<string, string> { { "fields", "name,email,picture,first_name,last_name" } };
            var request = new OAuth2Request("GET", UserInfoUri, dictionary, account);
            var response = await request.GetResponseAsync();
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                string userJson = await response.GetResponseTextAsync();
                var facebookUser = JsonConvert.DeserializeObject<FacebookUser>(userJson);
                user = new User
                {
                    Id = facebookUser.Id,
                    Token = token,
                    RefreshToken = null,
                    Name = facebookUser.Name,
                    email = facebookUser.email,
                    ExpiresIn = DateTime.UtcNow.Add(new TimeSpan(expriesIn)),
                    PictureUrl = facebookUser.Picture.Data.Url,
                    Provider = SNSProvider.Facebook,
                    LoggedInWithSNSAccount = true,
                };
            }

            return user;
        }

        public override Task<(bool IsRefresh, User User)> RefreshTokenAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
