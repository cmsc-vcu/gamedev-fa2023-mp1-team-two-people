using UnityEngine;

public class Player1Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    public int maxHealth = 10;
    private int currentHealth;
    float dirX;
    private enum MovementState { idle, running, jumping, falling, attack1 }

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    [SerializeField] private CircleCollider2D weaponHitCollider;  // Declare a variable to hold the CircleCollider2D reference
    [SerializeField] private bool isPlayer1 = true;
    bool player1_attacking;
    bool player2_attacking;

    private bool isAttacking; // Added to track attack animation

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!player1_attacking)
        {
            dirX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }

        UpdateAnimationState();

        if (player1_attacking)
        {
            CheckForHit();
        }
    }

    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle;

        float dirX = Input.GetAxisRaw("Horizontal");

        if (dirX > 0f)
        {
            state = MovementState.running;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        // Set isAttacking based on attack animation
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        isAttacking = stateInfo.IsName("attack1");

        if (isPlayer1)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
               
                player1_attacking = true;
                Debug.Log("Player1 attacked!");
                anim.SetTrigger("attack1");
            }
            else
            {
                player1_attacking = false;  // Reset when not attacking
            }
        }

        anim.SetInteger("state", (int)state);
    }

    private void CheckForHit()
    {
        if (weaponHitCollider == null)
        {
            Debug.LogWarning("Weapon hit collider reference not set.");
            return;
        }

        // Use the radius and position from the assigned circle collider
        float hitRadius = weaponHitCollider.radius;
        Vector2 hitPosition = weaponHitCollider.transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPosition, hitRadius, LayerMask.GetMask("Player2"));

        foreach (Collider2D hitCollider in hits)
        {
            Player2Movement player2 = hitCollider.GetComponent<Player2Movement>();
            if (player2 != null)
            {
                player2.TakeDamage(20);
                Debug.Log("Hit Player2!");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (player1_attacking && collision.gameObject.CompareTag("Player2"))
        {
            Debug.Log("Hit!");
            CheckForHit();
        }

        anim.SetBool("inRange", false);
        player1_attacking = false;
        player2_attacking = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
