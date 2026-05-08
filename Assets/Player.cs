using UnityEngine;
using UnityEngine.InputSystem;


public enum StateType
{
   Move,
   Attack

}

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    [SerializeField] Animator animator;
    int jumpcount = 0;
    int maxjump = 2;
   private bool isGrounded;
    State state;
    State[] states = new State[]
     {
        new StateMove(),
        new StateAttack(),
     };
    StateMove stateMove = new();
    public PlayerInput playerInput;
    public Rigidbody2D rb;

    public class State
    {
        public PlayerInput playerInput;
        public Rigidbody2D rb;
        public Animator animator;

        public virtual void Start() { }
        public virtual void Update(bool isGrounded, out StateType nextState)
        {
            {
                nextState = StateType.Move;
            }
        }
        public virtual void End() { }
    }
    public class StateMove : State
    {
        public float speed;
        public float jumpSpeed;

        public override void Update(bool isGrounded,out StateType nextState)
        {
            var move = playerInput.actions["Move"].ReadValue<Vector2>();
            rb.linearVelocityX = move.x * speed;

            if (playerInput.actions["Jump"].WasPressedThisFrame() && isGrounded)
            {
                rb.linearVelocityY = jumpSpeed;
            }
            if (playerInput.actions["Attack"].WasPerformedThisFrame())
            {
                nextState = StateType.Attack;
            }
            else
            {
                nextState = StateType.Move;
            }
        }
    }
    public class StateAttack : State
    {
        public override void Start()
        {
            animator.Play("Attack");
        }
        public override void Update(bool isGrounded, out StateType nextState)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                nextState = StateType.Move;
            }
            else
            {
                nextState = StateType.Attack;
            }
        }
    }
    

    void Start()
    {
       
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        states[(int)StateType.Move].playerInput = playerInput;
        states[(int)StateType.Move].rb = rb;
        (states[(int)StateType.Move] as StateMove).speed = speed;
        (states[(int)StateType.Move] as StateMove).jumpSpeed = jumpSpeed;
        states[(int)StateType.Move].animator = animator;
        states[(int)StateType.Attack].playerInput = playerInput;
        states[(int)StateType.Attack].rb = rb;
        states[(int)StateType.Attack].animator = animator;
        state = states[(int)StateType.Move];
        state.Start();
    }

   
    void Update()
    {
        state.Update(isGrounded, out var nextState);
        if (state != states[(int)nextState])
        {
            state.End();
            state = states[(int)nextState];
            state.Start();
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
