using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class YieldCache
{
    private static Dictionary<float, WaitForSeconds> _wfsPool = new();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!_wfsPool.TryGetValue(seconds, out WaitForSeconds wfs))
            _wfsPool.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
}
