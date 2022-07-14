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
    private RoleType currentRoleType;
    private GameObject currentRoleGameObject;
    private GameObject playerSyncRequest;
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
        roleDataDict.Add(RoleType.Red,new RoleData(RoleType.Red, "RedPlayer","BlueArrow", rolePositions.Find("Position2").position));
    }

    public void SpawnRoles()
    {
        foreach (var roleData in roleDataDict.Values)
        {
            GameObject go = Instantiate(roleData.rolePrefab, roleData.spawnPosition, Quaternion.identity);
            if (currentRoleType == roleData.roleType)
            {
                currentRoleGameObject = go;
            }
        }
    }

    public void SetCurrentRoleType(RoleType rt)
    {
        this.currentRoleType = rt;
    }

    public GameObject GetCurrentGameObject()
    {
        return currentRoleGameObject;
    }

    public void AddControlScript()
    {
        currentRoleGameObject.AddComponent<PlayerMove>();
        PlayerAttack playerAttack = currentRoleGameObject.AddComponent<PlayerAttack>();
        RoleType rt = currentRoleGameObject.GetComponent<PlayerInfo>().role;
        RoleData rd = GetRoleDataByRoleType(rt);
        playerAttack.SetPlayerArrow(rd.arrowPrefab);
    }

    private RoleData GetRoleDataByRoleType(RoleType rt)
    {
        RoleData rd = null;
        roleDataDict.TryGetValue(rt, out rd);
        return rd;
    }

    public void CreateSyncRequest()
    {
       playerSyncRequest = new GameObject("PlayerSyncRequest");
       playerSyncRequest.AddComponent<MoveRequest>().SetLocalPlayer(currentRoleGameObject.transform,currentRoleGameObject.GetComponent<PlayerMove>());
    }
}