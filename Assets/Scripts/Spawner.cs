using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spikePrefab;
    public float spawnInterval = 2f;
    public float spawnX = 12f;      // vị trí X mép phải màn hình, nơi gai xuất hiện
    public float floorY = -2.5f;    // vị trí Y của gai khi nằm dưới (gần Floor)
    public float ceilingY = 2.5f;   // vị trí Y của gai khi nằm trên (gần Ceiling)

    private void Start()
    {
        InvokeRepeating(nameof(SpawnSpike), 1f, spawnInterval);
    }

    void SpawnSpike()
    {
        bool onCeiling = Random.value > 0.5f;
        float y = onCeiling ? ceilingY : floorY;
        Instantiate(spikePrefab, new Vector3(spawnX, y, 0), Quaternion.identity);
    }
}