using Unit;

namespace Pool
{
    public class EnemyPool : BasePool<GameUnit>
    {
        public EnemyPool(GameUnit prefab, bool isDebug) : base(prefab, isDebug)
        {
        }

        protected override void OnSpawn(GameUnit spawnObject)
        {
            spawnObject.gameObject.SetActive(true);
            spawnObject.DiedCompleted.AddListener(Despawn);
        }

        protected override void OnDespawn(GameUnit despawnObject)
        {
            despawnObject.gameObject.SetActive(false);
            despawnObject.DiedCompleted.RemoveListener(Despawn);
        }

    }
}