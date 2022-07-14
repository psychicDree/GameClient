using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class MoveRequest : BaseRequest
{
    private Transform localPlayerTransform;
    private PlayerMove playerMove;
    private int SyncRate = 60;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }

    private void Start()
    {
        InvokeRepeating(nameof(SyncLocalPlayer),0,1/SyncRate);
    }

    public void SetLocalPlayer(Transform localPlayerTransform,PlayerMove playerMove)
    {
        this.localPlayerTransform = localPlayerTransform;
        this.playerMove = playerMove;
    }

    public void SyncLocalPlayer()
    {
        SendRequest(localPlayerTransform.position.x,localPlayerTransform.position.y,localPlayerTransform.position.z,
            localPlayerTransform.eulerAngles.x,localPlayerTransform.eulerAngles.y,localPlayerTransform.eulerAngles.z, 
            playerMove.forward);
    }
    private void SendRequest(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, float forward)
    {
        string data = $"{xPos},{yPos},{zPos},{xRot},{yRot},{zRot},{forward}";
        base.SendRequest(data);
    }
    
}
