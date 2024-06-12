using System;
using System.Collections;
using System.Threading.Tasks;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Vuplex.WebView;
using Beamable.Api.Autogenerated.Models;
using Beamable.Common.Leaderboards;
using Beamable;
using Beamable.Server.Clients;
using static Data;
using Newtonsoft.Json;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class OauthLogin : MonoBehaviour
{
   
    string lastConnectionString;
    public Text responseText;



    
    bool didLeaveApp;

    public Text lightningAddressField;
    public Text gamertagField;
    public Text accessTokenField;
    public Text refreshTokenField;
    public Text accessTokenexpiresInField;
    public Text refreshTokenExpiresInField;
    public Text responseField;

    public GameObject logoutButton;
    public GameObject loginButton;


    public bool testTokenExp;
  
    private void Awake()
    {
       
        // We can use this function to get uri parameters
        Application.deepLinkActivated += onDeepLinkActivated;
        Debug.Log("entered awake ");
        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            Debug.Log("The absoluteURL is " +(Application.absoluteURL));
            // Cold start and Application.absoluteURL not null so process Deep Link.
            onDeepLinkActivated(Application.absoluteURL);
        }

    }


    private async void Start()
    { 
        
        // Check if the game has been opened before
        if (PlayerPrefs.GetInt("GameOpenedBefore", 0) == 1)
        {
            CheckLoginStatus();  // Only check login status if it's not the first time opening
        }
        else
        {
            // Set the flag to indicate the game has been opened before
            PlayerPrefs.SetInt("GameOpenedBefore", 1);
            PlayerPrefs.Save();
        }
    }


  


    // Loing via the native browser, advantages it supports all login function and shares cookies allowing faster login to socials as passwords/usernames are likley remembered, disadvantage is it leaves the app to open the flow in the OS browser.
    public void LoginViaBrowser()
    {

        StartLogin();
    }
    async void CheckLoginStatus()
    {
        responseText.text = "Please wait. Checking login status...";
        var ctx = BeamContext.Default;
        await ctx.OnReady;
        Debug.Log("Beamable id is : "+ctx.PlayerId);
        var resultJson = await ctx.Microservices().LoginServer().CheckTokenStatus(ctx.PlayerId , testTokenExp );
        Debug.Log("Check Status : " +resultJson);
        try
        {
            var result = JsonConvert.DeserializeObject<LoginResult>(resultJson);

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                Debug.Log("Check Status Error : " + result.ErrorMessage);
                responseText.text = result.ErrorMessage;
              //  ShowLoginButton(); // Show the login button if there is an error
                return;
            }

            if (result.IsLoggedIn)
            {
                if (result.UserData != null && result.UserData.Success)
                {
                    Debug.Log("Check Status showing data : " + result.UserData.Data.LightningAddress + " user data " + result.UserData.Data.Gamertag);
                    Debug.Log("Refresh token : " + result.RefreshToken + " Access token : " + result.AccessToken);
                    DisplayUserData(result);
                    ShowLogoutButton(); // Show the logout button if logged in
                }
                else
                {
                    responseText.text = "Failed to load user data.";
                    ShowLoginButton();
                }
            }
            else
            {
               
                    responseText.text = "Please log in to continue.";
              
                   ShowLoginButton();
            }
        }
        catch (Exception ex)
        {
            responseText.text = "Failed to process login status: " + ex.Message;
            Debug.LogError("JSON parse error or unexpected response format: " + ex.Message);
             ShowLoginButton();
        }
       
    }
    void ShowLoginButton()
    {
        loginButton.gameObject.SetActive(true);
        logoutButton.gameObject.SetActive(false);
    }

    void ShowLogoutButton()
    {
        logoutButton.gameObject.SetActive(true);
        loginButton.gameObject.SetActive(false);
    }
    void DisplayUserData(LoginResult result)
    {
        lightningAddressField.text = result.UserData.Data.LightningAddress;
        gamertagField.text = result.UserData.Data.Gamertag;
        accessTokenField.text = result.AccessToken;
        refreshTokenField.text = result.RefreshToken;
        accessTokenexpiresInField.text = result.ExpiresIn.ToString() + " seconds";
        refreshTokenExpiresInField.text = result.RefreshTokenExpiresIn.ToString() + " seconds";
        responseField.text = "Login successful!";
    }
    public async void StartLogin()
    {
        responseText.text = "Please wait...";
        var ctx = BeamContext.Default;
        await ctx.OnReady;

        try
        {
            var loginResultJson = await ctx.Microservices().LoginServer().GenerateOauthUrl();
            Debug.Log("Received JSON: " + loginResultJson);

            var oauthresult = JsonConvert.DeserializeObject<OauthData>(loginResultJson);
            Debug.Log("The login result is: " + (oauthresult?.Url ?? "null") + " and message: " +
                      (oauthresult?.ErrorMessage ?? "null"));

            if (oauthresult == null)
            {
                responseText.text = "Failed to get login result. Please try again later.";
                return;
            }

            if (!string.IsNullOrEmpty(oauthresult.ErrorMessage))
            {
                responseText.text = oauthresult.ErrorMessage;
            }
            else
            {
               
                  
                    Application.OpenURL(oauthresult.Url);
               
               
               
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            responseText.text = e.ToString();
        }
    }
   
    // ZEBEDEE will redirect to the app with the code and state to continue login
    async Task ContinueLogin(string url)
    {
        try {
        var ctx = BeamContext.Default;
        await ctx.OnReady;
        var resultJson = await ctx.Microservices().LoginServer().ContinueLogin(url, ctx.PlayerId);
        var result = JsonConvert.DeserializeObject<LoginResult>(resultJson);
        if (!string.IsNullOrEmpty(result.ErrorMessage))
        {
            Debug.LogError(result.ErrorMessage);
            responseText.text = result.ErrorMessage;
          
            return;
        }
        Debug.Log(("User data is : ") +result.UserData.Data);
        Debug.Log(("User data success is : ") +result.UserData.Success);
        if (result.UserData != null && result.UserData.Success)
        {
           
            lightningAddressField.text = result.UserData.Data.LightningAddress;
            gamertagField.text = result.UserData.Data.Gamertag;
            accessTokenField.text = result.AccessToken;
            refreshTokenField.text = result.RefreshToken;
            accessTokenexpiresInField.text = result.ExpiresIn.ToString() + " seconds";
            refreshTokenExpiresInField.text = result.RefreshTokenExpiresIn.ToString() + " seconds";

            responseField.text = "Login successful!";
           ShowLogoutButton();
        }
        else
        {
            responseField.text = "Failed to load user data. "  +result.UserData.Data;           
            ShowLoginButton();
        }

       
        }
        catch (Exception ex)
        {
            responseText.text = "An error occurred in ContinueLogin: test " + ex.Message;
            Debug.LogError("An error occurred in ContinueLogin: " + ex.Message);
           
        }
    }


 

   
    private void OnApplicationPause(bool pause)
    {
        didLeaveApp = pause;
    }

    void CheckDidLeaveApp()
    {
        if (!didLeaveApp)
        {
            Application.OpenURL("https://zbd.gg");
        }

    }

   

    //On ios we can get the redirect info here
    private  void onDeepLinkActivated(string url)
    {
        Debug.Log("deep link url " + url);
         ContinueLogin(url);
        

    }

    
    // New Logout method
    public async void Logout()
    {
        responseText.text = "Logging out...";
        var ctx = BeamContext.Default;
        await ctx.OnReady;

        try
        {
            var resultJson = await ctx.Microservices().LoginServer().Logout(ctx.PlayerId);
            var result = JsonConvert.DeserializeObject<LogoutResponse>(resultJson);

            if (result.Success)
            {
                responseText.text = result.Message;
                Debug.Log("Logout successful.");
                ShowLoginButton(); // Show the login button after logging out
            }
            else
            {
                responseText.text = result.Message;
                Debug.LogError("Logout failed: " + result.Message);
            }
        }
        catch (Exception ex)
        {
            responseText.text = "An error occurred during logout: " + ex.Message;
            Debug.LogError("An error occurred during logout: " + ex.Message);
        }
    }

}
