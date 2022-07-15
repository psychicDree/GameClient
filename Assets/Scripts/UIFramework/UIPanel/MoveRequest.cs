using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class MoveRequest : BaseRequest
{
    private Transform localPlayerTransform;
    private Transform remotePlayerTransform;
    private Animator remotePlayerAnimator;
    private PlayerMove playerMove;
    private int syncRate = 60;
    private Vector3 pos;
    private Vector3 rot;
    private float forward;
    private bool isSyncRemotePlayer = false;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }

    private void Start()
    {
        InvokeRepeating(nameof(SyncLocalPlayer), 1f, 1f / syncRate);
    }

    private void Update()
    {
        if (isSyncRemotePlayer)
        {
            SyncRemotePlayer();
            isSyncRemotePlayer = false;
        }
    }

    public MoveRequest SetLocalPlayer(Transform localPlayerTransform, PlayerMove playerMove)
    {
        this.localPlayerTransform = localPlayerTransform;
        this.playerMove = playerMove;
        return this;
    }

    public MoveRequest SetRemotePlayer(Transform remotePlayerTransform)
    {
        this.remotePlayerTransform = remotePlayerTransform;
        this.remotePlayerAnimator = remotePlayerTransform.GetComponent<Animator>();
        return this;
    }
    public void SyncLocalPlayer()
    {
        SendRequest(localPlayerTransform.position.x, localPlayerTransform.position.y, localPlayerTransform.position.z,
            localPlayerTransform.eulerAngles.x, localPlayerTransform.eulerAngles.y, localPlayerTransform.eulerAngles.z,
            playerMove.forward);
    }

    private void SyncRemotePlayer()
    {
        remotePlayerTransform.position = pos;
        remotePlayerTransform.eulerAngles = rot;
        remotePlayerAnimator.SetFloat("Forward", forward);
    }
    private void SendRequest(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, float forward)
    {
        string data = $"{xPos},{yPos},{zPos}|{xRot},{yRot},{zRot}|{forward}";
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        pos = UnityTools.parseVector3(strs[0]);
        rot = UnityTools.parseVector3(strs[1]);
        forward = float.Parse(strs[2]);
        isSyncRemotePlayer = true;
    }

    
}
