using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private GameObject arrowPrefab;
    private Transform leftHandTransform;

    private void Start()
    {
        anim = GetComponent<Animator>();
        leftHandTransform = transform.Find("Erika_Archer_Meshes/Erika_Archer_Bow_Mesh");
    }

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                bool isCollider = Physics.Raycast(ray, out var hit);
                if (isCollider)
                {
                    Vector3 targetPoint = hit.point;
                    anim.SetTrigger("Attack");
                    targetPoint.y = transform.position.y;
                    Vector3 dir = targetPoint - transform.position;
                    transform.rotation = Quaternion.LookRotation(dir);
                    Shoot(dir);
                }
            }
        }
    }

    private void Shoot(Vector3 dir)
    {
        Instantiate(arrowPrefab, leftHandTransform.position, Quaternion.LookRotation(dir));
    }

    public void SetPlayerArrow(GameObject go)
    {
        arrowPrefab = go;
    }
}
