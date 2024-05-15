using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeEffect : ParticleObject
{
    public override void Pool() => ObjectPoolManager.Return(this);
}
