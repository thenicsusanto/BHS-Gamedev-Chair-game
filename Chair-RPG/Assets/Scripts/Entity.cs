using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected int MaxHealth;
    protected int Health;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Health = MaxHealth;
    }

    protected virtual void ChangeHealth(int amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
    }

    public virtual void Kill()
    {

    }

    public virtual void TakeDamage(int amount)
    {
        ChangeHealth(-amount);
        if(Health == 0)
        {
            Kill();
        }
    }

    public virtual void Heal(int amount)
    {
        ChangeHealth(amount);
    }
}
