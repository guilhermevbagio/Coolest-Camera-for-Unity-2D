using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public float moveSpeed = 5f, moveSpeedIncrement;
    public float dashSpeed = 10f, dashTimer, dashCooldown;

    private Vector3 moveDirection;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    float elapsed;
    CameraFollow cameraFollow;
    Rigidbody2D rb;
    Vector2 movePlayerInputs;
    PlayerInput playerControls;
    public bool boundByCamera;
    public float footstepTimer = 0.5f;  // Adjust this value to control the interval between footstep sounds

    public System.Action onDash;


    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        rb = GetComponent<Rigidbody2D>();
        
        playerControls = PlayerInputManager.Instance.playerControls;
        moveDirection = Vector2.zero;

        playerControls.Gameplay.Dash.performed += ctx => DashFunction();
        playerControls.Gameplay.Movement.performed += ctx => moveDirection = ctx.ReadValue<Vector2>().normalized;
        playerControls.Gameplay.Movement.canceled += ctx => moveDirection = Vector2.zero;
    }


    float footstepTimerElapsed = 0;
    void Update()
    {
        elapsed += Time.deltaTime;
        
        if (moveDirection.x != 0)
        {
            spriteRenderer.flipX = (moveDirection.x < 0);
        }

        UpdateAnimator();

        footstepTimerElapsed += Time.deltaTime;

        if (moveDirection.magnitude > 0  && footstepTimerElapsed > footstepTimer)
        {
            //play footsteps
            footstepTimerElapsed = 0;
        }
        
    }


    private void FixedUpdate() {
        movePlayerInputs = playerControls.Gameplay.Movement.ReadValue<Vector2>();

        moveDirection = movePlayerInputs.normalized;

        float actualMoveSpeed = moveSpeed + moveSpeedIncrement;

        Move(moveDirection.normalized * actualMoveSpeed * Time.fixedDeltaTime);
        
    }

    public void Move(Vector2 move){
        transform.Translate(move);
    }

    void DashFunction(){
        if(elapsed >= dashCooldown && moveDirection.magnitude > 0)
        {     
            StartCoroutine("Dash");
        }
    }

    IEnumerator Dash()
    {
        onDash?.Invoke();
        var baseMoveSpeed = moveSpeed;
        moveSpeed = dashSpeed;

        elapsed = 0;
        cameraFollow.ShakeCamera(.1f, .05f);
        rb.isKinematic = true;

        yield return new WaitForSeconds(dashTimer);

        rb.isKinematic = false;
        moveSpeed = baseMoveSpeed;
    }
    void UpdateAnimator()
    {
        if (moveDirection.magnitude > 0)
        {
            animator.SetFloat("speed", moveSpeed);
        }
        else
        {
            animator.SetFloat("speed", 0f);
        }
    }
}




