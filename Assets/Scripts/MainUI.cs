using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    public void StartGame() //메인에서 게임시작 연결
    {
        SceneManager.LoadScene(1);
    }
    public void GameExit() //메인에서 게임종료
    {
        Application.Quit();
    }
}
