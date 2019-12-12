using System;
using System.Collections.Generic;
using System.Text;

namespace owner.Social
{
    public class User
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string LastName { get; set; }
        public string email { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string PictureUrl { get; set; }
        public bool LoggedInWithSNSAccount { get; set; }

        public SNSProvider Provider { get; set; }
    }
}
