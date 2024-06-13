using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Behaviour;
using Cysharp.Threading.Tasks;

public class PlayScene : SceneSingletonBehaviour<PlayScene>
{
    public int MaxLevel;
    public readonly ReactiveProperty<long> Score = new(0);
    public readonly ReactiveProperty<Planet> LastPlanet = new();

    private bool _isGameover;

    private void OnEnable()
    {
        Score.Value = 0;
        MaxLevel = 1;

        _NextDongle();

        UIManager.Show<UIPlayPopup>().Bind();
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

        LastPlanet.Value = ObjectPoolManager.Release<Planet>();
        LastPlanet.Value.Tr.position = new Vector2(0f, 4.3f);
        LastPlanet.Value.Activate();

        _WaitNext().Forget();
    }

    private async UniTask _WaitNext()
    {
        await UniTask.WaitUntil(() => LastPlanet.Value == null);
        await UniTask.Delay(1800);

        _NextDongle();
    }

    public void Gameover()
    {
        if(_isGameover)
            return;
        _isGameover = true;

        _GameoverRoutine().Forget();
    }

    private async UniTask _GameoverRoutine()
    {
        // ���ȿ� Ȱ��ȭ �Ǿ��ִ� ��� ���� ��������
        var planets = ObjectPoolManager.GetActiveAll<Planet>();

        for (int i = 0; i < planets.Length; ++i)
        {
            planets[i].Rigid.simulated = false;
        }

        // �� ��� �ϳ��� �����ؼ� ����
        for (int i = 0; i < planets.Length; ++i)
        {
            planets[i].Hide(Vector3.up * 100);

            await UniTask.Delay(100);
        }

        await UniTask.Delay(2000);

        var highestScore = User.HighestScore;

        //�ְ� ���� ����
        await _RenewalHighestScore();

        //���ӿ��� ui
        UIManager.Show<UIGameoverPopup>().Bind(Score.Value, highestScore);
    }
    
    public void TouchDown()
    {
        if (_isGameover || LastPlanet.Value == null)
            return;

        LastPlanet.Value.Drag();
    }

    public void TouchUp()
    {
        if (_isGameover || LastPlanet.Value == null)
            return;

        LastPlanet.Value.Drop();
        LastPlanet.Value = null;
    }

    private async UniTask _RenewalHighestScore()
    {
        if (User.HighestScore >= Score.Value)
            return;

        if (User.LoginType == LoginType.Google)
            await RenewalHighestScore.Send(Score.Value);
        else
        {
            User.HighestScore = Score.Value;
            PlayerPrefs.SetString(PlayerPrefsKey.HIGHEST_SCORE, Score.Value.ToString());
        }
    }
}
