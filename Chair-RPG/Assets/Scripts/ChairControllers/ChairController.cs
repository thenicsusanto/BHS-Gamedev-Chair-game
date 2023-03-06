using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float SPD;
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

    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        
        Vector3 forceVector = new Vector3(xForce,0,yForce);
        forceVector.Normalize();
        Vector3 force = Quaternion.Euler(0, -45, 0) * new Vector3(forceVector.x, 0, forceVector.z);
        return force;
    }

    protected virtual void HandleWalk()
    {
        Vector3 force = GetForce()*SPD;

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
        if(CanMove == false){return;}
        HandleWalk();
        HandleJump();
    }

    //Combat
    IEnumerator pauseWalk(float amount)
    {
        CanMove = false;
        yield return new WaitForSeconds(amount);
        CanMove = true;
    }

    protected virtual void HandleCombat()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 force = GetForce()*AttackBoost;
            StartCoroutine(pauseWalk(moveStop));
            rb.AddForce(new Vector3(force.x,0,force.z));
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        HandleMovement();
        HandleCombat();
    }
}