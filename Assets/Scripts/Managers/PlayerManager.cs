using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseManager
{
    public PlayerManager(GameFacade facade) : base(facade) { }
    private UserData userData;

    public UserData UserData
    {
        set { userData = value;}
        get { return userData; }
    }
    public override void OnInit()
    {
        //base.OnInit();
    }

    public override void OnDestroy()
    {
        //base.OnDestroy();
    }
}