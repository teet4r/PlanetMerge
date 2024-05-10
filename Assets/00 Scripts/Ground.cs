using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    void Awake()
    {
        float cameraHalfHeight = Camera.main.orthographicSize;
        float cameraHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;

        leftWall.transform.localPosition = new Vector2(-cameraHalfWidth - 0.5f, 0f);
        rightWall.transform.localPosition = new Vector2(cameraHalfWidth + 0.5f, 0f);
    }

    public GameObject leftWall;
    public GameObject rightWall;
}
