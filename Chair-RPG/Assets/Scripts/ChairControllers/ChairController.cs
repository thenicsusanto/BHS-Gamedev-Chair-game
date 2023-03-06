using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairController : MonoBehaviour
{
    [SerializeField] protected float SPD;
    [SerializeField] protected float JumpForce;
    protected Rigidbody rb;

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundRadius = 0.2f;
    [SerializeField] protected LayerMask groundLayer;
    protected bool Grounded;
    protected bool JumpQued = false;

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

    protected virtual void HandleWalk()
    {
        float xForce = Input.GetAxisRaw("Horizontal");
        float yForce = Input.GetAxisRaw("Vertical");
        
        Vector3 forceVector = new Vector3(xForce,0,yForce);
        forceVector.Normalize();
        Vector3 force = Quaternion.Euler(0, -45, 0) * new Vector3(SPD * forceVector.x, 0, SPD * forceVector.z);

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
            print("E");
            Jump();
        }
    }

    protected virtual void HandleMovement()
    {
        HandleWalk();
        HandleJump();
    }

    //Combat
    protected virtual void HandleCombat()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        HandleMovement();
    }
}