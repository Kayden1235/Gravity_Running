using UnityEngine;
using UnityEngine.InputSystem;

public class GravityFlip : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    

    public void OnFlip(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            float diraction = Mathf.Sign(rb.gravityScale) * -1;
            rb.gravityScale = diraction * speed;
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
    }
}