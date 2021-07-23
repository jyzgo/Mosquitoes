using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoinBtn :BtnExtension
{
    protected override void OnPressed()
    {
        InitMgr.current.AddCoin(InitMgr.LEVEL_REWARD_COIN_NUM);
    }

   }
