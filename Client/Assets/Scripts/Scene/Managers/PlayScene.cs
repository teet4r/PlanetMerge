using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Behaviour;
using Cysharp.Threading.Tasks;

public class PlayScene : SceneSingletonBehaviour<PlayScene>
{
    public GameoverLine GameoverLine;
    public int MaxLevel;
    public readonly ReactiveProperty<int> Score = new(0);
    public Planet LastPlanet => _lastPlanet;

    private Planet _lastPlanet;
    private bool _isGameover;
    private bool _isTouchingDown;

    private void OnEnable()
    {
        Score.Value = 0;
        MaxLevel = 1;
        _isTouchingDown = false;

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

        var lastPlanet = ObjectPoolManager.Release<Planet>();
        lastPlanet.Tr.position = new Vector2(0f, 4.3f);
        lastPlanet.Activate();
        SFX.Play(Sfx.Next);

        _lastPlanet = lastPlanet;

        _WaitNext().Forget();
    }

    private async UniTask _WaitNext()
    {
        await UniTask.WaitUntil(() => _lastPlanet == null, cancellationToken: destroyCancellationToken);
        await UniTask.Delay(1000, cancellationToken: destroyCancellationToken);

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
        var planets = FindObjectsOfType<Planet>();

        for (int i = 0; i < planets.Length; ++i)
            planets[i].Rigid.simulated = false;

        // 윗 목록 하나씩 접근해서 삭제
        for (int i = 0; i < planets.Length; ++i)
        {
            planets[i].Hide(Vector3.up * 100);

            await UniTask.Delay(100, cancellationToken: destroyCancellationToken);
        };

        await UniTask.Delay(2000, cancellationToken: destroyCancellationToken);

        //최고 점수 갱신
        //await _RenewalHighestScore();
        var prevHighestScore = PlayerPrefs.GetInt(PlayerPrefsKey.HIGHEST_SCORE, 0);
        var curHighestScore = prevHighestScore < Score.Value ? Score.Value : prevHighestScore;
        PlayerPrefs.SetInt(PlayerPrefsKey.HIGHEST_SCORE, curHighestScore);

        //게임오버 ui
        UIManager.Show<UIGameoverPopup>().Bind(Score.Value, prevHighestScore, curHighestScore);
        SFX.Play(Sfx.GameOver);
    }
    
    public void TouchDown()
    {
        if (_isGameover || _lastPlanet == null)
            return;

        _isTouchingDown = true;
        _lastPlanet.Drag();
    }

    public void TouchUp()
    {
        if (_isGameover || _lastPlanet == null || !_isTouchingDown)
            return;

        _isTouchingDown = false;
        _lastPlanet.Drop();
        _lastPlanet = null;
    }

    //private async UniTask _RenewalHighestScore()
    //{
    //    if (User.HighestScore >= Score.Value)
    //        return;

    //    if (User.LoginType == LoginType.Google)
    //        await RenewalHighestScore.Send(Score.Value);
    //    else
    //    {
    //        User.HighestScore = Score.Value;
    //        PlayerPrefs.SetString(PlayerPrefsKey.HIGHEST_SCORE, Score.Value.ToString());
    //    }
    //}
}
