using UnityEngine;
using UnityEngine.Pool;

public class SpikeMover : MonoBehaviour
{
    public float speed = 3f;
    public float destroyX = -13f;

    [HideInInspector]
    public ObjectPool<GameObject> pool;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < destroyX)
        {
            if (pool != null)
                pool.Release(gameObject);
            else
                Destroy(gameObject);
        }
    }
}