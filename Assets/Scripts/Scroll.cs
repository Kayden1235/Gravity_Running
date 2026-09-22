using UnityEngine;

public class Scroll : MonoBehaviour
{
    public float speed = 3f;
    public float resetPositionX = -20f;
    public float segmentWidth = 40f;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if(transform.position.x < resetPositionX)
        {
            transform.position += new Vector3(segmentWidth, 0, 0);
        }
    }
}
