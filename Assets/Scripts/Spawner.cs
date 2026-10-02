using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject spikePrefab;
    public float spawnX = 12f;
    public float floorY = -2.5f;
    public float ceilingY = 2.5f;

    [System.Serializable]
    public class Stage
    {
        public int requiredScore;
        public float minInterval;
        public float maxInterval;
    }

    public Stage[] stages = new Stage[]
    {
        new Stage { requiredScore = 0,  minInterval = 0.3f, maxInterval = 3f   },
        new Stage { requiredScore = 20, minInterval = 0.5f, maxInterval = 2f   },
        new Stage { requiredScore = 40, minInterval = 0.7f, maxInterval = 1.2f },
    };

    [Header("Hành lang hẹp (twist)")]
    public float corridorChance = 0.2f;
    public float corridorSpacing = 1.8f;
    public int corridorMinLength = 3;
    public int corridorMaxLength = 5;

    [Header("An toàn — khoảng trống bắt buộc")]
    public float minGapWorld = 2.5f;

    private ObjectPool<GameObject> spikePool;

    void Awake()
    {
        spikePool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(spikePrefab),
            actionOnGet: (spike) => spike.SetActive(true),
            actionOnRelease: (spike) => spike.SetActive(false),
            actionOnDestroy: (spike) => Destroy(spike),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 50
        );
    }

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    Stage GetCurrentStage()
    {
        int score = GameManager.Instance.CurrentScore;
        Stage current = stages[0];
        foreach (Stage s in stages)
        {
            if (score >= s.requiredScore)
                current = s;
        }
        return current;
    }

    float GetSpeed()
    {
        SpikeMover mover = spikePrefab.GetComponent<SpikeMover>();
        return mover != null ? mover.speed : 3f;
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            Stage current = GetCurrentStage();
            float wait = Random.Range(current.minInterval, current.maxInterval);
            yield return new WaitForSeconds(wait);

            float speed = GetSpeed();

            if (Random.value < corridorChance)
            {
                float corridorWidth = SpawnCorridor();
                float clearTime = (corridorWidth + minGapWorld) / speed;
                yield return new WaitForSeconds(clearTime);
            }
            else
            {
                SpawnSingleSpike(Random.value > 0.5f);
                float clearTime = minGapWorld / speed;
                yield return new WaitForSeconds(clearTime);
            }
        }
    }

    void SpawnSingleSpike(bool onCeiling)
    {
        float y = onCeiling ? ceilingY : floorY;
        GameObject spike = spikePool.Get();
        spike.transform.position = new Vector3(spawnX, y, 0);

        SpikeMover mover = spike.GetComponent<SpikeMover>();
        mover.pool = spikePool;
    }

    float SpawnCorridor()
    {
        int length = Random.Range(corridorMinLength, corridorMaxLength + 1);
        bool startOnCeiling = Random.value > 0.5f;

        for (int i = 0; i < length; i++)
        {
            bool onCeiling = startOnCeiling ? (i % 2 == 0) : (i % 2 != 0);
            float x = spawnX + i * corridorSpacing;
            float y = onCeiling ? ceilingY : floorY;

            GameObject spike = spikePool.Get();
            spike.transform.position = new Vector3(x, y, 0);

            SpikeMover mover = spike.GetComponent<SpikeMover>();
            mover.pool = spikePool;
        }

        return (length - 1) * corridorSpacing;
    }
}