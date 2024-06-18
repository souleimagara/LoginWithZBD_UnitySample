    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Data
    {
        class AccessTokenRequest
        {
            public string code;
            public string client_secret;
            public string client_id;
            public string code_verifier;
            public string grant_type;
            public string redirect_uri;
        }
        public class ActionResponse
        {
            public bool error;
            public string response;
            public string type;
            public string data;
            public long responseCode;
        }
        public class OauthData
        {
            public string Url { get; set; }
            public string ErrorMessage { get; set; }
        }
        public class LoginResult
        {
            public string ErrorMessage { get; set; }
            public bool IsLoggedIn { get; set; }  
            public bool NeedsRefresh { get; set; }  
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
            public int ExpiresIn { get; set; }  
            public int RefreshTokenExpiresIn { get; set; }  
            public UserDataResponse UserData { get; set; }  
        }

        public class UserDataResponse
        {
            public bool Success { get; set; }
            public UserData Data { get; set; }
        }

        public class UserData
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string Gamertag { get; set; }
            public string Image { get; set; }
            public bool IsVerified { get; set; }
            public string LightningAddress { get; set; }
            public string PublicBio { get; set; }
            public string PublicStaticCharge { get; set; }
        }
        
        public class LogoutResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }
    } 
