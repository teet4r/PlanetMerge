using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    public void StartGame() //���ο��� ���ӽ��� ����
    {
        SceneManager.LoadScene(1);
    }
    public void GameExit() //���ο��� ��������
    {
        Application.Quit();
    }
}
