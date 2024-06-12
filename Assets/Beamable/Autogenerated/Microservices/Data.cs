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

    public class LoginResult
    {
        public string Url { get; set; }
        public string ErrorMessage { get; set; }
    }
}
