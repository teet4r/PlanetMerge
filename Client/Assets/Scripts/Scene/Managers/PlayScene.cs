using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Behaviour;
using Cysharp.Threading.Tasks;

public class PlayScene : SceneSingletonBehaviour<PlayScene>
{
    public const float GAMEOVER_TRIGGER_TIME = 3f;
    public int MaxLevel;
    public readonly ReactiveProperty<long> Score = new(0);

    private bool _isGameover;
    private Planet _lastPlanet;

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

        _lastPlanet = ObjectPoolManager.Release<Planet>();
        _lastPlanet.Tr.position = new Vector2(0f, 4.3f);
        _lastPlanet.Activate();

        _WaitNext().Forget();
    }

    private async UniTask _WaitNext()
    {
        await UniTask.WaitUntil(() => _lastPlanet == null);
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
        // 장면안에 활성화 되어있는 모든 동글 가져오기
        var planets = ObjectPoolManager.GetActiveAll<Planet>();

        for (int i = 0; i < planets.Length; ++i)
        {
            planets[i].Rigid.simulated = false;
        }

        // 윗 목록 하나씩 접근해서 삭제
        for (int i = 0; i < planets.Length; ++i)
        {
            planets[i].Hide(Vector3.up * 100);

            await UniTask.Delay(100);
        }

        await UniTask.Delay(2000);

        var highestScore = User.HighestScore;

        //최고 점수 갱신
        await _RenewalHighestScore();

        //게임오버 ui
        UIManager.Show<UIGameoverPopup>().Bind(Score.Value, highestScore);
    }
    
    public void TouchDown()
    {
        if (_isGameover || _lastPlanet == null)
            return;

        _lastPlanet.Drag();
    }

    public void TouchUp()
    {
        if (_isGameover || _lastPlanet == null)
            return;

        _lastPlanet.Drop();
        _lastPlanet = null;
    }

    private async UniTask _RenewalHighestScore()
    {
        if (User.HighestScore >= Score.Value)
            return;

        if (User.LoginType == LoginType.Google)
            await RenewalHighestScore.Send(Score.Value);
        else
            PlayerPrefs.SetString(PlayerPrefsKey.HIGHEST_SCORE, Score.Value.ToString());
    }
}
