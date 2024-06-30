public class MergeEffect : ParticleObject
{
    public override void Pool() => ObjectPoolManager.Return(this);
}
