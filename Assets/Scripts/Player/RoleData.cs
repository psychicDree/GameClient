using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RoleData
{
    public GameObject rolePrefab { get; private set; }
    public GameObject arrowPrefab{ get; private set; }
    public RoleType roleType { get; private set; }
    public Vector3 spawnPosition { get; private set; }
    private const string PlayerDataPath = "Player/";

    public RoleData(RoleType roleType, string rolepath, string arrowPath, Vector3 spawnPosition)
    {
        this.roleType = roleType;
        this.rolePrefab = Resources.Load(PlayerDataPath+rolepath) as GameObject;
        this.arrowPrefab = Resources.Load(PlayerDataPath+arrowPath) as GameObject;
        this.spawnPosition = spawnPosition;
    }

}
