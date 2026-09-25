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
        public float scoreText;
        public float minInterval;
        public float maxInterval;
    }

    public Stage[] stages = new Stage[]
    {
        new Stage { scoreText = 0f,  minInterval = 0.3f, maxInterval = 3f   },
        new Stage { scoreText = 20f, minInterval = 0.5f, maxInterval = 2f   },
        new Stage { scoreText = 40f, minInterval = 0.7f, maxInterval = 1.2f },
    };

    private float timeAlive = 0f;
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

    void Update()
    {
        timeAlive += Time.deltaTime;
    }

    Stage GetCurrentStage()
    {
        Stage current = stages[0];
        foreach (Stage s in stages)
        {
            if (timeAlive >= s.scoreText)
                current = s;
        }
        return current;
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            Stage current = GetCurrentStage();
            float wait = Random.Range(current.minInterval, current.maxInterval);
            yield return new WaitForSeconds(wait);
            SpawnSpike();
        }
    }

    void SpawnSpike()
    {
        bool onCeiling = Random.value > 0.5f;
        float y = onCeiling ? ceilingY : floorY;

        GameObject spike = spikePool.Get();
        spike.transform.position = new Vector3(spawnX, y, 0);

        SpikeMover mover = spike.GetComponent<SpikeMover>();
        mover.pool = spikePool;
    }
}