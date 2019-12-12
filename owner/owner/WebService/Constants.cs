using System;
using System.Collections.Generic;
using System.Text;

namespace owner.WebService
{
    public class Constants
    {
        // social login info
        public const string FB_CLIENT_ID = "513256342767131";
        public const string FB_CLIENT_SECRET = "2da1103b0809d1d7047d7ce8aaec976b";
        public const string FB_AUTHORIZATION_URI = "https://www.facebook.com/dialog/oauth";
        public const string FB_REQUEST_TOKEN_URI = "https://graph.facebook.com/oauth/access_token";
        public const string FB_REDIRECT_URI = "https://www.facebook.com/connect/login_success.html";
        public const string FB_USERINFO_URI = "https://graph.facebook.com/me";

        public const string TW_CONSUMER_KEY = "P4a4J57m7UEIGKh4p6LiFsmZa";
        public const string TW_CONSUMER_SECRET = "pAQRsnP0FFHDT6KVwvKxSPAVi9V2uCUnUGolgupB9QD1EMNbOj";
        public const string TW_REQUEST_TOKEN_URI = "https://api.twitter.com/oauth/request_token";
        public const string TW_AUTHORIZATION_URI = "https://api.twitter.com/oauth/authorize";
        public const string TW_ACCESS_TOKEN_URI = "https://api.twitter.com/oauth/access_token";
        public const string TW_CALLBACK_URI = "https://www.facebook.com/connect/login_success.html";
        public const string TW_OAUTH_URI = "https://api.twitter.com/1.1/account/verify_credentials.json";

        public const string LI_CLIENT_ID = "1619610726";
        public const string LI_CLIENT_SECRET = "db328e1363eb4d3179951cba8cd8c31b";
        public const string LI_AUTHORIZATION_URI = "https://access.line.me/oauth2/v2.1/authorize";
        public const string LI_REQUEST_TOKEN_URI = "https://api.line.me/oauth2/v2.1/token";
        public const string LI_REDIRECT_URI = "https://www.facebook.com/connect/login_success.html";
        public const string LI_USERINFO_URI = "https://api.line.me/v2/profile";

        // server http client info
        public const string SERVER_BASE_URL = "http://133.18.218.237:5000/real_estate_management/api/owner/";
        public const string ENDPOINT_URL = "http://133.18.218.237:5000/real_estate_management/";
        public const string IMAGE_UPLOAD_URL_PREFIX = "http://133.18.218.237:5000/uploads/profile/owner/";
        public const string IMAGE_UPLOAD_URL_PREFIX_TENANT = "http://133.18.218.237:5000/uploads/image/tenant/";
        public const string ESTATE_IMAGE_URL_PREFIX = "http://133.18.218.237:5000/uploads/image/estate/";
        public const string AGREEMENT_IMAGE_URL_PREFIX = "http://133.18.218.237:5000/uploads/image/agreement/";
        public const string LICENCE_IMAGE_URL_PREFIX = "http://133.18.218.237:5000/uploads/image/owner/";
        public const string SERVER_SOCIAL_LOGIN_URL = SERVER_BASE_URL + "social";
        public const string SERVER_OWNER_LOGIN_URL = SERVER_BASE_URL + "login";
        public const string SERVER_OWNER_REGIST_URL = SERVER_BASE_URL + "regist";
        public const string SERVER_ZERO_REGIST_URL = SERVER_BASE_URL + "regist_zero_info";
        public const string SERVER_GET_NOTICE_URL = SERVER_BASE_URL + "get_notice";
        public const string SERVER_GET_THREAD_LIST_URL = SERVER_BASE_URL + "get_thread_list";
        public const string SERVER_GET_THREAD_DETAIL_URL = SERVER_BASE_URL + "get_thread_detail";
        public const string SERVER_SEND_THREAD_COMMENT_URL = SERVER_BASE_URL + "send_thread_comment";
        public const string SERVER_GET_COMMENT_LIST_URL = SERVER_BASE_URL + "get_comment_list";
        public const string SERVER_OWNER_PROFILE_UPDATE_URL = SERVER_BASE_URL + "update";
        public const string SERVER_OWNER_PROFILEIMG_UPLOAD_URL = SERVER_BASE_URL + "upload_profile";
        public const string SERVER_PAY_CHECK_URL = SERVER_BASE_URL + "pay_check_info";
        public const string SERVER_REGIST_SALE_URL = SERVER_BASE_URL + "regist_sale_info";
        public const string SERVER_GET_NEWS_URL = SERVER_BASE_URL + "get_news_info";
        public const string SERVER_REGIST_COMMENT_URL = SERVER_BASE_URL + "regist_news_comment";
        public const string SERVER_LIKE_COMMENT_URL = SERVER_BASE_URL + "like_comment";
        public const string SERVER_DISLIKE_COMMENT_URL = SERVER_BASE_URL + "dislike_comment";
        public const string SERVER_VIDEO_URL = SERVER_BASE_URL + "get_video_info";
        public const string SERVER_GET_SALE_REQUEST_URL = SERVER_BASE_URL + "get_sale_request";

        public const string ESTATE_IMAGE_URL = "estate_image_url";
        public const string PROGRAM_FEE = "program_fee";
        public const string ESTATE_NEAR_STATTION = "estate_near_station";
        public const string ESTATE_WALKING_TIME = "estate_walking_time";
        public const string OTHER_FEE = "estate_other_fee";
        public const string LOAN_REPAY = "estate_loan_repay";
        public const string LOAN_AMOUNT = "estate_loan_amount";
        public const string YEARLY_PROFIT = "estate_yearly_profit";
        public const string REPAY_PERIOD = "estate_repay_period";

        public const string ALLOW_PUSH = "allow_push";

        public const string OWNER_EMAIL = "owner_email";
        public const string OWNER_NAME = "owner_name";
        public const string OWNER_NICKNAME = "owner_nickname";
        public const string OWNER_ADRESS = "owner_address";
        public const string OWNER_POSTAL_ADRESS = "owner_postal_address";
        public const string OWNER_PHONE1 = "owner_phone1";
        public const string OWNER_PASSWORD = "owner_password";
        public const string OWNER_REGIST_FROM = "owner_regist_from";
        public const string OWNER_ID = "owner_id";
        public const string REGISTERED = "registered";

        public const string ZERO_COMPANY = "zero_company";
        public const string ZERO_AGENCY_NAME = "zero_agency_name";
        public const string ZERO_AGENCY_PHONE = "zero_agency_phone";
        public const string ZERO_END_DATE = "zero_end_date";
        public const string ZERO_TYPE = "zero_type";

        public const string ESTATE_NAME = "estate_name";
        public const string ESTATE_ADDRESS = "estate_address";
        public const string ESTATE_ROOM_NUMBER = "estate_room_number";

        public const string THREAD_ID = "thread_id";
        public const string THREAD_COMMENT_CONTENTS = "thread_comment_contents";
        public const string THREAD_COMMENT_WRITER_ID = "thread_comment_writer_id";
        public const string THREAD_COMMENT_WRITER_NICKNAME = "thread_comment_writer_nickname";
        public const string THREAD_COMMENT_CATEGORY = "thread_comment_category";

        public const string LAST_NOTICE_ID = "last_notice_id";
        public const string LAST_THREAD_ID = "last_thread_id";
        public const string LAST_THREAD_COMMENT_ID = "last_thread_comment_id";
        public const string LAST_COMMENT_ID = "last_comment_id";

        public const string NETWORK_ERROR = "サーバー接続でエラーが発生しました。";

        public const string RENTAL_INCOME = "estate_rent";
        public const string ADMIN_EXPENSES = "estate_admin_expense";
        public const string REPAIR_RESERVE = "estate_repair_reserve";
        public const string AGENCY_FEE = "estate_agency_fee";
        public const string SALE_PRICE_ZONE = "sale_price_zone";
        public const string estate_property_tax = "estate_property_tax";

        public const string DEVICE_TOKEN = "device_token";

        public const string PAID_MEMBER = "PAID_MEMBER";

        public const string NEWS_ID = "news_id";
        public const string LAST_NEWS_ID = "last_news_id";
        public const string FAVORITE_NEWS_IDS = "favorite_news_ids";
    }
}
