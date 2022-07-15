using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class ShootRequest : BaseRequest
{
    private Vector3 pos;
    private Vector3 rot;
    private RoleType roleType;
    private PlayerManager playerManager;
    private bool isShoot = false;
    public void SetPlayerManager(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Shoot;
        base.Awake();
    }

    private void Update()
    {
        if (isShoot)
        {
            playerManager.RemoteShoot(roleType, pos, rot);
            isShoot = false;
        }
    }

    public void SendRequest(RoleType rt, Vector3 pos, Vector3 rot)
    {
        string data = $"{(int)rt}|{pos.x},{pos.y},{pos.z}|{rot.x},{rot.y},{rot.z}";
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        roleType = (RoleType)int.Parse(strs[0]);
        pos = UnityTools.parseVector3(strs[1]);
        rot = UnityTools.parseVector3(strs[2]);
        isShoot = true;
    }
}
