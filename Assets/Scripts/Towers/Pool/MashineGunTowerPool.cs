public class MashineGunTowerPool : BasePool<GameUnit>
{
    public MashineGunTowerPool(GameUnit prefab) : base(prefab)
    {
    }

    protected override void OnSpawn(GameUnit spawnObject)
    {
        spawnObject.gameObject.SetActive(true);
        spawnObject.DiedComplete.AddListener(Despawn);
    }

    protected override void OnDespawn(GameUnit despawnObject)
    {
       despawnObject.gameObject.SetActive(false);
       despawnObject.DiedComplete.RemoveListener(Despawn);
    }
}
