using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public static class User
{
    public static string UserId;
    public static long HighestScore;
    public static LoginType LoginType = LoginType.None;

    public static void Initialize()
    {
        UserId = "";
        HighestScore = 0;
        LoginType = LoginType.None;
    }

    public static void UpdateData(UserData data)
    {
        UserId = data.uid;
        HighestScore = data.highestScore;
    }
}
