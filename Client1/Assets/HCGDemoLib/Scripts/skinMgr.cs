using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class skinData
{
    public Sprite icon;
    public string bodyName;
}

public class skinMgr : MonoBehaviour
{
    public static skinMgr current;
    public static int skinCount;
    private void Awake()
    {
        skinCount = skinDatas.Length;
        current = this;
    }
    public skinData[] skinDatas;
}
