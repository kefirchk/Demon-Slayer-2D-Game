using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : Entity
{
    [SerializeField] private float jumpForce = 15f;

    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource damageSound;
    [SerializeField] private AudioSource missAttackSound;
    [SerializeField] private AudioSource attackMobSound;

    public GameObject levelCompletedMenu;
    public GameObject buttonsUI;


    private Rigidbody2D rigidBody;
    private SpriteRenderer sprite;
    private Animator animator;

    public float power;
    public int mobCount;

    private bool isGrounded = false;
    public bool isAttacking = false;
    public bool isRecharged = true;
    public bool isUltimate = false;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;
    public Joystick joystick;
    public static Hero Instance { get; set; }

    private void Start()
    {
        speed = 3f;
        lives = 100;
        power = 100f;
    }
    private void Awake()
    {
        Destroy(GameObject.Find("AudioPhone"));

        Instance = this;
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isRecharged = true;
    }

    public void UltimateAttack()
    {
        if (power > 0 && isRecharged)
        {
            State = States.UTIMATE;

            isUltimate = true;
            isRecharged = false;
            power -= 25;

            StartCoroutine(UltimateAnimation());
            StartCoroutine(UltimateCoolDown());
        }
    }

    public void Attack()
    {
        if(isGrounded && isRecharged)
        {
            State = States.ATTACK;
            isAttacking = true;
            isRecharged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }

    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

        if (colliders.Length == 0)
            missAttackSound.Play();
        else
            attackMobSound.Play();

        for (int i = 0; i < colliders.Length; ++i)
        {
            if(isAttacking)
                colliders[i].GetComponent<Entity>().GetDamage(5);
            if (isUltimate)
                colliders[i].GetComponent<Entity>().GetDamage(30);
            StartCoroutine(EnemyOnAttack(colliders[i]));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }

    private IEnumerator UltimateAnimation()
    {
        yield return new WaitForSeconds(0.4f);
        isUltimate = false;
    }

    private IEnumerator UltimateCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }

    private IEnumerator EnemyOnAttack(Collider2D enemy)
    {
        SpriteRenderer enemyColor = enemy.GetComponentInChildren<SpriteRenderer>();
        enemyColor.color = new Color(0.8203125f, 0.25f, 0.25f);
        yield return new WaitForSeconds(0.2f);
        enemyColor.color = new Color(1, 1, 1);
        
    }

    public override void GetDamage(float damagePower)
    {
        damageSound.Play();
        lives -= damagePower;
        Debug.Log("Hero lives: " + lives);
        if(lives <= 0)
        {
            Destroy(GameObject.Find("AudioPhone"));
            Die();
            SceneManager.LoadScene(0);
        }
    }

    public void GetDamageUp(float damagePower)
    {
        GetDamage(damagePower);
        rigidBody.AddForce(transform.up * 5, ForceMode2D.Impulse);
    }

    private void Run()
    {
        Vector3 dir = transform.right * joystick.Horizontal;

        if (isGrounded)
            State = States.RUN;
        else
            dir.y = 0;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f;

    }

    private void Jump()
    {
        rigidBody.velocity = Vector2.up * jumpForce;
        jumpSound.Play();
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        //Debug.Log("Colider length: " + collider.Length);

        if (collider.Length > 1)
        {
            isGrounded = true;
            for (int i = 0; i < collider.Length; ++i)
            {
                if (collider[i].gameObject.tag == "CameraBorder")
                {
                    isGrounded = false;
                }
                if (collider[i].gameObject.tag == "Ground")
                {
                    Vector2 colliderPosition = collider[i].transform.position;
                    if (colliderPosition.y > transform.position.y)
                    {
                        // Обрабатываем касание предмета головой
                        isGrounded = false;
                    }
                    else
                        isGrounded = true;
                    break;
                }
            }
        }
        else
            isGrounded = false;
        
        if (!isGrounded)
        {
            State = States.JUMP;
        }
    }


    public bool isJumped()
    {
        return !isGrounded;
    }

    [SerializeField] public States State
    {
        get { return (States)animator.GetInteger("state"); }
        set { animator.SetInteger("state", (int)value); }
    }


    public void DecrementMobCount()
    {
        mobCount--;
    }


    private void FixedUpdate()
    {
        CheckGround();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "HP_Bonus")
        {
            lives += 25;
            if(lives > 100f)
            {
                lives = 100f;
            }
            collision.collider.gameObject.GetComponent<Bonus>().Destroy();
        }

        if (collision.gameObject.tag == "Power_Bonus")
        {
            power += 25;
            if (power > 100f)
            {
                power = 100f;
            }
            collision.collider.gameObject.GetComponent<Bonus>().Destroy();
        }

        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D col in collider)
        {
            if (col.gameObject.tag == "FinishPoint" && mobCount < 1)
            {
                levelCompletedMenu.SetActive(true);
                buttonsUI.SetActive(false);
                Time.timeScale = 0f;
            }
        }
    }

    void Update()
    {
        if (isGrounded && !isAttacking)
            State = States.IDLE;
        if (!isAttacking && joystick.Horizontal != 0)
            Run();
        if (!isAttacking && isGrounded && joystick.Vertical > 0.5f)
            Jump();
    }
}
