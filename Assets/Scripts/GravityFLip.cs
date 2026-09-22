using UnityEngine;
using UnityEngine.InputSystem;

public class GravityFlip : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    

    public void OnFlip(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rb.gravityScale *= -1;
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
    }
}