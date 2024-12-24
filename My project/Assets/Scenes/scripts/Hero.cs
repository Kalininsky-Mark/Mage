using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : Entity
{
    public static Hero Instance { get; set; }
    public GameObject fireball;

    [SerializeField] private float speed = 3f;
    [SerializeField] new private int lives = 9;
    [SerializeField] private float jumpForce = 15f;
    private bool isGrounded = false;

    public bool isAttacking = false;
    public bool isRecharged = true;
    public bool isReadyToCast = true;

    public Transform attackPos;
    public float attackRange;
    public LayerMask Enemy;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        isRecharged = true;
    }

    private void Attack()
    {
        State = States.Attack;
        isAttacking = true;
        isRecharged = false;

        StartCoroutine(AttackAnimation());
        StartCoroutine(AttackCoolDown());
    }

    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, Enemy);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
        }
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.7f);
        isRecharged = true;
    }

    private IEnumerator CastCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isReadyToCast = true;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Cast()
    {
        isReadyToCast = false;
        {
            Instantiate(fireball, transform.position, Quaternion.identity);
        }
        StartCoroutine(CastCoolDown());
    }

    private void Update()
    {
        if (isGrounded && !isAttacking) State = States.Idle;

        if (Input.GetButton("Horizontal") && !isAttacking) Run();

        if ((isGrounded) && (Input.GetButtonDown("Jump") && !isAttacking)) Jump();
        if (Input.GetButtonDown("Fire1")) Attack();
        if (Input.GetButtonDown("Fire2") && isReadyToCast) Cast();
    }

    private void Run() 
    {
        if (isGrounded && !isAttacking) State = States.Run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        if ((isGrounded) && (Input.GetButtonDown("Jump"))) Jump();

        sprite.flipX = dir.x < 0.0f;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 2f);
        isGrounded = collider.Length > 1;

        if (!isGrounded && !isAttacking) State = States.Jump;
    }
    public virtual void GetDamage()
    {
        lives--;
        Debug.Log("здоровье противника" + lives);
        if (lives < 1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    

    private States State
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value);  }
    }
}

public enum States
{
    Idle,
    Run,
    Jump,
    Attack
}
