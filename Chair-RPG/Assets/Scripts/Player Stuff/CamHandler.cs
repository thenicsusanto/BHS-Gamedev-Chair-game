using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHandler : MonoBehaviour
{
    [SerializeField]
    public Vector3 offset;

    private Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerPos.position + offset;
    }
}
