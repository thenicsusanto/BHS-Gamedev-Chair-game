using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairController : MonoBehaviour
{
    [SerializeField] protected GameObject model;
    protected Animator animator;

    [Header("Movement")]
    [SerializeField] protected float SPD;
    protected float currSPD;
    [SerializeField] protected float JumpForce;
    protected Rigidbody rb;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundRadius = 0.2f;
    [SerializeField] protected LayerMask groundLayer;
    protected bool Grounded;
    protected bool JumpQued = false;
    protected bool CanMove = true;

    [Header("Combat")]
    [SerializeField] protected float AttackBoost;
    [SerializeField] protected float moveStop; //Amount of time that the player can't move for after attacking
    [SerializeField] protected float AttackCooldown;
    [SerializeField] protected float comboBreakTime;
    [SerializeField] protected int comboLength;
    protected float breakTimer;
    protected bool canAttack =true;
    protected int currComboAtk = 1;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = model.GetComponent<Animator>();
        currSPD = SPD;
    }

    //Movement stuff
    protected virtual void GroundCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(groundCheck.position, groundRadius, groundLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                Grounded = true;
                return;
            }
        }

        Grounded = false;
    }

    protected virtual void Jump()
    {
        JumpQued = false;
        rb.velocity = new Vector3(rb.velocity.x,JumpForce,rb.velocity.z);
    }

    protected IEnumerator QueJump()
    {
        JumpQued = true;
        yield return new WaitForSeconds(0.15f);
        JumpQued = false;
    }

    protected virtual Vector3 GetForce()
    {
        float xForce = Input.GetAxisRaw("Horizontal");
        float yForce = Input.GetAxisRaw("Vertical");
        print(xForce.ToString()+yForce.ToString());
        Vector3 forceVector = new Vector3(xForce,0,yForce);
        forceVector.Normalize();
        Vector3 force = Quaternion.Euler(0, -45, 0) * new Vector3(forceVector.x, 0, forceVector.z);
        return force;
    }

    protected virtual void HandleWalk()
    {
        Vector3 force = GetForce()*currSPD;
        if(force != Vector3.zero && canAttack == true)
        {
            transform.LookAt(force+ transform.position); 
        }
        print(force);    
        rb.AddForce(new Vector3(force.x-rb.velocity.x,0,force.z-rb.velocity.z));
    }

    protected virtual void HandleJump()
    {
        GroundCheck();

        if(Input.GetKeyDown("space") && JumpQued == false)
        {
            StartCoroutine(QueJump());
        }

        if(Grounded && JumpQued)
        {
            Jump();
        }
    }

    protected virtual void HandleMovement()
    {
        if(CanMove == false){print("e");return;}
        HandleWalk();
        HandleJump();
    }

    protected virtual void HandleWalkAnim()
    {
        Vector3 force = GetForce();
        if(force != Vector3.zero && Grounded)
        {
            animator.SetBool("Walking",true);
        }
        else
        {
            animator.SetBool("Walking",false);
        }
    }

    //Combat
    protected virtual IEnumerator slowWalk(float amount)
    {
        currSPD = SPD/4;
        yield return new WaitForSeconds(amount);
        currSPD = SPD;
    }

    protected virtual IEnumerator handleAttackCooldown(float amount)
    {
        canAttack = false;
        yield return new WaitForSeconds(amount);
        canAttack = true;
    }

    protected virtual void Attack()
    {
        breakTimer = comboBreakTime;
        Vector3 force = GetForce()*AttackBoost;
        StartCoroutine(slowWalk(moveStop));
        StartCoroutine(handleAttackCooldown(AttackCooldown));
        rb.AddForce(new Vector3(force.x,0,force.z),ForceMode.VelocityChange);
        print(currComboAtk.ToString());
        animator.SetTrigger("Attack"+currComboAtk.ToString());
        currComboAtk++;
    }

    protected virtual void HandleCombat()
    {
        breakTimer -= Time.deltaTime;
        if(breakTimer <= 0 || currComboAtk > comboLength)
        {
            currComboAtk = 1;
        }
        if(Input.GetMouseButtonDown(0) && canAttack)
        {
            Attack();
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        HandleWalkAnim();
        HandleMovement();
        HandleCombat();
    }
}