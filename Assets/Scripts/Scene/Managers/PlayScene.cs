using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Behaviour;
using Cysharp.Threading.Tasks;

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

        UIManager.Show<UIPlayPopup>().Bind();

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

        _WaitNext().Forget();
    }

    private async UniTask _WaitNext()
    {
        await UniTask.WaitUntil(() => _lastDongle == null);
        await UniTask.Delay(1800);

        _NextDongle();
    }

    public void GameOver()
    {
        if(_isGameover)
            return;

        _isGameover = true;

        _GameOverRoutine().Forget();
    }

    private async UniTask _GameOverRoutine()
    {
        // ���ȿ� Ȱ��ȭ �Ǿ��ִ� ��� ���� ��������
        var dongles = ObjectPoolManager.GetActiveAll<Dongle>();

        for (int i = 0; i < dongles.Length; ++i)
        {
            dongles[i].Rigid.simulated = false;
        }

        // �� ��� �ϳ��� �����ؼ� ����
        for (int i = 0; i < dongles.Length; ++i)
        {
            dongles[i].Hide(Vector3.up * 100);

            await UniTask.Delay(100);
        }

        await UniTask.Delay(2000);

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
