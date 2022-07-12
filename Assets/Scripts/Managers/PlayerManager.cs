using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Common;
using UnityEngine;

public class PlayerManager : BaseManager
{
    public PlayerManager(GameFacade facade) : base(facade) { }
    private UserData userData;
    private Dictionary<RoleType, RoleData> roleDataDict = new Dictionary<RoleType, RoleData>();
    private Transform rolePositions;
    public UserData UserData
    {
        set { userData = value;}
        get { return userData; }
    }
    public override void OnInit()
    {
        rolePositions = GameObject.Find("RolePositions").transform;
        InitRoleDataDictionary();
    }

    private void InitRoleDataDictionary()
    {
        roleDataDict.Add(RoleType.Blue,new RoleData(RoleType.Blue, "BluePlayer","BlueArrow", rolePositions.Find("Position1").position));
        roleDataDict.Add(RoleType.Red,new RoleData(RoleType.Blue, "RedPlayer","BlueArrow", rolePositions.Find("Position2").position));
    }

    public void SpawnRoles()
    {
        foreach (var roleData in roleDataDict.Values)
        {
            Instantiate(roleData.rolePrefab, roleData.spawnPosition, Quaternion.identity);
        }
    }
}