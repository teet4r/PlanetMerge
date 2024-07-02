using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void SetPositionX(this Transform tr, float newX)
    {
        var p = tr.position;
        p.x = newX;
        tr.position = p;
    }

    public static void SetPositionY(this Transform tr, float newY)
    {
        var p = tr.position;
        p.y = newY;
        tr.position = p;
    }
}
