using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;

    PlayerInput playerInput;
    Rigidbody2D rb;
    int jumpcount = 0;
    int maxjump = 2;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var move = playerInput.actions["Move"].ReadValue<Vector2>();

        rb.linearVelocityX = move.x * speed;

        if (playerInput.actions["Jump"].WasPressedThisFrame() && jumpcount < maxjump)
        {
            rb.linearVelocityY = jumpSpeed;
            jumpcount++;
        }
  
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            jumpcount = 0;
        }
    }
}
