using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnConfig
    {
        public NpcType npcType;
        [Range(0f, 1f)] public float spawnWeight = 0.25f;
    }

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private EnemySpawnConfig[] enemyConfigs;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxEnemies = 20;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            int active = PoolManager.Instance.ActiveCount<FsmManagerMelee>() +
                         PoolManager.Instance.ActiveCount<FsmManagerRanged>() +
                         PoolManager.Instance.ActiveCount<FsmManagerGranadier>();

            if (active >= maxEnemies) continue;

            NpcType type = GetRandomNpcType();
            SpawnEnemy(type);
        }
    }

    private void SpawnEnemy(NpcType type)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        FsmManager enemy = type switch
        {
            NpcType.ZombieMelee => PoolManager.Instance.Get<FsmManagerMelee>(),
            NpcType.ZombieRanged => PoolManager.Instance.Get<FsmManagerRanged>(),
            NpcType.ZombieGranadier => PoolManager.Instance.Get<FsmManagerGranadier>()
        };

        if (enemy == null) return;

        enemy.transform.position = spawnPoint.position;
        enemy.transform.rotation = spawnPoint.rotation;
        AudioManager.Instance?.RegisterEnemy(enemy);
        enemy.Activate();
    }

    private NpcType GetRandomNpcType()
    {
        float total = 0f;
        foreach (EnemySpawnConfig config in enemyConfigs)
            total += config.spawnWeight;

        float roll = Random.Range(0f, total);
        float cumulative = 0f;

        foreach (EnemySpawnConfig config in enemyConfigs)
        {
            cumulative += config.spawnWeight;
            if (roll <= cumulative)
                return config.npcType;
        }

        return enemyConfigs[0].npcType;
    }
}