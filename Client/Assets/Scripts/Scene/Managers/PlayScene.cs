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
    public readonly List<Planet> Planets = new();
    public Planet LastPlanet => _lastPlanet;

    private Planet _lastPlanet;
    private bool _isGameover;

    private void OnEnable()
    {
        Score.Value = 0;
        MaxLevel = 1;
        Planets.Clear();

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

        _lastPlanet = lastPlanet;

        _WaitNext().Forget();
    }

    private async UniTask _WaitNext()
    {
        await UniTask.WaitUntil(() => _lastPlanet == null, cancellationToken: destroyCancellationToken);
        await UniTask.Delay(1800, cancellationToken: destroyCancellationToken);

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
        Planets.ForEach(planet => planet.Rigid.simulated = false);

        // �� ��� �ϳ��� �����ؼ� ����
        Planets.ForEach(async planet =>
        {
            planet.Hide(Vector3.up * 100);

            await UniTask.Delay(100, cancellationToken: destroyCancellationToken);
        });

        await UniTask.Delay(2000, cancellationToken: destroyCancellationToken);

        //�ְ� ���� ����
        //await _RenewalHighestScore();
        var prevHighestScore = long.Parse(PlayerPrefs.GetString(PlayerPrefsKey.HIGHEST_SCORE, "0"));
        var curHighestScore = prevHighestScore < Score.Value ? Score.Value : prevHighestScore;
        PlayerPrefs.SetString(PlayerPrefsKey.HIGHEST_SCORE, curHighestScore.ToString());

        //���ӿ��� ui
        UIManager.Show<UIGameoverPopup>().Bind(Score.Value, prevHighestScore, curHighestScore);
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
