using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoThanks :BtnExtension
{
    protected override void OnPressed()
    {
        InitMgr.current.playNextLevel();
    }

}
