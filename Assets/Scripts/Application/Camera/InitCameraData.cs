using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCameraData : IReference
{
    private Transform playerTF;
    private Map map;

    public Transform PlayerTF => playerTF;
    public Map Map => map;

    public static InitCameraData Create(Transform playerTF,Map map)
    {
        InitCameraData initCameraData = ReferencePool.Instance.Acquire<InitCameraData>();
        initCameraData.playerTF = playerTF;
        initCameraData.map = map;
        return initCameraData;
    }
    public void Clear()
    {
        playerTF = null;
        map = null;
    }
}
