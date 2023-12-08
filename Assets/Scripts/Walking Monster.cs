using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMonster : Entity
{
    private Vector3 dir;
    SpriteRenderer sprite;

    private void Start()
    {
        lives = 20;
        speed = 1.5f;
        dir = transform.right;
    }

    private void Update()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        Move();
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position/*+ transform.up * 0.1f + transform.right * dir.x * 0.7f*/, 0.01f);
        if (colliders.Length > 1)
            dir *= -1f;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, Time.deltaTime);
        sprite.flipX = dir.x > 0.0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage(10);
        }    
    }

    public override void GetDamage(float damagePower)
    {
        lives -= damagePower;
        Debug.Log("Walking monster lives: " + lives);
    }
}
