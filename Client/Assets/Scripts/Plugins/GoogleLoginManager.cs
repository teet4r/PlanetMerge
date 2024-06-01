using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Behaviour;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;

public class GoogleLoginManager : SingletonBehaviour<GoogleLoginManager>
{
    public static FirebaseUser User => _auth.CurrentUser;

    private static string webClientId = "198148377120-u6lhqe4oof52slibvorgsbu8kv2atqcr.apps.googleusercontent.com";
    private static FirebaseAuth _auth;
    private static GoogleSignInConfiguration _configuration;
    private static string _infoText;

    protected override void Awake()
    {
        base.Awake();

        _configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestEmail = true,
            RequestIdToken = true
        };

        _CheckFirebaseDependencies();
    }

    private static void _CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                    _auth = FirebaseAuth.DefaultInstance;
                //else
                //    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                //AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    public static async UniTask<bool> SignInWithGoogle()
    {
        var loading = UIManager.Show<UILoadingPopup>();

        var success = await _OnSignIn();

        loading.Hide();

        return success;
    }
    public static void SignOutFromGoogle() => _OnSignOut();

    private static async UniTask<bool> _OnSignIn()
    {
        GoogleSignIn.Configuration = _configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        //AddToInformation("Calling SignIn");

        return await await GoogleSignIn.DefaultInstance.SignIn().ContinueWith(async task =>
        {
            return await _OnAuthenticationFinished(task);
        });
    }

    private static void _OnSignOut()
    {
        //AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public static void OnDisconnect()
    {
        //AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal static async UniTask<bool> _OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            //AddToInformation("task is faulted");
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    //AddToInformation(error.Status.ToString());
                    //AddToInformation(error.Message);
                }
                else
                {
                    //AddToInformation(task.Exception.ToString());
                }
            }
        }
        else if (task.IsCanceled)
        {
            //AddToInformation("Canceled");
        }
        else
        {
            //AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            //AddToInformation("Google ID Token = " + task.Result.IdToken);
            //AddToInformation("Email = " + task.Result.Email);
            return await _SignInWithGoogleOnFirebase(task.Result.IdToken);
        }

        return false;
    }

    private static async UniTask<bool> _SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        return await await _auth.SignInWithCredentialAsync(credential).ContinueWith(async task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                //if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                //    AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                var result = await UserLogin.Send();

                //string data = "";
                //data += $"DisplayName: {User.DisplayName}\n";
                //data += $"UserId: {User.UserId}\n";
                //data += $"PhoneNumber: {User.PhoneNumber}\n";
                //data += $"Email: {User.Email}\n";
                //data += $"IsEmailVerified: {User.IsEmailVerified}\n";
                //data += $"IsAnonymous: {User.IsAnonymous}\n";
                //data += $"ProviderId: {User.ProviderId}\n";

                //await Api_GoogleUserTest.Send(data);

                Login.Type = LoginType.Google;
                PlayerPrefs.SetInt(PlayerPrefsKey.LAST_LOGIN, (int)LoginType.Google);
                //AddToInformation("Sign In Successful.");
                return result.success;
            }

            return false;
        });
    }

    public static void OnSignInSilently()
    {
        GoogleSignIn.Configuration = _configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        //AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(_OnAuthenticationFinished);
    }

    public static void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = _configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        //AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(_OnAuthenticationFinished);
    }

    private static void AddToInformation(string str)
    {
        _infoText += "\n" + str;

        Debug.Log(_infoText);
    }
}
