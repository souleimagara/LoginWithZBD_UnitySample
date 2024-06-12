# ZBD Oauth Login Example

Sample scene showing a ZBD login flow for unity mobile (android/ios)

## Usage

Download and install the sample APK found in releases

Checkout the code in ```Assets/Scenes/ZBDLoginScene.unity```

The main file containing the login code is ```Assets/ZEBEDEE/Scripts/OauthLogin.cs```
 
## Create Client ID, Secret and set Redirect uri

In you ZEBEDEE Developer dashboard, under your game, you can press the oauth2 tab to generate the Client ID/Secret and set the Redirect uri. 
This sample project is using the redirect uri of the app which is ```zbdsample://authorize```

## Web View vs Native Browser

The sample project shows two flows

### WebView Flow
The first flow uses a 3rd party WebView asset for Unity.
Unity has no native webview library so a 3rd party plugin is required. However it has limitations. The main limitation is it cannot support social logins such as google login. there for for this flow the only option to login to ZEBEDEE is via the "Connect to ZBD" app button, which will open the ZBD Wallet to complete the login flow. Another limitation is the 3rd party webview sdk is required which can cause build issues and conflicts with other SDKs The benefit of this flow is it lets you show a webview inside the app which doesn't take up the whole screen area.

### Native browser flow
This flow opens the native browser of the device and continues the ZBD login flow, advantages are it requires no 3rd party SDK such as a webview, it also supports social logins. The disadvantage is it does take the user out of the game.

## Important 

All the code to perform the login is contained inside of ```OauthLogin.cs```
This is for the purpose of education.
However in production you must not include the ```client_secret``` in the client side code, and the call to get the ```access_token``` (which requires the ```client_secret```) must be called from a server.