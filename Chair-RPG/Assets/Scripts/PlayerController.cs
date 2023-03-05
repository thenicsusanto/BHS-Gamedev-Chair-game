using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float SPD;
    protected Rigidbody rb;

    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void HandleMovement()
    {

        float xForce = Input.GetAxisRaw("Horizontal");
        float yForce = Input.GetAxisRaw("Vertical");

        Vector3 forceVector = new Vector3(xForce,0,yForce);
        forceVector.Normalize();

        rb.velocity =  Quaternion.Euler(0, -45, 0) * new Vector3(SPD*forceVector.x,rb.velocity.y,SPD*forceVector.z);
    }

    // Update is called once per frame
    protected void Update()
    {
        HandleMovement();
    }
}
