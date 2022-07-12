using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rgd;
    private void Start()
    {
        rgd = GetComponent<Rigidbody>();
        transform.position += new Vector3(0, 1, 0);
    }

    void Update()
    {
        rgd.MovePosition(transform.position + transform.forward * 10 * Time.deltaTime);
    }
}
