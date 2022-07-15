using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    private Vector3 offset = new Vector3(0, 2.5f, -4);
    private float smoothing = 2f;
    void Update()
    {
        if(target==null) return;
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        transform.LookAt(target);
    }
}
