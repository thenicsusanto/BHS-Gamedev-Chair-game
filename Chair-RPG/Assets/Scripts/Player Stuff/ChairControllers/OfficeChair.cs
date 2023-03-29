using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeChair : ChairController
{
    [SerializeField] protected float spin;

    protected override void HandleWalk()
    {
        Vector3 force = GetForce()*currSPD;
        print(force);    
        if(force != Vector3.zero)
        {
            rb.AddForce(new Vector3(force.x-rb.velocity.x,0,force.z-rb.velocity.z));
            rb.AddTorque(transform.up * (spin - rb.angularVelocity.y));
        }
    }
}
