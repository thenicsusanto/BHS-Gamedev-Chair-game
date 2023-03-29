using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEntity : Entity
{
    [SerializeField] protected GameObject model;
    [SerializeField] protected int Damage;
    [SerializeField] protected float AttackCooldown;
    protected bool canAttack =true;
    protected Animator animator;
    protected Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        animator = model.GetComponent<Animator>();
    }

    protected virtual void Attack()
    {

    }
}
