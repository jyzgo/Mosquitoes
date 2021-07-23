using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSendMsgBtn :SendMsgBtn
{
    protected override void OnPressed()
    {
        AdsMgr.current.ShowRewardAds(RewardCallBack);
    }

    void RewardCallBack()
    {
        TimeLineMgr.current.PlayingTimeLine(msg);
    }
}
