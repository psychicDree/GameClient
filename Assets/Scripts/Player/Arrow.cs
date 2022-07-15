using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Common;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rgd;
    public RoleType RoleType;
    public bool isLocal = false;
    private void Start()
    {
        rgd = GetComponent<Rigidbody>();
        transform.position += new Vector3(0, 1, 0);
    }

    void FixedUpdate()
    {
        rgd.MovePosition(transform.position + transform.forward * 10 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLocal)
        {
            if (other.GetComponent<PlayerInfo>())
            {
                bool isLocalPlayer = other.GetComponent<PlayerInfo>().isLocal;
                if (isLocal != isLocalPlayer)
                {
                    GameFacade.Instance.SendAttack(50);
                }
            }
        }
        Destroy(this.gameObject);
    }
}
