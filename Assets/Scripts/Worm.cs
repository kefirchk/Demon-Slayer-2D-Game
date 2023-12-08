using UnityEngine;

public class Worm : Entity
{
    private void Start()
    {
        speed = 0;
        lives = 15;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage(5);
            lives--;
            Debug.Log("Worm lives: " + lives);
        }
        if (lives < 1)
            Die();
    }
}
