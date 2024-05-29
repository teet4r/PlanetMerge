using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Behaviour;
using Cysharp.Threading.Tasks;

public class PlayScene : SceneSingletonBehaviour<PlayScene>
{
    public readonly ReactiveProperty<long> Score = new();
    public int MaxLevel;
    
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

    public void GameOver()
    {
        if(_isGameover)
            return;

        _isGameover = true;

        _GameOverRoutine().Forget();
    }

    private async UniTask _GameOverRoutine()
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

        //최고 점수 갱신
        await _RenewalHighestScore();
        //int maxScore = Mathf.Max(Score.Value, PlayerPrefs.GetInt(PlayerPrefsKey.BEST_SCORE));
        //PlayerPrefs.SetInt(PlayerPrefsKey.BEST_SCORE, maxScore);

        //게임오버 ui
        UIManager.Show<UIGameoverPopup>().Bind(Score.Value);
    }
    
    public void TouchDown()
    {
        if (_lastPlanet == null)
            return;

        _lastPlanet.Drag();
    }

    public void TouchUp()
    {
        if (_lastPlanet == null)
            return;

        _lastPlanet.Drop();
        _lastPlanet = null;
    }

    private async UniTask _RenewalHighestScore()
    {
        if (Login.Type == LoginType.Google)
        {
            var result = await RenewalHighestScore.Send(Score.Value);

            Score.Value = result.highestScore;
        }
        else
        {

        }
    }
}
