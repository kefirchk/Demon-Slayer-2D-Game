using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum States
    {
        IDLE,
        RUN,
        JUMP,
        ATTACK,
        UTIMATE,
        WAVE
    }

    [SerializeField] public float lives;
    [SerializeField] protected float speed;
    public virtual void GetDamage(float damagePower) 
    {
        lives -= damagePower;
        if (lives < 1)
            Die();
    }

    public virtual void Die()
    {
        Hero.Instance.DecrementMobCount();
        Destroy(this.gameObject);
    }
}
