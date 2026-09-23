using UnityEngine;

public class SpikeMover : MonoBehaviour
{
    public float speed = 3f;
    public float destroyX = -13f;   // ngưỡng bên trái, ra khỏi đây thì huỷ

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < destroyX)
        {
            Destroy(gameObject);
        }
    }
}