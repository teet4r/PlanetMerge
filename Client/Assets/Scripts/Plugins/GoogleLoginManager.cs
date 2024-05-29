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
    public static string UserId => _auth.CurrentUser.UserId;

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
                else
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    public static async UniTask SignInWithGoogle() => await _OnSignIn();
    public static void SignOutFromGoogle() => _OnSignOut();

    private static async UniTask _OnSignIn()
    {
        GoogleSignIn.Configuration = _configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn");

        await GoogleSignIn.DefaultInstance.SignIn().ContinueWith(_OnAuthenticationFinished);
    }

    private static void _OnSignOut()
    {
        AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public static void OnDisconnect()
    {
        AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal static async UniTask _OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            AddToInformation("task is faulted");
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation(error.Status.ToString());
                    AddToInformation(error.Message);
                }
                else
                {
                    AddToInformation(task.Exception.ToString());
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
        }
        else
        {
            AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            AddToInformation("Google ID Token = " + task.Result.IdToken);
            AddToInformation("Email = " + task.Result.Email);
            await _SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }

    private static async UniTask _SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        await _auth.SignInWithCredentialAsync(credential).ContinueWith(async task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                await UserLogin.Send();

                Login.Type = LoginType.Google;
                PlayerPrefs.SetInt(PlayerPrefsKey.LAST_LOGIN, (int)LoginType.Google);
                AddToInformation("Sign In Successful.");
            }
        });
    }

    public static void OnSignInSilently()
    {
        GoogleSignIn.Configuration = _configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(_OnAuthenticationFinished);
    }

    public static void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = _configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(_OnAuthenticationFinished);
    }

    private static void AddToInformation(string str)
    {
        _infoText += "\n" + str;

        Debug.Log(_infoText);
    }
}
