using Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : SceneSingletonBehaviour<Ground>
{
    public GameoverLine GameoverLine => _gameoverLine;

    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private Transform _groundSpace;
    [SerializeField] private GameoverLine _gameoverLine;

    private Camera _mainCamera;

    protected override void Awake()
    {
        base.Awake();

        _mainCamera = Camera.main;

        float cameraHalfHeight = _mainCamera.orthographicSize;
        float cameraHalfWidth = _mainCamera.orthographicSize * Screen.width / Screen.height;

        _leftWall.transform.localPosition = new Vector2(-cameraHalfWidth - 0.5f, 0f);
        _rightWall.transform.localPosition = new Vector2(cameraHalfWidth + 0.5f, 0f);

        var spacePos = _groundSpace.position;
        spacePos.y += _mainCamera.ScreenSizeYToWorldSizeY(AdManager.BannerHeight + 10);
        _groundSpace.position = spacePos;
    }
}