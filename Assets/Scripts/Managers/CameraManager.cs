using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraManager : BaseManager
{
    public CameraManager(GameFacade facade) : base(facade) { }

    private Vector3 orignalPos;
    private Vector3 orignalRot;
    private GameObject cameraGo;
    private Animator anim;
    private FollowTarget followTarget;
    
    public override void OnInit()
    {
        cameraGo = Camera.main.gameObject;
        anim = cameraGo.GetComponent<Animator>();
        followTarget = cameraGo.GetComponent<FollowTarget>();
        
    }

    public void FollowTarget()
    {
        followTarget.target = facade.GetCurrentGameObject().transform;
        anim.enabled = false;
        orignalPos = cameraGo.transform.position;
        orignalRot = cameraGo.transform.eulerAngles;
        Quaternion targetQuaternion = Quaternion.LookRotation(followTarget.target.position - cameraGo.transform.position);
        cameraGo.transform.DORotateQuaternion(targetQuaternion, 1f).OnComplete(() => followTarget.enabled = true);
    }

    public void WalkThroughScene()
    {
        cameraGo.transform.DOMove(orignalPos, 1);
        cameraGo.transform.DORotate(orignalRot, 1).OnComplete(() =>
        {
            anim.enabled = true;
            followTarget.enabled = false;
        });
    }

}