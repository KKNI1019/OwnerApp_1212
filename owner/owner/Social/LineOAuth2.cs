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
    public class LineOAuth2 : OAuth2Base
    {
        private static readonly Lazy<LineOAuth2> lazy = new Lazy<LineOAuth2>(() => new LineOAuth2());

        public static LineOAuth2 Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private LineOAuth2()
        {
            Initialize();
        }

        void Initialize()
        {
            ProviderName = "Line";
            Description = "Line Login Provider";
            Provider = SNSProvider.Line;
            ClientId = Constants.LI_CLIENT_ID;
            ClientSecret = Constants.LI_CLIENT_SECRET;
            Scope = "openid profile email";
            AuthorizationUri = new Uri(Constants.LI_AUTHORIZATION_URI);
            RequestTokenUri = new Uri(Constants.LI_REQUEST_TOKEN_URI);
            RedirectUri = new Uri(Constants.LI_REDIRECT_URI);
            UserInfoUri = new Uri(Constants.LI_USERINFO_URI);
        }

        public override async Task<User> GetUserInfoAsync(Account account)
        {
            User user = null;
            string token = account.Properties["access_token"];
            string refreshToke = account.Properties["refresh_token"];
            int expriesIn;
            int.TryParse(account.Properties["expires_in"], out expriesIn);

            Dictionary<string, string> dictionary = new Dictionary<string, string> { { "Authorization", token + ", name, email" } };
            var request = new OAuth2Request("POST", UserInfoUri, dictionary, account);
            var response = await request.GetResponseAsync();
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                string userJson = await response.GetResponseTextAsync();
                var lineUser = JsonConvert.DeserializeObject<LineUser>(userJson);

                user = new User
                {
                    Id = lineUser.Id,
                    Token = token,
                    RefreshToken = refreshToke,
                    Name = lineUser.Name,
                    ExpiresIn = DateTime.UtcNow.Add(new TimeSpan(expriesIn)),
                    PictureUrl = lineUser.ProfileImage,
                    Provider = SNSProvider.Line,
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
