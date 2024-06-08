using System;
using System.Collections.Generic;
using System.Threading;
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
    }

    public static async UniTask<bool> SignInWithGoogleClient()
    {
        var loading = UIManager.Show<UILoadingPopup>();

        if (!await _CheckFirebaseDependencies())
        {
            loading.Hide();
            return false;
        }
        if (!await _OnSignIn())
        {
            loading.Hide();
            return false;
        }
        loading.Hide();

        return true;
    }

    private static async UniTask<bool> _CheckFirebaseDependencies()
    {
        return await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && task.Result == DependencyStatus.Available)
            {
                _auth = FirebaseAuth.DefaultInstance;
                return true;
            }

            return false;
        }, cancellationToken: cancellationToken);
    }

    public static void SignOutFromGoogleClient() => _OnSignOut();

    private static async UniTask<bool> _OnSignIn()
    {
        GoogleSignIn.Configuration = _configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        return await await GoogleSignIn.DefaultInstance.SignIn().ContinueWith(async task =>
        {
            return await _OnAuthenticationFinished(task);
        }, cancellationToken: cancellationToken);
    }

    private static void _OnSignOut()
    {
        GoogleSignIn.DefaultInstance.SignOut();
        _auth = null;
    }

    public static void OnDisconnect()
    {
        GoogleSignIn.DefaultInstance.Disconnect();
        _auth = null;
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
        }
        else
            return await _SignInWithGoogleOnFirebase(task.Result.IdToken);

        return false;
    }

    private static async UniTask<bool> _SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        return await _auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            //if (ex != null)
            //{
            //    if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
            //        AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            //}
            //else
            //{
            //    AddToInformation("Sign In Successful.");
            //}

            return ex == null;
        }, cancellationToken: cancellationToken);
    }

    public static void OnSignInSilently()
    {
        GoogleSignIn.Configuration = _configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(_OnAuthenticationFinished);
    }

    public static void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = _configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(_OnAuthenticationFinished);
    }

    private static void AddToInformation(string str)
    {
        _infoText += "\n" + str;

        Debug.Log(_infoText);
    }
}
