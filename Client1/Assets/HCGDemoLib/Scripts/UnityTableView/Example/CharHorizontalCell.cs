using EasyUI.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum CharEnum
{
    DonHave = 0,
    Have = 1
}

public enum GetCharEnum
{
    Default,
    Ads,
    Money
}

public class CharHorizontalCell: MonoBehaviour
{
    public static Action updateBG;
    public Button btnPress;
    //public Button btnCurrent;
    //public GameObject btnLocked;
    //public Text levelCount;

    public GameObject normalBg;
    public GameObject selectBg;
    public Text freeText;
    public Text moneyText;
    public Image skin_image;

    private void Awake()
    {
        updateBG += UpdateBG;
        btnPress.onClick.AddListener(BtnPressed);
        //btnCurrent.onClick.AddListener(BtnPressed);
    }

    private void OnDestroy()
    {
        updateBG -= UpdateBG;
    }

    void BtnPressed()
    {
        print("pressed " + curSkinIndex);
        if (hasGot)
        {
            InitMgr.current.SkinIndex= curSkinIndex;
            InitMgr.current.UpdateSkin();
            updateBG();
        }
        else
        {
            int curCoin = InitMgr.current.getCurrentCoinNum();
            if(curCoin >= price)
            {
                AnalyzeMgr.current.onSkinPurchased(skinMgr.current.skinDatas[curSkinIndex].bodyName);
                InitMgr.current.ReduceCoin(price);
                PlayerPrefs.SetInt(InitMgr.SKIN_INDEX + curSkinIndex.ToString(), 1);
                InitMgr.current.SkinIndex= curSkinIndex;
                UpdateCell(curSkinIndex);
                InitMgr.current.UpdateSkin();

            }
            else
            {
                ConfirmDialogUI.Instance
      .SetTitle("Not enough money")
      .SetMessage("You need " + price.ToString() + " cash to unlock current skin." )
      //.SetButtonsColor(DialogButtonColor.Yellow)
      .SetFadeDuration(.6f)
      .OnPositiveButtonClicked(() => Debug.Log("Message1 closed"))
      .Show();

            }
        }
        //InitMgr.current.LoadLevel(_levelIndex);
    }
    int curSkinIndex = 0;
    internal void UpdateCell(int i)
    {
        curSkinIndex = i;
        hasGot = PlayerPrefs.GetInt(InitMgr.SKIN_INDEX + curSkinIndex.ToString(), 0) != 0;

        skin_image.sprite = skinMgr.current.skinDatas[i].icon;
        moneyText.gameObject.SetActive(!hasGot);
        moneyText.text = price.ToString();
        UpdateBG();
    }


    public void UpdateBG()
    {
        if (InitMgr.current.SkinIndex == curSkinIndex)
        {
            normalBg.SetActive(false);
            selectBg.SetActive(true);
        }
        else
        {
            normalBg.SetActive(true);
            selectBg.SetActive(false);
        }

    }
    bool hasGot = false;
    const int price = 400;

    // Start is called before the first frame update
}
