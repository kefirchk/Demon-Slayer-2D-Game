using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

public class BossLevel2 : Entity
{
    private SpriteRenderer sprite;
    [SerializeField] private AIPath aiPath;

    private Rigidbody2D rigidBody;
    private Animator animator;

    private bool isGrounded = false;
    public bool isAttacking = false;
    public bool isRecharged = true;

    public Transform attackPos;
    public float attackRange;

    // Start is called before the first frame update
    void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        lives = 100;
    }

    // Update is called once per frame
    void Update()
    {
        sprite.flipX = aiPath.desiredVelocity.x > 0.01f;

        //Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

    }

    public void Attack()
    {
        if (isRecharged)
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
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

        //for (int i = 0; i < colliders.Length; ++i)
        //{
        //    if (isAttacking)
        //        colliders[i].GetComponent<Entity>().GetDamage(5);
        //}
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

    public override void GetDamage(float damagePower)
    {
        lives -= damagePower;
        if (lives <= 0)
        {
            Die();
        }
    }

    private void Walk()
    {
        if (isGrounded)
            State = States.RUN;

    }

    [SerializeField]
    public States State
    {
        get { return (States)animator.GetInteger("state"); }
        set { animator.SetInteger("state", (int)value); }
    }
}
