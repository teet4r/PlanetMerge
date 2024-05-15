using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using Behaviour;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class PlayScene : SceneSingletonBehaviour<PlayScene>
{
    public readonly ReactiveProperty<int> Score = new();
    public int MaxLevel;
    
    private bool _isGameover;
    private Dongle _lastDongle;

    private void OnEnable()
    {
        Score.Value = 0;
        MaxLevel = 1;

        UIManager.Show<UIPlayPopup>();

        _NextDongle();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            Application.Quit();
    }

    private void _NextDongle()
    {
        if(_isGameover)
            return;

        _lastDongle = ObjectPoolManager.Release<Dongle>();
        _lastDongle.Tr.position = new Vector2(0f, 4.3f);
        _lastDongle.Activate();

        StartCoroutine(_WaitNext());
    }

    private IEnumerator _WaitNext()
    {
        while (_lastDongle != null)
            yield return null;

        yield return YieldCache.WaitForSeconds(1.8f);

        _NextDongle();
    }

    public void GameOver()
    {
        if(_isGameover)
            return;

        _isGameover = true;

        StartCoroutine(_GameOverRoutine());
    }

    private IEnumerator _GameOverRoutine()
    {
        // ���ȿ� Ȱ��ȭ �Ǿ��ִ� ��� ���� ��������
        Dongle[] dongles = FindObjectsOfType<Dongle>();

        // �� ��� �ϳ��� �����ؼ� ����
        for (int i = 0; i < dongles.Length; i++)
        {
            dongles[i].Hide(Vector3.up * 100);

            yield return YieldCache.WaitForSeconds(0.1f);
        }

        yield return YieldCache.WaitForSeconds(2f);

        //�ְ� ���� ����
        int maxScore = Mathf.Max(Score.Value, PlayerPrefs.GetInt(PlayerPrefsKey.BEST_SCORE));
        PlayerPrefs.SetInt(PlayerPrefsKey.BEST_SCORE, maxScore);

        //���ӿ��� ui
        UIManager.Show<UIGameoverPopup>().Bind(Score.Value);
    }
    
    public void TouchDown()
    {
        if (_lastDongle == null)
            return;

        _lastDongle.Drag();
    }

    public void TouchUp()
    {
        if (_lastDongle == null)
            return;

        _lastDongle.Drop();
        _lastDongle = null;
    }
}
