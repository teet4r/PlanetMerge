using UnityEngine;

public static class CameraExtensions
{
    public static float ScreenSizeXToWorldSizeX(this Camera camera, float pixelX)
    {
        return ScreenSizeToWorldSize(camera, new Vector3(pixelX, 0, 0)).x;
    }

    public static float ScreenSizeYToWorldSizeY(this Camera camera, float pixelY)
    {
        return ScreenSizeToWorldSize(camera, new Vector3(0, pixelY, 0)).y;
    }

    public static Vector3 ScreenSizeToWorldSize(this Camera camera, Vector3 pixelSize)
    {
        var worldPoint = camera.ScreenToWorldPoint(pixelSize);
        worldPoint.x += camera.orthographicSize * camera.aspect;
        worldPoint.y += camera.orthographicSize;
        return worldPoint;
    }
}