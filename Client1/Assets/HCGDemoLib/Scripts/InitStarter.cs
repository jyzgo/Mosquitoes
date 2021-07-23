using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitStarter : MonoBehaviour
{
    private void Awake()
    {
        InitMgr.current.InitCall();
        int curLevel = InitMgr.current.GetCurrentLevelIndex();
        print("cur level is " + curLevel); 

        InitMgr.current.LoadLevel(InitMgr.current.GetCurrentLevelIndex());
    }
}
